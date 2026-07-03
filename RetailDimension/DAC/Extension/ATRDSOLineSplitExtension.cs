using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;
using System;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDSOLineSplitExtension : PXCacheExtension<SOLineSplit>
    { 
        [PXRemoveBaseAttribute(typeof(PXDefaultAttribute))]
        public Int32? LocationID { get; set; }
        public abstract class locationID : PX.Data.BQL.BqlInt.Field<locationID> { }
    }
}
