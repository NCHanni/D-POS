using System;
using PX.Data;
using PX.Data.BQL;
using RetailDimension.DAC;

namespace RetailDimension.DAC.Projection
{
    [Serializable]
    [PXCacheName("RD-BatchSalePaymentDetail")]
    [PXProjection(typeof(Select<ATRDSale>))]
#if Version23R1
    public partial class ATRDBatchSalePaymentDetail : IBqlTable
#endif
#if Version25R1 || Version25R2
    public partial class ATRDBatchSalePaymentDetail : PXBqlTable, IBqlTable
#endif
    {
        #region BatchRefNbr 
        [PXDBString(15, IsUnicode = true, BqlField = typeof(ATRDSale.batchRefNbr))]
        [PXUIField(DisplayName = ATRDMessages.BatchRefNbr)]
        [PXSelector(typeof(Search4<ATRDSale.batchRefNbr, Aggregate<GroupBy<ATRDSale.batchRefNbr>>>))]
        public virtual string BatchRefNbr { get; set; }
        public abstract class batchRefNbr : BqlString.Field<batchRefNbr> { }
        #endregion

        #region Code 
        [PXDBString(12, IsKey = true, IsUnicode = true, BqlField = typeof(ATRDSale.batchRefNbr))]
        [PXUIField(DisplayName = ATRDMessages.Code)]
        public virtual string Code { get; set; }
        public abstract class code : BqlString.Field<code> { }
        #endregion

        #region CashAmount
        [PXDecimal(2)]
        [PXUIField(DisplayName = ATRDMessages.Cash)]
        [PXUnboundDefault(typeof(Search4<ATRDSaleCashDetail.paidAmount,
            Where<ATRDSaleCashDetail.saleCode, Equal<Current<code>>>,
            Aggregate<GroupBy<ATRDSaleCashDetail.saleCode, Sum<ATRDSaleCashDetail.paidAmount>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CashAmount { get; set; }
        public abstract class cashAmount : BqlDecimal.Field<cashAmount> { }
        #endregion

        #region CreditCardAmount
        [PXDecimal(2)]
        [PXUIField(DisplayName = ATRDMessages.CreditCard)]
        [PXUnboundDefault(typeof(Search4<ATRDSaleCardDetail.amount, 
            Where<ATRDSaleCardDetail.saleCode, Equal<Current<code>>>,
            Aggregate<GroupBy<ATRDSaleCardDetail.saleCode, Sum<ATRDSaleCardDetail.amount>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? CreditCardAmount { get; set; }
        public abstract class creditCardAmount : BqlDecimal.Field<creditCardAmount> { }
        #endregion

        #region DiscountAmount
        [PXDecimal(2)]
        [PXUIField(DisplayName = ATRDMessages.Discount)]
        [PXUnboundDefault(typeof(Search4<ATRDSaleDiscountDetail.totalDiscount,
            Where<ATRDSaleDiscountDetail.saleCode, Equal<Current<code>>>,
            Aggregate<GroupBy<ATRDSaleDiscountDetail.saleCode, Sum<ATRDSaleDiscountDetail.totalDiscount>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? DiscountAmount { get; set; }
        public abstract class discountAmount : BqlDecimal.Field<discountAmount> { }
        #endregion
    }
}
