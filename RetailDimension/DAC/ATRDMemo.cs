using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CS;
using PX.Objects.AR;
using RetailDimension.DAC.Extension;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("RD-Memo")]
    public class ATRDMemo : Base.Table, IBqlTable
    {
        #region RefNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = ATRDMessages.C15)]
        [PXUIField(DisplayName = ATRDMessages.ReferenceNbr)]
        [PXSelector(typeof(Search<refNbr>))]
        [AutoNumber(typeof(ATRDSetup.memoNumberSequenceID), typeof(AccessInfo.businessDate))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : BqlString.Field<refNbr> { }
        #endregion

        #region CustomerID
        [PXDBInt]
        [PXUIField(DisplayName = ATRDMessages.Customer)]
        [PXSelector(typeof(Search<
            Customer.bAccountID, 
            Where<ATRDCustomer.usrATRDIsPOS, Equal<True>>>), typeof(Customer.acctCD), typeof(Customer.acctName), 
            SubstituteKey = typeof(Customer.acctCD), 
            DescriptionField = typeof(Customer.acctName))]
        [PXDefault]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID : BqlInt.Field<customerID> { }
        #endregion

        #region Date
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.Date)]
        [PXDefault(typeof(AccessInfo.businessDate))]
        public virtual DateTime? Date { get; set; }
        public abstract class date : BqlDateTime.Field<date> { }
        #endregion

        #region Description
        [PXDBString(250, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Description)]
        public virtual string Description { get; set; }
        public abstract class description : BqlString.Field<description> { }
        #endregion

        #region TotalAmount
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = ATRDMessages.TotalAmount)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? TotalAmount { get; set; }
        public abstract class totalAmount : BqlDecimal.Field<totalAmount> { }
        #endregion

        #region Unbound

        #region CustomerCD
        [PXString(15, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CustomerCode)]
        [PXFormula(typeof(Selector<customerID, Customer.acctCD>))]
        [PXFormula(typeof(Default<customerID>))]
        public virtual string CustomerCD { get; set; }
        public abstract class customerCD : BqlString.Field<customerCD> { }
        #endregion

        #region CustomerName
        [PXString(250, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CustomerName)]
        [PXFormula(typeof(Selector<customerID, Customer.acctName>))]
        [PXFormula(typeof(Default<customerID>))]
        public virtual string CustomerName { get; set; }
        public abstract class customerName : BqlString.Field<customerName> { }
        #endregion

        #endregion
    }
}
