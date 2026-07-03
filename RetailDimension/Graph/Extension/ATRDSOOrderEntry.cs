using PX.Data;
using PX.Objects.AR;
using PX.Objects.SO;
using PX.Objects.SO.DAC.Unbound;
using PX.Objects.SO.GraphExtensions.SOOrderEntryExt;
using RetailDimension.DAC;
using RetailDimension.DAC.Extension;
using System.Collections;
using System.Linq;

namespace RetailDimension.Graph.Extension
{
#if Version23R1
    public class ATRDSOOrderEntry : PXGraphExtension<SOOrderEntry>
#endif
#if Version25R1 || Version25R2
    public class ATRDSOOrderEntry : PXGraphExtension<AddInvoiceExt, SOOrderEntry>
#endif
    {
        public static bool IsActive() { return true; }

        public PXSelect<ATRDBatchSO, Where<ATRDBatchSO.orderNbr, Equal<Current<SOOrder.orderNbr>>>> POSDetail;

        protected void AddInvoiceFilter_UsrATRDSaleCode_FieldUpdated(PXCache cache, PXFieldUpdatedEventArgs e)
        {
            AddInvoiceFilter row = e.Row as AddInvoiceFilter;
            if(row != null)
            {
                ATRDAddInvoiceFilter rowExt = row.GetExtension<ATRDAddInvoiceFilter>();
                if (rowExt != null)
                {
                    ARTran ar = PXSelect<ARTran, Where<ARTran.sOOrderNbr, In2<Search<ATRDBatchSO.orderNbr, Where<ATRDBatchSO.saleCode, Equal<Required<ATRDBatchSO.saleCode>>>>>>>.Select(Base, rowExt.UsrATRDSaleCode);
                    if (ar != null)
                    {
#if Version23R1
                        Base.addinvoicefilter.SetValueExt<AddInvoiceFilter.refNbr>(row, ar.RefNbr);
#endif
#if Version25R1 || Version25R2
                        Base1.AddInvoiceFilter.SetValueExt<AddInvoiceFilter.aRRefNbr>(row, ar.RefNbr);
#endif
                    }
                }
            }
        }

        public delegate IEnumerable AddInvoiceDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable AddInvoice(PXAdapter adapter, AddInvoiceDelegate baseMethod)
        {
            var result = baseMethod(adapter);

#if Version23R1
            AddInvoiceFilter current = Base.addinvoicefilter.Current;
#endif
#if Version25R1 || Version25R2
            AddInvoiceFilter current = Base1.AddInvoiceFilter.Current;
#endif

            if (current != null)
            {
                ATRDAddInvoiceFilter currentExt = current.GetExtension<ATRDAddInvoiceFilter>();
                if (currentExt != null)
                {
                    if(!POSDetail.Select().RowCast<ATRDBatchSO>().Where(w => w.SaleCode == currentExt.UsrATRDSaleCode).Any())
                    {
                        if (!POSDetail.Cache.Inserted.RowCast<ATRDBatchSO>().Where(w => w.SaleCode == currentExt.UsrATRDSaleCode).Any())
                        {
                            ATRDBatchSO line = new ATRDBatchSO();
                            line.SaleCode = currentExt.UsrATRDSaleCode;
                            POSDetail.Insert(line);
                        }
                    }
                }
            }

            return result;
        }
    }
}