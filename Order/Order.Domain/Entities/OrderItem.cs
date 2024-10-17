using System;
using System.Collections.Generic;

namespace Order.Domain.Entities
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Orderid { get; set; }
        public int? Quantity { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
