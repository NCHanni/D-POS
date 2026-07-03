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
    public static class SOModuleX
    {
        public static SOOrder CreateOrder(
            string batchNbr, 
            string type, 
            ATRDSetup setup, 
            List<ATRDSaleDetail> details, 
            List<ATRDSale> saleDetails,
            List<ATRDSaleCashDetail> _cashDetails, 
            List<ATRDSaleCardDetail> _cardDetails, 
            List<ATRDSaleMemoDetail> _memoDetails, 
            List<ATRDSaleGiftDetail> _giftDetails)
        {
            if (details.Count < 1)
                throw new Exception(string.Format(ATRDMessages.NoTransactionToProcess, batchNbr));

            decimal? total = 0;
            decimal? payment = 0;
            decimal? change = 0;

            SOOrderEntry graph = PXGraph.CreateInstance<SOOrderEntry>();

            SOOrder order = new SOOrder
            {
                OrderType = setup.OrderType
            };

            ATRDBatchSale data = PXSelect<ATRDBatchSale,
                Where<ATRDBatchSale.refNbr, Equal<Required<ATRDBatchSale.refNbr>>>>.Select(graph, batchNbr);

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
            else if (type == "Exist")
            {
                customerCode = PXSelect<Customer, 
                    Where<Customer.acctName, Equal<Required<Customer.acctName>>>>.Select(graph, saleDetails[0].CustomerName);
                order.CustomerID = customerCode.BAccountID;
            }
            else
            {
                order.CustomerID = setup.CustomerID;
            }

            order.OrderDesc = string.Format("POS Batch Transaction : {0} | Cashier : {1}", batchNbr, data.CashierName);
            order.OrderDate = data.Date;
            order.RequestDate = data.Date;

            order = graph.Document.Insert(order);

            //Document Details  
            foreach (ATRDSaleDetail d in details)
            {
                ATRDSale dsale = PXSelect<ATRDSale, Where<ATRDSale.code, Equal<Required<ATRDSale.code>>>>.Select(graph, d.SaleCode);
                if ((d.Amount) <= Decimal.Zero) continue;

                //ATRDSale curSale = saleDetails.RowCast<ATRDSale>().Where(x => x.Code == d.SaleCode).FirstOrDefault();

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
                    //line.CuryUnitPrice = ((d.Price * d.Qty) / d.Qty);
                    line.CuryUnitPrice = d.Price;
                    line.ManualPrice = true;
                    line.CuryExtPrice = (d.Price * d.Qty);
                    line.CreatedDateTime = d.CreatedDateTime;
                    if (dsale.SalespersonCode != null)
                    {
                        line.SalesPersonID = dsale.SalespersonCode;
                        line.Commissionable = true;
                    }
                    //line.CuryLineAmt = (d.Price * d.Qty);                

                    if (d.DiscountPercent > 0)
                    {
                        line.ManualDisc = true;
                        line.DiscPct = d.DiscountPercent;
                        decimal vat = 1.12m;
                        decimal? discount;
                        decimal? netVat;

                        #region Discount of SC/PWD
                        ATRDInventoryItem rowExt = GetInvetoryItem(graph, line.InventoryID).GetExtension<ATRDInventoryItem>();

                        if (rowExt.UsrATRDIsSeniorPWD.GetValueOrDefault())
                        {
                            //line.CuryExtPrice = (line.CuryUnitPrice - d.VatAmount) * d.Qty;
                            netVat = (line.CuryUnitPrice / vat);
                            discount = (netVat * (d.DiscountPercent / 100));
                            //line.CuryUnbilledAmt = netVat - discount;
                            line.DiscountID = rowExt.UsrATRDDiscountID;
                        }
                        #endregion
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
                ATRDBatchSO so = new ATRDBatchSO();
                so.SaleCode = d.Code;

                graphExt.POSDetail.Insert(so);
            }

            foreach (INItemXRef item in graph.Caches[typeof(INItemXRef)].Updated)
            {
                if (item.CreatedByScreenID == null) item.CreatedByScreenID = item.LastModifiedByScreenID;
                if (item.CreatedByID == null) item.CreatedByID = graph.Accessinfo.UserID;
                if (item.CreatedDateTime == null) item.CreatedDateTime = graph.Accessinfo.BusinessDate;
                graph.Caches[typeof(INItemXRef)].Update(item);
            }
            
            graph.Actions.PressSave();

            foreach (ATRDSaleCashDetail cashDetails in _cashDetails)
            {
                if (cashDetails != null)
                {
                    if (cashDetails.Amount > Decimal.Zero)
                    {
                        payment = cashDetails.Amount;
                        change = cashDetails.Change;
                        total += (payment - change);
                    }
                }
            }

            DateTime dateTime = Convert.ToDateTime(data.Date);
            string year = dateTime.Year.ToString();
            string qtr = dateTime.Month.ToString();

            if (dateTime.Month.ToString().Length == 1)
            {
                qtr = "0" + dateTime.Month.ToString();
            }

            MasterFinPeriod period = PXSelect<MasterFinPeriod, Where<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                    And<MasterFinPeriod.periodNbr, Equal<Required<MasterFinPeriod.periodNbr>>>>>.Select(graph, year, qtr);

            if (total > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                //DateTime dateTime = Convert.ToDateTime(data.Date);
                //string year = dateTime.Year.ToString();
                //string qtr = dateTime.Month.ToString();

                //if (dateTime.Month.ToString().Length == 1)
                //{
                //    qtr = "0" + dateTime.Month.ToString();
                //}

                //MasterFinPeriod period = PXSelect<MasterFinPeriod, Where<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                //        And<MasterFinPeriod.periodNbr, Equal<Required<MasterFinPeriod.periodNbr>>>>>.Select(graph, year, qtr);

                ARPayment cashPayment = paymentGraph.Document.Insert(new ARPayment()
                {
                    DocType = ARDocType.Payment,
                    CustomerID = order.CustomerID,
                    PaymentMethodID = setup.CashPaymentMethodID,
                    DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, data.CashierName),
                    OrigDocAmt = total,
                    CuryOrigDocAmt = total
                });

                cashPayment.AdjFinPeriodID = period.FinPeriodID;
                cashPayment.AdjDate = data.Date;

                cashPayment = paymentGraph.Document.Update(cashPayment);

                SOAdjust cashOrder = new SOAdjust();
                cashOrder.CustomerID = order.CustomerID;
                cashOrder.AdjdOrderType = setup.OrderType;
                cashOrder.AdjdOrderNbr = order.OrderNbr;
                cashOrder.AdjgDocDate = data.Date;

                var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                ordersToApply.SOAdjustments.Insert(cashOrder);

                paymentGraph.Actions.PressSave();
            }

            //Credit Card
            total = 0;
            change = 0;
            payment = 0;

            #region Individual
            foreach (ATRDSaleCardDetail cardDetails in _cardDetails)
            {
                if (cardDetails != null)
                {
                    if (cardDetails.Amount > Decimal.Zero)
                    {
                        ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                        ARPayment cardPayment = paymentGraph.Document.Insert(new ARPayment()
                        {
                            DocType = ARDocType.Payment,
                            CustomerID = order.CustomerID,
                            PaymentMethodID = setup.CardPaymentMethodID,
                            DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, data.CashierName),
                            OrigDocAmt = cardDetails.Amount,
                            CuryOrigDocAmt = cardDetails.Amount
                        });

                        paymentGraph.Document.Cache.RaiseFieldUpdated<ARPayment.paymentMethodID>(cardPayment, null);

                        cardPayment.AdjFinPeriodID = period.FinPeriodID;
                        cardPayment.AdjDate = data.Date;

                        cardPayment = paymentGraph.Document.Update(cardPayment);

                        var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);

                        SOAdjust cardOrder = ordersToApply.SOAdjustments.Insert(new SOAdjust()
                        //SOAdjust cardOrder = paymentGraph.SOAdjustments.Insert(new SOAdjust()
                        {
                            CustomerID = order.CustomerID,
                            AdjdOrderType = setup.OrderType,
                            AdjdOrderNbr = order.OrderNbr,
                            AdjgDocDate = data.Date
                        });

                        //var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                        ordersToApply.SOAdjustments.Insert(cardOrder);

                        // paymentGraph.SOAdjustments.Insert(cardOrder);

                        paymentGraph.Actions.PressSave();
                    }
                }
            }
            #endregion

            #region Aggregate
            //foreach (ATRDSaleCardDetail cardDetails in _cardDetails)
            //{
            //    if (cardDetails != null)
            //    {
            //        if (cardDetails.Amount > Decimal.Zero)
            //        {
            //            total += cardDetails.Amount;
            //        }
            //    }
            //}

            //if (total > 0)
            //{
            //    ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

            //    ARPayment cardPayment = paymentGraph.Document.Insert(new ARPayment()
            //    {
            //        DocType = ARDocType.Payment,
            //        CustomerID = order.CustomerID,
            //        PaymentMethodID = setup.CardPaymentMethodID,
            //        DocDesc = string.Format("POS Batch Transaction : {0}", batchNbr)
            //        OrigDocAmt = total,
            //        CuryOrigDocAmt = total
            //    });

            //    cardPayment.AdjFinPeriodID = period.FinPeriodID;
            //    cardPayment.AdjDate = data.Date;

            //    cardPayment = paymentGraph.Document.Update(cardPayment);

            //    SOAdjust cardOrder = paymentGraph.SOAdjustments.Insert(new SOAdjust()
            //    {
            //        CustomerID = order.CustomerID,
            //        AdjdOrderType = setup.OrderType,
            //        AdjdOrderNbr = order.RefNbr,
            //        AdjgDocDate = data.Date
            //    });

            //    paymentGraph.SOAdjustments.Insert(cardOrder);

            //    paymentGraph.Actions.PressSave();
            //}
            #endregion

            //Memo
            total = 0;
            change = 0;
            payment = 0;

            foreach (var memoDetails in _memoDetails)
            {
                if (memoDetails != null)
                {
                    if (memoDetails.Amount > Decimal.Zero)
                    {
                        total += memoDetails.Amount;
                    }
                }
            }

            if (total > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();
                ARPayment memoPayment = new ARPayment();

                memoPayment.DocType = ARDocType.Payment;
                memoPayment.CustomerID = order.CustomerID;
                memoPayment.PaymentMethodID = setup.MemoPaymentMethodID;
                memoPayment.DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, data.CashierName);
                memoPayment.CuryOrigDocAmt = total;

                paymentGraph.Document.Insert(memoPayment);

                SOAdjust memoOrder = new SOAdjust();
                memoOrder.CustomerID = order.CustomerID;
                memoOrder.AdjdOrderType = setup.OrderType;
                memoOrder.AdjdOrderNbr = order.OrderNbr;

                var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                ordersToApply.SOAdjustments.Insert(memoOrder);

                //paymentGraph.SOAdjustments.Insert(memoOrder);
                paymentGraph.Actions.PressSave();
            }

            //Gift
            total = 0;
            change = 0;
            payment = 0;

            foreach (var giftDetails in _giftDetails)
            {
                if (giftDetails != null)
                {
                    if (giftDetails.Amount > Decimal.Zero)
                    {
                        total += giftDetails.Amount;
                    }
                }
            }

            if (total > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();
                ARPayment giftPayment = new ARPayment();

                giftPayment.DocType = ARDocType.Payment;
                giftPayment.CustomerID = order.CustomerID;
                //giftPayment.PaymentMethodID = setup.GiftPaymentMethodID;
                giftPayment.DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, data.CashierName);
                giftPayment.CuryOrigDocAmt = total;

                paymentGraph.Document.Insert(giftPayment);

                SOAdjust giftOrder = new SOAdjust();
                giftOrder.CustomerID = order.CustomerID;
                giftOrder.AdjdOrderType = setup.OrderType;
                giftOrder.AdjdOrderNbr = order.OrderNbr;

                var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                ordersToApply.SOAdjustments.Insert(giftOrder);

                //paymentGraph.SOAdjustments.Insert(giftOrder);
                paymentGraph.Actions.PressSave();
            }

            return order;
        }

        public static SOOrder CreateOrder(string batchNbr, string type, ATRDBranchSetup setup, List<ATRDSaleDetail> details, List<ATRDSale> saleDetails,
            List<ATRDSaleCashDetail> _cashDetails, List<ATRDSaleCardDetail> _cardDetails, List<ATRDSaleMemoDetail> _memoDetails, List<ATRDSaleGiftDetail> _giftDetails)
        {
            if (details.Count < 1)
                throw new Exception(string.Format(ATRDMessages.NoTransactionToProcess, batchNbr));

            decimal? total = 0;
            decimal? payment = 0;
            decimal? change = 0;

            SOOrderEntry graph = PXGraph.CreateInstance<SOOrderEntry>();

            SOOrder order = new SOOrder();
            order.OrderType = setup.OrderType;


            ATRDBatchSale data = PXSelect<
                ATRDBatchSale,
                Where<ATRDBatchSale.refNbr, Equal<Required<ATRDBatchSale.refNbr>>>>
                .Select(graph, batchNbr);

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
            else if (type == "Exist")
            {
                customerCode = PXSelect<Customer, Where<Customer.acctName, Equal<Required<Customer.acctName>>>>.Select(graph, saleDetails[0].CustomerName);
                order.CustomerID = customerCode.BAccountID;
            }
            else
            {
                order.CustomerID = setup.CustomerID;
            }

            order.OrderDesc = string.Format("POS Batch Transaction : {0} | Cashier : {1}", batchNbr, data.CashierName);
            order.OrderDate = data.Date;
            order.RequestDate = data.Date;

            order = graph.Document.Insert(order);

            //Document Details  
            foreach (ATRDSaleDetail d in details)
            {
                ATRDSale dsale = PXSelect<ATRDSale, Where<ATRDSale.code, Equal<Required<ATRDSale.code>>>>.Select(graph, d.SaleCode);
                if ((d.Amount) <= Decimal.Zero) continue;

                //ATRDSale curSale = saleDetails.RowCast<ATRDSale>().Where(x => x.Code == d.SaleCode).FirstOrDefault();

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
                    line.SiteID = setup.SiteID;
                    line.Qty = d.Qty;
                    line.UOM = d.UnitOfMeasure;
                    line.AlternateID = d.AlternateID;
                    //line.CuryUnitPrice = ((d.Price * d.Qty) / d.Qty);
                    line.CuryUnitPrice = d.Price;
                    line.ManualPrice = true;
                    line.CuryExtPrice = (d.Price * d.Qty);
                    line.CreatedDateTime = d.CreatedDateTime;
                    if (dsale.SalespersonCode != null)
                    {
                        line.SalesPersonID = dsale.SalespersonCode;
                        line.Commissionable = true;
                    }
                    //line.CuryLineAmt = (d.Price * d.Qty);                

                    if (d.DiscountPercent > 0)
                    {
                        line.ManualDisc = true;
                        line.DiscPct = d.DiscountPercent;
                        decimal vat = 1.12m;
                        decimal? discount;
                        decimal? netVat;

                        #region Discount of SC/PWD
                        ATRDInventoryItem rowExt = GetInvetoryItem(graph, line.InventoryID).GetExtension<ATRDInventoryItem>();

                        if (rowExt.UsrATRDIsSeniorPWD.GetValueOrDefault())
                        {
                            //line.CuryExtPrice = (line.CuryUnitPrice - d.VatAmount) * d.Qty;
                            netVat = (line.CuryUnitPrice / vat);
                            discount = (netVat * (d.DiscountPercent / 100));
                            //line.CuryUnbilledAmt = netVat - discount;
                            line.DiscountID = rowExt.UsrATRDDiscountID;
                        }
                        #endregion
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
                ATRDBatchSO so = new ATRDBatchSO();
                so.SaleCode = d.Code;

                graphExt.POSDetail.Insert(so);
            }

            foreach (INItemXRef item in graph.Caches[typeof(INItemXRef)].Updated)
            {
                if (item.CreatedByScreenID == null) item.CreatedByScreenID = item.LastModifiedByScreenID;
                if (item.CreatedByID == null) item.CreatedByID = graph.Accessinfo.UserID;
                if (item.CreatedDateTime == null) item.CreatedDateTime = graph.Accessinfo.BusinessDate;
                graph.Caches[typeof(INItemXRef)].Update(item);
            }

            graph.Actions.PressSave();

            foreach (ATRDSaleCashDetail cashDetails in _cashDetails)
            {
                if (cashDetails != null)
                {
                    if (cashDetails.Amount > Decimal.Zero)
                    {
                        payment = cashDetails.Amount;
                        change = cashDetails.Change;
                        total += (payment - change);
                    }
                }
            }

            DateTime dateTime = Convert.ToDateTime(data.Date);
            string year = dateTime.Year.ToString();
            string qtr = dateTime.Month.ToString();

            if (dateTime.Month.ToString().Length == 1)
            {
                qtr = "0" + dateTime.Month.ToString();
            }

            MasterFinPeriod period = PXSelect<MasterFinPeriod, Where<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                    And<MasterFinPeriod.periodNbr, Equal<Required<MasterFinPeriod.periodNbr>>>>>.Select(graph, year, qtr);

            if (total > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                //DateTime dateTime = Convert.ToDateTime(data.Date);
                //string year = dateTime.Year.ToString();
                //string qtr = dateTime.Month.ToString();

                //if (dateTime.Month.ToString().Length == 1)
                //{
                //    qtr = "0" + dateTime.Month.ToString();
                //}

                //MasterFinPeriod period = PXSelect<MasterFinPeriod, Where<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                //        And<MasterFinPeriod.periodNbr, Equal<Required<MasterFinPeriod.periodNbr>>>>>.Select(graph, year, qtr);

                ARPayment cashPayment = paymentGraph.Document.Insert(new ARPayment()
                {
                    DocType = ARDocType.Payment,
                    CustomerID = order.CustomerID,
                    PaymentMethodID = setup.CashPaymentMethodID,
                    DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, data.CashierName),
                    OrigDocAmt = total,
                    CuryOrigDocAmt = total
                });

                cashPayment.AdjFinPeriodID = period.FinPeriodID;
                cashPayment.AdjDate = data.Date;

                cashPayment = paymentGraph.Document.Update(cashPayment);

                SOAdjust cashOrder = new SOAdjust();
                cashOrder.CustomerID = order.CustomerID;
                cashOrder.AdjdOrderType = setup.OrderType;
                cashOrder.AdjdOrderNbr = order.OrderNbr;
                cashOrder.AdjgDocDate = data.Date;

                var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                ordersToApply.SOAdjustments.Insert(cashOrder);

                paymentGraph.Actions.PressSave();
            }

            //Credit Card
            total = 0;
            change = 0;
            payment = 0;

            #region Individual
            foreach (ATRDSaleCardDetail cardDetails in _cardDetails)
            {
                if (cardDetails != null)
                {
                    if (cardDetails.Amount > Decimal.Zero)
                    {
                        ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                        ARPayment cardPayment = paymentGraph.Document.Insert(new ARPayment()
                        {
                            DocType = ARDocType.Payment,
                            CustomerID = order.CustomerID,
                            PaymentMethodID = setup.CardPaymentMethodID,
                            DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, data.CashierName),
                            OrigDocAmt = cardDetails.Amount,
                            CuryOrigDocAmt = cardDetails.Amount
                        });

                        paymentGraph.Document.Cache.RaiseFieldUpdated<ARPayment.paymentMethodID>(cardPayment, null);

                        cardPayment.AdjFinPeriodID = period.FinPeriodID;
                        cardPayment.AdjDate = data.Date;

                        cardPayment = paymentGraph.Document.Update(cardPayment);

                        var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);

                        SOAdjust cardOrder = ordersToApply.SOAdjustments.Insert(new SOAdjust()
                        //SOAdjust cardOrder = paymentGraph.SOAdjustments.Insert(new SOAdjust()
                        {
                            CustomerID = order.CustomerID,
                            AdjdOrderType = setup.OrderType,
                            AdjdOrderNbr = order.OrderNbr,
                            AdjgDocDate = data.Date
                        });

                        //var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                        ordersToApply.SOAdjustments.Insert(cardOrder);

                        // paymentGraph.SOAdjustments.Insert(cardOrder);

                        paymentGraph.Actions.PressSave();
                    }
                }
            }
            #endregion

            #region Aggregate
            //foreach (ATRDSaleCardDetail cardDetails in _cardDetails)
            //{
            //    if (cardDetails != null)
            //    {
            //        if (cardDetails.Amount > Decimal.Zero)
            //        {
            //            total += cardDetails.Amount;
            //        }
            //    }
            //}

            //if (total > 0)
            //{
            //    ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

            //    ARPayment cardPayment = paymentGraph.Document.Insert(new ARPayment()
            //    {
            //        DocType = ARDocType.Payment,
            //        CustomerID = order.CustomerID,
            //        PaymentMethodID = setup.CardPaymentMethodID,
            //        DocDesc = string.Format("POS Batch Transaction : {0}", batchNbr)
            //        OrigDocAmt = total,
            //        CuryOrigDocAmt = total
            //    });

            //    cardPayment.AdjFinPeriodID = period.FinPeriodID;
            //    cardPayment.AdjDate = data.Date;

            //    cardPayment = paymentGraph.Document.Update(cardPayment);

            //    SOAdjust cardOrder = paymentGraph.SOAdjustments.Insert(new SOAdjust()
            //    {
            //        CustomerID = order.CustomerID,
            //        AdjdOrderType = setup.OrderType,
            //        AdjdOrderNbr = order.RefNbr,
            //        AdjgDocDate = data.Date
            //    });

            //    paymentGraph.SOAdjustments.Insert(cardOrder);

            //    paymentGraph.Actions.PressSave();
            //}
            #endregion

            //Memo
            total = 0;
            change = 0;
            payment = 0;

            foreach (var memoDetails in _memoDetails)
            {
                if (memoDetails != null)
                {
                    if (memoDetails.Amount > Decimal.Zero)
                    {
                        total += memoDetails.Amount;
                    }
                }
            }

            if (total > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();
                ARPayment memoPayment = new ARPayment();

                memoPayment.DocType = ARDocType.Payment;
                memoPayment.CustomerID = order.CustomerID;
                memoPayment.PaymentMethodID = setup.MemoPaymentMethodID;
                memoPayment.DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, data.CashierName);
                memoPayment.CuryOrigDocAmt = total;

                paymentGraph.Document.Insert(memoPayment);

                SOAdjust memoOrder = new SOAdjust();
                memoOrder.CustomerID = order.CustomerID;
                memoOrder.AdjdOrderType = setup.OrderType;
                memoOrder.AdjdOrderNbr = order.OrderNbr;

                var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                ordersToApply.SOAdjustments.Insert(memoOrder);

                //paymentGraph.SOAdjustments.Insert(memoOrder);
                paymentGraph.Actions.PressSave();
            }

            //Gift
            total = 0;
            change = 0;
            payment = 0;

            foreach (var giftDetails in _giftDetails)
            {
                if (giftDetails != null)
                {
                    if (giftDetails.Amount > Decimal.Zero)
                    {
                        total += giftDetails.Amount;
                    }
                }
            }

            if (total > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();
                ARPayment giftPayment = new ARPayment();

                giftPayment.DocType = ARDocType.Payment;
                giftPayment.CustomerID = order.CustomerID;
                //giftPayment.PaymentMethodID = setup.GiftPaymentMethodID;
                giftPayment.DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, data.CashierName);
                giftPayment.CuryOrigDocAmt = total;

                paymentGraph.Document.Insert(giftPayment);

                SOAdjust giftOrder = new SOAdjust();
                giftOrder.CustomerID = order.CustomerID;
                giftOrder.AdjdOrderType = setup.OrderType;
                giftOrder.AdjdOrderNbr = order.OrderNbr;

                var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                ordersToApply.SOAdjustments.Insert(giftOrder);

                //paymentGraph.SOAdjustments.Insert(giftOrder);
                paymentGraph.Actions.PressSave();
            }

            return order;
        }

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

            decimal? total = 0;
            decimal? payment = 0;
            decimal? change = 0;

            SOOrderEntry graph = PXGraph.CreateInstance<SOOrderEntry>();

            SOOrder order = new SOOrder
            {
                OrderType = setup.OrderType
            };

            ATRDBatchSale batch = PXSelect<ATRDBatchSale,
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
            else if (type == "Exist")
            {
                customerCode = PXSelect<Customer, 
                    Where<Customer.acctName, Equal<Required<Customer.acctName>>>>.Select(graph, saleDetails[0].CustomerName);
                order.CustomerID = customerCode.BAccountID;
            }
            else
            {
                order.CustomerID = setup.CustomerID;
            }

            order.OrderDesc = string.Format("POS Batch Transaction : {0} | Cashier : {1}", batchNbr, batch.CashierName);
            order.CustomerRefNbr = batchNbr;
            order.OrderDate = batch.Date;
            order.RequestDate = batch.Date;

            order = graph.Document.Insert(order);

            //Document Details  
            foreach (ATRDSaleDetail d in details)
            {
                ATRDSale dsale = PXSelect<ATRDSale, 
                    Where<ATRDSale.code, Equal<Required<ATRDSale.code>>, 
                        And<ATRDSale.branchID, Equal<Required<ATRDSale.branchID>>>>>.Select(graph, d.SaleCode, d.BranchID);

                if (dsale == null) continue;
                if ((d.Amount) <= Decimal.Zero) continue;

                //ATRDSale curSale = saleDetails.RowCast<ATRDSale>().Where(x => x.Code == d.SaleCode).FirstOrDefault();

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

                        #region Discount of SC/PWD
                        ATRDInventoryItem rowExt = GetInvetoryItem(graph, line.InventoryID).GetExtension<ATRDInventoryItem>();

                        if (rowExt != null && rowExt.UsrATRDIsSeniorPWD.GetValueOrDefault())
                        {
                            netVat = (line.CuryUnitPrice / vat);
                            discount = (netVat * (d.DiscountPercent / 100));
                            line.DiscountID = rowExt.UsrATRDDiscountID;
                        }
                        #endregion
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
                if (item == null) continue;

                if (item.CreatedByScreenID == null) item.CreatedByScreenID = item.LastModifiedByScreenID;
                if (item.CreatedByID == null) item.CreatedByID = graph.Accessinfo.UserID;
                if (item.CreatedDateTime == null) item.CreatedDateTime = graph.Accessinfo.BusinessDate;

                graph.Caches[typeof(INItemXRef)].Update(item);
            }

            graph.Actions.PressSave();

            foreach (ATRDSaleCashDetail cashDetails in _cashDetails)
            {
                if (cashDetails == null) continue;

                if (cashDetails.Amount > Decimal.Zero)
                {
                    payment = cashDetails.Amount;
                    change = cashDetails.Change;
                    total += (payment - change);
                }
            }

            DateTime dateTime = Convert.ToDateTime(batch.Date);
            string year = dateTime.Year.ToString();
            string qtr = dateTime.Month.ToString();

            if (dateTime.Month.ToString().Length == 1)
            {
                qtr = "0" + dateTime.Month.ToString();
            }

            MasterFinPeriod period = PXSelect<MasterFinPeriod, 
                Where<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                    And<MasterFinPeriod.periodNbr, Equal<Required<MasterFinPeriod.periodNbr>>>>>.Select(graph, year, qtr);

            if (total > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                ARPayment cashPayment = paymentGraph.Document.Insert(new ARPayment()
                {
                    DocType = ARDocType.Payment,
                    CustomerID = order.CustomerID,
                    PaymentMethodID = setup.CashPaymentMethodID,
                    DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, batch.CashierName),
                    OrigDocAmt = total,
                    CuryOrigDocAmt = total
                });

                cashPayment.AdjFinPeriodID = period.FinPeriodID;
                cashPayment.AdjDate = batch.Date;

                cashPayment = paymentGraph.Document.Update(cashPayment);

                SOAdjust cashOrder = new SOAdjust
                {
                    CustomerID = order.CustomerID,
                    AdjdOrderType = setup.OrderType,
                    AdjdOrderNbr = order.OrderNbr,
                    AdjgDocDate = batch.Date
                };

                var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                ordersToApply.SOAdjustments.Insert(cashOrder);

                paymentGraph.Actions.PressSave();
            }

            //Credit Card
            total = 0;
            change = 0;
            payment = 0;

            #region Individual
            foreach (ATRDSaleCardDetail cardDetails in _cardDetails)
            {
                if (cardDetails == null) continue;

                if (cardDetails.Amount > Decimal.Zero)
                {
                    ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                    ARPayment cardPayment = paymentGraph.Document.Insert(new ARPayment()
                    {
                        DocType = ARDocType.Payment,
                        CustomerID = order.CustomerID,
                        PaymentMethodID = setup.CardPaymentMethodID,
                        DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, batch.CashierName),
                        OrigDocAmt = cardDetails.Amount,
                        CuryOrigDocAmt = cardDetails.Amount
                    });

                    paymentGraph.Document.Cache.RaiseFieldUpdated<ARPayment.paymentMethodID>(cardPayment, null);

                    cardPayment.AdjFinPeriodID = period.FinPeriodID;
                    cardPayment.AdjDate = batch.Date;

                    cardPayment = paymentGraph.Document.Update(cardPayment);

                    var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);

                    SOAdjust cardOrder = ordersToApply.SOAdjustments.Insert(new SOAdjust()
                    {
                        CustomerID = order.CustomerID,
                        AdjdOrderType = setup.OrderType,
                        AdjdOrderNbr = order.OrderNbr,
                        AdjgDocDate = batch.Date
                    });

                    ordersToApply.SOAdjustments.Insert(cardOrder);

                    paymentGraph.Actions.PressSave();
                }
            }
            #endregion

            #region Aggregate
            //foreach (ATRDSaleCardDetail cardDetails in _cardDetails)
            //{
            //    if (cardDetails != null)
            //    {
            //        if (cardDetails.Amount > Decimal.Zero)
            //        {
            //            total += cardDetails.Amount;
            //        }
            //    }
            //}

            //if (total > 0)
            //{
            //    ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

            //    ARPayment cardPayment = paymentGraph.Document.Insert(new ARPayment()
            //    {
            //        DocType = ARDocType.Payment,
            //        CustomerID = order.CustomerID,
            //        PaymentMethodID = setup.CardPaymentMethodID,
            //        DocDesc = string.Format("POS Batch Transaction : {0}", batchNbr)
            //        OrigDocAmt = total,
            //        CuryOrigDocAmt = total
            //    });

            //    cardPayment.AdjFinPeriodID = period.FinPeriodID;
            //    cardPayment.AdjDate = data.Date;

            //    cardPayment = paymentGraph.Document.Update(cardPayment);

            //    SOAdjust cardOrder = paymentGraph.SOAdjustments.Insert(new SOAdjust()
            //    {
            //        CustomerID = order.CustomerID,
            //        AdjdOrderType = setup.OrderType,
            //        AdjdOrderNbr = order.OrderNbr,
            //        AdjgDocDate = data.Date
            //    });

            //    paymentGraph.SOAdjustments.Insert(cardOrder);

            //    paymentGraph.Actions.PressSave();
            //}
            #endregion

            //Memo
            total = 0;
            change = 0;
            payment = 0;

            foreach (var memoDetails in _memoDetails)
            {
                if (memoDetails == null) continue;

                if (memoDetails.Amount > Decimal.Zero)
                {
                    total += memoDetails.Amount;
                }
            }

            if (total > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                ARPayment memoPayment = new ARPayment
                {
                    DocType = ARDocType.Payment,
                    CustomerID = order.CustomerID,
                    PaymentMethodID = setup.MemoPaymentMethodID,
                    DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, batch.CashierName),
                    CuryOrigDocAmt = total
                };

                paymentGraph.Document.Insert(memoPayment);

                SOAdjust memoOrder = new SOAdjust
                {
                    CustomerID = order.CustomerID,
                    AdjdOrderType = setup.OrderType,
                    AdjdOrderNbr = order.OrderNbr
                };

                var ordersToApply = paymentGraph.GetOrdersToApplyTabExtension(true);
                ordersToApply.SOAdjustments.Insert(memoOrder);

                paymentGraph.Actions.PressSave();
            }

            //Gift
            total = 0;
            change = 0;
            payment = 0;

            foreach (var giftDetails in _giftDetails)
            {
                if (giftDetails == null) continue;

                if (giftDetails.Amount > Decimal.Zero)
                {
                    total += giftDetails.Amount;
                }
            }

            if (total > 0)
            {
                ARPaymentEntry paymentGraph = PXGraph.CreateInstance<ARPaymentEntry>();

                ARPayment giftPayment = new ARPayment
                {
                    DocType = ARDocType.Payment,
                    CustomerID = order.CustomerID,
                    DocDesc = string.Format("POS Batch Transaction : {0} | Cashier Name : {1}", batchNbr, batch.CashierName),
                    CuryOrigDocAmt = total
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

        private static MasterFinPeriod GetPeriod(
            PXGraph graph,
            string PeriodNbr, 
            string FinYear) =>
                PXSelect<MasterFinPeriod, Where<MasterFinPeriod.finYear, Equal<Required<MasterFinPeriod.finYear>>,
                    And<MasterFinPeriod.periodNbr, Equal<Required<MasterFinPeriod.periodNbr>>>>>.Select(graph, FinYear, PeriodNbr);

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

        public static InventoryItem GetInvetoryItem(PXGraph graph, int? InventoryID)
            => PXSelectReadonly<InventoryItem, Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(graph, InventoryID);
    }

    //public struct POSSetup
    //{
    //    public bool IsBranchSetup;
    //    public int? CustomerPWDID;
    //    public int? CustomerSCID;
    //    public int? CustomerID;
    //    public int? SiteID;
    //    public string CashPaymentMethodID;
    //    public string MemoPaymentMethodID;
    //    public string CardPaymentMethodID;
    //    public string OrderType;
    //}
}
