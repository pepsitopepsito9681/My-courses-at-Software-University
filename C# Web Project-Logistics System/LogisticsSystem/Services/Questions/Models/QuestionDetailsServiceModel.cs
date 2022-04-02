
namespace LogisticsSystem.Services.Questions.Models
{
    public class QuestionDetailsServiceModel:IQuestionModel
    {
        public int Id { get; set; }

        public string Content { get; init; }

        public string UserName { get; init; }

        public string PublishedOn { get; init; }

        public string LoadId { get; init; }

        public string LoadTitle { get; init; }

        public string LoadImage { get; init; }

        public string LoadPrice { get; init; }

        public int LoadCondition { get; init; }

        public int ResponsesCount { get; init; }

        public bool IsPublic { get; init; }
    }
}
