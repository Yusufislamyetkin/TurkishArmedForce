using InterviewPrep.Web.Models.Questions;

namespace InterviewPrep.Web.Services.Questions;

public interface IQuestionRepository
{
    Task<IReadOnlyList<Question>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Question>> GetRandomAsync(int count, CancellationToken cancellationToken = default);

    Task<Question?> FindAsync(int id, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<Question> questions, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
