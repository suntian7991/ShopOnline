using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

    }
}
