using AutoMapper;
using AutoMapper.QueryableExtensions;
using LogisticsSystem.Data;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Services.Responses.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Services.Responses
{
    public class ResponsesService : IResponsesService
    {
        private readonly LogisticsSystemDbContext data;

        private readonly IConfigurationProvider mapper;

        public ResponsesService(LogisticsSystemDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public void Create(
            int questionId,
            string userId,
            string content,
            bool IsPublic = false)
        {

            var response = new Response
            {
                QuestionId = questionId,
                UserId = userId,
                Content = content,
                PublishedOn = DateTime.UtcNow,
                IsPublic = IsPublic
            };

            this.data.Responses.Add(response);

            this.data.SaveChanges();

        }

        public IEnumerable<ResponseServiceModel> ResponsesOfQuestion(int questionId)
        => this.data.Responses.Where(x => x.QuestionId == questionId && x.IsPublic)
            .OrderBy(x => x.PublishedOn)
            .ProjectTo<ResponseServiceModel>(mapper)
            .ToList();

        public void ChangeVisibility(int id)
        {
            var response = this.data.Responses.Find(id);

            if (response == null)
            {
                return;
            }

            response.IsPublic = !response.IsPublic;

            this.data.SaveChanges();
        }

        public bool Delete(int id)
        {
            var response = this.data.Responses.Find(id);

            if (response == null)
            {
                return false;
            }

            this.data.Responses.Remove(response);

            this.data.SaveChanges();

            return true;
        }

        public ResponseQueryModel All(
            string searchTerm = null,
            int currentPage = 1,
            int responsesPerPage = int.MaxValue,
            bool IsPublicOnly = true)
        {
            var responsesQuery = this.data.Responses
                .Where(x => !IsPublicOnly || x.IsPublic)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {

                responsesQuery = responsesQuery
                                         .Where(x =>
                                         x.Content.ToLower().Contains(searchTerm.ToLower()));

            }

            var totalResponses = responsesQuery.Count();

            var responses = responsesQuery
                  .Skip((currentPage - 1) * responsesPerPage)
                    .Take(responsesPerPage)
                    .OrderByDescending(x => x.PublishedOn)
                    .ProjectTo<ResponseServiceModel>(mapper)
                    .ToList();

            return new ResponseQueryModel
            {
                Responses = responses,
                CurrentPage = currentPage,
                TotalResponses = totalResponses,
                ResponsesPerPage = responsesPerPage
            };
        }
    }
}
