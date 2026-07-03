using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Objects.SO;
using RetailDimension.DAC;
using RetailDimension.Helper;
using System.Collections.Generic;
using System.Linq;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE : ATRD5001
    /// </summary>
    public class ATRDSaleProcess : PXGraph<ATRDSaleProcess>
    {
        #region Views
        public PXCancel<ATRDBatchSale> Cancel;
        public PXProcessing<ATRDBatchSale, Where<ATRDBatchSale.orderNbr, IsNull, And<ATRDBatchSale.branchID, Equal<Current<AccessInfo.branchID>>>>> Document;
        #endregion

        #region Events

        protected virtual void _(Events.RowSelected<ATRDBatchSale> e)
        {
            Document.SetProcessDelegate(delegate (ATRDSaleProcess instance, ATRDBatchSale row)
            {
                Process(instance, row);
            });

            if (e.Row == null) return;
        }

        #endregion

        #region Methods

        public static void Process(PXGraph graph, ATRDBatchSale row)
        {
            List<ATRDSale> pwdSalesList = new List<ATRDSale>();
            List<ATRDSale> scSalesList = new List<ATRDSale>();
            List<ATRDSale> walkInSalesList = new List<ATRDSale>();
            //Dictionary<Customer, List<ATRDSale>> customerSales = new Dictionary<Customer, List<ATRDSale>>();

            using (PXTransactionScope ts = new PXTransactionScope())
            {
                #region Get sales data
                var batchSales = PXSelect<ATRDSale, 
                    Where<ATRDSale.batchRefNbr, Equal<Required<ATRDSale.batchRefNbr>>, 
                        And<ATRDSale.branchID, Equal<Required<ATRDSale.branchID>>>>>
                    .Select(graph, row.RefNbr, row.BranchID)
                    .RowCast<ATRDSale>()
                    .ToList();

                foreach (ATRDSale saleTransaction in batchSales)
                {
                    //Customer customer = PXSelect<Customer, Where<Customer.acctName, Equal<Required<Customer.acctName>>>>.Select(graph, saleTransaction.CustomerName);
                    // Check if customer is existing in Acumatica

                    if (saleTransaction.IsPWD.GetValueOrDefault())
                    {
                        pwdSalesList.Add(saleTransaction);
                    }
                    else if (saleTransaction.IsSC.GetValueOrDefault())
                    {
                        scSalesList.Add(saleTransaction);
                    }
                    //else if (customer != null)
                    //{ 
                    //    // Check if customer is in the Dictionary
                    //    // If it is in the dictionary, then update the sale list for that customer
                    //    if (customer.AcctName == "WALK-IN")
                    //    {
                    //        walkInSalesList.Add(item);
                    //    }
                    //    else if (customerSales.ContainsKey(customer))
                    //    {
                    //        var list = customerSales[key: customer];
                    //        if (!list.Contains(item))
                    //        {
                    //            list.Add(item);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        // Customer is not yet on the dictionary
                    //        var list = new List<ATRDSale>();
                    //        list.Add(item);
                    //        customerSales.Add(customer, list);
                    //    }
                    //}
                    else
                    {
                        walkInSalesList.Add(saleTransaction);
                    }
                }
                #endregion

                SOOrder soOrder = null;

                #region Process Sales
                // Process PWD Sales Order
                if (pwdSalesList.Any())
                {
                    soOrder = ProcessSales(
                        list: pwdSalesList,
                        batchRefNbr: row.RefNbr,
                        branchID: row.BranchID,
                        saleType: "PWD");
                }

                // Process SC Sales Order
                if (scSalesList.Any())
                {
                    soOrder = ProcessSales(
                        list: scSalesList,
                        batchRefNbr: row.RefNbr,
                        branchID: row.BranchID,
                        saleType: "SC");
                }

                // Process Walk-In Sales Order
                if (walkInSalesList.Any())
                {
                    soOrder = ProcessSales(
                        list: walkInSalesList,
                        batchRefNbr: row.RefNbr,
                        branchID: row.BranchID,
                        saleType: "WALKIN");
                }

                // Process Existing Customer Sales Order
                //if (customerSales.Any())
                //{
                //    foreach (Customer item in customerSales.Keys)
                //    {
                //        soOrder = ProcessSales(
                //            list: customerSales[item],
                //            batchRefNbr: row.RefNbr,
                //            branchID: row.BranchID,
                //            saleType: "CUSTOMER");
                //    }
                //}
                #endregion

                if (soOrder != null)
                    UpdateBatchFromSalesOrder(row, soOrder);

                ts.Complete();
            }
        }

        public static SOOrder ProcessSales(List<ATRDSale> list, string batchRefNbr, int? branchID, string saleType)
        {
            var parameters = GetSaleInformation(list);

            POSSetup posSetup = new POSSetup();

            if (parameters.BranchSetup != null)
            {
                posSetup.IsBranchSetup = true;
                posSetup.CustomerPWDID = parameters.BranchSetup.CustomerPWDID;
                posSetup.CustomerSCID = parameters.BranchSetup.CustomerSCID;
                posSetup.CustomerID = parameters.BranchSetup.CustomerID;
                posSetup.SiteID = parameters.BranchSetup.SiteID;
                posSetup.CashPaymentMethodID = parameters.BranchSetup.CashPaymentMethodID;
                posSetup.MemoPaymentMethodID = parameters.BranchSetup.MemoPaymentMethodID;
                posSetup.CardPaymentMethodID = parameters.BranchSetup.CardPaymentMethodID;
                posSetup.OrderType = parameters.BranchSetup.OrderType;
            }
            else
            {
                posSetup.IsBranchSetup = false;
                posSetup.CustomerPWDID = parameters.Setup.CustomerPWDID;
                posSetup.CustomerSCID = parameters.Setup.CustomerSCID;
                posSetup.CustomerID = parameters.Setup.CustomerID;
                posSetup.CashPaymentMethodID = parameters.Setup.CashPaymentMethodID;
                posSetup.MemoPaymentMethodID = parameters.Setup.MemoPaymentMethodID;
                posSetup.CardPaymentMethodID = parameters.Setup.CardPaymentMethodID;
                posSetup.OrderType = parameters.Setup.OrderType;
            }

            return SOModule.CreateOrder(
                batchNbr: batchRefNbr,
                type: saleType,
                branchID: branchID,
                setup: posSetup,
                details: parameters.SaleDetail,
                saleDetails: list,
                _cashDetails: parameters.CashDetail,
                _cardDetails: parameters.CardDetail,
                _memoDetails: parameters.MemoDetail,
                _giftDetails: parameters.GiftDetail);
        }

        public static void UpdateBatchFromSalesOrder(ATRDBatchSale batch, SOOrder order)
        {
            var instance = CreateInstance<ATRDSaleEntry>();
            instance.Clear();
            instance.Document.Current = instance.Document.Search<ATRDBatchSale.refNbr>(batch.RefNbr);
            instance.Document.Current.OrderNbr = order.OrderNbr;
            instance.Document.Current.BatchTotalAmount = order.CuryOrderTotal;
            instance.Document.UpdateCurrent();
            instance.Save.Press();
        }

        public static ATRDSaleProcessHelper.ATRDSaleDetailInformation GetSaleInformation(List<ATRDSale> listSales)
        {
            var instanceHelper = CreateInstance<ATRDSaleProcessHelper>();
            instanceHelper.Clear();

            ATRDSaleProcessHelper.ATRDSaleDetailInformation result = new ATRDSaleProcessHelper.ATRDSaleDetailInformation();

            foreach (ATRDSale saleTransaction in listSales)
            {
                var parameter = instanceHelper.GetSaleDetails(saleTransaction);
                result.Setup = parameter.Setup;
                result.BranchSetup = parameter.BranchSetup;

                if (parameter.SaleDetail.Any())
                {
                    foreach (var s in parameter.SaleDetail)
                    {
                        result.SaleDetail.Add(s);
                    }
                }

                if (parameter.CashDetail.Any())
                {
                    foreach (var s in parameter.CashDetail)
                    {
                        result.CashDetail.Add(s);
                    }
                }

                if (parameter.CardDetail.Any())
                {
                    foreach (var s in parameter.CardDetail)
                    {
                        result.CardDetail.Add(s);
                    }
                }

                if (parameter.GiftDetail.Any())
                {
                    foreach (var s in parameter.GiftDetail)
                    {
                        result.GiftDetail.Add(s);
                    }
                }

                if (parameter.MemoDetail.Any())
                {
                    foreach (var s in parameter.MemoDetail)
                    {
                        result.MemoDetail.Add(s);
                    }
                }
            }

            return result;
        }

        #endregion

        #region Actions & Delegates
        public PXAction<ATRDBatchSale> openBatchTransaction;
        [PXButton(Tooltip = "Open Batch Transaction", CommitChanges = true)]
        [PXUIField()]
        public virtual void OpenBatchTransaction()
        {
            ATRDBatchSale row = Document.Current;

            ATRDSaleEntry graph = PXGraph.CreateInstance<ATRDSaleEntry>();

            ATRDBatchSale data = PXSelect<ATRDBatchSale,
                Where<ATRDBatchSale.refNbr, Equal<Required<ATRDBatchSale.refNbr>>>>
                .Select(this, row.RefNbr);

            graph.Document.Current = data;

            throw new PXRedirectRequiredException(graph, true, "Request") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }
        #endregion
    }

    public class ATRDSaleProcessHelper : PXGraph<ATRDSaleProcessHelper>
    {
        public PXSetup<ATRDSetup> Setup;
        public PXSetup<ATRDBranchSetup>.Where<ATRDBranchSetup.branchID.IsEqual<AccessInfo.branchID.FromCurrent>> BranchSetup;

        public PXResultset<ATRDSale> GetSaleTransactions(string batchNbr, int? branchID)
        {
            return PXSelect<ATRDSale,
                Where<ATRDSale.batchRefNbr, Equal<Required<ATRDSale.batchRefNbr>>,
                    And<ATRDSale.branchID, Equal<Required<ATRDSale.branchID>>>>>
                .Select(this, batchNbr, branchID);
        }

        public PXResultset<ATRDSaleDetail> GetDetailsGroupBy(string saleCode, int? branchID)
        {
            return PXSelectJoinGroupBy<ATRDSaleDetail,
                InnerJoin<ATRDSale,
                    On<ATRDSale.code, Equal<ATRDSaleDetail.saleCode>,
                        And<ATRDSale.branchID, Equal<ATRDSaleDetail.branchID>>>>,
                Where<ATRDSale.code, Equal<Required<ATRDSaleDetail.saleCode>>,
                    And<ATRDSale.branchID, Equal<Required<ATRDSaleDetail.branchID>>>>,
                Aggregate<
                    GroupBy<ATRDSaleDetail.itemCode,
                    GroupBy<ATRDSaleDetail.alternateID,
                    GroupBy<ATRDSaleDetail.discountPercent,
                    GroupBy<ATRDSaleDetail.saleCode,
                        Sum<ATRDSaleDetail.qty,
                        Sum<ATRDSaleDetail.price,
                        Sum<ATRDSaleDetail.amount>>>>>>>>>
                .Select(this, saleCode, branchID);
        }

        public ATRDSaleCashDetail GetCashDetailsGroupBy(string saleCode, int? branchID)
        {
            return PXSelectJoinGroupBy<ATRDSaleCashDetail,
                InnerJoin<ATRDSale,
                    On<ATRDSale.code, Equal<ATRDSaleCashDetail.saleCode>,
                        And<ATRDSale.branchID, Equal<ATRDSaleCashDetail.branchID>>>>,
                Where<ATRDSale.code, Equal<Required<ATRDSaleCashDetail.saleCode>>,
                    And<ATRDSale.branchID, Equal<Required<ATRDSaleCashDetail.branchID>>>>,
                Aggregate<
                    Sum<ATRDSaleCashDetail.amount>>>
                .Select(this, saleCode, branchID);
        }

        public ATRDSaleCardDetail GetCardDetailsGroupBy(string saleCode, int? branchID)
        {
            return PXSelectJoinGroupBy<ATRDSaleCardDetail,
                InnerJoin<ATRDSale,
                    On<ATRDSale.code, Equal<ATRDSaleCardDetail.saleCode>,
                        And<ATRDSale.branchID, Equal<ATRDSaleCardDetail.branchID>>>>,
                Where<ATRDSale.code, Equal<Required<ATRDSaleCardDetail.saleCode>>,
                    And<ATRDSale.branchID, Equal<Required<ATRDSaleCardDetail.branchID>>>>,
                Aggregate<
                    Sum<ATRDSaleCardDetail.amount>>>
                .Select(this, saleCode, branchID);
        }

        public ATRDSaleCardDetail GetCardDetails(string saleCode, int? branchID)
        {
            return PXSelectJoin<ATRDSaleCardDetail,
                InnerJoin<ATRDSale,
                    On<ATRDSale.code, Equal<ATRDSaleCardDetail.saleCode>,
                        And<ATRDSale.branchID, Equal<ATRDSaleCardDetail.branchID>>>>,
                Where<ATRDSale.code, Equal<Required<ATRDSaleCardDetail.saleCode>>,
                    And<ATRDSale.branchID, Equal<Required<ATRDSaleCardDetail.branchID>>>>>
                .Select(this, saleCode, branchID);
        }

        public ATRDSaleMemoDetail GetMemoDetailsGroupBy(string saleCode, int? branchID)
        {
            return PXSelectJoinGroupBy<ATRDSaleMemoDetail,
                InnerJoin<ATRDSale,
                    On<ATRDSale.code, Equal<ATRDSaleMemoDetail.saleCode>, 
                        And<ATRDSale.branchID, Equal<ATRDSaleMemoDetail.branchID>>>>,
                Where<ATRDSale.code, Equal<Required<ATRDSaleMemoDetail.saleCode>>,
                    And<ATRDSale.branchID, Equal<Required<ATRDSaleMemoDetail.branchID>>>>,
                Aggregate<
                    Sum<ATRDSaleMemoDetail.amount>>>
                .Select(this, saleCode, branchID);
        }

        public ATRDSaleGiftDetail GetGiftDetailsGroupBy(string saleCode, int? branchID)
        {
            return PXSelectJoinGroupBy<ATRDSaleGiftDetail,
                InnerJoin<ATRDSale,
                    On<ATRDSale.code, Equal<ATRDSaleGiftDetail.saleCode>, 
                        And<ATRDSale.branchID, Equal<ATRDSaleGiftDetail.branchID>>>>,
                Where<ATRDSale.code, Equal<Required<ATRDSaleGiftDetail.saleCode>>,
                    And<ATRDSale.branchID, Equal<Required<ATRDSaleGiftDetail.branchID>>>>,
                Aggregate<
                    Sum<ATRDSaleGiftDetail.amount>>>
                .Select(this, saleCode, branchID);
        }

        public ATRDSaleDetailInformation GetSaleDetails(ATRDSale saleTransaction)
        {
            List<ATRDSaleDetail> salesDetailList = new List<ATRDSaleDetail>();
            List<ATRDSaleCardDetail> cardDetailList = new List<ATRDSaleCardDetail>();
            List<ATRDSaleCashDetail> cashDetailList = new List<ATRDSaleCashDetail>();
            List<ATRDSaleMemoDetail> memoDetailList = new List<ATRDSaleMemoDetail>();
            List<ATRDSaleGiftDetail> giftDetailList = new List<ATRDSaleGiftDetail>();

            foreach (ATRDSaleDetail itemDetail in GetDetailsGroupBy(saleTransaction.Code, saleTransaction.BranchID))
            {
                salesDetailList.Add(itemDetail);
            }

            //cardDetailList.Add(GetCardDetailsGroupBy(item.Code));
            cardDetailList.Add(GetCardDetails(saleTransaction.Code, saleTransaction.BranchID));
            cashDetailList.Add(GetCashDetailsGroupBy(saleTransaction.Code, saleTransaction.BranchID));
            giftDetailList.Add(GetGiftDetailsGroupBy(saleTransaction.Code, saleTransaction.BranchID));
            memoDetailList.Add(GetMemoDetailsGroupBy(saleTransaction.Code, saleTransaction.BranchID));

            return new ATRDSaleDetailInformation()
            {
                Setup = Setup.Current,
                BranchSetup = BranchSetup.Current,
                SaleDetail = salesDetailList,
                CashDetail = cashDetailList,
                CardDetail = cardDetailList,
                MemoDetail = memoDetailList,
                GiftDetail = giftDetailList
            };
        }

        public class ATRDSaleDetailInformation
        {
            public ATRDSetup Setup { get; set; }
            public ATRDBranchSetup BranchSetup { get; set; }
            public List<ATRDSaleDetail> SaleDetail = new List<ATRDSaleDetail>();
            public List<ATRDSaleCashDetail> CashDetail = new List<ATRDSaleCashDetail>();
            public List<ATRDSaleCardDetail> CardDetail = new List<ATRDSaleCardDetail>();
            public List<ATRDSaleMemoDetail> MemoDetail = new List<ATRDSaleMemoDetail>();
            public List<ATRDSaleGiftDetail> GiftDetail = new List<ATRDSaleGiftDetail>();
        }
    }
}