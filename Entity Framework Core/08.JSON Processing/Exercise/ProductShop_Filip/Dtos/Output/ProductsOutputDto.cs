using System.Collections.Generic;

namespace ProductShop.Dtos.Output
{
    public class ProductsOutputDto
    {
        public int Count { get; set; }

        public List<ProductOutputDto> Products { get; set; }
    }
}
