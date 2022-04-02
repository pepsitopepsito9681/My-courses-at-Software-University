using AutoMapper;
using AutoMapper.QueryableExtensions;
using LogisticsSystem.Data;
using LogisticsSystem.Data.Models;
using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Services.Loads.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Services.Loads
{
    public class LoadsService:ILoadsService
    {
        private readonly LogisticsSystemDbContext data;
        private readonly IConfigurationProvider mapper;



        public LoadsService(LogisticsSystemDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public LoadQueryServiceModel All(
            string kind = null,
            string subKind = null,
            string searchTerm = null,
            int currentPage = 1,
            int loadsPerPage = int.MaxValue,
            LoadSorting loadSorting = LoadSorting.Newest,
            bool IsPublicOnly = true,
            bool IsDeleted = false
            )
        {


            var loadsQuery = this.data.Loads
               .Where(x => (!IsPublicOnly || x.IsPublic) && x.IsDeleted == IsDeleted)
               .AsQueryable();

            ;

            if (!string.IsNullOrEmpty(kind))
            {
                if (!string.IsNullOrEmpty(subKind) &&
                    this.SubKindParticipateInKind(subKind, kind))
                {

                    loadsQuery = loadsQuery
                   .Where(x => x.Kind.Name.Contains(kind) && x.SubKind.Name.Contains(subKind));

                }
                else
                {

                    loadsQuery = loadsQuery
                    .Where(x => x.Kind.Name.Contains(kind));

                }
            }



            if (!string.IsNullOrEmpty(searchTerm))
            {

                loadsQuery = loadsQuery



                    .Where(x => x.SubKind.Name.ToLower().Contains(searchTerm.ToLower())
                    || x.Kind.Name.ToLower().Contains(searchTerm.ToLower())
                    || x.Title.ToLower().Contains(searchTerm.ToLower())
                    || (x.Kind.Name + " " + x.SubKind.Name).ToLower().Contains(searchTerm.ToLower())
                    || x.Description.ToLower().Contains(searchTerm.ToLower()));

            }

            loadsQuery = loadSorting switch
            {
                LoadSorting.NameAlphabetically => loadsQuery = loadsQuery.OrderBy(x => x.Title),
                LoadSorting.NameDescending => loadsQuery = loadsQuery.OrderByDescending(x => x.Title),
                LoadSorting.PriceAscending => loadsQuery = loadsQuery.OrderBy(x => x.Price),
                LoadSorting.PriceDescending => loadsQuery = loadsQuery.OrderByDescending(x => x.Price),
                LoadSorting.Newest or _ => loadsQuery.OrderByDescending(x => x.PublishedOn)

            };

            var totalLoads = loadsQuery.Count();

            var loads = GetLoads(
                 loadsQuery
                  .Skip((currentPage - 1) * loadsPerPage)
                    .Take(loadsPerPage)
                );





            return new LoadQueryServiceModel
            {
                Loads = loads,
                CurrentPage = currentPage,
                LoadsPerPage = loadsPerPage,
                TotalLoads = totalLoads

            };
        }

        public IEnumerable<LoadServiceModel> Latest()
       => this.data.Loads
              .Where(x => x.IsPublic)
              .OrderByDescending(x => x.PublishedOn)
              .ProjectTo<LoadServiceModel>(this.mapper)
              .Take(5)
              .ToList();

        public string Create(string title,
                string description,
                string firstImageUrl,
                string secondImageUrl,
                string thirdImageUrl,
                decimal price,
                byte quantity,
                int kindId,
                int subKindId,
                LoadCondition loadCondition,
                PersonType personType,
                string traderId,
                bool IsPublic = false
            )
        {
            var loadData = new Load()
            {
                Title = title,
                Description = description,
                Price = price,
                Quantity = quantity,
                KindId = kindId,
                SubKindId = subKindId,
                LoadCondition = loadCondition,
                PersonType = personType,
                PublishedOn = DateTime.UtcNow,
                TraderId = traderId,
                IsPublic = IsPublic,
                IsDeleted = false

            };

            foreach (var imageUrl in new List<string>() { firstImageUrl, secondImageUrl, thirdImageUrl })
            {
                if (imageUrl != null)
                {
                    loadData.Images.Add(new LoadImage()
                    {
                        LoadId = loadData.Id,
                        ImageUrl = imageUrl
                    });
                }

            }

            data.Loads.Add(loadData);

            data.SaveChanges();

            return loadData.Id;

        }

        public bool Edit(
          string Id,
          string title,
          string description,
          string firstImageUrl,
          string secondImageUrl,
          string thirdImageUrl,
          decimal price,
          byte quantity,
          int kindId,
          int subKindId,
          LoadCondition loadCondition,
          PersonType personType,
          string dealerId,
          bool IsPublic = false
          )
        {
            var loadData = this.data.Loads.Include(x => x.Images).FirstOrDefault(x => x.Id == Id);

            if (loadData == null)
            {
                return false;
            }

            if (loadData.IsDeleted)
            {
                IsPublic = false;
            }

            loadData.Title = title;
            loadData.Description = description;
            loadData.Price = price;
            loadData.Quantity = quantity;
            loadData.LoadCondition = loadCondition;
            loadData.PersonType = personType;
            loadData.KindId = kindId;
            loadData.SubKindId = subKindId;
            loadData.IsPublic = IsPublic;

            var mainImage = loadData.Images.FirstOrDefault();

            if (mainImage.ImageUrl != firstImageUrl)
            {
                mainImage.ImageUrl = firstImageUrl;
            }

            if (secondImageUrl != null)
            {
                var secondImage = loadData.Images.Skip(1).Take(1).FirstOrDefault();

                if (secondImage == null)
                {
                    loadData.Images.Add(new LoadImage() { LoadId = loadData.Id, ImageUrl = secondImageUrl });
                }
                else if (secondImage.ImageUrl != secondImageUrl)
                {
                    secondImage.ImageUrl = secondImageUrl;
                }
            }
            if (thirdImageUrl != null)
            {
                var thirdImage = loadData.Images.Skip(2).Take(1).FirstOrDefault();

                if (thirdImage == null)
                {
                    loadData.Images.Add(new LoadImage() { LoadId = loadData.Id, ImageUrl = thirdImageUrl });
                }
                else if (thirdImage.ImageUrl != thirdImageUrl)
                {
                    thirdImage.ImageUrl = thirdImageUrl;
                }
            }

            data.SaveChanges();

            return true;
        }

        public IEnumerable<LoadServiceModel> ByUser(string userId)
        => GetLoads(
            this.data
                    .Loads
                      .Where(x => x.Trader.UserId == userId && !x.IsDeleted)
            );

        public IEnumerable<LoadServiceModel> GetSimilarLoads(string Id)
        {
            var load = Details(Id);

            if (load == null)
            {
                return null;
            }

            var loadFirstPartOfTitle = load.Title.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0];

            return GetLoads(
                 this.data
                 .Loads
                 .Where(x => x.Kind.Name == load.KindName &&
                 x.SubKind.Name == load.SubKindName &&
                 x.Title.Contains(loadFirstPartOfTitle) && x.Title != load.Title)
                 .Take(5));
        }



        private IEnumerable<LoadServiceModel> GetLoads(IQueryable<Load> loadsQuery)
         => loadsQuery
            .ProjectTo<LoadServiceModel>(this.mapper)
              .ToList();

        public IEnumerable<LoadKindServiceModel> AllKinds()
        => this.data
            .Kinds
              .ProjectTo<LoadKindServiceModel>(this.mapper)
                .ToList();

        public bool SubKindExists(int subKindId, int kindId)
        => this.data.SubKinds.Any(x => x.Id == subKindId && x.KindId == kindId);

        public bool SubKindParticipateInKind(string subKind, string kind)
         => this.data.SubKinds.Any(x => x.Name == subKind && x.Kind.Name == kind);

        public bool SubKindIsValid(string subKind, string kind)
        {
            if (!string.IsNullOrEmpty(kind) && !string.IsNullOrEmpty(subKind))
            {
                if (!this.SubKindParticipateInKind(subKind, kind))
                {
                    return false;
                }

            }
            else if (!string.IsNullOrEmpty(subKind))
            {
                return false;
            }

            return true;

        }


        public bool KindExists(int kindId)
        => this.data.Kinds.Any(x => x.Id == kindId);

        public LoadDetailsServiceModel Details(string Id)
        => this.data.Loads
            .Where(x => x.Id == Id)
            .ProjectTo<LoadDetailsServiceModel>(this.mapper)
            .FirstOrDefault();

        public bool LoadIsByTrader(string id, string traderId)
        => this.data.Loads
            .Any(x => x.Id == id && x.TraderId == traderId);

        public IEnumerable<LoadSubKindServiceModel> AllSubKinds()
        => this.data
            .SubKinds
             .ProjectTo<LoadSubKindServiceModel>(this.mapper)
                .ToList();

        public void ChangeVisibility(string id)
        {
            var load = this.data.Loads.Find(id);

            load.IsPublic = !load.IsPublic;

            this.data.SaveChanges();
        }

        public bool LoadExists(string Id)
        => this.data.Loads.Any(x => x.Id == Id);

        public bool DeleteLoad(
            string id,
            bool IsAdmin = false)
        {
            var load = this.data.Loads.Find(id);

            if (load == null)
            {
                return false;
            }

            if (IsAdmin)
            {
                this.data.Loads.Remove(load);
            }
            else
            {
                load.IsDeleted = true;
                load.IsPublic = false;
                load.DeletedOn = DateTime.UtcNow;

            }

            this.data.SaveChanges();

            return true;
        }


        public bool ReviveLoad(string id)
        {
            var load = this.data.Loads.Find(id);

            if (load == null)
            {
                return false;
            }

            load.IsDeleted = false;
            load.DeletedOn = null;
            load.PublishedOn = DateTime.UtcNow;

            this.data.SaveChanges();

            return true;

        }

        public bool IsLoadPublic(string id)
        => this.data.Loads.Any(x => x.Id == id && x.IsPublic);
    }
}
