using LogisticsSystem.Data.Models.Enums;
using LogisticsSystem.Services.Loads.Models;
using System.Collections.Generic;

namespace Logistics_System.Services.Loads
{
    public interface ILoadsService
    {
        LoadQueryServiceModel All(
            string kind = null,
            string subKind = null,
            string searchTerm = null,
            int currentPage = 1,
            int loadsPerPage = int.MaxValue,
            LoadSorting loadSorting = LoadSorting.Newest,
            bool IsPublicOnly = true,
            bool IsDeleted = false);

        IEnumerable<LoadServiceModel> Latest();

        string Create(string title,
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
                );

        bool Edit(
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
               );

        LoadDetailsServiceModel Details(string Id);

        IEnumerable<LoadServiceModel> ByUser(string userId);

        IEnumerable<LoadServiceModel> GetSimilarLoads(string Id);

        bool LoadIsByTrader(string id, string traderId);

        IEnumerable<LoadKindServiceModel> AllKinds();

        IEnumerable<LoadSubKindServiceModel> AllSubKinds();

        bool IsLoadPublic(string id);

        bool LoadExists(string Id);

        bool KindExists(int kindId);

        bool SubKindExists(int subKindId, int kindId);

        bool SubKindParticipateInKind(string subKind, string kind);

        bool SubKindIsValid(string subKind, string kind);

        bool DeleteLoad(
            string id,
            bool IsAdmin = false);

        bool ReviveLoad(string id);

        void ChangeVisibility(string id);
    }
}
