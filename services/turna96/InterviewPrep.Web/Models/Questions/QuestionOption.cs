using System.ComponentModel.DataAnnotations;

namespace InterviewPrep.Web.Models.Questions;

public class QuestionOption
{
    public int Id { get; set; }

    [Required]
    [MaxLength(512)]
    public string Text { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }

    public Question Question { get; set; } = null!;
}
