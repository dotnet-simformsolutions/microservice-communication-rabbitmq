using System;
using System.Collections.Generic;

namespace Product.Domain.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? ShipToAddress { get; set; }
        public int? DeliveryMethod { get; set; }
        public string? UserEmail { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
