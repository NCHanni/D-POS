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
    [PXCacheName("RD-Sale-Card-Detail")]
    [PXPrimaryGraph(typeof(ATRDSaleEntry))]
    public class ATRDSaleCardDetail : Table, IBqlTable
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
        [PXParent(typeof(Select<ATRDSale, Where<ATRDSale.code, Equal<Current<ATRDSaleCardDetail.saleCode>>>>))]
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

        #region TypeCode
        //type_code varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.TypeCode)]
        public virtual string TypeCode { get; set; }
        public abstract class typeCode : BqlString.Field<typeCode> { }
        #endregion

        #region TypeName
        //type_name varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.TypeName)]
        public virtual string TypeName { get; set; }
        public abstract class typeName : BqlString.Field<typeName> { }
        #endregion

        #region CardNo
        //card_no varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CardNo)]
        public virtual string CardNo { get; set; }
        public abstract class cardNo : BqlString.Field<cardNo> { }
        #endregion

        #region CardHolder
        //card_holder varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CardHolder)]
        public virtual string CardHolder { get; set; }
        public abstract class cardHolder : BqlString.Field<cardHolder> { }
        #endregion

        #region BankName
        //bank_name varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.BankName)]
        public virtual string BankName { get; set; }
        public abstract class bankName : BqlString.Field<bankName> { }
        #endregion

        #region ApprovalCode
        //approval_code varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.ApprovalCode)]
        public virtual string ApprovalCode { get; set; }
        public abstract class approvalCode : BqlString.Field<approvalCode> { }
        #endregion

        #region Amount
        //amount numeric(18, 2)
        [ATRDDecimal]
        [PXUIField(DisplayName = ATRDMessages.Amount)]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : BqlDecimal.Field<amount> { }
        #endregion

    }
}
