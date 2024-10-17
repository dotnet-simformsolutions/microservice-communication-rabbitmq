using Order.Domain.Entities;

namespace Order.API.Dtos
{
    public class OrderDto
    {
        public string? UserEmail { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? ShipToAddress { get; set; }
        public int? DeliveryMethod { get; set; }
    }
}
