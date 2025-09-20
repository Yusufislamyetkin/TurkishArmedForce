using InterviewPrep.Web.Data;
using InterviewPrep.Web.Models.Questions;
using Microsoft.EntityFrameworkCore;

namespace InterviewPrep.Web.Services.Questions;

public class QuestionRepository : IQuestionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly Random _random = new();

    public QuestionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Question>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Questions
            .Include(q => q.Options)
            .OrderByDescending(q => q.CreatedAtUtc)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Question>> GetRandomAsync(int count, CancellationToken cancellationToken = default)
    {
        var total = await _context.Questions.CountAsync(cancellationToken);
        if (total == 0)
        {
            return Array.Empty<Question>();
        }

        var skip = _random.Next(0, Math.Max(total - count, 0) + 1);

        return await _context.Questions
            .Include(q => q.Options)
            .OrderBy(q => q.Id)
            .Skip(skip)
            .Take(count)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Question?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Question> questions, CancellationToken cancellationToken = default)
    {
        await _context.Questions.AddRangeAsync(questions, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
