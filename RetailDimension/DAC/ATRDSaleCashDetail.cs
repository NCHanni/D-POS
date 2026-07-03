using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.GL;
using RetailDimension.DAC.Base;
using RetailDimension.Graph;
using static RetailDimension.Attributes.Base.AggregateAttribute;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("RD-Sale-Cash-Detail")]
    [PXPrimaryGraph(typeof(ATRDSaleEntry))]
    public class ATRDSaleCashDetail : Table, IBqlTable
    {
        #region Id
        [PXDBLongIdentity(IsKey = true)]
        public virtual long? Id { get; set; }
        public abstract class id : BqlLong.Field<id> { }
        #endregion

        #region SaleCode
        //sale_code varchar(12) 
        [PXDBString(12, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.SaleCode)]
        [PXDBDefault(typeof(ATRDSale.code))]
        [PXParent(typeof(Select<ATRDSale, Where<ATRDSale.code, Equal<Current<ATRDSaleCashDetail.saleCode>>>>))]
        public virtual string SaleCode { get; set; }
        public abstract class saleCode : BqlString.Field<saleCode> { }
        #endregion

        #region BranchID
        [Branch(DisplayName = ATRDMessages.Branch)]
        [PXUIField(Enabled = false)]
        [PXDBDefault(typeof(ATRDSale.branchID))]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion

        #region Amount
        //amount numeric(18,2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.Amount)]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : BqlDecimal.Field<amount> { }
        #endregion

        #region Change
        //amount numeric(18,2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.Change)]
        public virtual decimal? Change { get; set; }
        public abstract class change : BqlDecimal.Field<change> { }
        #endregion

        #region Unbound

        #region PaidAmount
        [PXDecimal(2)]
        [PXFormula(typeof(Sub<amount,change>))]
        public virtual decimal? PaidAmount { get; set; }
        public abstract class paidAmount : BqlDecimal.Field<paidAmount> { }
        #endregion

        #endregion
    }
}
