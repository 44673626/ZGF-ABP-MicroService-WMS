using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;
using Win.Sfs.Shared.Enums;

namespace Win.Sfs.Shared.DomainBase
{
    public class InventoryInfo : ValueObject,IItem<Guid>
    {
      
        public Guid Id { get; set; }
        public Guid LabelId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemCode { get; set; }
        public string Lot { get; set; }
        public string Serial { get; set; }
        public EnumInventoryStatus InventoryStatus { get; set; }
        public string BasicUomCode { get; set; }
        public decimal Qty { get; set; }


        public InventoryInfo( Guid labelId, Guid itemId, string itemCode, string lot, string serial,
            string basicUomCode, decimal qty, EnumInventoryStatus inventoryStatus = EnumInventoryStatus.OK)
        {
            LabelId = labelId;
            ItemId = itemId;
            ItemCode = itemCode;
            Lot = lot;
            Serial = serial;
            InventoryStatus = inventoryStatus;
            BasicUomCode = basicUomCode;
            Qty = qty;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return LabelId;
            yield return ItemId;
            yield return Lot;
            yield return Serial;
            yield return InventoryStatus;
            yield return BasicUomCode;
            yield return Qty;
        }

    }
}