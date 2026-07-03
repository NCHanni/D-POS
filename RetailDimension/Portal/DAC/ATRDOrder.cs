using PX.Data;
using PX.Data.BQL;
using PX.Objects.IN;
using RetailDimension.DAC.Extension;
using System;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("Order")]
    public class ATRDOrder : Base.Audit, IBqlTable
    {
        [PXDBLongIdentity(IsKey = true)]
        public virtual long? ID { get; set; }
        public abstract class iD : BqlLong.Field<iD> { }

        [PXDBInt]
        [PXSelector(typeof(Search<InventoryItem.inventoryID,
                            Where<ATRDInventoryItem.usrATRDIsPOS, Equal<True>>>),
                    SubstituteKey = typeof(InventoryItem.inventoryCD))]
        [PXUIField(DisplayName = ATRDMessages.InventoryID)]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : BqlInt.Field<inventoryID> { }

        [PXDBQuantity(MinValue = 0)]
        [PXUIField(DisplayName = ATRDMessages.Qty)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? Qty { get; set; }
        public abstract class qty : BqlDecimal.Field<qty> { }

        [PXDBString(15, IsUnicode = true)]
        public virtual string OrderNbr { get; set; }
        public abstract class orderNbr : BqlString.Field<orderNbr> { }

        [PXDBInt]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID : BqlInt.Field<customerID> { }

        #region Unbound
        #region Description
        [PXString(60, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Description, Enabled = false)]
        [PXUnboundDefault(typeof(Selector<inventoryID, InventoryItem.descr>))]
        [PXFormula(typeof(Default<inventoryID>))]
        public virtual string Description { get; set; }
        public abstract class description : BqlString.Field<description> { }
        #endregion

        #region Price
        [PXDecimal(2)]
        [PXUIField(DisplayName = ATRDMessages.Price, Enabled = false)]
        [PXUnboundDefault(typeof(Selector<inventoryID, InventoryItemCurySettings.basePrice>))]
        [PXFormula(typeof(Default<inventoryID>))]
        public virtual decimal? Price { get; set; }
        public abstract class price : BqlDecimal.Field<price> { }
        #endregion

        #region UOM
        [PXString(10)]
        [PXUIField(DisplayName = ATRDMessages.UnitOfMeasure, Enabled = false)]
        [PXUnboundDefault(typeof(Selector<inventoryID, InventoryItem.baseUnit>))]
        [PXFormula(typeof(Default<inventoryID>))]
        public virtual string Uom { get; set; }
        public abstract class uom : BqlString.Field<uom> { }
        #endregion

        #region Total
        [PXDecimal(2)]
        [PXUIField(DisplayName = ATRDMessages.Total, Enabled = false)]
        [PXFormula(typeof(Mult<qty, price>))]
        [PXFormula(typeof(Default<qty, price>))]
        public virtual decimal? Total { get; set; }
        public abstract class total : BqlDecimal.Field<total> { }
        #endregion
        #endregion
    }
}
