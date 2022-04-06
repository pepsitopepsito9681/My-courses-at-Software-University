using AutoMapper;
using LogisticsSystem.Data;
using LogisticsSystem.Infrastructure;
using LogisticsSystem.Models.Loads;
using LogisticsSystem.Services.Loads;
using LogisticsSystem.Services.Questions;
using LogisticsSystem.Services.Reviews;
using LogisticsSystem.Services.Traders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LogisticsSystem.WebConstants;
using static LogisticsSystem.Areas.Admin.AdminConstants;

namespace LogisticsSystem.Controllers
{
    public class LoadsController : Controller
    {
        private readonly ILoadsService loads;
        private readonly ITradersService traders;
        private readonly IReviewsService reviews;
        private readonly IQuestionsService questions;
        private readonly IMapper mapper;
        private readonly LogisticsSystemDbContext data;

        public LoadsController(LogisticsSystemDbContext data, ILoadsService loads, ITradersService traders, IMapper mapper, IReviewsService reviews, IQuestionsService questions)
        {
            this.data = data;
            this.loads = loads;
            this.traders = traders;
            this.mapper = mapper;
            this.reviews = reviews;
            this.questions = questions;
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.traders.IsUserTrader(this.User.Id()) && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(TradersController.Become), "Traders");
            }

