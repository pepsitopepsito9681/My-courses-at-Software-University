using LogisticsSystem.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace LogisticsSystem.Infrastructure
{
    public static class EndPointRouteBuilderExtensions
    {
        public static void MapDefaultAreaRoute(this IEndpointRouteBuilder endpoints)
        => endpoints.MapControllerRoute(
            name: "Areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );


        public static void MapReviewsDetailsRoute(this IEndpointRouteBuilder endpoints)
            => endpoints.MapControllerRoute
                (
                  name: "Review Details",
                  pattern: "/Reviews/Details/{id}/{information}",
                  defaults: new
                  {
                      controller = typeof(ReviewsController).GetControllerName(),
                      action = nameof(ReviewsController.Details)
                  }
                );

        public static void MapQuestionsDetailsRoute(this IEndpointRouteBuilder endpoints)
           => endpoints.MapControllerRoute
               (
                 name: "Question Details",
                 pattern: "/Questions/Details/{id}/{information}",
                 defaults: new
                 {
                     controller = typeof(QuestionsController).GetControllerName(),
                     action = nameof(QuestionsController.Details)
                 }
               );

        public static void MapResponsesAddRoute(this IEndpointRouteBuilder endpoints)
        => endpoints.MapControllerRoute
            (
              name: "Responses Add",
              pattern: "/Responses/Add/{id}/{information}",
              defaults: new
              {
                  controller = typeof(ResponsesController).GetControllerName(),
                  action = nameof(ResponsesController.Add)
              }
            );

        public static void MapCommentsAddRoute(this IEndpointRouteBuilder endpoints)
       => endpoints.MapControllerRoute
           (
             name: "Comments Add",
             pattern: "/Comments/Add/{id}/{information}",
             defaults: new
             {
                 controller = typeof(CommentsController).GetControllerName(),
                 action = nameof(CommentsController.Add)
             }
           );
    }
}
