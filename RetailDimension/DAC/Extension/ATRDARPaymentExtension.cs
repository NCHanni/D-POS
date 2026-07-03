using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.CS;
using RetailDimension.Attributes;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDARPaymentExtension : PXCacheExtension<ARPayment>
    {

        public static bool IsActive() { return true; }

        #region BoundFields
        #region UsrReceiptType
        [PXDBString(1, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.ReceiptType, Required = true)]
        [PXDefault(typeof(ATRDReceiptTypeAttribute.collection), PersistingCheck = PXPersistingCheck.Nothing)]
        [ATRDReceiptType]
        [PXUIEnabled(typeof(Where<ARPayment.docType, Equal<ARDocType.payment>, Or<ARPayment.docType, Equal<ARDocType.prepayment>>>))]
        [PXUIVisible(typeof(Where<ARPayment.docType, Equal<ARDocType.payment>, Or<ARPayment.docType, Equal<ARDocType.prepayment>>>))]
        public string UsrReceiptType { get; set; }
        public abstract class usrReceiptType : BqlString.Field<usrReceiptType> { }

        #region UsrReceiptNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.ReceiptNbr)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(usrReceiptNbr))]
        [PXUIEnabled(typeof(Where<ARPayment.docType, Equal<ARDocType.payment>, Or<ARPayment.docType, Equal<ARDocType.prepayment>>>))]
        [PXUIVisible(typeof(Where2<Where<ARPayment.docType, Equal<ARDocType.payment>, Or<ARPayment.docType, Equal<ARDocType.prepayment>>>,
            And<usrReceiptType, NotEqual<ATRDReceiptTypeAttribute.blank>>>))]
        [ARReceiptTypeAutonumber(typeof(ATRDARSetup.usrOfficialReceiptNumberingSequence), typeof(AccessInfo.businessDate))]
        public string UsrReceiptNbr { get; set; }
        public abstract class usrReceiptNbr : BqlString.Field<usrReceiptNbr> { }
        #endregion
        #endregion

        #endregion

        #region Unbound Fields
        #region UsrARPaymentTotalAmount
        public abstract class usrARPaymentTotalAmount : BqlString.Field<usrARPaymentTotalAmount> { }
        [PXString]
        [ToWords(typeof(ARPayment.curyOrigDocAmt))]
        public string UsrARPaymentTotalAmount { get; set; }
        #endregion

        #region UsrARPaymentRefNbr
        public abstract class usrARPaymentRefNbr : BqlString.Field<usrARPaymentRefNbr> { }
        [PXString]
        [PXSelector(typeof(Search<ARPayment.refNbr>), DescriptionField = typeof(ARPayment.refNbr))]
        public string UsrARPaymentRefNbr { get; set; }
        #endregion
        #endregion

    }
}
