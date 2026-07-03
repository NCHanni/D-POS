using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE : ICDH4011
    /// POS Credit Memo Inquiry
    /// </summary>
    public class ICDHPOSCreditMemoInq : PXGraph<ICDHPOSCreditMemoInq>
    {
        public SelectFrom<ARInvoice>
            .InnerJoin<Customer>.On<Customer.bAccountID.IsEqual<ARInvoice.customerID>>
            .Where<ARInvoice.docType.IsEqual<ARDocType.creditMemo>
                .And<ARInvoice.status.IsEqual<ARDocStatus.open>>>
            .View.ReadOnly Document;

        public PXAction<ARInvoice> ViewDocument;
        [PXButton]
        [PXUIField(DisplayName = "View Document", Visible = false)]
        protected virtual void viewDocument()
        {
            if (Document.Current != null)
            {
                ARInvoiceEntry graph = PXGraph.CreateInstance<ARInvoiceEntry>();
                graph.Document.Current = graph.Document.Search<ARInvoice.refNbr>(
                    Document.Current.RefNbr, Document.Current.DocType);

                throw new PXRedirectRequiredException(graph, true, "View Document");
            }
        }
    }
}
