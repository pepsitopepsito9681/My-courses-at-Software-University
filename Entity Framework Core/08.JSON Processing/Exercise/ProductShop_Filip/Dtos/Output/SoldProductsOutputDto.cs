using System.Collections.Generic;

namespace ProductShop.Dtos.Output
{
    public class SoldProductsOutputDto
    {
        public int Count { get; set; }

        public IEnumerable<ProductOutputDto> Products { get; set; }
    }
}
