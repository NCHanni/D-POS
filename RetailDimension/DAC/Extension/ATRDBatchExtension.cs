using PX.Data;
using PX.Objects.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data.BQL;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDBatchExtension : PXCacheExtension<Batch>
    {
        public static bool IsActive() { return true; }

        #region UsrBatchNbr
        public abstract class usrBatchNbr : BqlString.Field<usrBatchNbr> { }
        [PXString]
        [PXSelector(typeof(Search<Batch.batchNbr>), DescriptionField = typeof(Batch.batchNbr))]
        public string UsrBatchNbr { get; set; }
        #endregion
    }
}
