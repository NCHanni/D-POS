using PX.Data;
using PX.Objects.AR;
using RetailDimension.DAC.Extension;

namespace RetailDimension.Graph {
    /// <summary>
    /// PAGE : ATRD4004
    /// </summary>
    public class ATRDDiscountInq : PXGraph<ATRDDiscountInq>
    {
        public PXSelectReadonly<DiscountSequence, Where<ATRDDiscountSequence.usrATRDIsPOS, Equal<True>>> Document;
    }
}