using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using InterviewPrep.Web.Models.Questions;
using InterviewPrep.Web.Models.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace InterviewPrep.Web.Services.Questions;

public interface IPracticeSessionManager
{
    string CreateSession(IEnumerable<PracticeSessionQuestion> questions);

    bool TryEvaluate(QuestionSubmissionModel submission, out PracticeEvaluationResult result);
}

public class PracticeSessionManager : IPracticeSessionManager
{
    private const string CachePrefix = "practice-session-";
    private static readonly TimeSpan SessionLifetime = TimeSpan.FromMinutes(45);

    private readonly IMemoryCache _memoryCache;

    public PracticeSessionManager(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public string CreateSession(IEnumerable<PracticeSessionQuestion> questions)
    {
        var sessionId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        var session = new PracticeSession();

        foreach (var question in questions)
        {
            session.Questions[question.ViewModel.Id] = question.State;
        }

        _memoryCache.Set(GetCacheKey(sessionId), session, SessionLifetime);
        return sessionId;
    }

    public bool TryEvaluate(QuestionSubmissionModel submission, out PracticeEvaluationResult result)
    {
        result = PracticeEvaluationResult.Failed("Oturum bulunamadı.");

        if (string.IsNullOrWhiteSpace(submission.PracticeId) || string.IsNullOrWhiteSpace(submission.QuestionId))
        {
            return false;
        }

        if (!_memoryCache.TryGetValue(GetCacheKey(submission.PracticeId), out PracticeSession? session) || session is null)
        {
            return false;
        }

        if (!session.Questions.TryGetValue(submission.QuestionId, out var state))
        {
            return false;
        }

        if (state.IsSolved)
        {
            result = PracticeEvaluationResult.AlreadyCompleted(state);
            return true;
        }

        state.AttemptCount++;
        var evaluation = Evaluate(state, submission);
        if (evaluation.IsCorrect)
        {
            state.IsSolved = true;
        }

        result = evaluation;
        return true;
    }

    private PracticeEvaluationResult Evaluate(PracticeSessionQuestionState state, QuestionSubmissionModel submission)
    {
        return state.Type switch
        {
            QuestionType.MultipleChoice => EvaluateMultipleChoice(state, submission),
            QuestionType.OpenEnded => EvaluateOpenEnded(state, submission),
            QuestionType.Coding => EvaluateCoding(state, submission),
            QuestionType.Sql => EvaluateSql(state, submission),
            _ => PracticeEvaluationResult.Failed("Bu soru tipi henüz desteklenmiyor.")
        };
    }

    private static PracticeEvaluationResult EvaluateMultipleChoice(PracticeSessionQuestionState state, QuestionSubmissionModel submission)
    {
        if (string.IsNullOrWhiteSpace(submission.SelectedOptionId))
        {
            return PracticeEvaluationResult.Failed("Lütfen bir seçenek seç.");
        }

        var isCorrect = string.Equals(state.CorrectOptionId, submission.SelectedOptionId, StringComparison.Ordinal);
        if (!isCorrect)
        {
            var message = "Seçtiğin cevap yanlış. Tekrar dene!";
            if (!string.IsNullOrWhiteSpace(state.ReferenceAnswer))
            {
                message += $"\nİpucu: {state.ReferenceAnswer}";
            }

            return PracticeEvaluationResult.Incorrect(message);
        }

        return PracticeEvaluationResult.Correct("Harika! Doğru şıkkı seçtin.");
    }

    private static PracticeEvaluationResult EvaluateOpenEnded(PracticeSessionQuestionState state, QuestionSubmissionModel submission)
    {
        if (string.IsNullOrWhiteSpace(submission.Answer))
        {
            return PracticeEvaluationResult.Failed("Yanıt alanını doldur.");
        }

        if (string.IsNullOrWhiteSpace(state.ReferenceAnswer))
        {
            return PracticeEvaluationResult.Correct("Yanıtın kaydedildi. Referans cevap belirlenmemiş olduğundan otomatik olarak doğru kabul edildi.");
        }

        var similarity = CalculateKeywordOverlap(state.ReferenceAnswer!, submission.Answer);
        if (similarity >= 0.6)
        {
            return PracticeEvaluationResult.Correct("Yanıtın referans çözümle örtüşüyor!");
        }

        return PracticeEvaluationResult.Incorrect($"Yanıtın hedeflenen noktaları kaçırıyor. Örnek cevap: {state.ReferenceAnswer}");
    }

    private static PracticeEvaluationResult EvaluateCoding(PracticeSessionQuestionState state, QuestionSubmissionModel submission)
    {
        if (string.IsNullOrWhiteSpace(submission.Answer))
        {
            return PracticeEvaluationResult.Failed("Kod alanını doldur.");
        }

        var answerNormalized = NormalizeCode(submission.Answer);
        var referenceNormalized = string.IsNullOrWhiteSpace(state.ReferenceAnswer) ? string.Empty : NormalizeCode(state.ReferenceAnswer);
        var expectedOutput = string.IsNullOrWhiteSpace(state.ExpectedOutput) ? string.Empty : NormalizeCode(state.ExpectedOutput);

        var matchesReference = !string.IsNullOrEmpty(referenceNormalized) && answerNormalized.Contains(referenceNormalized, StringComparison.OrdinalIgnoreCase);
        var matchesOutput = !string.IsNullOrEmpty(expectedOutput) && answerNormalized.Contains(expectedOutput, StringComparison.OrdinalIgnoreCase);

        if (matchesReference || matchesOutput)
        {
            var message = "Kodun örnek çözümle uyumlu görünüyor.";
            if (!string.IsNullOrWhiteSpace(state.ExpectedOutput))
            {
                message += $" Test çıktısı: {state.ExpectedOutput}";
            }

            return PracticeEvaluationResult.Correct(message);
        }

        var failureMessage = "Kod çözümü beklentileri karşılamıyor.";
        if (!string.IsNullOrWhiteSpace(state.ReferenceAnswer))
        {
            failureMessage += $" Örnek çözüm: {state.ReferenceAnswer}";
        }

        if (!string.IsNullOrWhiteSpace(state.ExpectedOutput))
        {
            failureMessage += $" Beklenen çıktı: {state.ExpectedOutput}";
        }

        return PracticeEvaluationResult.Incorrect(failureMessage);
    }

    private static PracticeEvaluationResult EvaluateSql(PracticeSessionQuestionState state, QuestionSubmissionModel submission)
    {
        if (string.IsNullOrWhiteSpace(submission.Answer))
        {
            return PracticeEvaluationResult.Failed("SQL sorgunu yaz.");
        }

        var normalizedAnswer = NormalizeSql(submission.Answer);
        if (!normalizedAnswer.StartsWith("select", StringComparison.OrdinalIgnoreCase))
        {
            return PracticeEvaluationResult.Incorrect("Sorgun SELECT ile başlamalı.");
        }

        if (!string.IsNullOrWhiteSpace(state.ReferenceAnswer))
        {
            var normalizedReference = NormalizeSql(state.ReferenceAnswer);
            var referenceTokens = ExtractSqlTokens(normalizedReference);
            var answerTokens = ExtractSqlTokens(normalizedAnswer);
            var coverage = referenceTokens.Count == 0 ? 0 : referenceTokens.Count(token => answerTokens.Contains(token)) / (double)referenceTokens.Count;
            if (coverage < 0.6)
            {
                return PracticeEvaluationResult.Incorrect($"Sorgu anahtar alanları içermiyor. Örnek: {state.ReferenceAnswer}");
            }
        }

        return PracticeEvaluationResult.Correct("SQL sorgusu doğru formatta görünüyor.");
    }

    private static HashSet<string> ExtractSqlTokens(string sql)
    {
        var separators = new[] { ' ', '\n', '\r', '\t', ',', '(', ')', '=', '.', ';' };
        return sql.ToLowerInvariant()
            .Split(separators, StringSplitOptions.RemoveEmptyEntries)
            .Where(token => token.Length > 2 && !IsSqlKeyword(token))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsSqlKeyword(string token)
    {
        return token is "select" or "from" or "inner" or "join" or "left" or "right" or "where" or "on" or "and" or "or";
    }

    private static double CalculateKeywordOverlap(string reference, string answer)
    {
        var referenceTokens = Tokenize(reference);
        var answerTokens = Tokenize(answer);

        if (referenceTokens.Count == 0)
        {
            return 0;
        }

        var matches = referenceTokens.Count(token => answerTokens.Contains(token));
        return matches / (double)referenceTokens.Count;
    }

    private static HashSet<string> Tokenize(string input)
    {
        var separators = new[] { ' ', '\n', '\r', '\t', '.', ',', ';', ':', '(', ')', '[', ']', '{', '}', '"', '\'', '!' };
        return input.ToLowerInvariant()
            .Split(separators, StringSplitOptions.RemoveEmptyEntries)
            .Select(token => token.Trim())
            .Where(token => token.Length > 2)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static string NormalizeCode(string code)
    {
        return new string(code.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLowerInvariant();
    }

    private static string NormalizeSql(string sql)
    {
        return string.Join(' ', sql.Split(new[] { '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)).Trim();
    }

    private static string GetCacheKey(string practiceId) => $"{CachePrefix}{practiceId}";

    private sealed class PracticeSession
    {
        public ConcurrentDictionary<string, PracticeSessionQuestionState> Questions { get; } = new(StringComparer.Ordinal);
    }
}

public record PracticeSessionQuestion(QuestionViewModel ViewModel, PracticeSessionQuestionState State);

public class PracticeSessionQuestionState
{
    public QuestionType Type { get; init; }

    public string? CorrectOptionId { get; init; }

    public string? ReferenceAnswer { get; init; }

    public string? TestInput { get; init; }

    public string? ExpectedOutput { get; init; }

    public int AttemptCount { get; set; }

    public bool IsSolved { get; set; }
}

public class PracticeEvaluationResult
{
    private PracticeEvaluationResult(bool isCorrect, string message, bool alreadyCompleted)
    {
        IsCorrect = isCorrect;
        Message = message;
        AlreadyCompleted = alreadyCompleted;
        ScoreAwarded = isCorrect && !alreadyCompleted ? 10 : 0;
    }

    public bool IsCorrect { get; }

    public string Message { get; }

    public int ScoreAwarded { get; }

    public bool AlreadyCompleted { get; }

    public static PracticeEvaluationResult Correct(string message) => new(true, message, false);

    public static PracticeEvaluationResult Incorrect(string message) => new(false, message, false);

    public static PracticeEvaluationResult Failed(string message) => new(false, message, false);

    public static PracticeEvaluationResult AlreadyCompleted(PracticeSessionQuestionState state)
    {
        var message = state.IsSolved
            ? "Bu soruyu zaten doğru yanıtladın."
            : "Bu soruyu daha önce denedin.";
        return new PracticeEvaluationResult(state.IsSolved, message, true);
    }
}
