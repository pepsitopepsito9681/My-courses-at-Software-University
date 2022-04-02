
namespace Logistics_System.Services.Questions.Models
{
    public class QuestionListingServiceModel:IQuestionModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string PublishedOn { get; set; }

        public string LoadId { get; set; }

        public string LoadTitle { get; set; }

        public string LoadImage { get; set; }

        public int LoadCondition { get; set; }

        public int ResponsesCount { get; init; }

        public bool IsPublic { get; init; }
    }
}
