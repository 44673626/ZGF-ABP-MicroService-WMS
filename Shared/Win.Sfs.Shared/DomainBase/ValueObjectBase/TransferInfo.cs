using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;
using Win.Sfs.Shared.Enums;

namespace Win.Sfs.Shared.DomainBase
{
    public class TransferInfo : ValueObject
    {
        public EnumInventoryStatus InventoryStatus { get; set; }
        public decimal Qty { get; set; }
        public Guid BranchId { get; set; }
        public string BranchCode { get; set; }
        public string WhseCode { get; set; }
        public string AreaCode { get; set; }
        public string SlgCode { get; set; }
        public Guid LocId { get; set; }
        public string LocCode { get; set; }
        public Guid? EqptId { get; set; }
        public string EqptCode { get; set; }

        public string Lot { get; set; }
        public string Serial { get; set; }

        protected TransferInfo() { }

        public TransferInfo(Guid branchId, string locCode,string lot,string serial, decimal qty = 0,
            EnumInventoryStatus inventoryStatus = EnumInventoryStatus.OK, Guid? eqptId = null)
        {
            InventoryStatus = inventoryStatus;
            Qty = qty;
            BranchId = branchId;
            LocCode = locCode;
            EqptId = eqptId;
            Lot = lot;
            Serial = serial;
        }


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return InventoryStatus;
            yield return Qty;
            yield return BranchId;
            yield return LocId;
            yield return EqptId;
            yield return Lot;
            yield return Serial;
        }
    }
}