
namespace Logistics_System.Services.Questions.Models
{
    public interface IQuestionModel
    {
        int LoadCondition { get; }

        string Content { get; }

        string PublishedOn { get; }
    }
}
