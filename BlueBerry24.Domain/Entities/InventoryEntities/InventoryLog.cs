using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBerry24.Domain.Entities.InventoryEntities
{
    public class InventoryLog : IAuditableEntity
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int CurrentStockQuantity { get; set; }

        public int QuantityChanged { get; set; }

        public InventoryChangeType ChangeType { get; set; }

        public int ReferenceId { get; set; }
        public string ReferenceType { get; set; }

        public int? PerformedByUserId { get; set; }

        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
