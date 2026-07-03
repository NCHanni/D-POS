using PX.Data;
using PX.Objects.AP;
using System.Collections;
using System.Collections.Generic;

namespace RetailDimension.Graph.Extension
{
    public class ATRDAPPaymentEntryExtension : PXGraphExtension<APPaymentEntry>
    {
        public static bool IsActive() => true;

        public override void Initialize()
        {
            base.Initialize();

            //Base.report.AddMenuAction(PrintCheckVoucher);
            PrintCheckVoucher.SetEnabled(true);
        }

        #region Actions
        public PXAction<APPayment> PrintCheckVoucher;
        #endregion

        #region Action Delegates
        [PXButton(Category = "Reports")]
        [PXUIField(DisplayName = "Print Check Voucher")]
        public IEnumerable printCheckVoucher(PXAdapter adapter)
        {
            foreach (APPayment payment in adapter.Get<APPayment>())
            {
                APPaymentEntry graph = PXGraph.CreateInstance<APPaymentEntry>();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["RefNbr"] = payment.RefNbr;

                var report = new PXReportRequiredException(parameters, "RL641700", "Print Check Voucher");

                throw new PXRedirectWithReportException(graph, report, "Preview");
            }

            return adapter.Get();
        }
        #endregion
    }
}