using AutoMapper;
using AutoMapper.QueryableExtensions;
using LogisticsSystem.Data;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Services.Reviews.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Services.Reviews
{
    public class ReviewsService:IReviewsService
    {
        private readonly LogisticsSystemDbContext data;
        private readonly IConfigurationProvider mapper;

        public ReviewsService(LogisticsSystemDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }



        public void Create(
            string loadId,
            string userId,
            ReviewKind rating,
            string content,
            string title,
            bool IsPublic = false)
        {
            title = ValidateTitle(rating, title);

            var review = new Review
            {
                Rating = rating,
                Content = content,
                Title = title,
                LoadId = loadId,
                UserId = userId,
                PublishedOn = DateTime.UtcNow,
                IsPublic = IsPublic

            };


            this.data.Add(review);

            this.data.SaveChanges();


        }



        public ReviewDetailsServiceModel Details(int id)
        => this.data.Reviews.Where(x => x.Id == id)
           .ProjectTo<ReviewDetailsServiceModel>(mapper)
            .FirstOrDefault();

        public IEnumerable<ReviewListingServiceModel> ByUser(string userId)
        => this.data.Reviews.Where(x => x.UserId == userId)
        .ProjectTo<ReviewListingServiceModel>(mapper)
        .ToList();



        public bool Edit(int id, ReviewKind rating, string content, string title,
            bool IsPublic = false)
        {

            var review = this.data.Reviews.FirstOrDefault(x => x.Id == id);

            if (review == null)
            {
                return false;
            }

            title = ValidateTitle(rating, title);


            review.Title = title;
            review.Rating = rating;
            review.Content = content;
            review.IsPublic = IsPublic;

            this.data.SaveChanges();

            return true;


        }

        public bool Delete(int id)
        {
            var review = this.data.Reviews.FirstOrDefault(x => x.Id == id);

            if (review == null)
            {
                return false;
            }


            this.data.Reviews.Remove(review);

            this.data.SaveChanges();

            return true;
        }

        public ReviewsLoadStatisticsServiceModel GetStatisticsForLoad(string loadId)
        {

            var reviews = this.All(loadId: loadId).Reviews;

            var reviewsCount = reviews.Count();

            var rating = reviewsCount == 0 ? "0" : ((decimal)(reviews.Sum(x => x.Rating)) / (decimal)reviewsCount).ToString("F2");

            var fiveStarRatings = reviews.Where(x => x.Rating == 5).Count();
            var fourStarRatings = reviews.Where(x => x.Rating == 4).Count();
            var threeStarRatings = reviews.Where(x => x.Rating == 3).Count();
            var twoStarRatings = reviews.Where(x => x.Rating == 2).Count();
            var oneStarRatings = reviews.Where(x => x.Rating == 1).Count();

            return new ReviewsLoadStatisticsServiceModel
            {
                Rating = rating,
                TotalReviews = reviewsCount,
                FiveStarRatings = fiveStarRatings,
                FourStarRatings = fourStarRatings,
                ThreeStarRatings = threeStarRatings,
                TwoStarRatings = twoStarRatings,
                OneStarRatings = oneStarRatings
            };
        }

        public ReviewServiceModel ReviewByLoadAndUser(string loadId, string userId)
        => this.data.Reviews
            .Where(x => x.LoadId == loadId && x.UserId == userId)
            .ProjectTo<ReviewServiceModel>(mapper)
            .FirstOrDefault();

        public ReviewServiceModel ReviewById(int id)
         => this.data.Reviews
            .Where(x => x.Id == id)
            .ProjectTo<ReviewServiceModel>(mapper)
            .FirstOrDefault();


        public void ChangeVisibility(int id)
        {
            var review = this.data.Reviews.Find(id);

            if (review == null)
            {
                return;
            }

            review.IsPublic = !review.IsPublic;

            this.data.SaveChanges();
        }

        private static string ValidateTitle(ReviewKind rating, string title)
        {
            if (title == null)
            {
                title = rating.ToString();
            }

            return title;
        }

        public bool ReviewIsByUser(int id, string userId)
        => this.data.Reviews.Any(x => x.Id == id && x.UserId == userId);

        public bool ReviewExists(int id)
        => this.data.Reviews.Any(x => x.Id == id);

        public ReviewQueryModel All(
            string searchTerm = null,
            int currentPage = 1,
            int reviewsPerPage = int.MaxValue,
            ReviewKind? reviewFiltering = null,
            bool IsPublicOnly = true,
            string loadId = null)
        {
            var reviewsQuery = this.data.Reviews
                .Where(x => !IsPublicOnly || x.IsPublic)
                .AsQueryable();

            if (!string.IsNullOrEmpty(loadId))
            {

                reviewsQuery = reviewsQuery.Where(x => x.LoadId == loadId);

            }


            if (!string.IsNullOrEmpty(searchTerm))
            {

                reviewsQuery = reviewsQuery
                                         .Where(x =>
                                         x.Content.ToLower().Contains(searchTerm.ToLower()) ||
                                         x.Title.ToLower().Contains(searchTerm.ToLower()) ||
                                         x.Load.Title.ToLower().Contains(searchTerm.ToLower()));

            }



            if (reviewFiltering.HasValue)
            {
                reviewsQuery = reviewFiltering switch
                {
                    ReviewKind.NotRecommended => reviewsQuery = reviewsQuery.Where(x => x.Rating == ReviewKind.NotRecommended),
                    ReviewKind.Weak => reviewsQuery = reviewsQuery.Where(x => x.Rating == ReviewKind.Weak),
                    ReviewKind.Average => reviewsQuery = reviewsQuery.Where(x => x.Rating == ReviewKind.Average),
                    ReviewKind.Good => reviewsQuery = reviewsQuery.Where(x => x.Rating == ReviewKind.Good),
                    ReviewKind.Excellent or _ => reviewsQuery = reviewsQuery.Where(x => x.Rating == ReviewKind.Excellent),

                };

            }


            var totalReviews = reviewsQuery.Count();

            var reviews = reviewsQuery
                  .Skip((currentPage - 1) * reviewsPerPage)
                    .Take(reviewsPerPage)
                    .OrderByDescending(x => x.PublishedOn)
                    .ProjectTo<ReviewServiceModel>(mapper)
                    .ToList();

            return new ReviewQueryModel
            {
                Reviews = reviews,
                CurrentPage = currentPage,
                TotalReviews = totalReviews,
                ReviewsPerPage = reviewsPerPage
            };
        }
    }
}
