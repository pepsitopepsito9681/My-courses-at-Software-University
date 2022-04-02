
namespace LogisticsSystem.Services.Questions.Models
{
    public interface IQuestionModel
    {
        int LoadCondition { get; }

        string Content { get; }

        string PublishedOn { get; }
    }
}
