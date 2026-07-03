using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AR;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDSalesPerson : PXCacheExtension<SalesPerson>
    {
        public static bool IsActive() { return true; }

        #region UsrATRDIsPOS
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.IsPOSSalesperson)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATRDIsPOS { get; set; }
        public abstract class usrATRDIsPOS : BqlBool.Field<usrATRDIsPOS> { }
        #endregion

        #region UsrATRDBarcode
        [PXDBString(30, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Barcode)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        [PXCheckUnique(Where = typeof(Where<usrATRDBarcode, Equal<Current<usrATRDBarcode>>>), ErrorMessage = "This value already exists.")]
        public string UsrATRDBarcode { get; set; }
        public abstract class usrATRDBarcode : BqlString.Field<usrATRDBarcode> { }
        #endregion
    }
}
