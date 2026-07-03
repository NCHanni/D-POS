using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AR;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDDiscountSequence : PXCacheExtension<DiscountSequence>
    {
        public static bool IsActive() { return true; }

        #region UsrATRDIsPOS
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.IsPOSDiscount)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATRDIsPOS { get; set; }
        public abstract class usrATRDIsPOS : BqlBool.Field<usrATRDIsPOS> { }
        #endregion

        #region UsrATRDIsVatExempt
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.VatExempt)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public bool? UsrATRDIsVatExempt { get; set; }
        public abstract class usrATRDIsVatExempt : BqlBool.Field<usrATRDIsVatExempt> { }
        #endregion

        #region UsrATRDRate
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = ATRDMessages.DiscountRate)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public decimal? UsrATRDRate { get; set; }
        public abstract class usrATRDRate : BqlDecimal.Field<usrATRDRate> { }
        #endregion

        #region ATRDIsSCPWDDiscount
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.IsSCPWDDiscount)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATRDIsSCPWDDiscount { get; set; }
        public abstract class usrATRDIsSCPWDDiscount : BqlBool.Field<usrATRDIsSCPWDDiscount> { }
        #endregion
    }
}
