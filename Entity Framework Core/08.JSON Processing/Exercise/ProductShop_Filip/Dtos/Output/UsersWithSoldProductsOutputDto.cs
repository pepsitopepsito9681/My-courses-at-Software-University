using System.Collections.Generic;

namespace ProductShop.Dtos.Output
{
    public class UsersWithSoldProductsOutputDto
    {
        public int UsersCount { get; set; }


        public IEnumerable<UserProductsOutputDto> Users { get; set; }
    }
}
