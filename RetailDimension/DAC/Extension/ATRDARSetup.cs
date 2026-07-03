using PX.Data;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.CA;
using PX.Objects.CS;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDARSetup : PXCacheExtension<ARSetup>
    {
        public static bool IsActive() { return true; }

        #region Bound Fields
        #region UsrAcknowledgementReceiptNumberingSequence
        [PXDBString(10, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = ATRDMessages.AcknowledgementReceiptNumberingSequence, Visibility = PXUIVisibility.Visible)]
        public string UsrAcknowledgementReceiptNumberingSequence { get; set; }
        public abstract class usrAcknowledgementReceiptNumberingSequence : PX.Data.BQL.BqlString.Field<usrAcknowledgementReceiptNumberingSequence> { }
        #endregion        

        #region UsrCollectionReceiptNumberingSequence
        [PXDBString(10, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = ATRDMessages.CollectionReceiptNumberingSequence, Visibility = PXUIVisibility.Visible)]
        public string UsrCollectionReceiptNumberingSequence { get; set; }
        public abstract class usrCollectionReceiptNumberingSequence : PX.Data.BQL.BqlString.Field<usrCollectionReceiptNumberingSequence> { }
        #endregion

        #region UsrOfficialReceiptNumberingSequence
        [PXDBString(10, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = ATRDMessages.OfficialReceiptNumberingSequence, Visibility = PXUIVisibility.Visible)]
        public string UsrOfficialReceiptNumberingSequence { get; set; }
        public abstract class usrOfficialReceiptNumberingSequence : PX.Data.BQL.BqlString.Field<usrOfficialReceiptNumberingSequence> { }
        #endregion

        #region UsrProvisionalReceiptNumberingSequence
        [PXDBString(10, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = ATRDMessages.ProvisionalReceiptNumberingSequence, Visibility = PXUIVisibility.Visible)]
        public string UsrProvisionalReceiptNumberingSequence { get; set; }
        public abstract class usrProvisionalReceiptNumberingSequence : PX.Data.BQL.BqlString.Field<usrProvisionalReceiptNumberingSequence> { }
        #endregion
        #endregion

    }
}