            return View(new LoadFormModel
            {
                Kinds = this.loads.AllKinds(),
                SubKinds = this.loads.AllSubKinds()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(LoadFormModel load)
        {
            string dealerId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (dealerId == null && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!load.AgreeOnTermsOfPolitics && !isUserAdmin)
            {
                this.ModelState.AddModelError(nameof(load.AgreeOnTermsOfPolitics), "You must agree before submiting.");
            }
            if (!this.loads.KindExists(load.KindId))
            {
                this.ModelState.AddModelError(nameof(load.KindId), "Kind does not exists.");
            }
            if (!this.loads.SubKindExists(load.SubKindId, load.KindId))
            {
                this.ModelState.AddModelError(nameof(load.SubKindId), "SubKind is not valid.");
            }

            if (!ModelState.IsValid)
            {

                load.Kinds = this.loads.AllKinds();
                load.SubKinds = this.loads.AllSubKinds();

                return View(load);
            }

            var loadId = this.loads
                   .Create(load.Title,
                           load.Description,
                           load.FirstImageUrl,
                           load.SecondImageUrl,
                           load.ThirdImageUr,
                           load.Price,
                           load.Quantity,
                           load.KindId,
                           load.SubKindId,
                           load.Condition.Value,
                           load.Delivery.Value,
                           dealerId,
                           isUserAdmin);

            TempData[GlobalMessageKey] = $"Your load was added { (isUserAdmin ? string.Empty : "and is awaiting for approval!") }";

            return RedirectToAction(nameof(Details), new { id = loadId });

        }

        public IActionResult All([FromQuery] LoadsQueryModel query)
        {
            if (!this.loads.SubKindIsValid(query.SubKind, query.Kind))
            {
                return BadRequest();
            }

            var queryResult = this.loads.All(
            query.Kind,
            query.SubKind,
            query.SearchTerm,
            query.CurrentPage,
            LoadsQueryModel.LoadsPerPage,
            query.LoadSorting);

            var kinds = this.loads.AllKinds();
            var subKinds = this.loads.AllSubKinds();

            query.Loads = queryResult.Loads;
            query.Kinds = kinds;
            query.SubKinds = subKinds;
            query.TotalLoads = queryResult.TotalLoads;

            return this.View(query);
        }


        [Authorize]
        public IActionResult MyLoads()
        {
            var myLoads = this.loads.ByUser(this.User.Id());

            if (this.User.IsAdmin())
            {
                return RedirectToAction(nameof(All));
            }

            return this.View(myLoads);
        }

        [Authorize]
        public IActionResult Edit(string Id)
        {
            string dealerId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (dealerId == null && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!this.loads.LoadExists(Id))
            {
                return NotFound();
            }

            if (!this.loads.LoadIsByTrader(Id, dealerId) && !isUserAdmin)
            {
                return BadRequest();
            }

            var load = this.loads.Details(Id);

            var loadForm = this.mapper.Map<LoadFormModel>(load);

            loadForm.Kinds = this.loads.AllKinds();
            loadForm.SubKinds = this.loads.AllSubKinds();

            return View(loadForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(string Id, LoadFormModel load)
        {
            string dealerId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (dealerId == null && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!this.loads.LoadIsByTrader(Id, dealerId) && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!this.loads.LoadExists(Id))
            {
                return NotFound();
            }

            if (!this.loads.KindExists(load.KindId))
            {
                this.ModelState.AddModelError(nameof(load.KindId), "Kind does not exists.");
            }
            if (!this.loads.SubKindExists(load.SubKindId, load.KindId))
            {
                this.ModelState.AddModelError(nameof(load.SubKindId), "SubKind is not valid.");
            }

            if (!ModelState.IsValid)
            {

                load.Kinds = this.loads.AllKinds();
                load.SubKinds = this.loads.AllSubKinds();

                return View(load);
            }

            this.loads.Edit(
                  Id,
                  load.Title,
                  load.Description,
                  load.FirstImageUrl,
                  load.SecondImageUrl,
                  load.ThirdImageUr,
                  load.Price,
                  load.Quantity,
                  load.KindId,
                  load.SubKindId,
                  load.Condition.Value,
                  load.Delivery.Value,
                  dealerId,
                  isUserAdmin);

            TempData[GlobalMessageKey] = $"Your load was edited { (isUserAdmin ? string.Empty : "and is awaiting for approval!") } ";

            return RedirectToAction(nameof(Details), new { Id });
        }


        public IActionResult Details(string id, [FromQuery] LoadsDetailsQueryModel query)
        {
            string dealerId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            var load = this.loads.Details(id);

            if (load == null)
            {
                return NotFound();
            }

            if (!this.loads.LoadIsByTrader(id, dealerId) &&
                !load.IsPublic &&
                !isUserAdmin)
            {
                return BadRequest();
            }

            var similarLoads = this.loads.GetSimilarLoads(id);

            var reviewsQueryResult = this.reviews.All(
                query.ReviewsSearchTerm,
                query.ReviewsCurrentPage,
                LoadsDetailsQueryModel.ReviewsPerPage,
                query.ReviewFiltering,
                loadId: id);

            var questionsQueryResult = this.questions.All(
                currentPage: query.QuestionsCurrentPage,
                questionsPerPage: LoadsDetailsQueryModel.QuestionsPerPage,
                loadId: id);

            var reviewsStatistics = this.reviews.GetStatisticsForLoad(id);

            query.Load = load;
            query.SimilarLoads = similarLoads;
            query.LoadReviewsStatistics = reviewsStatistics;

            query.Reviews = reviewsQueryResult.Reviews;
            query.TotalReviews = reviewsQueryResult.TotalReviews;

            query.Questions = questionsQueryResult.Questions;
            query.TotalQuestions = questionsQueryResult.TotalQuestions;

            return this.View(query);
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            string dealerId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (dealerId == null && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!this.loads.LoadExists(id))
            {
                return NotFound();
            }

            if (!this.loads.LoadIsByTrader(id, dealerId) && !isUserAdmin)
            {
                return BadRequest();
            }

            return View();

        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(string id, LoadDeleteFormModel loadDelete)
        {
            string dealerId = this.traders.IdByUser(this.User.Id());

            var isUserAdmin = this.User.IsAdmin();

            if (dealerId == null && !isUserAdmin)
            {
                return BadRequest();
            }

            if (!this.loads.LoadIsByTrader(id, dealerId) && !isUserAdmin)
            {
                return BadRequest();
            }

            if (loadDelete.ConfirmDeletion)
            {
                var deleted = this.loads.DeleteLoad(id, isUserAdmin);

                if (!deleted)
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction(nameof(Details), new { id });
            }

            this.TempData[GlobalMessageKey] = "Your load was deleted!";

            if (isUserAdmin)
            {
                return RedirectToAction(nameof(Areas.Admin.Controllers.LoadsController.Existing), new { area = AreaName });
            }

            return RedirectToAction(nameof(All));
        }
    }
}
