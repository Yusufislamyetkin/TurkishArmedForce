using System.ComponentModel.DataAnnotations;

namespace InterviewPrep.Web.Models.Questions;

public class Question
{
    public int Id { get; set; }

    [Required]
    [MaxLength(180)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Prompt { get; set; } = string.Empty;

    public QuestionType Type { get; set; }

    [MaxLength(64)]
    public string? Difficulty { get; set; }

    [MaxLength(128)]
    public string? Category { get; set; }

    public string? ReferenceAnswer { get; set; }

    public string? TestInput { get; set; }

    public string? ExpectedOutput { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
}
