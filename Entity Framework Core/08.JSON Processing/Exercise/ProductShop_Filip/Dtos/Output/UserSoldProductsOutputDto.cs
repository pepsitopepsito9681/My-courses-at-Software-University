namespace ProductShop.Dtos.Output
{
    using System.Collections.Generic;

    public class UserSoldProductsOutputDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<SoldProdcutOutputDto> SoldProducts { get; set; }
    }
}
