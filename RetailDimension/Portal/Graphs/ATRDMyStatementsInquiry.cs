using PX.Data;
using PX.Data.BQL;
using PX.Objects.AR;
using PX.Objects.AR.Repositories;
using PX.Objects.CM;
using PX.Objects.CR;
using PX.Objects.GL;
using PX.SM;
using RetailDimension.DAC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RetailDimension.Portal.Graphs
{
    /// <summary>
    /// SCREEN ID: ATRD4011
    /// </summary>
    public class ATRDMyStatementsInquiry : PXGraph<ATRDMyStatementsInquiry>
    {
        #region Constructor

        public CustomerRepository CustomerRepository;

        public ATRDMyStatementsInquiry()
        {
            CustomerRepository = new CustomerRepository(this);
        }
        #endregion

        #region Actions & Delegates

        public PXCancel<ATRDMyStatementsFilter> Cancel;

        public PXAction<ATRDMyStatementsFilter> PrintInvoiceMemo;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = ATRDMessages.PrintInvoiceMemo)]
        public IEnumerable printInvoiceMemo(PXAdapter adapter)
        {
            return adapter.Get();
        }

        #endregion

        #region Data Views & Delegates
        public PXFilter<ATRDMyStatementsFilter> Filter;
        [PXFilterable]
        public PXSelect<DetailsResult> StatementTransactions;
        public IEnumerable statementTransactions()
        {
            ATRDMyStatementsFilter header = Filter.Current;

            Dictionary<DetailKey, DetailsResult> result = new Dictionary<DetailKey, DetailsResult>(EqualityComparer<DetailKey>.Default);

            List<DetailsResult> curyResult = new List<DetailsResult>();

            if (header == null)
            {
                return curyResult;
            }

            Customer customer = CustomerRepository.FindByID(header.CustomerID);

            int? branchID = GetBranchID();

            if (customer != null)
            {
                bool useCurrency = customer.PrintCuryStatements ?? false;
                Company company = PXSelect<Company>.Select(this);

                PXSelectBase<ARStatement> sel = new PXSelect<ARStatement,
                       Where<ARStatement.statementCustomerID, Equal<Required<ARStatement.customerID>>>,
                       OrderBy<Asc<ARStatement.statementCycleId, Asc<ARStatement.statementDate, Asc<ARStatement.curyID>>>>>(this);



                if (branchID != null)
                {
                    sel.WhereAnd<Where<ARStatement.branchID, Equal<Required<ARStatement.branchID>>>>();
                }

                if (header.FromDate != null || header.ToDate != null) 
                {
                    sel.WhereAnd<Where<ARStatement.statementDate, Between<Required<ARStatement.statementDate>, Required<ARStatement.statementDate>>>>();
                }

                foreach (ARStatement st in sel.Select(header.CustomerID, branchID, header.FromDate, header.ToDate))
                {
                    DetailsResult res = new DetailsResult()
                    {
                        StatementCycleId = st.StatementCycleId,
                        StatementDate = st.StatementDate,
                        StatementBalance = st.EndBalance ?? decimal.Zero,
                        AgeBalance00 = st.AgeBalance00 ?? decimal.Zero,
                        CuryID = st.CuryID,
                        CuryStatementBalance = st.CuryEndBalance ?? decimal.Zero,
                        CuryAgeBalance00 = st.CuryAgeBalance00 ?? decimal.Zero,
                        DontPrint = st.DontPrint,
                        Printed = st.Printed,
                        DontEmail = st.DontEmail,
                        Emailed = st.Emailed,
                        BranchID = st.BranchID,
                        OnDemand = st.OnDemand,
                        PreparedOn = st.LastModifiedDateTime
                    };

                    if (useCurrency)
                    {
                        DetailsResult last = curyResult.Count > 0 ? curyResult[curyResult.Count - 1] : null;

                        if (last != null
                            && last.StatementCycleId == res.StatementCycleId
                            && last.StatementDate == res.StatementDate && last.CuryID == res.CuryID)
                        {
                            last.StatementBalance += res.StatementBalance;
                            last.AgeBalance00 += res.AgeBalance00;
                            if (last.CuryID == res.CuryID)
                            {
                                last.CuryStatementBalance += res.CuryStatementBalance;
                                last.CuryAgeBalance00 += res.CuryAgeBalance00;
                            }
                            else
                            {
                                last.CuryStatementBalance = Decimal.Zero;
                                last.CuryAgeBalance00 = Decimal.Zero;
                            }

                            if (res.DontPrint == false)
                                last.DontPrint = false;
                            if (res.DontEmail == false)
                                last.DontEmail = false;
                            if (res.Printed == false)
                                last.Printed = false;
                            if (res.Emailed == false)
                                last.Emailed = false;
                        }
                        else
                        {
                            curyResult.Add(res);
                        }
                    }
                    else
                    {
                        DetailKey key = new DetailKey(res.StatementDate.Value, res.StatementCycleId, res.BranchID.Value);
                        res.CuryID = company.BaseCuryID;
                        res.CuryStatementBalance = res.StatementBalance;
                        res.CuryAgeBalance00 = res.AgeBalance00;
                        if (!result.ContainsKey(key))
                        {
                            result[key] = res;
                        }
                        else
                        {
                            result[key].StatementBalance += res.StatementBalance;
                            result[key].AgeBalance00 += res.AgeBalance00;
                            if (result[key].CuryID == res.CuryID)
                            {
                                result[key].CuryStatementBalance += res.CuryStatementBalance;
                                result[key].CuryAgeBalance00 += res.CuryAgeBalance00;
                            }
                            else
                            {
                                result[key].CuryStatementBalance = Decimal.Zero;
                                result[key].CuryAgeBalance00 = Decimal.Zero;
                            }

                            if (res.DontPrint == false)
                                result[key].DontPrint = false;
                            if (res.DontEmail == false)
                                result[key].DontEmail = false;
                            if (res.Printed == false)
                                result[key].Printed = false;
                            if (res.Emailed == false)
                                result[key].Emailed = false;
                        }
                    }
                }

                return useCurrency ? (curyResult as IEnumerable) : (result.Values as IEnumerable);
            }

            return curyResult;
        }
        //public PXSelectReadonly2<ATRDPortalStatement,
        //                    InnerJoin<ATRDPortalAccount, On<ATRDPortalAccount.customerID, Equal<ATRDPortalStatement.customerID>>,
        //                        InnerJoin<Users, On<Users.pKID, Equal<ATRDPortalAccount.userID>>>>,
        //                            Where<Users.pKID, Equal<Current<AccessInfo.userID>>,
        //                                And<ATRDPortalStatement.statementDate, Between<Current<ATRDMyStatementsFilter.fromDate>, Current<ATRDMyStatementsFilter.toDate>>>>> StatementTransactions;
        [PXFilterable]
        public PXSelect<ATRDInvoiceDetails> InvoiceTransactions;
        public IEnumerable invoiceTransactions()
        {
            int? customerID = GetCustomerID();

            ATRDMyStatementsFilter header = Filter.Current;

            var invoices = GetInvoices(customerID, header.FromDate, header.ToDate);

            var payments = GetPayments(customerID, header.FromDate, header.ToDate);

            int count = 1;

            List<ATRDInvoiceDetails> result = new List<ATRDInvoiceDetails>();

            foreach (ARInvoice item in invoices.RowCast<ARInvoice>().ToList())
            {
                ATRDInvoiceDetails row = new ATRDInvoiceDetails
                {
                    LineID = count,
                    RefNbr = item.RefNbr,
                    Type = "I",
                    Description = item.DocDesc,
                    Status = item.Status,
                    Balance = item.CuryDocBal.GetValueOrDefault(),
                    DocumentTotal = item.CuryOrigDocAmt.GetValueOrDefault(),
                    TransactionDate = item.DocDate
                };

                if (!result.Contains(row))
                    result.Add(row);

                count++;
            }

            foreach (ARPayment item in payments.RowCast<ARPayment>().ToList())
            {
                ATRDInvoiceDetails row = new ATRDInvoiceDetails
                {
                    LineID = count,
                    RefNbr = item.RefNbr,
                    Type = "P",
                    Description = item.DocDesc,
                    Status = item.Status,
                    Balance = item.CuryUnappliedBal.GetValueOrDefault(),
                    DocumentTotal = item.CuryOrigDocAmt.GetValueOrDefault(),
                    TransactionDate = item.DocDate
                };

                if (!result.Contains(row))
                    result.Add(row);

                count++;
            }

            foreach (ATRDInvoiceDetails item in result.OrderByDescending(_ => _.RefNbr))
            {
                yield return item;
            }
        }
        #endregion

        #region Internal Types

        #region Filter
        [Serializable]
        [PXCacheName("My Statements Filter")]
