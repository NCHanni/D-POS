using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.SO;
using PX.Objects.AP;
using PX.Data.BQL;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDSOOrderExtension: PXCacheExtension<SOOrder>
    {
        #region UsrSOOrderOrderTotalInWords
        public abstract class usrSOOrderOrderTotalInWords : BqlString.Field<usrSOOrderOrderTotalInWords> { }
        [PXString]
        [ToWords(typeof(SOOrder.curyOrderTotal))]
        public string UsrSOOrderOrderTotalInWords { get; set; }
        #endregion
    }
}
