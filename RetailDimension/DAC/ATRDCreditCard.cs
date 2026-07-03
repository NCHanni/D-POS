using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CA;

namespace RetailDimension.DAC {
    [Serializable]
    [PXCacheName("RD-Credit Card")]
    public class ATRDCreditCard : Base.Table, IBqlTable {
        #region Code
        [PXDBString(10, IsKey = true, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Code)]
        [PXDefault]
        public virtual string Code { get; set; }
        public abstract class code : BqlString.Field<code> { }
        #endregion

        #region Description
        [PXDBString(250, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Description)]
        [PXDefault]
        public virtual string Description { get; set; }
        public abstract class description : BqlString.Field<description> { }
        #endregion

        #region BankName
        [PXDBString(30, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.BankName)]
        [PXDefault]
        public virtual string BankName { get; set; }
        public abstract class bankName : BqlString.Field<bankName> { }
        #endregion

        #region IsActive
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.Active)]
        [PXDefault(true, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsActive { get; set; }
        public abstract class isActive : BqlBool.Field<isActive> { }
        #endregion

        #region PaymentMethodID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.PaymentMethodID, Enabled = true)]
        [PXSelector(typeof(Search<PaymentMethod.paymentMethodID>),
            typeof(PaymentMethod.paymentMethodID),
            typeof(PaymentMethod.descr),
            DescriptionField = typeof(PaymentMethod.descr))]
        public virtual string PaymentMethodID { get; set; }
        public abstract class paymentMethodID : BqlString.Field<paymentMethodID> { }
        #endregion
    }
}