#if Version23R1
    public partial class ATRDMyStatementsFilter : IBqlTable
#endif
#if Version25R1 || Version25R2
        public partial class ATRDMyStatementsFilter : PXBqlTable, IBqlTable
#endif
        {
            public abstract class customerID : BqlInt.Field<customerID> { }
            [CustomerActive(DisplayName = ATRDMessages.Customer, Visible = false, IsKey = true)]
            [PXUnboundDefault(typeof(Search<ATRDPortalAccount.customerID,
                                        Where<ATRDPortalAccount.userID,
                                        Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(typeof(Default<AccessInfo.userID>))]
            public virtual int? CustomerID { get; set; }

            public abstract class fromDate : BqlDateTime.Field<fromDate> { }
            [PXDate]
            [PXUIField(DisplayName = ATRDMessages.FromDate)]
            [PXUnboundDefault(typeof(AccessInfo.businessDate))]
            public virtual DateTime? FromDate { get; set; }

            public abstract class toDate : BqlDateTime.Field<toDate> { }
            [PXDate]
            [PXUIField(DisplayName = ATRDMessages.ToDate)]
            [PXUnboundDefault(typeof(AccessInfo.businessDate))]
            public virtual DateTime? ToDate { get; set; }

            public abstract class creditLimit : BqlDecimal.Field<creditLimit> { }
            [PXDecimal(2)]
            [PXUIField(DisplayName = ATRDMessages.CreditLimit, Enabled = false)]
            [PXUnboundDefault(typeof(Selector<customerID, Customer.creditLimit>), PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(typeof(Default<customerID>))]
            public virtual decimal? CreditLimit { get; set; }
        }

        #endregion

        #region Invoices & Payments

        #region Details
        [Serializable]
        [PXCacheName("Invoice Details")]
#if Version23R1
    public partial class ATRDInvoiceDetails : IBqlTable
#endif
#if Version25R1 || Version25R2
        public partial class ATRDInvoiceDetails : PXBqlTable, IBqlTable
#endif
        {
            #region LineID
            [PXInt(MinValue = 1, IsKey = true)]
            public virtual int? LineID { get; set; }
            public abstract class lineID : BqlInt.Field<lineID> { }
            #endregion
            #region Type
            [PXString(1, IsFixed = true)]
            [PXStringList(new string[]
            {
                "P",
                "I"
            }, new string[]
            {
                "Payments",
                "Invoices"
            })]
            [PXUIField(DisplayName = ATRDMessages.Type, Enabled = false)]
            public virtual string Type { get; set; }
            public abstract class type : BqlString.Field<type> { }
            #endregion
            #region RefNbr
            [PXString(15, IsKey = true)]
            [PXUIField(DisplayName = ATRDMessages.RefNbr, Enabled = false)]
            public virtual string RefNbr { get; set; }
            public abstract class refNbr : BqlString.Field<refNbr> { }
            #endregion
            #region Status
            [PXString(1, IsFixed = true)]
            [ARDocStatus.List]
            [PXUIField(DisplayName = ATRDMessages.Status, Enabled = false)]
            public virtual string Status { get; set; }
            public abstract class status : BqlString.Field<status> { }
            #endregion
            #region TransactionDate
            [PXDate]
            [PXUIField(DisplayName = ATRDMessages.Date, Enabled = false)]
            public virtual DateTime? TransactionDate { get; set; }
            public abstract class transactionDate : BqlDateTime.Field<transactionDate> { }
            #endregion
            #region Description
            [PXString(255)]
            [PXUIField(DisplayName = ATRDMessages.Description, Enabled = false)]
            public virtual string Description { get; set; }
            public abstract class description : BqlString.Field<description> { }
            #endregion
            #region DocumentTotal
            [PXDecimal(2)]
            [PXUIField(DisplayName = ATRDMessages.DocumentTotal, Enabled = false)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? DocumentTotal { get; set; }
            public abstract class documentTotal : BqlDecimal.Field<documentTotal> { }
            #endregion
            #region Balance
            [PXDecimal(2)]
            [PXUIField(DisplayName = ATRDMessages.Balance, Enabled = false)]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual decimal? Balance { get; set; }
            public abstract class balance : BqlDecimal.Field<balance> { }
            #endregion
        }
        #endregion
        #endregion

        #region Statements
        [Serializable]
        [PXHidden]
        public class ATRDPortalStatement : ARStatement, IBqlTable
        {
            #region Outstanding Balance
            public abstract class outstandingBalance : IBqlField { }
            [PXDecimal(2)]
            [PXUIField(DisplayName = "Outstanding Balance", Enabled = false)]
            [PXFormula(typeof(Sub<ARStatement.endBalance, ARStatement.ageBalance00>))]
            [PXDefault()]
            public decimal? OutstandingBalance { get; set; }
            #endregion
        }

        [Serializable]
        [PXHidden]
#if Version23R1
    public partial class DetailsResult : IBqlTable
#endif
#if Version25R1 || Version25R2
        public partial class DetailsResult : PXBqlTable, IBqlTable
#endif
        {
            #region StatementCycleId
            public abstract class statementCycleId : PX.Data.BQL.BqlString.Field<statementCycleId> { }
            protected String _StatementCycleId;
            [PXString(10, IsUnicode = true)]
            [PXUIField(DisplayName = "Statement Cycle")]
            [PXSelector(typeof(ARStatementCycle.statementCycleId))]
            public virtual String StatementCycleId
            {
                get
                {
                    return this._StatementCycleId;
                }
                set
                {
                    this._StatementCycleId = value;
                }
            }
            #endregion
            #region StatementDate
            public abstract class statementDate : PX.Data.BQL.BqlDateTime.Field<statementDate> { }
            protected DateTime? _StatementDate;
            [PXDate(IsKey = true)]
            [PXUnboundDefault]
            [PXUIField(DisplayName = "Statement Date")]
            [PXSelector(typeof(Search4<ARStatement.statementDate, Aggregate<GroupBy<ARStatement.statementDate>>>))]
            public virtual DateTime? StatementDate
            {
                get
                {
                    return this._StatementDate;
                }
                set
                {
                    this._StatementDate = value;
                }
            }
            #endregion
            #region StatementBalance
            public abstract class statementBalance : PX.Data.BQL.BqlDecimal.Field<statementBalance> { }
            protected Decimal? _StatementBalance;
            [PXDBBaseCury()]
            [PXUIField(DisplayName = "Statement Balance")]
            public virtual Decimal? StatementBalance
            {
                get
                {
                    return this._StatementBalance;
                }
                set
                {
                    this._StatementBalance = value;
                }
            }
            #endregion
            #region CuryID
            public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID> { }
            protected String _CuryID;
            [PXDBString(5, IsUnicode = true, InputMask = ">LLLLL")]
            [PXUIField(DisplayName = "Currency", Visibility = PXUIVisibility.SelectorVisible)]
            public virtual String CuryID
            {
                get
                {
                    return this._CuryID;
                }
                set
                {
                    this._CuryID = value;
                }
            }
            #endregion
            #region CuryStatementBalance
            public abstract class curyStatementBalance : PX.Data.BQL.BqlDecimal.Field<curyStatementBalance> { }
            protected Decimal? _CuryStatementBalance;
            [PXCury(typeof(DetailsResult.curyID))]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = "FC Statement Balance")]
            public virtual Decimal? CuryStatementBalance
            {
                get
                {
                    return this._CuryStatementBalance;
                }
                set
                {
                    this._CuryStatementBalance = value;
                }
            }
            #endregion
            #region DontPrint
            public abstract class dontPrint : PX.Data.BQL.BqlBool.Field<dontPrint> { }
            protected Boolean? _DontPrint;
            [PXBool()]
            [PXUnboundDefault(true, PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = "Don't Print")]
            public virtual Boolean? DontPrint
            {
                get
                {
                    return this._DontPrint;
                }
                set
                {
                    this._DontPrint = value;
                }
            }
            #endregion
            #region Printed
            public abstract class printed : PX.Data.BQL.BqlBool.Field<printed> { }
            protected Boolean? _Printed;
            [PXBool()]
            [PXUnboundDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = "Printed")]
            public virtual Boolean? Printed
            {
                get
                {
                    return this._Printed;
                }
                set
                {
                    this._Printed = value;
                }
            }
            #endregion
            #region DontEmail
            public abstract class dontEmail : PX.Data.BQL.BqlBool.Field<dontEmail> { }
            protected Boolean? _DontEmail;
            [PXDBBool()]
            [PXDefault(true, PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = "Don't Email")]
            public virtual Boolean? DontEmail
            {
                get
                {
                    return this._DontEmail;
                }
                set
                {
                    this._DontEmail = value;
                }
            }
            #endregion
            #region Emailed
            public abstract class emailed : PX.Data.BQL.BqlBool.Field<emailed> { }
            protected Boolean? _Emailed;
            [PXDBBool()]
            [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = "Emailed")]
            public virtual Boolean? Emailed
            {
                get
                {
                    return this._Emailed;
                }
                set
                {
                    this._Emailed = value;
                }
            }
            #endregion
            #region AgeBalance00
            public abstract class ageBalance00 : PX.Data.BQL.BqlDecimal.Field<ageBalance00> { }
            protected Decimal? _AgeBalance00;
            [PXDBBaseCury()]
            [PXDefault(TypeCode.Decimal, "0.0")]
            [PXUIField(DisplayName = "Age00 Balance")]
            public virtual Decimal? AgeBalance00
            {
                get
                {
                    return this._AgeBalance00;
                }
                set
                {
                    this._AgeBalance00 = value;
                }
            }
            #endregion
            #region CuryAgeBalance00
            public abstract class curyAgeBalance00 : PX.Data.BQL.BqlDecimal.Field<curyAgeBalance00> { }
            protected Decimal? _CuryAgeBalance00;
            [PXCury(typeof(DetailsResult.curyID))]
            [PXUnboundDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
            [PXUIField(DisplayName = "FC Age00 Balance")]
            public virtual Decimal? CuryAgeBalance00
            {
                get
                {
                    return this._CuryAgeBalance00;
                }
                set
                {
                    this._CuryAgeBalance00 = value;
                }
            }
            #endregion
            #region OverdueBalance
            public abstract class overdueBalance : PX.Data.BQL.BqlDecimal.Field<overdueBalance> { }
            [PXBaseCury()]
            [PXUIField(DisplayName = "Overdue Balance")]
            public virtual Decimal? OverdueBalance
            {
                [PXDependsOnFields(typeof(statementBalance), typeof(ageBalance00))]
                get
                {
                    return this.StatementBalance - this.AgeBalance00;
                }
            }
            #endregion
            #region CuryOverdueBalance
            public abstract class curyOverdueBalance : PX.Data.BQL.BqlDecimal.Field<curyOverdueBalance> { }
            [PXCury(typeof(DetailsResult.curyID))]
            [PXUIField(DisplayName = "FC Overdue Balance")]
            public virtual Decimal? CuryOverdueBalance
            {
                [PXDependsOnFields(typeof(curyStatementBalance), typeof(curyAgeBalance00))]
                get
                {
                    return (this._CuryStatementBalance ?? Decimal.Zero) - (this.CuryAgeBalance00 ?? Decimal.Zero);
                }
            }
            #endregion
            #region BranchID
            public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
            protected Int32? _BranchID;
            [Branch(IsKey = true, Required = false, PersistingCheck = PXPersistingCheck.Nothing)]
            public virtual Int32? BranchID
            {
                get
                {
                    return this._BranchID;
                }
                set
                {
                    this._BranchID = value;
                }
            }
            #endregion
            #region OnDemand
            public abstract class onDemand : PX.Data.BQL.BqlBool.Field<onDemand> { }
            [PXBool]
            [PXUIField(DisplayName = PX.Objects.AR.Messages.OnDemandStatement)]
            public virtual bool? OnDemand
            {
                get;
                set;
            }
            #endregion
            #region PreparedOn
            public abstract class preparedOn : PX.Data.BQL.BqlDateTime.Field<preparedOn> { }
            [PXDate]
            [PXUIField(DisplayName = PX.Objects.AR.Messages.PreparedOn)]
            public virtual DateTime? PreparedOn
            {
                get;
                set;
            }
            #endregion
        }

        public class DetailKey : PX.Objects.AP.Triplet<DateTime, string, Int32>, IEquatable<DetailKey>
        {
            public DetailKey(DateTime aFirst, string aSecond, Int32 aThird)
                : base(aFirst, aSecond, aThird)
            {

            }

            #region IComparable<CashAcctKey> Members


            public override int GetHashCode()
            {
                return (this.first.GetHashCode()) ^ (this.second.GetHashCode()) ^ (this.third.GetHashCode()); //Force to call the CompareTo methods in dicts
            }
            #endregion

            #region IEquatable<DetailKey> Members

            public virtual bool Equals(DetailKey other)
            {
                return base.Equals(other);
            }

            #endregion
        }
        #endregion

        #endregion

        #region Methods

        public PXResultset<ARInvoice> GetInvoices(int? customerID, DateTime? fromDate, DateTime? toDate)
        {
            return PXSelect<ARInvoice, Where<ARInvoice.customerID, Equal<Required<ARInvoice.customerID>>,
                        And<ARInvoice.docDate, Between<Required<ARInvoice.docDate>, Required<ARInvoice.docDate>>>>>.Select(this, customerID, fromDate, toDate);
        }

        public int? GetCustomerID()
        {
            ATRDPortalAccount account = PXSelect<ATRDPortalAccount, Where<ATRDPortalAccount.userID, Equal<Current<AccessInfo.userID>>>>.Select(this);

            return account.CustomerID;
        }

        public int? GetBranchID()
        {
            ATRDPortalAccount account = PXSelect<ATRDPortalAccount, Where<ATRDPortalAccount.userID, Equal<Current<AccessInfo.userID>>>>.Select(this);

            return account.BranchID;
        }

        public PXResultset<ARPayment> GetPayments(int? customerID, DateTime? fromDate, DateTime? toDate)
        {
            return PXSelect<ARPayment, Where<ARPayment.customerID, Equal<Required<ARPayment.customerID>>,
                        And<ARPayment.docDate, Between<Required<ARPayment.docDate>, Required<ARPayment.docDate>>>>>.Select(this, customerID, fromDate, toDate);
        }

        #endregion
    }

}