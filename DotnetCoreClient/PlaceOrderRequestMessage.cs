using System;
using System.Collections.Generic;

namespace DotnetCoreClient
{
    public sealed class PlaceOrderRequestMessage
    {
        public int CustomerName { get; set; }
        public string CartName { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public sealed class OrderItemDTO
    {
        public int Id { get; set; }
        public int ItemQty { get; set; }
        public decimal Total { get; set; }
    }
}
