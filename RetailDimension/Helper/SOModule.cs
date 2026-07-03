using PX.Data;
using PX.Objects.AR;
using PX.Objects.GL.FinPeriods;
using PX.Objects.IN;
using PX.Objects.SO;
using RetailDimension.DAC;
using RetailDimension.DAC.Extension;
using RetailDimension.Graph.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetailDimension.Helper
{
    public static class SOModule
    {
        public static SOOrder CreateOrder(
            string batchNbr, 
            int? branchID, 
            string type, 
            POSSetup setup, 
            List<ATRDSaleDetail> details, 
            List<ATRDSale> saleDetails,
            List<ATRDSaleCashDetail> _cashDetails, 
            List<ATRDSaleCardDetail> _cardDetails, 
            List<ATRDSaleMemoDetail> _memoDetails, 
            List<ATRDSaleGiftDetail> _giftDetails)
        {
            if (details.Count == 0)
                throw new Exception(string.Format(ATRDMessages.NoTransactionToProcess, batchNbr));

            SOOrderEntry graph = PXGraph.CreateInstance<SOOrderEntry>();

            SOOrder order = new SOOrder
            {
                OrderType = setup.OrderType
            };

            ATRDBatchSale batchDetails = PXSelect<ATRDBatchSale,
                Where<ATRDBatchSale.refNbr, Equal<Required<ATRDBatchSale.refNbr>>, 
                    And<ATRDBatchSale.branchID, Equal<Required<ATRDBatchSale.branchID>>>>>
                .Select(graph, batchNbr, branchID);

            //ATRDSale docDetail = saleDetails.RowCast<ATRDSale>().Where(x => x.Code == salesCode).FirstOrDefault();

            Customer customerCode = null;

            if (type == "PWD")
            {
                order.CustomerID = setup.CustomerPWDID;
            }
            else if (type == "SC")
            {
                order.CustomerID = setup.CustomerSCID;
            }
            else if (type == "CUSTOMER")
            {
                customerCode = PXSelect<Customer, 
                    Where<Customer.acctName, Equal<Required<Customer.acctName>>>>.Select(graph, saleDetails[0].CustomerName);
                order.CustomerID = customerCode.BAccountID;
            }
            else
            {
                order.CustomerID = setup.CustomerID;
            }

            order.OrderDesc = string.Format("POS Batch Transaction : {0} | Cashier : {1}", batchNbr, batchDetails.CashierName);
            order.CustomerRefNbr = batchNbr;
            order.OrderDate = batchDetails.Date;
            order.RequestDate = batchDetails.Date;

            order = graph.Document.Insert(order);

            //Document Details  
            foreach (ATRDSaleDetail d in details)
            {
                ATRDSale dsale = PXSelect<ATRDSale, 
                    Where<ATRDSale.code, Equal<Required<ATRDSale.code>>, 
                        And<ATRDSale.branchID, Equal<Required<ATRDSale.branchID>>>>>.Select(graph, d.SaleCode, d.BranchID);

                if (dsale == null) continue;
                if ((d.Amount) <= Decimal.Zero) continue;

                var i = graph.Transactions.Select().RowCast<SOLine>().Where(x => x.InventoryID == d.InventoryID && x.DiscPct == d.DiscountPercent).FirstOrDefault();

                SOLine line = new SOLine();

                if (i != null && i.UOM == d.UnitOfMeasure && i.AlternateID == d.AlternateID && i.DiscPct == d.DiscountPercent && i.SalesPersonID == dsale.SalespersonCode)
                {
                    line = graph.Transactions.Select().RowCast<SOLine>().Where(x => x.InventoryID == d.InventoryID && x.DiscPct == d.DiscountPercent).FirstOrDefault();
                    line.Qty += d.Qty;
                    line.CuryExtPrice += (d.Price * d.Qty);
                    if (dsale.SalespersonCode != null)
                    {
                        line.SalesPersonID = dsale.SalespersonCode;
                        line.Commissionable = true;
                    }
                    line = graph.Transactions.Update(line);
                }
                else
                {
                    line.InventoryID = d.InventoryID;
                    line.Qty = d.Qty;
                    line.UOM = d.UnitOfMeasure;
                    line.AlternateID = d.AlternateID;
                    line.CuryUnitPrice = d.Price;
                    line.ManualPrice = true;
                    line.CuryExtPrice = (d.Price * d.Qty);
                    line.CreatedDateTime = d.CreatedDateTime;
                    if (setup.IsBranchSetup)
                    {
                        line.SiteID = setup.SiteID;
                    }

                    if (dsale.SalespersonCode != null)
                    {
                        line.SalesPersonID = dsale.SalespersonCode;
                        line.Commissionable = true;
                    }

                    if (d.DiscountPercent > 0)
                    {
                        line.ManualDisc = true;
                        line.DiscPct = d.DiscountPercent;

                        decimal vat = 1.12m;
                        decimal? discount;
                        decimal? netVat;

                        // Discount of SC/PWD
                        ATRDInventoryItem rowExt = GetInventoryItem(graph, line.InventoryID).GetExtension<ATRDInventoryItem>();

                        if (rowExt != null && rowExt.UsrATRDIsSeniorPWD.GetValueOrDefault())
                        {
                            netVat = (line.CuryUnitPrice / vat);
                            discount = (netVat * (d.DiscountPercent / 100));
                            line.DiscountID = rowExt.UsrATRDDiscountID;
                        }
                    }

                    if (!string.IsNullOrEmpty(d.SerialLotNo))
                    {
                        line.LotSerialNbr = d.SerialLotNo;
                    }

                    line = graph.Transactions.Insert(line);
                }
            }

            //POS Details
            ATRDSOOrderEntry graphExt = graph.GetExtension<ATRDSOOrderEntry>();
            foreach (ATRDSale d in saleDetails)
            {
                ATRDBatchSO so = new ATRDBatchSO
                {
                    SaleCode = d.Code
                };

                graphExt.POSDetail.Insert(so);
            }

            foreach (INItemXRef item in graph.Caches[typeof(INItemXRef)].Updated)
            {
                if (item != null)
                {
                    if (item.CreatedByScreenID == null) item.CreatedByScreenID = item.LastModifiedByScreenID;
                    if (item.CreatedByID == null) item.CreatedByID = graph.Accessinfo.UserID;
                    if (item.CreatedDateTime == null) item.CreatedDateTime = graph.Accessinfo.BusinessDate;

                    graph.Caches[typeof(INItemXRef)].Update(item);
                }
            }

            graph.Actions.PressSave();

            decimal? totalPayment = 0;

            foreach (ATRDSaleCashDetail cashDetails in _cashDetails)
            {
                if (cashDetails != null && cashDetails.Amount > Decimal.Zero)
                {
                    totalPayment += (cashDetails.Amount - cashDetails.Change);
                }
            }

            DateTime dateTime = Convert.ToDateTime(batchDetails.Date);
            string year = dateTime.Year.ToString();
            string qtr = dateTime.Month.ToString();

            if (dateTime.Month.ToString().Length == 1)
            {
                qtr = "0" + dateTime.Month.ToString();
            }

            MasterFinPeriod period = PXSelect<MasterFinPeriod, 
                Where<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                    And<MasterFinPeriod.periodNbr, Equal<Required<MasterFinPeriod.periodNbr>>>>>.Select(graph, year, qtr);

            if (totalPayment > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                ARPayment cashPayment = paymentGraph.Document.Insert(new ARPayment()
                {
                    DocType = ARDocType.Payment,
                    CustomerID = order.CustomerID,
                    PaymentMethodID = setup.CashPaymentMethodID,
                    DocDesc = order.OrderDesc,
                    OrigDocAmt = totalPayment,
                    CuryOrigDocAmt = totalPayment
                });

                cashPayment.AdjFinPeriodID = period.FinPeriodID;
                cashPayment.AdjDate = batchDetails.Date;

                cashPayment = paymentGraph.Document.Update(cashPayment);

                SOAdjust cashOrder = new SOAdjust
                {
                    CustomerID = order.CustomerID,
                    AdjdOrderType = setup.OrderType,
                    AdjdOrderNbr = order.OrderNbr,
                    AdjgDocDate = batchDetails.Date
                };

                var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                ordersToApply.SOAdjustments.Insert(cashOrder);

                //Memo
                totalPayment = 0;
                foreach (var memoDetails in _memoDetails) {
                    if (memoDetails != null && memoDetails.Amount > Decimal.Zero) {

                        SOAdjust memoOrder = new SOAdjust {
                            CustomerID = order.CustomerID,
                            AdjdOrderType = setup.OrderType,
                            AdjdOrderNbr = order.OrderNbr,
                            AdjgRefNbr = memoDetails.MemoCode,
                            AdjgDocType = ARDocType.CreditMemo,
                            CuryAdjdAmt = memoDetails.Amount,
                            OrigAdjAmt = memoDetails.Amount,
                            IsCCPayment = false,
                            PaymentReleased = true,
                            IsCCCaptured = false,
                            IsCCAuthorized = false,
                            Voided = false,
                            PendingPayment = false,
                            ValidateCCRefundOrigTransaction = false
                        };

                        ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                        ordersToApply.SOAdjustments.Insert(memoOrder);

                        paymentGraph.Actions.PressSave();

                        totalPayment += memoDetails.Amount;
                    }
                }

                paymentGraph.Actions.PressSave();
            }

            //Credit Card
            foreach (ATRDSaleCardDetail cardDetails in _cardDetails)
            {
                if (cardDetails != null && cardDetails.Amount > Decimal.Zero)
                {
                    ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                    // Get payment method from ATRDCreditCard based on TypeCode
                    ATRDCreditCard creditCard = PXSelect<ATRDCreditCard,
                        Where<ATRDCreditCard.code, Equal<Required<ATRDCreditCard.code>>>>
                        .Select(graph, cardDetails.TypeCode);

                    string paymentMethodID = creditCard?.PaymentMethodID ?? setup.CardPaymentMethodID;

                    ARPayment cardPayment = paymentGraph.Document.Insert(new ARPayment()
                    {
                        DocType = ARDocType.Payment,
                        CustomerID = order.CustomerID,
                        PaymentMethodID = paymentMethodID,
                        DocDesc = order.OrderDesc,
                        OrigDocAmt = cardDetails.Amount,
                        CuryOrigDocAmt = cardDetails.Amount
                    });

                    paymentGraph.Document.Cache.RaiseFieldUpdated<ARPayment.paymentMethodID>(cardPayment, null);

                    cardPayment.AdjFinPeriodID = period.FinPeriodID;
                    cardPayment.AdjDate = batchDetails.Date;

                    cardPayment = paymentGraph.Document.Update(cardPayment);

                    var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);

                    SOAdjust cardOrder = ordersToApply.SOAdjustments.Insert(new SOAdjust()
                    {
                        CustomerID = order.CustomerID,
                        AdjdOrderType = setup.OrderType,
                        AdjdOrderNbr = order.OrderNbr,
                        AdjgDocDate = batchDetails.Date
                    });

                    ordersToApply.SOAdjustments.Insert(cardOrder);

                    paymentGraph.Actions.PressSave();
                }
            }

            

            //Gift Certificates
            totalPayment = 0;
            foreach (var giftDetails in _giftDetails)
            {
                if (giftDetails != null && giftDetails.Amount > Decimal.Zero)
                {
                    totalPayment += giftDetails.Amount;
                }
            }

            if (totalPayment > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                ARPayment giftPayment = new ARPayment
                {
                    DocType = ARDocType.Payment,
                    CustomerID = order.CustomerID,
                    DocDesc = order.OrderDesc,
                    CuryOrigDocAmt = totalPayment
                };

                paymentGraph.Document.Insert(giftPayment);

                SOAdjust giftOrder = new SOAdjust
                {
                    CustomerID = order.CustomerID,
                    AdjdOrderType = setup.OrderType,
                    AdjdOrderNbr = order.OrderNbr
                };

                var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                ordersToApply.SOAdjustments.Insert(giftOrder);

                paymentGraph.Actions.PressSave();
            }

            return order;
        }

        public static SOOrder CreatePortalOrder(ATRDSetup setup, int? customerID, PXResultset<ATRDOrder> orders)
        {
            SOOrderEntry graph = PXGraph.CreateInstance<SOOrderEntry>();

            SOOrder order = new SOOrder
            {
                OrderType = setup.OrderType,
                CustomerID = customerID,
                OrderDesc = string.Format("Ordered from Customer Portal"),
                BranchID = setup.BranchID
            };

            order = graph.Document.Insert(order);
            foreach (ATRDOrder o in orders)
            {
                if ((o.Total) <= Decimal.Zero) continue;

                SOLine line = new SOLine
                {
                    InventoryID = o.InventoryID,
                    BranchID = setup.BranchID,
                    Qty = o.Qty,
                    CuryUnitPrice = ((o.Price * o.Qty) / o.Qty),
                    ManualPrice = true,
                    CuryExtPrice = (o.Price * o.Qty),
                    CuryLineAmt = (o.Price * o.Qty)
                };

                graph.Transactions.Insert(line);
            }

            graph.Actions.PressSave();

            return order;
        }

        public static InventoryItem GetInventoryItem(PXGraph graph, int? InventoryID)
            => PXSelectReadonly<InventoryItem, Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(graph, InventoryID);
    }

    public struct POSSetup
    {
        public bool IsBranchSetup;
        public int? CustomerPWDID;
        public int? CustomerSCID;
        public int? CustomerID;
        public int? SiteID;
        public string CashPaymentMethodID;
        public string MemoPaymentMethodID;
        public string CardPaymentMethodID;
        public string OrderType;
    }
}
