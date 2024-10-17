using System;
using System.Collections.Generic;

namespace Product.Domain.Entities
{
    public partial class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? PictureUrl { get; set; }
        public int? Quantity { get; set; }
    }
}
