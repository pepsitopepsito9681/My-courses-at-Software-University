using LogisticsSystem.Services.Questions.Models;
using LogisticsSystem.Services.Reviews.Models;
using System;
using System.Linq;

namespace LogisticsSystem.Infrastructure
{
    public static class ModelsExtensions
    {
        public static string GetInformation(this IReviewModel model)
        => String.Concat(model.Title + "-" + model.Rating + "-" + model.PublishedOn);

        public static string GetInformation(this IQuestionModel model)
        => String.Concat(model.LoadCondition + "-" + model.PublishedOn + "-" + new string(model.Content.Take(5).ToArray()));
    }
}
