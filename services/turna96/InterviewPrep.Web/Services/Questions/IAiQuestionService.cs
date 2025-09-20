using InterviewPrep.Web.Services.Questions.Models;

namespace InterviewPrep.Web.Services.Questions;

public interface IAiQuestionService
{
    Task<IReadOnlyList<QuestionDraft>> GenerateQuestionsAsync(AiQuestionGenerationRequest request, CancellationToken cancellationToken = default);
}
