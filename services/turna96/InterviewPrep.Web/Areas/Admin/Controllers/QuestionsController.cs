using InterviewPrep.Web.Areas.Admin.ViewModels;
using InterviewPrep.Web.Models.Questions;
using InterviewPrep.Web.Services.Questions;
using InterviewPrep.Web.Services.Questions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace InterviewPrep.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class QuestionsController : Controller
{
    private const string GenerationCachePrefix = "admin-question-generation-";

    private readonly IQuestionRepository _questionRepository;
    private readonly IAiQuestionService _aiQuestionService;
    private readonly IQuestionModelFactory _questionModelFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<QuestionsController> _logger;

    public QuestionsController(
        IQuestionRepository questionRepository,
        IAiQuestionService aiQuestionService,
        IQuestionModelFactory questionModelFactory,
        IMemoryCache memoryCache,
        ILogger<QuestionsController> logger)
    {
        _questionRepository = questionRepository;
        _aiQuestionService = aiQuestionService;
        _questionModelFactory = questionModelFactory;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var questions = await _questionRepository.GetAllAsync(cancellationToken);
        var model = questions
            .Select(q => new QuestionListItemViewModel
            {
                Id = q.Id,
                Title = q.Title,
                Type = q.Type,
                Category = q.Category,
                Difficulty = q.Difficulty,
                CreatedAtUtc = q.CreatedAtUtc,
                OptionCount = q.Options.Count
            })
            .ToList();

        ViewBag.StatusMessage = TempData["StatusMessage"] as string;
        return View(model);
    }

    [HttpGet]
    public IActionResult Generate()
    {
        ViewBag.AvailableTypes = Enum.GetValues<QuestionType>();
        return View(new AiQuestionGenerationRequest
        {
            Level = "Orta Seviye",
            Category = "Genel Teknik",
            Count = 3
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Generate(AiQuestionGenerationRequest request, CancellationToken cancellationToken)
    {
        ViewBag.AvailableTypes = Enum.GetValues<QuestionType>();

        if (request.Types.Count == 0)
        {
            ModelState.AddModelError(nameof(request.Types), "En az bir soru tipi seçmelisin.");
        }

        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var drafts = await _aiQuestionService.GenerateQuestionsAsync(request, cancellationToken);
        if (drafts.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "AI yeni soru üretemedi. Lütfen tekrar dene.");
            return View(request);
        }

        var generationId = Guid.NewGuid().ToString("N");
        _memoryCache.Set(GetCacheKey(generationId), drafts, TimeSpan.FromMinutes(10));

        var previewQuestions = drafts
            .Select(draft => _questionModelFactory.CreateViewModel(_questionModelFactory.CreateEntityFromDraft(draft)))
            .ToList();

        var previewModel = new QuestionGenerationPreviewViewModel
        {
            GenerationId = generationId,
            Questions = previewQuestions
        };

        ViewBag.GenerationRequest = request;
        return View("Preview", previewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmGeneration(string generationId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(generationId))
        {
            TempData["StatusMessage"] = "Geçersiz üretim kimliği.";
            return RedirectToAction(nameof(Index));
        }

        if (!_memoryCache.TryGetValue(GetCacheKey(generationId), out IReadOnlyList<QuestionDraft>? drafts) || drafts is null)
        {
            TempData["StatusMessage"] = "Ön izleme süresi doldu, lütfen soruları yeniden üret.";
            return RedirectToAction(nameof(Index));
        }

        var entities = drafts.Select(d => _questionModelFactory.CreateEntityFromDraft(d)).ToList();
        await _questionRepository.AddRangeAsync(entities, cancellationToken);
        await _questionRepository.SaveChangesAsync(cancellationToken);

        _memoryCache.Remove(GetCacheKey(generationId));

        TempData["StatusMessage"] = $"{entities.Count} soru başarıyla kaydedildi.";
        return RedirectToAction(nameof(Index));
    }

    private static string GetCacheKey(string generationId) => $"{GenerationCachePrefix}{generationId}";
}
