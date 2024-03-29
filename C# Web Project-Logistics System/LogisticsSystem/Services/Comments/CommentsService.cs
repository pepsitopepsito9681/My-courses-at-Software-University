﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using LogisticsSystem.Data;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Comments.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Services.Comments
{
    public class CommentsService:ICommentsService
    {
        private readonly LogisticsSystemDbContext data;

        private readonly IConfigurationProvider mapper;

        public CommentsService(LogisticsSystemDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<CommentServiceModel> CommentsOfReview(int reviewId)
         => this.data.Comments.Where(x => x.ReviewId == reviewId && x.IsPublic)
            .OrderBy(x => x.PublishedOn)
            .ProjectTo<CommentServiceModel>(mapper)
            .ToList();

        public void Create(
            int reviewId,
            string userId,
            string content,
            bool IsPublic = false)
        {
            var comment = new Comment
            {
                ReviewId = reviewId,
                UserId = userId,
                Content = content,
                PublishedOn = DateTime.UtcNow,
                IsPublic = IsPublic
            };

            this.data.Comments.Add(comment);

            this.data.SaveChanges();
        }

        public void ChangeVisibility(int id)
        {
            var comment = this.data.Comments.Find(id);

            if (comment == null)
            {
                return;
            }

            comment.IsPublic = !comment.IsPublic;

            this.data.SaveChanges();
        }

        public bool Delete(int id)
        {
            var comment = this.data.Comments.Find(id);

            if (comment == null)
            {
                return false;
            }

            this.data.Comments.Remove(comment);

            this.data.SaveChanges();

            return true;
        }

        public CommentQueryModel All(
            string searchTerm = null,
            int currentPage = 1,
            int commentsPerPage = int.MaxValue,
            bool IsPublicOnly = true)
        {
            var commentsQuery = this.data.Comments
                .Where(x => !IsPublicOnly || x.IsPublic)
                .AsQueryable();


            if (!string.IsNullOrEmpty(searchTerm))
            {

                commentsQuery = commentsQuery
                                         .Where(x =>
                                         x.Content.ToLower().Contains(searchTerm.ToLower()));

            }


            var totalComments = commentsQuery.Count();

            var comments = commentsQuery
                  .Skip((currentPage - 1) * commentsPerPage)
                    .Take(commentsPerPage)
                    .OrderByDescending(x => x.PublishedOn)
                    .ProjectTo<CommentServiceModel>(mapper)
                    .ToList();

            return new CommentQueryModel
            {
                Comments = comments,
                CurrentPage = currentPage,
                TotalComments = totalComments,
                CommentsPerPage = commentsPerPage
            };
        }
    }
}
   
