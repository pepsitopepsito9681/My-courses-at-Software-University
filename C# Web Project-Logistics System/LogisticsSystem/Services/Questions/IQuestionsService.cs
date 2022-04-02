using Logistics_System.Services.Questions.Models;
using System.Collections.Generic;

namespace Logistics_System.Services.Questions
{
    public interface IQuestionsService
    {
        void Create(
            string loadId,
            string userId,
            string content,
            bool IsPublic = false
            );

        void ChangeVisibility(int id);

        bool QuestionExists(int id);

        bool QuestionIsByUser(int id, string userId);

        bool Delete(int id);

        QuestionDetailsServiceModel Details(int id);

        QuestionServiceModel QuestionById(int id);

        IEnumerable<QuestionListingServiceModel> MyQuestions(string userId);

        QuestionQueryModel All(
            string searchTerm = null,
            int currentPage = 1,
            int questionsPerPage = int.MaxValue,
            bool IsPublicOnly = true,
            string loadId = null
            );
    }
}
