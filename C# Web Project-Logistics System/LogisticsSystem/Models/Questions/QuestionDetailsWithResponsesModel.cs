using LogisticsSystem.Services.Questions.Models;
using LogisticsSystem.Services.Responses.Models;
using System.Collections.Generic;

namespace LogisticsSystem.Models.Questions
{
    public class QuestionDetailsWithResponsesModel
    {
        public QuestionDetailsServiceModel Question { get; init; }

        public IEnumerable<ResponseServiceModel> Responses { get; init; }
    }
}
