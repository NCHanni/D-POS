using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.GL;
using PX.Objects.IN;
using RetailDimension.DAC.Base;
using RetailDimension.Graph;
using static RetailDimension.Attributes.Base.AggregateAttribute;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("RD-Sale-Detail")]
    [PXPrimaryGraph(typeof(ATRDSaleEntry))]
    public class ATRDSaleDetail : Table, IBqlTable
    {
        #region Id
        [PXDBLongIdentity(IsKey = true)]
        public virtual long? Id { get; set; }
        public abstract class id : BqlLong.Field<id> { }
        #endregion

        #region ItemLineID
        //item_line_id int
        [PXDBInt]
        [PXUIField(DisplayName = ATRDMessages.ItemLineID)]
        public virtual int? ItemLineID { get; set; }
        public abstract class itemLineID : BqlInt.Field<itemLineID> { }
        #endregion

        #region SaleCode
        //sale_code varchar(12) 
        [PXDBString(12, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.SaleCode)]
        [PXDBDefault(typeof(ATRDSale.code))]
        [PXParent(typeof(Select<ATRDSale, Where<ATRDSale.code, Equal<Current<ATRDSaleDetail.saleCode>>>>))]
        public virtual string SaleCode { get; set; }
        public abstract class saleCode : BqlString.Field<saleCode> { }
        #endregion

        #region BranchID
        [Branch(DisplayName = ATRDMessages.Branch)]
        [PXUIField(Enabled = false)]
        [PXDBDefault(typeof(ATRDSale.branchID))]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion

        #region ItemCode
        //item_code varchar(12)
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.ItemCode)]
        public virtual string ItemCode { get; set; }
        public abstract class itemCode : BqlString.Field<itemCode> { }
        #endregion

        #region Description
        //description varchar(255)
        [PXDBString(255, IsUnicode = true)]
        public virtual string Description { get; set; }
        public abstract class description : BqlString.Field<description> { }
        #endregion

        #region ClassCode
        //class_code varchar(6)
        [PXDBString(6, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.ClassCode)]
        public virtual string ClassCode { get; set; }
        public abstract class classCode : BqlString.Field<classCode> { }
        #endregion

        #region Price
        //price numeric(18, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.Price)]
        public virtual decimal? Price { get; set; }
        public abstract class price : BqlDecimal.Field<price> { }
        #endregion 

        #region Qty
        //qty numeric(18, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.Qty)]
        public virtual decimal? Qty { get; set; }
        public abstract class qty : BqlDecimal.Field<qty> { }
        #endregion 

        #region QtyReturned
        //qty_returned numeric(18, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.QtyReturned)]
        public virtual decimal? QtyReturned { get; set; }
        public abstract class qtyReturned : BqlDecimal.Field<qtyReturned> { }
        #endregion

        #region UnitOfMeasure
        //unit_of_measure varchar(10)
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.UnitOfMeasure)]
        public virtual string UnitOfMeasure { get; set; }
        public abstract class unitOfMeasure : BqlString.Field<unitOfMeasure> { }
        #endregion

        #region QtyPerUom
        //qty_per_uom numeric(10, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.QtyPerUom)]
        public virtual decimal? QtyPerUom { get; set; }
        public abstract class qtyPerUom : BqlDecimal.Field<qtyPerUom> { }
        #endregion

        #region IsRegularDiscount
        //is_regular_discount varchar(5)
        [ATRDBoolean]
        [PXUIField(DisplayName = ATRDMessages.IsRegularDiscount)]
        public virtual Boolean? IsRegularDiscount { get; set; }
        public abstract class isRegularDiscount : BqlBool.Field<isRegularDiscount> { }
        #endregion

        #region DiscountPercent
        //discount_percent numeric(5, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.DiscountPercent)]
        public virtual decimal? DiscountPercent { get; set;}
        public abstract class discountPercent : BqlDecimal.Field<discountPercent> { }
        #endregion

        #region DiscountAmount
        //discount_amount numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.DiscountAmount)]
        public virtual decimal? DiscountAmount { get; set; }
        public abstract class discountAmount : BqlDecimal.Field<discountAmount> { }
        #endregion

        #region DiscountedPrice
        //discounted_price numeric(18, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.DiscountedPrice)]
        public virtual decimal? DiscountedPrice { get; set; }
        public abstract class discountedPrice : BqlDecimal.Field<discountedPrice> { }
        #endregion

        #region VatPercent
        //vat_percent numeric(5, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.VatPercent)]
        public virtual decimal? VatPercent {get; set;}
        public abstract class vatPercent : BqlDecimal.Field<vatPercent> { }
        #endregion

        #region VatAmount
        //vat_amount numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.VatAmount)]
        public virtual decimal? VatAmount { get; set; }
        public abstract class vatAmount : BqlDecimal.Field<vatAmount> { }
        #endregion

        #region VatExemptAmount
        //vat_exempt_amount numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.VatExemptAmount)]
        public virtual decimal? VatExemptAmount { get; set; }
        public abstract class vatExemptAmount : BqlDecimal.Field<vatExemptAmount> { }
        #endregion

        #region Amount
        //amount numeric(18, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.Amount)]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : BqlDecimal.Field<amount> { }
        #endregion

        #region IsVatable
        //is_vatable varchar(5)
        [ATRDBoolean]
        [PXUIField(DisplayName = ATRDMessages.IsVatable)]
        public virtual Boolean? IsVatable { get; set; }
        public abstract class isVatable : BqlBool.Field<isVatable> { }
        #endregion

        #region IsZeroRated
        //is_zero_rated varchar(5)  
        [ATRDBoolean]
        [PXUIField(DisplayName = ATRDMessages.IsZeroRated)]
        public virtual Boolean? IsZeroRated { get; set; }
        public abstract class isZeroRated : BqlBool.Field<isZeroRated> { }
        #endregion

        #region IsVatExempt
        //is_vat_exempt varchar(5)  
        [ATRDBoolean]
        [PXUIField(DisplayName = ATRDMessages.IsVatExempt)]
        public virtual Boolean? IsVatExempt { get; set; }
        public abstract class isVatExempt : BqlBool.Field<isVatExempt> { }
        #endregion

        #region IsGiftCertificate
        //is_gift_certificate varchar(5)  
        [ATRDBoolean]
        [PXUIField(DisplayName = ATRDMessages.IsGiftCertificate)]
        public virtual Boolean? IsGiftCertificate { get; set; }
        public abstract class isGiftCertificate : BqlBool.Field<isGiftCertificate> { }
        #endregion

        #region SerialLotNo
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.SerialLotNo)]
        public virtual string SerialLotNo { get; set; }
        public abstract class serialLotNo : BqlString.Field<serialLotNo> { }
        #endregion

        #region AlternateID
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.AlternateID)]
        public virtual String AlternateID { get; set; }
        public abstract class alternateID : PX.Data.BQL.BqlString.Field<alternateID> { }
        #endregion

        #region Unbound

        #region InventoryID
        [PXInt]
        [PXUnboundDefault(typeof(Search<InventoryItem.inventoryID, Where<InventoryItem.inventoryCD, Equal<Current<itemCode>>>>))]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : BqlInt.Field<inventoryID> { }
        #endregion

        #endregion
    }
}
