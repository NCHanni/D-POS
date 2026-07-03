using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;
using RetailDimension.DAC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RetailDimension.Graphs
{
    /// <summary>
    /// Screen ID: ATRD5002
    /// </summary>
    public class ATRDCancelOrderProcess : PXGraph<ATRDCancelOrderProcess>
    {
        #region Actions & Delegates
        public PXAction<ATRDCancelOrderView> PrintOrder;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Print Order")]
        public virtual IEnumerable printOrder(PXAdapter adapter)
        {
            return adapter.Get();
        }


        public PXAction<ATRDCancelOrderView> CancelOrder;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Cancel Order")]
        public virtual IEnumerable cancelOrder(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, delegate ()
            {
                var orders = this.Orders.Select().RowCast<ATRDCancelOrderView>().ToList();

                foreach (ATRDCancelOrderView item in orders)
                {
                    if (!(item.Selected.GetValueOrDefault())) continue;

                    if (item.OrderNbr == null) continue;

                    using (PXTransactionScope ts = new PXTransactionScope())
                    {
                        SOOrderEntry graph = PXGraph.CreateInstance<SOOrderEntry>();

                        graph.Document.Current = graph.Document.Search<SOOrder.orderNbr>(item.OrderNbr, Setup.Current.OrderType);

                        graph.cancelOrder.Press();

                        ts.Complete();
                    }
                }
            });

            return adapter.Get();
        }
        #endregion

        #region Views & Delegates

        public PXSetup<ATRDSetup> Setup;

        public PXFilter<ATRDCancelOrderView> Orders;

        protected virtual IEnumerable orders()
        {
            List<ATRDCancelOrderView> result = new List<ATRDCancelOrderView>();

            ATRDPortalAccount account = PXSelect<ATRDPortalAccount,
                                            Where<ATRDPortalAccount.userID,
                                            Equal<Current<AccessInfo.userID>>>>
                                            .Select(this)
                                            .RowCast<ATRDPortalAccount>()
                                            .FirstOrDefault();

            if (account != null)
            {
                var orders = PXSelect<SOOrder, Where<SOOrder.customerID,
                                Equal<Required<SOOrder.customerID>>,
                                And<SOOrder.status, NotEqual<SOOrderStatus.cancelled>>>>
                                .Select(this, account.CustomerID)
                                .RowCast<SOOrder>()
                                .ToList();

                foreach (SOOrder item in orders)
                {
                    ATRDCancelOrderView o = new ATRDCancelOrderView();

                    o.Selected = item.Selected ?? false;
                    o.OrderNbr = item.OrderNbr;
                    o.Status = item.Status;
                    o.Total = item.OrderTotal;
                    o.TransactionDate = item.OrderDate;
                    o.DeliveryDate = item.ShipDate;
                    o.Qty = item.OrderQty;


                    ATRDCancelOrderView cache = (ATRDCancelOrderView)Orders.Cache.Locate(o);

                    if (cache == null)
                    {
                        o = Orders.Insert(new ATRDCancelOrderView
                        {
                            OrderNbr = item.OrderNbr,
                            Status = item.Status,
                            Total = item.OrderTotal,
                            TransactionDate = item.OrderDate,
                            DeliveryDate = item.ShipDate,
                            Qty = item.OrderQty
                        });
                        result.Add(o);
                    }
                    else
                    {
                        result.Add(cache);
                    }
                }
            }

            return result;
        }
        #endregion

        #region Internal Types
        [Serializable]
        [PXCacheName("Orders")]
#if Version23R1
    public partial class ATRDCancelOrderView : IBqlTable
#endif
#if Version25R1 || Version25R2
        public partial class ATRDCancelOrderView : PXBqlTable, IBqlTable
#endif
        {
            [PXBool]
            [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual bool? Selected { get; set; }
            public abstract class selected : BqlBool.Field<selected> { }

            [PXString(IsKey = true)]
            [PXUIField(DisplayName = "Order Nbr", Enabled = false)]
            public virtual string OrderNbr { get; set; }
            public abstract class orderNbr : BqlString.Field<orderNbr> { }

            [PXString(1, IsFixed = true)]
            [PXUIField(DisplayName = "Status", Enabled = false)]
            [SOOrderStatus.List]
            public virtual string Status { get; set; }
            public abstract class status : BqlString.Field<status> { }

            [PXDate]
            [PXUIField(DisplayName = "Date", Enabled = false)]
            public virtual DateTime? TransactionDate { get; set; }
            public abstract class transactionDate : BqlDateTime.Field<transactionDate> { }

            [PXDate]
            [PXUIField(DisplayName = "Delivery Date", Enabled = false)]
            public virtual DateTime? DeliveryDate { get; set; }
            public abstract class deliveryDate : BqlDateTime.Field<deliveryDate> { }

            [PXDecimal]
            [PXUIField(DisplayName = "Qty", Enabled = false)]
            public virtual decimal? Qty { get; set; }
            public abstract class qty : BqlDecimal.Field<qty> { }

            [PXDecimal]
            [PXUIField(DisplayName = "Total", Enabled = false)]
            public virtual decimal? Total { get; set; }
            public abstract class total : BqlDecimal.Field<total> { }
        }
        #endregion
    }
}