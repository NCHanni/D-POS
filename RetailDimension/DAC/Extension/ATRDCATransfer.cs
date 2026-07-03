using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using PX.Objects.CA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDCATransfer : PXCacheExtension<CATransfer>
    {
        public static bool IsActive() { return true; }

        #region Bound Fields

        #region UsrRLCheckNbr
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CheckNbr)]
        public string UsrRLCheckNbr { get; set; }
        public abstract class usrRLCheckNbr : BqlString.Field<usrRLCheckNbr> { }
        #endregion

        #endregion

        #region Unbound Fields
        #region UsrRLCuryTranOutToWords
        public abstract class usrRLCuryTranOutToWords : PX.Data.BQL.BqlString.Field<usrRLCuryTranOutToWords> { }
        [PXString]
        [ToWords(typeof(CATransfer.curyTranOut))]
        public string UsrRLCuryTranOutToWords { get; set; }
        #endregion

        #region UsrRLCuryTranInToWords
        public abstract class usrRLCuryTranInToWords : PX.Data.BQL.BqlString.Field<usrRLCuryTranInToWords> { }
        [PXString]
        [ToWords(typeof(CATransfer.curyTranIn))]
        public string UsrRLCuryTranInToWords { get; set; }
        #endregion
        #endregion

    }
}
