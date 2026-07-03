using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CS;
using PX.Objects.GL;
using RetailDimension.DAC.Base;
using RetailDimension.Graph;
using System;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("RD-Batch")]
    [PXPrimaryGraph(typeof(ATRDSaleEntry))]
    public class ATRDBatchSale : Table, IBqlTable
    {
        #region RefNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ATRDMessages.C15)]
        [PXUIField(DisplayName = ATRDMessages.BatchRefNbr)]
        [PXSelector(typeof(Search<refNbr>))]
        [PXRestrictor(typeof(Where<branchID.IsEqual<AccessInfo.branchID.FromCurrent>>),
            ATRDMessages.RestrictBatchByBranch)]
        [AutoNumber(typeof(ATRDSetup.numberSequenceID), typeof(AccessInfo.businessDate))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region TransactionDate
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.TransactionDate)]
        [PXDefault(typeof(AccessInfo.businessDate))]
        public virtual DateTime? Date { get; set; }
        public abstract class date : BqlDateTime.Field<date> { }
        #endregion

        #region SyncDate
        [PXDBDateAndTime]
        [PXUIField(DisplayName = ATRDMessages.SyncDate, Enabled = false)]
        public virtual DateTime? SyncDate { get; set; }
        public abstract class syncDate : BqlDateTime.Field<syncDate> { }
        #endregion

        #region CashierName
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CashierName)]
        public virtual string CashierName { get; set; }
        public abstract class cashierName : BqlString.Field<cashierName> { }
        #endregion

        #region OrderNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.OrderNbr, Enabled = false)]
        public virtual string OrderNbr { get; set; }
        public abstract class orderNbr : BqlString.Field<orderNbr> { }
        #endregion

        #region BranchID
        [Branch(DisplayName = ATRDMessages.Branch)]
        [PXUIField(Enabled = false)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion

        #region BranchCD
        [PXDBString(30, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.BranchCD, Visible = false)]
        public virtual string BranchCD { get; set; }   
        public abstract class branchCD : BqlString.Field<branchCD> { }
        #endregion

        #region BatchTotalAmount
        [PXDecimal]
        [PXUnboundDefault(TypeCode.Decimal, "0.0",
            typeof(SelectFrom<ATRDSale>
                        .Where<ATRDSale.batchRefNbr.IsEqual<refNbr.FromCurrent>
                        .And<ATRDSale.branchID.IsEqual<branchID.FromCurrent>>>
            .AggregateTo<Sum<ATRDSale.totalAmount>>
            .SearchFor<ATRDSale.totalAmount>))]
        [PXUIField(DisplayName = ATRDMessages.TotalAmount, Enabled = false)]
        public virtual decimal? BatchTotalAmount { get; set; }
        public abstract class batchTotalAmount : BqlDecimal.Field<batchTotalAmount> { }
        #endregion

        #region Unbound
        #region Selected
        [PXBool]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : BqlBool.Field<selected> { }
        #endregion
        #endregion
    }
}
