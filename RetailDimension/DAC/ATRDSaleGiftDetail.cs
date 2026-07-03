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
    [PXCacheName("RD-Sale-Gift-Detail")]
    [PXPrimaryGraph(typeof(ATRDSaleEntry))]
    public class ATRDSaleGiftDetail : Table, IBqlTable
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
        [PXParent(typeof(Select<ATRDSale, Where<ATRDSale.code, Equal<Current<ATRDSaleGiftDetail.saleCode>>>>))]
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

        #region GcCode
        //gc_code varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.GcCode)]
        public virtual string GcCode { get; set; }
        public abstract class gcCode : BqlString.Field<gcCode> { }
        #endregion

        #region Description
        //description varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Description)]
        public virtual string Description { get; set; }
        public abstract class description : BqlString.Field<description> { }
        #endregion 

        #region Amount
        //amount numeric(18,2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.Amount)]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : BqlDecimal.Field<amount> { }
        #endregion
    }
}
