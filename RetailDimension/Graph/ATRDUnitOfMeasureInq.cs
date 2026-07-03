using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Objects.IN;

namespace RetailDimension.Graph
{
    public class ATRDUnitOfMeasureInq : PXGraph<ATRDUnitOfMeasureInq>
    {
        public PXSelectGroupBy<INUnit, Aggregate<GroupBy<INUnit.fromUnit>>> Document;
    }
}