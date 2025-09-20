using InterviewPrep.Web.Models.ViewModels;

namespace InterviewPrep.Web.Areas.Admin.ViewModels;

public class QuestionGenerationPreviewViewModel
{
    public string GenerationId { get; set; } = string.Empty;

    public IReadOnlyList<QuestionViewModel> Questions { get; set; } = Array.Empty<QuestionViewModel>();
}
