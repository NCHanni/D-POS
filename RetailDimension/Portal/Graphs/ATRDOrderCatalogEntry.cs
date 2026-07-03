using PX.Data;
using PX.Data.BQL;
using PX.Objects.CR;
using PX.Objects.IN;
using PX.Objects.SO;
using RetailDimension.DAC;
using RetailDimension.DAC.Extension;
using RetailDimension.Helper;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RetailDimension.Graphs
{

    /// <summary>
    /// Screen ID: ATRD3005
    /// </summary>
    public class ATRDOrderCatalogEntry : PXGraph<ATRDOrderCatalogEntry>
    {
        public PXCancel<ATRDOrderCatalogFilter> Cancel;

        public PXSetup<ATRDSetup> Setup;

        public PXAction<ATRDOrderCatalogFilter> SubmitOrder;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = ATRDMessages.SubmitOrder)]
        protected virtual IEnumerable submitOrder(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    var account = Accounts.Current;
                    var setup = Setup.Current;
                    var orders = Orders.Select();

                    string message = string.Format(
                                    ATRDMessages.ErrorProcessingTransaction,
                                    "Unable to create order.");

                    if (setup.OrderType == null) throw new Exception(message);

                    SOOrder order = SOModule.CreatePortalOrder(
                                        setup,
                                        account.CustomerID,
                                        orders);

                    if (order == null) throw new Exception(message);

                    foreach (ATRDOrder item in orders)
                    {
                        item.OrderNbr = order.OrderNbr;
                        item.CustomerID = account.CustomerID;
                        Orders.Update(item);
                    }

                    this.Actions.PressSave();

                    ts.Complete();
                }
            });

            return adapter.Get();
        }

        public PXFilter<ATRDOrderCatalogFilter> Accounts;

        public PXSelect<ATRDOrder, Where<ATRDOrder.orderNbr, IsNull>> Orders;

        #region Filter

        [Serializable]
        [PXCacheName("Order Catalog Filter")]
#if Version23R1
    public partial class ATRDOrderCatalogFilter : IBqlTable
#endif
#if Version25R1 || Version25R2
        public partial class ATRDOrderCatalogFilter : PXBqlTable, IBqlTable
#endif
        {
            #region CustomerID
            [PXInt]
            [PXUnboundDefault(typeof(Search<ATRDPortalAccount.customerID, Where<ATRDPortalAccount.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual int? CustomerID { get; set; }
            public abstract class customerID : BqlInt.Field<customerID> { }
            #endregion

            #region AccountName
            [PXString(60, IsUnicode = true)]
            [PXSelector(typeof(Search<BAccount.acctName, Where<BAccount.bAccountID, Equal<Current<customerID>>>>))]
            [PXUIField(DisplayName = ATRDMessages.CustomerName, Enabled = false)]
            [PXUnboundDefault(typeof(Search<BAccount.acctName, Where<BAccount.bAccountID, Equal<Current<customerID>>>>))]
            [PXFormula(typeof(Default<customerID>))]
            public virtual string AccountName { get; set; }
            public abstract class accountName : BqlString.Field<accountName> { }
            #endregion

            #region OutstandingBalanceAmt
            [PXDecimal(2)]
            [PXUIField(DisplayName = ATRDMessages.OutstandingBalance, Enabled = false)]
            [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(typeof(Default<customerID>))]
            public virtual decimal? OutstandingBalanceAmt { get; set; }
            public abstract class outstandingBalanceAmt : BqlDecimal.Field<outstandingBalanceAmt> { }
            #endregion

            #region CreditLimit
            [PXDecimal(2)]
            [PXUIField(DisplayName = ATRDMessages.CreditLimit, Enabled = false)]
            [PXUnboundDefault(PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(typeof(Default<customerID>))]
            public virtual decimal? CreditLimit { get; set; }
            public abstract class creditLimit : BqlDecimal.Field<creditLimit> { }
            #endregion
        }

        #endregion
    }
}