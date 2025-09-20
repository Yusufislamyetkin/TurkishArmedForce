using InterviewPrep.Web.Models.Questions;

namespace InterviewPrep.Web.Services.Questions.Models;

public class QuestionDraft
{
    public string Title { get; set; } = string.Empty;

    public string Prompt { get; set; } = string.Empty;

    public QuestionType Type { get; set; }

    public string? Difficulty { get; set; }

    public string? Category { get; set; }

    public string? ReferenceAnswer { get; set; }

    public string? TestInput { get; set; }

    public string? ExpectedOutput { get; set; }

    public List<QuestionOptionDraft> Options { get; set; } = new();
}
