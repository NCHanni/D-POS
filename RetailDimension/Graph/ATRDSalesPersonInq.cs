using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Objects.AR;
using RetailDimension.DAC.Extension;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE : ATRD4006
    /// </summary>
    public class ATRDSalesPersonInq : PXGraph<ATRDSalesPersonInq>
    {
        public PXSelectReadonly<SalesPerson, Where<ATRDSalesPerson.usrATRDIsPOS, Equal<True>>> Document;
    }
}