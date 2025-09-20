using System.Text.Json.Serialization;

namespace InterviewPrep.Web.Models.ViewModels;

public class QuestionOptionViewModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
