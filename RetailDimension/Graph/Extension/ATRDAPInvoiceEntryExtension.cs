using PX.Data;
using PX.Objects.AP;
using System.Collections;
using System.Collections.Generic;

namespace RetailDimension.Graph.Extension
{
    public class ATRDAPInvoiceEntryExtension : PXGraphExtension<APInvoiceEntry>
    {
        public static bool IsActive() => true;

        public override void Initialize()
        {
            base.Initialize();

            //Base.report.AddMenuAction(PrintAPVoucher);
        }

        #region Actions
        public PXAction<APInvoice> PrintAPVoucher;
        #endregion

        #region Action Delegates
        [PXButton(Category = "Reports")]
        [PXUIField(DisplayName = "Print Accounts Payable Voucher")]
        public IEnumerable printAPVoucher(PXAdapter adapter)
        {

            foreach (APInvoice invoice in adapter.Get<APInvoice>())
            {
                APInvoiceEntry graph = PXGraph.CreateInstance<APInvoiceEntry>();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["RefNbr"] = invoice.RefNbr;

                var report = new PXReportRequiredException(parameters, "RL641600", "Print Accounts Payable Voucher");

                throw new PXRedirectWithReportException(graph, report, "Preview");
            }

            return adapter.Get();
        }
        #endregion
    }
}