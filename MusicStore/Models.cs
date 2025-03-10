using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore
{
    class Models
    {
        public class Sale
        {
            public int Id { get; set; }
            public int RecordId { get; set; }
            public DateTime SaleDate { get; set; }
            public decimal Price { get; set; }
            public string CustomerName { get; set; }
        }

        public class StockOperation
        {
            public int Id { get; set; }
            public int RecordId { get; set; }
            public DateTime OperationDate { get; set; }
            public string OperationType { get; set; }
            public int Quantity { get; set; }
            public string Reason { get; set; }
        }

        public class Promotion
        {
            public int Id { get; set; }
            public int RecordId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal DiscountPercentage { get; set; }
            public string Description { get; set; }
        }

        public class ReservedRecord
        {
            public int Id { get; set; }
            public int RecordId { get; set; }
            public DateTime ReserveDate { get; set; }
            public DateTime ExpireDate { get; set; }
            public string CustomerName { get; set; }
            public bool IsConfirmed { get; set; }
        }

    }
}
