using System.Text.Json.Serialization;
using InterviewPrep.Web.Models.Questions;

namespace InterviewPrep.Web.Models.ViewModels;

public class QuestionViewModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public QuestionType Type { get; set; }

    [JsonPropertyName("difficulty")]
    public string? Difficulty { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("options")]
    public IReadOnlyList<QuestionOptionViewModel> Options { get; set; } = Array.Empty<QuestionOptionViewModel>();

    [JsonPropertyName("testInput")]
    public string? TestInput { get; set; }

    [JsonPropertyName("expectedOutput")]
    public string? ExpectedOutput { get; set; }

    [JsonPropertyName("referenceAnswer")]
    public string? ReferenceAnswer { get; set; }
}
