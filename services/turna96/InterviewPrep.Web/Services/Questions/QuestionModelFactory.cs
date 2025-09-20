using System.Globalization;
using System.Linq;
using InterviewPrep.Web.Models.Questions;
using InterviewPrep.Web.Models.ViewModels;
using InterviewPrep.Web.Services.Questions.Models;

namespace InterviewPrep.Web.Services.Questions;

public interface IQuestionModelFactory
{
    PracticeSessionQuestion CreatePracticeQuestion(Question question);

    QuestionViewModel CreateViewModel(Question question);

    Question CreateEntityFromDraft(QuestionDraft draft);
}

public class QuestionModelFactory : IQuestionModelFactory
{
    public PracticeSessionQuestion CreatePracticeQuestion(Question question)
    {
        var viewModel = CreateViewModelInternal(question, out var correctOptionId);

        var state = new PracticeSessionQuestionState
        {
            Type = question.Type,
            CorrectOptionId = correctOptionId,
            ReferenceAnswer = question.ReferenceAnswer,
            TestInput = question.TestInput,
            ExpectedOutput = question.ExpectedOutput
        };

        return new PracticeSessionQuestion(viewModel, state);
    }

    public QuestionViewModel CreateViewModel(Question question)
    {
        return CreateViewModelInternal(question, out _);
    }

    public Question CreateEntityFromDraft(QuestionDraft draft)
    {
        var question = new Question
        {
            Title = draft.Title,
            Prompt = draft.Prompt,
            Type = draft.Type,
            Difficulty = draft.Difficulty,
            Category = draft.Category,
            ReferenceAnswer = draft.ReferenceAnswer,
            TestInput = draft.TestInput,
            ExpectedOutput = draft.ExpectedOutput,
            CreatedAtUtc = DateTime.UtcNow
        };

        if (draft.Type == QuestionType.MultipleChoice && draft.Options.Any())
        {
            foreach (var option in draft.Options)
            {
                question.Options.Add(new QuestionOption
                {
                    Text = option.Text,
                    IsCorrect = option.IsCorrect
                });
            }
        }

        return question;
    }

    private QuestionViewModel CreateViewModelInternal(Question question, out string? correctOptionId)
    {
        var id = DetermineId(question);
        var options = new List<QuestionOptionViewModel>();
        correctOptionId = null;

        var index = 0;
        foreach (var option in question.Options.OrderBy(o => o.Id))
        {
            var optionId = BuildOptionId(id, option.Id, index);
            options.Add(new QuestionOptionViewModel
            {
                Id = optionId,
                Text = option.Text
            });

            if (option.IsCorrect)
            {
                correctOptionId = optionId;
            }

            index++;
        }

        return new QuestionViewModel
        {
            Id = id,
            Title = question.Title,
            Prompt = question.Prompt,
            Type = question.Type,
            Difficulty = question.Difficulty,
            Category = question.Category,
            Options = options,
            TestInput = question.TestInput,
            ExpectedOutput = question.ExpectedOutput,
            ReferenceAnswer = question.ReferenceAnswer
        };
    }

    private static string DetermineId(Question question)
    {
        return question.Id > 0
            ? question.Id.ToString(CultureInfo.InvariantCulture)
            : Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
    }

    private static string BuildOptionId(string questionId, int optionId, int index)
    {
        return optionId > 0
            ? optionId.ToString(CultureInfo.InvariantCulture)
            : $"{questionId}-opt-{index}";
    }
}
