using System.Collections.Generic;

namespace Logistics_System.Services.Questions.Models
{
    public class QuestionQueryModel
    {
        public int CurrentPage { get; init; }

        public int QuestionsPerPage { get; init; }

        public int TotalQuestions { get; init; }

        public IEnumerable<QuestionServiceModel> Questions { get; init; }
    }
}
