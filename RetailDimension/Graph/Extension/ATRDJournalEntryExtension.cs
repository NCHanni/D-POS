using PX.Data;
using PX.Objects.GL;
using System.Collections;
using System.Collections.Generic;

namespace RetailDimension.Graph.Extension
{
    public class ATRDJournalEntryExtension : PXGraphExtension<JournalEntry>
    {
        public static bool IsActive() => true;

        public override void Initialize()
        {
            base.Initialize();
            Base.report.AddMenuAction(PrintJournalVoucher);
            PrintJournalVoucher.SetEnabled(true);
        }

        #region Actions
        public PXAction<Batch> PrintJournalVoucher;
        [PXButton]
        [PXUIField(DisplayName = ATRDMessages.PrintJournalVoucher)]
        public IEnumerable printJournalVoucher(PXAdapter adapter)
        {
            foreach (Batch batch in adapter.Get<Batch>())
            {
                JournalEntry graph = PXGraph.CreateInstance<JournalEntry>();

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters[ATRDMessages.BatchNbr] = batch.BatchNbr;

                var report = new PXReportRequiredException(parameters, "RL641800", ATRDMessages.PrintJournalVoucher);

                throw new PXRedirectWithReportException(graph, report, ATRDMessages.Preview);
            }

            return adapter.Get();
        }
        #endregion
    }
}