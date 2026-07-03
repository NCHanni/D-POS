using System;
using PX.Data;
using PX.Data.BQL;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("RD-GiftCert")]
    public class ATRDGiftCert : Base.Table, IBqlTable
    {
        #region Id
        [PXDBLongIdentity(IsKey = true)]
        public virtual long? Id { get; set; }
        public abstract class id : BqlInt.Field<id> { }
        #endregion

        #region Code
        [PXDBString(250, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Code)]
        public virtual string Code { get; set; }
        public abstract class code : BqlString.Field<code> { }
        #endregion

        #region Description
        [PXDBString(250, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Description)]
        public virtual string Description { get; set; }
        public abstract class description : BqlString.Field<description> { }
        #endregion

        #region ExpiryDate
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.ExpiryDate)]
        [PXDefault]
        public virtual DateTime? ExpiryDate { get; set; }
        public abstract class expiryDate : BqlDateTime.Field<expiryDate> { }
        #endregion

        #region Amount
        [PXDBDecimal(2, MinValue = 1)]
        [PXUIField(DisplayName = ATRDMessages.Amount)]
        [PXDefault]
        public virtual decimal? Amount { get; set; }
        public abstract class amount : BqlDecimal.Field<amount> { }
        #endregion

        #region IsActive
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.Active)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsActive { get; set; }
        public abstract class isActive : BqlBool.Field<isActive> { }
        #endregion

        #region IsSold
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.Sold)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsSold { get; set; }
        public abstract class isSold : BqlBool.Field<isSold> { }
        #endregion

        #region IsUsed
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.Used)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? IsUsed { get; set; }
        public abstract class isUsed : BqlBool.Field<isUsed> { }
        #endregion
    }
}
