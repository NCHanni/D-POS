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
    /// PAGE : ATRD4007
    /// </summary>
    public class ATRDItemBarcodeInq : PXGraph<ATRDItemBarcodeInq>
    {
        public PXSelectReadonly2<
            INItemXRef, 
            InnerJoin<InventoryItem, 
                On<InventoryItem.inventoryID, Equal<INItemXRef.inventoryID>, 
                And<ATRDInventoryItem.usrATRDIsPOS, Equal<True>>>>, 
            Where<INItemXRef.alternateType, Equal<INAlternateType.barcode>>> 
            Document;
    }
}