using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDAPPaymentExtension: PXCacheExtension<APPayment>
    {
        public static bool IsActive() { return true; }

        #region UsrRLRefNbr
        public abstract class usrRLRefNbr : BqlString.Field<usrRLRefNbr> { }
        [PXString]
        [PXSelector(typeof(Search<APPayment.refNbr>), DescriptionField = typeof(APPayment.refNbr))]
        public string UsrRLRefNbr { get; set; }
        #endregion
    }
}
