using PX.Data;
using PX.Objects.CA;
using RetailDimension.DAC.Extension;
using System.Collections;
using System.Collections.Generic;

namespace RetailDimension.Graph.Extension
{
    public class ATRDCashTransferEntryExtension : PXGraphExtension<CashTransferEntry>
    {
        public override void Initialize()
        {
            base.Initialize();

            Reports.MenuAutoOpen = true;
            Reports.AddMenuAction(PrintCVoucher);
            Reports.AddMenuAction(PrintFundTransfer);
            Reports.AddMenuAction(PrintCheck);
            PrintCVoucher.SetEnabled(true);
            PrintFundTransfer.SetEnabled(true);
            PrintCheck.SetEnabled(true);
        }

        #region Events
        protected virtual void CATransfer_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
        {
            CATransfer row = e.Row as CATransfer;

            if(row != null)
            {
                ATRDCATransfer rowExt = row.GetExtension<ATRDCATransfer>();
                if(rowExt != null)
                {
                    PXUIFieldAttribute.SetEnabled<ATRDCATransfer.usrRLCheckNbr>(cache, row, true);
                }
            }
        }
        #endregion

        #region Actions
        public PXAction<CATransfer> PrintCVoucher;
        public PXAction<CATransfer> PrintFundTransfer;
        public PXAction<CATransfer> PrintCheck;
        public PXAction<CATransfer> Reports;
        #endregion

        #region Action Delegates
        [PXButton]
        [PXUIField(DisplayName = "Reports")]
        public void reports(){}

        [PXButton]
        [PXUIField(DisplayName = "Print Check Voucher")]
        public IEnumerable printCVoucher(PXAdapter adapter)
        {

            foreach (CATransfer transfer in adapter.Get<CATransfer>())
            {
                CashTransferEntry graph = PXGraph.CreateInstance<CashTransferEntry>();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["TransferNbr"] = transfer.TransferNbr;

                var report = new PXReportRequiredException(parameters, "RL641710", "Print Check Voucher");

                throw new PXRedirectWithReportException(graph, report, "Preview");
            }

            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = "Print Fund Transfer")]
        public IEnumerable printFundTransfer(PXAdapter adapter)
        {

            foreach (CATransfer transfer in adapter.Get<CATransfer>())
            {
                CashTransferEntry graph = PXGraph.CreateInstance<CashTransferEntry>();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["TransferNbr"] = transfer.TransferNbr;

                var report = new PXReportRequiredException(parameters, "RL641100", "Print Fund Transfer");

                throw new PXRedirectWithReportException(graph, report, "Preview");
            }

            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = "Print Check")]
        public IEnumerable printCheck(PXAdapter adapter)
        {

            foreach (CATransfer transfer in adapter.Get<CATransfer>())
            {
                CashTransferEntry graph = PXGraph.CreateInstance<CashTransferEntry>();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["TransferNbr"] = transfer.TransferNbr;

                var report = new PXReportRequiredException(parameters, "RL643800", "Print Check");

                throw new PXRedirectWithReportException(graph, report, "Preview");
            }

            return adapter.Get();
        }
        #endregion

        public static bool IsActive() => true;
    }
}