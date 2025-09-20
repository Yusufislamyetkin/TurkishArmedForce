using InterviewPrep.Web.Models.Questions;

namespace InterviewPrep.Web.Areas.Admin.ViewModels;

public class QuestionListItemViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public QuestionType Type { get; set; }

    public string? Category { get; set; }

    public string? Difficulty { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public int OptionCount { get; set; }
}
