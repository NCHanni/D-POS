using PX.Data;
using PX.Objects.AR;
using RetailDimension.Attributes;
using RetailDimension.DAC.Extension;

namespace RetailDimension.Graph.Extension
{
    public class ATRDARPaymentEntryExtension : PXGraphExtension<ARPaymentEntry>
    {
        public static bool IsActive() => true;

        public PXSetup<ARSetup> numSetup;

        #region Events
        protected virtual void _(Events.RowPersisting<ARPayment> e)
        {
            ATRDARPaymentExtension rowExt = e.Row.GetExtension<ATRDARPaymentExtension>();

            if (e.Row != null)
            {
                ATRDARSetup setup = Base.arsetup.SelectSingle().GetExtension<ATRDARSetup>();

                switch (rowExt.UsrReceiptType)
                {
                    case ATRDReceiptTypeAttribute.Acknowledgement:
                        if (setup.UsrAcknowledgementReceiptNumberingSequence != null)
                            ARReceiptTypeAutonumberAttribute.SetNumberingId<ATRDARPaymentExtension.usrReceiptNbr>(e.Cache, setup.UsrAcknowledgementReceiptNumberingSequence);
                        break;
                    case ATRDReceiptTypeAttribute.Collection:
                        if (setup.UsrCollectionReceiptNumberingSequence != null)
                            ARReceiptTypeAutonumberAttribute.SetNumberingId<ATRDARPaymentExtension.usrReceiptNbr>(e.Cache, setup.UsrCollectionReceiptNumberingSequence);
                        break;
                    case ATRDReceiptTypeAttribute.Official:
                        if (setup.UsrOfficialReceiptNumberingSequence != null)
                            ARReceiptTypeAutonumberAttribute.SetNumberingId<ATRDARPaymentExtension.usrReceiptNbr>(e.Cache, setup.UsrOfficialReceiptNumberingSequence);
                        break;
                    case ATRDReceiptTypeAttribute.Provisional:
                        if (setup.UsrProvisionalReceiptNumberingSequence != null)
                            ARReceiptTypeAutonumberAttribute.SetNumberingId<ATRDARPaymentExtension.usrReceiptNbr>(e.Cache, setup.UsrProvisionalReceiptNumberingSequence);
                        break;
                }
            }
        }
        #endregion
    }
}