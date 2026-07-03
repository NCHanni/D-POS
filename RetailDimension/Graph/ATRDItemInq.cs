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
    /// PAGE: ATRD4001
    /// </summary>
    public class ATRDItemInq : PXGraph<ATRDItemInq>
    {
        public PXSelectReadonly2<
            InventoryItem,
            LeftJoin<INLotSerClass,
                On<INLotSerClass.lotSerClassID, Equal<InventoryItem.lotSerClassID>>>,
            Where<ATRDInventoryItem.usrATRDIsPOS, Equal<True>>>
            Document;

    }
}