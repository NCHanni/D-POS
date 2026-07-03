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
    public sealed class ATRDContact: PXCacheExtension<Contact>
    {
        public static bool IsActive() { return true; }

        #region UsrATRDBusinessStyle
        [PXDBString(250, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.BusinessStyle)]
        public string UsrATRDBusinessStyle { get; set; }
        public abstract class usrATRDBusinessStyle : BqlString.Field<usrATRDBusinessStyle> { }
        #endregion
    }
}
