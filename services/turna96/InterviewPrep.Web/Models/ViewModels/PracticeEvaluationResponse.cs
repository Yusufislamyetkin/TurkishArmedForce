using System.Text.Json.Serialization;

namespace InterviewPrep.Web.Models.ViewModels;

public class PracticeEvaluationResponse
{
    [JsonPropertyName("correct")]
    public bool Correct { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("alreadyCompleted")]
    public bool AlreadyCompleted { get; set; }
}
