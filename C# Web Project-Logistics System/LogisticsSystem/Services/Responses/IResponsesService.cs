using LogisticsSystem.Services.Responses.Models;
using System.Collections.Generic;

namespace LogisticsSystem.Services.Responses
{
    public interface IResponsesService
    {
        void Create(
            int questionId,
            string userId,
            string content,
            bool IsPublic = false
            );

        IEnumerable<ResponseServiceModel> ResponsesOfQuestion(int questionId);

        ResponseQueryModel All(
           string searchTerm = null,
           int currentPage = 1,
           int answersPerPage = int.MaxValue,
           bool IsPublicOnly = true);

        void ChangeVisibility(int id);

        bool Delete(int id);
    }
}
