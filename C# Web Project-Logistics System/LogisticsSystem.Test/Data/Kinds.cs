using LogisticsSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsSystem.Test.Data
{
    public static class Kinds
    {
        public static IEnumerable<Kind> GetKinds(int count = 5)
            => Enumerable.Range(0, count).Select(i => new Kind()
            {
                SubKinds = GetSubKinds(1).ToList()

            });

        public static IEnumerable<SubKind> GetSubKinds(int count = 5)
           => Enumerable.Range(0, count).Select(i => new SubKind()
           {

           });
    }
}
