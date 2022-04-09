using LogisticsSystem.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace LogisticsSystem.Test.Routing
{
    

    public class HomeRoutingTest
    {

        [Fact]
        public void IndexRouteShouldBeMapped()
            => MyRouting
                 .Configuration()
                  .ShouldMap("/")
                   .To<HomeController>(c => c.Index());

        [Fact]
        public void ErrorRouteShouldBeMapped()
          => MyRouting
               .Configuration()
                .ShouldMap("/Home/Error")
                 .To<HomeController>(c => c.Error());
    }
}
