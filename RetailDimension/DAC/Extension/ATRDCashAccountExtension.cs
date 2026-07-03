using PX.Data;
using PX.Data.BQL;
using PX.Objects.CA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDCashAccountExtension : PXCacheExtension<CashAccount>
    {
        public static bool IsActive() { return true; }

        #region UsrATRDAcctName
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.AccountName)]
        //[PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public string UsrATRDAcctName { get; set; }
        public abstract class usrATRDAcctName : BqlString.Field<usrATRDAcctName> { }
        #endregion
    }
}
