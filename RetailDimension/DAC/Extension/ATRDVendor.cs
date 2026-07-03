using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using RetailDimension.Attributes;


namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDVendor : PXCacheExtension<Vendor>
    {
        public static bool IsActive() { return true; }

        #region UsrATRDOldVendorID
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.OldVendorID)]
        public string UsrATRDOldVendorID { get; set; }
        public abstract class usrATRDOldVendorID : BqlString.Field<usrATRDOldVendorID> { }
        #endregion

    }
}
