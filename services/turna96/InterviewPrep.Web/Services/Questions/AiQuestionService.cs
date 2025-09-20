using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Linq;
using InterviewPrep.Web.Models.Questions;
using InterviewPrep.Web.Services.Questions.Models;

namespace InterviewPrep.Web.Services.Questions;

public class AiQuestionService : IAiQuestionService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AiQuestionService> _logger;

    public AiQuestionService(HttpClient httpClient, IConfiguration configuration, ILogger<AiQuestionService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IReadOnlyList<QuestionDraft>> GenerateQuestionsAsync(AiQuestionGenerationRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Types.Count == 0)
        {
            request.Types.Add(QuestionType.MultipleChoice);
        }

        var apiKey = _configuration["OpenAI:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogWarning("OpenAI API anahtarı bulunamadı. Örnek sorular döndürülüyor.");
            return BuildSampleQuestions(request);
        }

        try
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/responses");
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpRequest.Headers.Add("OpenAI-Beta", "assistants=v2");

            var payload = BuildPayload(request);
            httpRequest.Content = JsonContent.Create(payload);

            using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(contentStream, cancellationToken: cancellationToken);
            var json = ExtractJson(document.RootElement);
            if (string.IsNullOrWhiteSpace(json))
            {
                _logger.LogWarning("AI servisi boş yanıt döndürdü. Örnek sorular kullanılıyor.");
                return BuildSampleQuestions(request);
            }

            var questions = new List<QuestionDraft>();
            using var payloadDocument = JsonDocument.Parse(json);
            if (payloadDocument.RootElement.TryGetProperty("questions", out var questionArray))
            {
                foreach (var element in questionArray.EnumerateArray())
                {
                    var draft = ParseQuestion(element);
                    if (draft != null)
                    {
                        questions.Add(draft);
                    }
                }
            }

            if (questions.Count == 0)
            {
                _logger.LogWarning("AI yanıtından soru ayrıştırılamadı. Örnek sorular kullanılacak.");
                return BuildSampleQuestions(request);
            }

            return questions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI'den soru üretilirken hata oluştu. Örnek veriye düşülüyor.");
            return BuildSampleQuestions(request);
        }
    }

    private static object BuildPayload(AiQuestionGenerationRequest request)
    {
        var types = string.Join(", ", request.Types.Select(t => t.ToString()));
        var sb = new StringBuilder();
        sb.AppendLine("Aşağıdaki formatta JSON döndür:");
        sb.AppendLine("{\"questions\": [{...}]} formatı");
        sb.AppendLine("Alanlar: title (string), prompt (string), type (MultipleChoice|OpenEnded|Coding|Sql), difficulty, category, referenceAnswer, testInput, expectedOutput, options (MultipleChoice için).");
        sb.AppendLine("referenceAnswer alanı açık uçlu, kodlama ve SQL için örnek çözümü içersin.");
        sb.AppendLine("Kodlama tipinde testInput ve expectedOutput değerleri dolu olsun.");
        sb.AppendLine("Türkçe üret.");
        sb.Append("Seviye: ").AppendLine(request.Level);
        sb.Append("Kategori: ").AppendLine(request.Category);
        sb.Append("Soru adedi: ").AppendLine(request.Count.ToString());
        sb.Append("Soru tipleri: ").AppendLine(types);

        return new
        {
            model = "gpt-4.1-mini",
            response_format = new
            {
                type = "json_schema",
                json_schema = new
                {
                    name = "question_payload",
                    schema = new
                    {
                        type = "object",
                        properties = new Dictionary<string, object?>
                        {
                            ["questions"] = new Dictionary<string, object?>
                            {
                                ["type"] = "array",
                                ["items"] = new Dictionary<string, object?>
                                {
                                    ["type"] = "object",
                                    ["properties"] = new Dictionary<string, object?>
                                    {
                                        ["title"] = new Dictionary<string, object?> { ["type"] = "string" },
                                        ["prompt"] = new Dictionary<string, object?> { ["type"] = "string" },
                                        ["type"] = new Dictionary<string, object?>
                                        {
                                            ["type"] = "string",
                                            ["enum"] = Enum.GetNames(typeof(QuestionType))
                                        },
                                        ["difficulty"] = new Dictionary<string, object?> { ["type"] = "string" },
                                        ["category"] = new Dictionary<string, object?> { ["type"] = "string" },
                                        ["referenceAnswer"] = new Dictionary<string, object?> { ["type"] = "string" },
                                        ["testInput"] = new Dictionary<string, object?> { ["type"] = "string" },
                                        ["expectedOutput"] = new Dictionary<string, object?> { ["type"] = "string" },
                                        ["options"] = new Dictionary<string, object?>
                                        {
                                            ["type"] = "array",
                                            ["items"] = new Dictionary<string, object?>
                                            {
                                                ["type"] = "object",
                                                ["properties"] = new Dictionary<string, object?>
                                                {
                                                    ["text"] = new Dictionary<string, object?> { ["type"] = "string" },
                                                    ["isCorrect"] = new Dictionary<string, object?> { ["type"] = "boolean" }
                                                },
                                                ["required"] = new[] { "text", "isCorrect" }
                                            }
                                        }
                                    },
                                    ["required"] = new[] { "title", "prompt", "type" }
                                }
                            }
                        },
                        required = new[] { "questions" }
                    }
                }
            },
            input = new object[]
            {
                new
                {
                    role = "system",
                    content = new object[]
                    {
                        new { type = "text", text = "Deneyimli bir teknik mülakat eğitmeni olarak davran." }
                    }
                },
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new { type = "text", text = sb.ToString() }
                    }
                }
            }
        };
    }

    private static string? ExtractJson(JsonElement root)
    {
        if (root.TryGetProperty("output", out var output))
        {
            foreach (var item in output.EnumerateArray())
            {
                if (item.TryGetProperty("content", out var contentArray))
                {
                    foreach (var content in contentArray.EnumerateArray())
                    {
                        if (content.TryGetProperty("text", out var textElement))
                        {
                            var text = textElement.GetString();
                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                return text;
                            }
                        }
                    }
                }
            }
        }

        if (root.TryGetProperty("output_text", out var legacyText))
        {
            return legacyText.GetString();
        }

        return null;
    }

    private static QuestionDraft? ParseQuestion(JsonElement element)
    {
        if (!element.TryGetProperty("type", out var typeElement))
        {
            return null;
        }

        if (!Enum.TryParse<QuestionType>(typeElement.GetString(), out var type))
        {
            return null;
        }

        var draft = new QuestionDraft
        {
            Title = element.GetProperty("title").GetString() ?? string.Empty,
            Prompt = element.GetProperty("prompt").GetString() ?? string.Empty,
            Type = type,
            Difficulty = element.TryGetProperty("difficulty", out var diff) ? diff.GetString() : null,
            Category = element.TryGetProperty("category", out var cat) ? cat.GetString() : null,
            ReferenceAnswer = element.TryGetProperty("referenceAnswer", out var reference) ? reference.GetString() : null,
            TestInput = element.TryGetProperty("testInput", out var input) ? input.GetString() : null,
            ExpectedOutput = element.TryGetProperty("expectedOutput", out var output) ? output.GetString() : null
        };

        if (element.TryGetProperty("options", out var options) && options.ValueKind == JsonValueKind.Array)
        {
            foreach (var option in options.EnumerateArray())
            {
                draft.Options.Add(new QuestionOptionDraft
                {
                    Text = option.TryGetProperty("text", out var text) ? text.GetString() ?? string.Empty : string.Empty,
                    IsCorrect = option.TryGetProperty("isCorrect", out var correct) && correct.GetBoolean()
                });
            }
        }

        return draft;
    }

    private static IReadOnlyList<QuestionDraft> BuildSampleQuestions(AiQuestionGenerationRequest request)
    {
        var pool = new List<QuestionDraft>
        {
            new()
            {
                Title = "HTTP Status Kodları",
                Prompt = "Bir REST API'de başarılı bir GET isteği hangi HTTP durum kodu ile sonuçlanır?",
                Type = QuestionType.MultipleChoice,
                Category = request.Category,
                Difficulty = request.Level,
                Options =
                {
                    new QuestionOptionDraft { Text = "200 OK", IsCorrect = true },
                    new QuestionOptionDraft { Text = "201 Created", IsCorrect = false },
                    new QuestionOptionDraft { Text = "400 Bad Request", IsCorrect = false },
                    new QuestionOptionDraft { Text = "500 Internal Server Error", IsCorrect = false }
                }
            },
            new()
            {
                Title = "SQL Join Temelleri",
                Prompt = "orders ve customers tablolarını sipariş bazında birleştiren SELECT sorgusunu yaz.",
                Type = QuestionType.Sql,
                Category = request.Category,
                Difficulty = request.Level,
                ReferenceAnswer = "SELECT o.Id, c.Name FROM orders o INNER JOIN customers c ON c.Id = o.CustomerId;",
                TestInput = null,
                ExpectedOutput = null
            },
            new()
            {
                Title = "Dizideki En Büyük Sayı",
                Prompt = "Verilen tamsayı dizisindeki en büyük değeri döndüren bir C# metodu yaz.",
                Type = QuestionType.Coding,
                Category = request.Category,
                Difficulty = request.Level,
                ReferenceAnswer = "return numbers.Max();",
                TestInput = "[4, 12, 7, 2]",
                ExpectedOutput = "12"
            },
            new()
            {
                Title = "REST vs SOAP",
                Prompt = "REST ile SOAP arasındaki temel farkları açıklayın.",
                Type = QuestionType.OpenEnded,
                Category = request.Category,
                Difficulty = request.Level,
                ReferenceAnswer = "REST kaynak odaklıdır, SOAP ise protokol bazlıdır. REST HTTP metodlarını kullanırken SOAP XML tabanlı mesajlar ile çalışır."
            }
        };

        var filtered = request.Types.Count > 0
            ? pool.Where(q => request.Types.Contains(q.Type)).ToList()
            : pool.ToList();

        if (filtered.Count == 0)
        {
            filtered = pool;
        }

        var result = new List<QuestionDraft>();
        while (result.Count < Math.Max(1, request.Count))
        {
            result.AddRange(filtered);
        }

        return result.Take(Math.Max(1, request.Count)).ToList();
    }
}
