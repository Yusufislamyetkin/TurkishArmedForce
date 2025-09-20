using InterviewPrep.Web.Models.ViewModels;
using InterviewPrep.Web.Services.Questions;
using InterviewPrep.Web.Services.Questions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InterviewPrep.Web.Controllers;

[Authorize]
public class QuestionsController : Controller
{
    private const int DefaultQuestionCount = 5;

    private readonly IQuestionRepository _questionRepository;
    private readonly IAiQuestionService _aiQuestionService;
    private readonly IPracticeSessionManager _practiceSessionManager;
    private readonly IQuestionModelFactory _questionModelFactory;
    private readonly ILogger<QuestionsController> _logger;

    public QuestionsController(
        IQuestionRepository questionRepository,
        IAiQuestionService aiQuestionService,
        IPracticeSessionManager practiceSessionManager,
        IQuestionModelFactory questionModelFactory,
        ILogger<QuestionsController> logger)
    {
        _questionRepository = questionRepository;
        _aiQuestionService = aiQuestionService;
        _practiceSessionManager = practiceSessionManager;
        _questionModelFactory = questionModelFactory;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> StartPractice(CancellationToken cancellationToken)
    {
        var questions = (await _questionRepository.GetRandomAsync(DefaultQuestionCount, cancellationToken)).ToList();

        if (questions.Count < DefaultQuestionCount)
        {
            var generationRequest = new AiQuestionGenerationRequest
            {
                Level = "Orta Seviye",
                Category = "Genel Teknik",
                Count = DefaultQuestionCount - questions.Count,
                Types = Enum.GetValues(typeof(Models.Questions.QuestionType)).Cast<Models.Questions.QuestionType>().ToList()
            };

            try
            {
                var drafts = await _aiQuestionService.GenerateQuestionsAsync(generationRequest, cancellationToken);
                foreach (var draft in drafts)
                {
                    questions.Add(_questionModelFactory.CreateEntityFromDraft(draft));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI sorusu alınamadı, mevcut sorular kullanılacak.");
            }
        }

        if (questions.Count == 0)
        {
            return View(new PracticeSessionViewModel
            {
                PracticeId = string.Empty,
                Questions = Array.Empty<QuestionViewModel>()
            });
        }

        var practiceQuestions = questions
            .Select(question => _questionModelFactory.CreatePracticeQuestion(question))
            .ToList();

        var practiceId = _practiceSessionManager.CreateSession(practiceQuestions);

        var model = new PracticeSessionViewModel
        {
            PracticeId = practiceId,
            Questions = practiceQuestions.Select(q => q.ViewModel).ToList()
        };

        ViewData["Title"] = "Pratik Başlat";
        return View(model);
    }

    [HttpPost]
    public IActionResult Evaluate([FromBody] QuestionSubmissionModel submission)
    {
        if (submission is null)
        {
            return BadRequest();
        }

        if (!_practiceSessionManager.TryEvaluate(submission, out var evaluation))
        {
            return BadRequest(new PracticeEvaluationResponse
            {
                Correct = false,
                Score = 0,
                Message = "Yanıt değerlendirilemedi. Oturumun süresi dolmuş olabilir.",
                AlreadyCompleted = false
            });
        }

        return Ok(new PracticeEvaluationResponse
        {
            Correct = evaluation.IsCorrect,
            Score = evaluation.ScoreAwarded,
            Message = evaluation.Message,
            AlreadyCompleted = evaluation.AlreadyCompleted
        });
    }
}
