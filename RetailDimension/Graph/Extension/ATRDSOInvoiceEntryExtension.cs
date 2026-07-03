using PX.Data;
using PX.Objects.AR;
using PX.Objects.SO;
using RetailDimension.DAC;
using System.Linq;

namespace RetailDimension.Graph.Extension
{
    public class ATRDSOInvoiceEntryExtension : PXGraphExtension<SOInvoiceEntry> {

        public static bool IsActive() => true;

        public PXSetup<ATRDSetup> POSSetup;

        public virtual void _(Events.RowInserting<ARTran> e)
        // public virtual void ARTran_RowInserting(PXCache sender, PXRowInsertingEventArPags e)
        {
            try
            {
                ARTran line = (ARTran)e.Row;
                ARInvoice invoice = Base.Document.Current;
                ATRDSetup setup = POSSetup.Current;

                if (setup == null)
                {
                    setup = PXSelect<ATRDSetup>.Select(Base).FirstOrDefault();
                }

                if (line.SOOrderType == setup.OrderType)
                {
                    SOOrder order = SOOrder.PK.Find(Base, line.SOOrderType, line.SOOrderNbr);

                    if (order != null && invoice.DocDate != order.RequestDate)
                    {
                        // Acuminator disable once PX1048 RowChangesInEventHandlersAllowedForArgsOnly [Justification]
                        Base.Document.Cache.SetValueExt<ARInvoice.docDate>(invoice, order.RequestDate);
                        Base.Document.Cache.RaiseFieldUpdated<ARInvoice.docDate>(Base.Document.Current, null);
                    }
                }
            }
            catch { }
        }
    }
}