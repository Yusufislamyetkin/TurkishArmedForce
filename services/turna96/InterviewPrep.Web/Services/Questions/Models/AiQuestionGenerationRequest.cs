using System.ComponentModel.DataAnnotations;
using InterviewPrep.Web.Models.Questions;

namespace InterviewPrep.Web.Services.Questions.Models;

public class AiQuestionGenerationRequest
{
    [Required]
    [Display(Name = "Seviye")]
    [MaxLength(32)]
    public string Level { get; set; } = "Junior";

    [Required]
    [Display(Name = "Kategori")]
    [MaxLength(64)]
    public string Category { get; set; } = "Genel";

    [Display(Name = "Soru Adedi")]
    [Range(1, 10)]
    public int Count { get; set; } = 3;

    [Display(Name = "Soru Tipleri")]
    public List<QuestionType> Types { get; set; } = new();
}
