namespace InterviewPrep.Web.Models.ViewModels;

public class PracticeSessionViewModel
{
    public string PracticeId { get; set; } = string.Empty;

    public IReadOnlyList<QuestionViewModel> Questions { get; set; } = Array.Empty<QuestionViewModel>();
}
