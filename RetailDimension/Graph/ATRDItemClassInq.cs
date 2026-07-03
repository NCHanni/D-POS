using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Objects.IN;
using RetailDimension.DAC.Extension;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE : ATRD4005
    /// </summary>
    public class ATRDItemClassInq : PXGraph<ATRDItemClassInq>
    {
        public PXSelect<
            INItemClass,
            Where<INItemClass.itemClassID, In2<Search<InventoryItem.itemClassID, Where<ATRDInventoryItem.usrATRDIsPOS, Equal<True>>>>>> 
            Document;
    }
}
