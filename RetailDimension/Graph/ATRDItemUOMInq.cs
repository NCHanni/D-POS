using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Objects.IN;
using RetailDimension.DAC;
using RetailDimension.DAC.Extension;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE : ATRD4008
    /// </summary>
    public class ATRDItemUOMInq : PXGraph<ATRDItemUOMInq>
    {
        public PXSelectReadonly2<
            INUnit, 
            InnerJoin<InventoryItem, 
                On<InventoryItem.inventoryID, Equal<INUnit.inventoryID>, 
                And<ATRDInventoryItem.usrATRDIsPOS, Equal<True>>>>, 
            Where<INUnit.fromUnit, NotEqual<INUnit.toUnit>>>
            Document;
    }
}
