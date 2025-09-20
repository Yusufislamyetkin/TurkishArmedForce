using System.Text.Json.Serialization;

namespace InterviewPrep.Web.Models.ViewModels;

public class QuestionSubmissionModel
{
    [JsonPropertyName("practiceId")]
    public string PracticeId { get; set; } = string.Empty;

    [JsonPropertyName("questionId")]
    public string QuestionId { get; set; } = string.Empty;

    [JsonPropertyName("answer")]
    public string? Answer { get; set; }

    [JsonPropertyName("selectedOptionId")]
    public string? SelectedOptionId { get; set; }
}
