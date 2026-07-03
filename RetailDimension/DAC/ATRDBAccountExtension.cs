using PX.Data;
using PX.Data.BQL;
using PX.Objects.CR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDBAccountExtension : PXCacheExtension<BAccount>
    {
        public static bool IsActive() => true;

        #region UsrATRDPermitNumber
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.PermitNumber, Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public string UsrATRDPermitNumber { get; set; }
        public abstract class usrATRDPermitNumber : BqlString.Field<usrATRDPermitNumber> { }
        #endregion

        #region UsrATRDPermitMin
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.PermitMin, Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public string UsrATRDPermitMin { get; set; }
        public abstract class usrATRDPermitMin : BqlString.Field<usrATRDPermitMin> { }
        #endregion

        #region UsrATRDPermitIssued
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.PermitIssued, Required = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public DateTime? UsrATRDPermitIssued { get; set; }
        public abstract class usrATRDPermitIssued : BqlDateTime.Field<usrATRDPermitIssued> { }
        #endregion

        #region UsrATRDPermitExpiry
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.PermitExpiry)]
        public DateTime? UsrATRDPermitExpiry { get; set; }
        public abstract class usrATRDPermitExpiry : BqlDateTime.Field<usrATRDPermitExpiry> { }
        #endregion
    }
}
