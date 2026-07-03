using PX.Data;
using PX.Objects.IN;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetailDimension.Graph.Extension
{
    public class ATRDInventoryItemMaintExtension : PXGraphExtension<InventoryItemMaint>
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        //[PXOverride]
        //public void Persist()
        //{
        //    using (PXTransactionScope scope = new PXTransactionScope())
        //    {
        //        List<INItemXRef> barcode = new List<INItemXRef>();

        //        foreach (INItemXRef item in Base.itemxrefrecords.Select())
        //        {
        //            INItemXRef items = PXSelect<INItemXRef, Where<INItemXRef.alternateID, Equal<Required<INItemXRef.alternateID>>>>.Select(Base, item.AlternateID);

        //            if (items != null)
        //            {
        //                throw new Exception("Barcode already exist.");
        //            }
        //        }
        //    }

        //    //Base.Persist();
        //}

        public void InventoryItem_RowPersisting(PXCache sender, PXRowPersistingEventArgs e, PXRowPersisting del)
        {
            List<INItemXRef> barcode = new List<INItemXRef>();

            foreach (INItemXRef item in Base.itemxrefrecords.Select())
            {
                INItemXRef items = PXSelect<INItemXRef, Where<INItemXRef.alternateID, Equal<Required<INItemXRef.alternateID>>, And<INItemXRef.inventoryID, NotEqual<Required<INItemXRef.inventoryID>>>>>.Select(Base, item.AlternateID, item.InventoryID).FirstOrDefault();

                if (items != null)
                {
                    InventoryItem i = PXSelect<InventoryItem, Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(Base, items.InventoryID);
                    throw new Exception("Barcode has already been used in item " + i.InventoryCD);

                    //warning CS0162: Unreachable code detected
                    //e.Cancel = true;
                }
            }

            del(sender, e);
        }

        public static bool IsActive() { return true; }
    }
}
