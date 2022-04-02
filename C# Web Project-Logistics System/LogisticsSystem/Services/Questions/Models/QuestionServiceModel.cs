
namespace Logistics_System.Services.Questions.Models
{
    public class QuestionServiceModel:IQuestionModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public bool IsPublic { get; set; }

        public string LoadTitle { get; set; }

        public int LoadCondition { get; set; }

        public string PublishedOn { get; set; }

        public int ResponsesCount { get; set; }
    }
}
