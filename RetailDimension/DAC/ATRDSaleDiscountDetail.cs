using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.GL;
using RetailDimension.DAC.Base;
using RetailDimension.Graph;
using static RetailDimension.Attributes.Base.AggregateAttribute;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("RD-Sale-Discount-Detail")]
    [PXPrimaryGraph(typeof(ATRDSaleEntry))]
    public class ATRDSaleDiscountDetail : Table, IBqlTable
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
        [PXParent(typeof(Select<ATRDSale, Where<ATRDSale.code, Equal<Current<ATRDSaleDiscountDetail.saleCode>>>>))]
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

        #region DiscountType
        //discount_type varchar(3)
        [PXDBString(3, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.DiscountType)]
        public virtual string DiscountType { get; set; }
        public abstract class discountType : BqlString.Field<discountType> { }
        #endregion

        #region IdNo
        //id_no varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.IdNo)]
        public virtual string IdNo { get; set; }
        public abstract class idNo : BqlString.Field<idNo> { }
        #endregion

        #region Name
        //name varchar(255)
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Name)]
        public virtual string Name { get; set; }
        public abstract class name : BqlString.Field<name> { }
        #endregion

        #region Gender
        //gender varchar(6)
        [PXDBString(6, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Gender)]
        public virtual string Gender { get; set; }
        public abstract class gender : BqlString.Field<gender> { }
        #endregion

        #region BirthDate
        //birthdate date
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.BirthDate)]
        public virtual DateTime? BirthDate { get; set; }
        public abstract class birthDate : BqlDateTime.Field<birthDate> { }
        #endregion

        #region IssuedDate
        //issued_date date
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.IssuedDate)]
        public virtual DateTime? IssuedDate { get; set; }
        public abstract class issueDate : BqlDateTime.Field<issueDate> { }
        #endregion

        #region TotalDiscount
        //total_discount numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.TotalDiscount)]
        public virtual decimal? TotalDiscount { get; set; }
        public abstract class totalDiscount : BqlDecimal.Field<totalDiscount> { }
        #endregion

        #region TotalLessVat
        //total_less_vat numeric(18, 8)
        [ATRDDecimal8]
        [PXUIField(DisplayName = ATRDMessages.TotalLessVat)]
        public virtual decimal? TotalLessVat { get; set; }
        public abstract class totalLessVat : BqlDecimal.Field<totalLessVat> { }
        #endregion
    }
}
