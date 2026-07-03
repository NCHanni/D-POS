using PX.Data;
using PX.Data.BQL;
using PX.Objects.IN;
using PX.Objects.SO;
using System;
using System.Collections;
using System.Collections.Generic;


namespace RetailDimension.Graph {
    public class ATRDItemOnHandInq : PXGraph<ATRDItemOnHandInq>
    {
        #region Views 
        public PXFilter<ATRDItemOnHandFilter> Filter;
        public PXSelectReadonly<ATRDItemOnHandDetails> Details;
        #endregion
        
        #region View Delegates
        public IEnumerable details()
        {
            List<ATRDItemOnHandDetails> result = new List<ATRDItemOnHandDetails>();

            PXResultset<SOLine> soLine = PXSelect<SOLine,
                 Where<SOLine.orderNbr, Equal<Current<ATRDItemOnHandFilter.orderNbr>>>>.Select(this);    

            SOOrder ordr = PXSelect<SOOrder, Where<SOOrder.orderNbr, Equal<Required<SOOrder.orderNbr>>>>.Select(this, Filter.Current.OrderNbr);
     
            if (soLine != null)
            {
                foreach (SOLine order in soLine)
                {
                    if (order.OrderType == ordr.OrderType)
                    {
                        INLocationStatus summary = PXSelect<INLocationStatus,
                           Where<INLocationStatus.siteID,    Equal<Required<INLocationStatus.siteID>>,
                           And<INLocationStatus.inventoryID, Equal<Required<INLocationStatus.inventoryID>>,
                           And<INLocationStatus.locationID,  Equal<Required<INLocationStatus.locationID>>>>>>
                            .Select(this, order.SiteID, order.InventoryID, order.LocationID);

                        decimal? qtyOnHand = 0;
                        if (summary != null)
                        {
                            qtyOnHand = summary.QtyOnHand < 0 ? 0 : summary.QtyOnHand;
                        }
                     
                        InventoryItem invItem = PXSelect<InventoryItem,
                            Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(this, order.InventoryID);
                                 
                        decimal? ordrQty = order.BaseOrderQty;
                  
                        ATRDItemOnHandDetails details = new ATRDItemOnHandDetails()
                        {
                            OrderNbr     = order.OrderNbr,
                            InventoryCD  = order.InventoryID,
                            Descr        = order.TranDesc,
                            WarehouseID  = order.SiteID,
                            Location     = order.LocationID,
                            BaseUnit     = invItem?.BaseUnit,
                            BaseOrderQty = order.BaseOrderQty,
                            Qty          = qtyOnHand,
                            ShortOver    = qtyOnHand - order.BaseOrderQty
                        };
                        result.Add(details);
                    }
                }
            }
            return result;
        }
        #endregion

        #region Internal Types
        [Serializable]
        [PXCacheName("Details")]
#if Version23R1
    public partial class ATRDItemOnHandDetails : IBqlTable
#endif
#if Version25R1 || Version25R2
        public partial class ATRDItemOnHandDetails : PXBqlTable, IBqlTable
#endif
        {
            #region SOOrderNbr  
            [PXString(15, IsUnicode = true)]
            [PXUIField(DisplayName = "Order Nbr.", Visibility = PXUIVisibility.SelectorVisible)]
            [PXSelector(typeof(Search<SOOrder.orderNbr>))]
            public virtual string OrderNbr { get; set; }
            public abstract class orderNbr : BqlString.Field<orderNbr> { }
            #endregion
            #region InventoryCD
            [PXInt]
            //[PXInt(BqlTable = typeof(StorageSplit))]
            [PXUIField(DisplayName = "Inventory ID", IsReadOnly = true)]
            [PXSelector(typeof(Search<InventoryItem.inventoryID>),
                DescriptionField = typeof(InventoryItem.inventoryCD),
                SubstituteKey = typeof(InventoryItem.inventoryCD))]
            public int? InventoryCD { get; set; }
            public abstract class inventoryCD : PX.Data.BQL.BqlDecimal.Field<inventoryCD> { }

            #endregion
            #region Descr
            [PXString]
            [PXUIField(DisplayName = "Description", Enabled = false)]
            public virtual String Descr { get; set; }
            public abstract class descr : PX.Data.BQL.BqlString.Field<descr> { }
            #endregion
            #region WarehouseID            
            [PXInt]
            [PXUIField(DisplayName = "Warehouse")]
            [PXSelector(typeof(Search<INSite.siteID>),
                DescriptionField = typeof(INSite.siteCD),
                SubstituteKey = typeof(INSite.siteCD))]
            public int? WarehouseID { get; set; }
            public abstract class warehouseID : PX.Data.BQL.BqlInt.Field<warehouseID> { }
            #endregion
            #region Location  
            [PXInt]
            [PXUIField(DisplayName = "Location", Visibility = PXUIVisibility.SelectorVisible)]
            [PXSelector(typeof(Search2<INLocation.locationID,
            InnerJoin<INLocation, On<SOLine.locationID, Equal<INLocation.locationID>>>,
            Where <SOLine.orderNbr, Equal<Current<orderNbr>>>>),
            DescriptionField = typeof(INLocation.locationCD),
            SubstituteKey = typeof(INLocation.locationCD))]
            public int? Location { get; set; }
            public abstract class location : BqlString.Field<location> { }
            #endregion
            #region BaseUnit
            // Acuminator disable once PX1023 MultipleTypeAttributesOnProperty [Justification]
            [INUnit(DisplayName = "Base Unit", BqlTable = typeof(InventoryItem), Enabled = false)]
            public virtual String BaseUnit { get; set; }
            public abstract class baseUnit : PX.Data.BQL.BqlString.Field<baseUnit> { }
            #endregion
            #region BaseOrderQty
            [PXDecimal]
            //[PXDecimal(BqlTable = typeof(StorageSplit))]
            [PXUIField(DisplayName = "Base Order Qty.", IsReadOnly = true)]
            public virtual Decimal? BaseOrderQty { get; set; }
            public abstract class baseOrderQty : PX.Data.BQL.BqlDecimal.Field<baseOrderQty> { }
            #endregion
            #region Qty
            [PXDecimal]
            //[PXDecimal(BqlTable = typeof(StorageSplit))]
            [PXUIField(DisplayName = "Qty. On Hand", IsReadOnly = true)]
            public virtual Decimal? Qty { get; set; }
            public abstract class qty : PX.Data.BQL.BqlDecimal.Field<qty> { }
            #endregion
            #region ShortOver
            [PXDecimal]
            //[PXDecimal(BqlTable = typeof(StorageSplit))]
            [PXUIField(DisplayName = "Short / Over ", IsReadOnly = true)]
            public virtual Decimal? ShortOver { get; set; }
            public abstract class shortOver : PX.Data.BQL.BqlDecimal.Field<shortOver> { }
            #endregion
        }

        [Serializable]
        [PXCacheName("Filter")]
#if Version23R1
    public partial class ATRDItemOnHandFilter : IBqlTable
#endif
#if Version25R1 || Version25R2
        public partial class ATRDItemOnHandFilter : PXBqlTable, IBqlTable
#endif
        {
            #region SOOrderNbr  
            [PXString(15, IsUnicode = true)]
            [PXUIField(DisplayName = "Sales Order Nbr.", Visibility = PXUIVisibility.SelectorVisible, Required = true)]
            [PXSelector(typeof(Search3<SOOrder.orderNbr, OrderBy<Desc<SOOrder.orderNbr>>>))]

            public virtual string OrderNbr { get; set; }
            public abstract class orderNbr : BqlString.Field<orderNbr> { }
            #endregion
            #region SOOrder Type
            [PXString()]
            [PXUnboundDefault(typeof(Search2<SOLine.orderType,
                InnerJoin<SOOrder, On<SOOrder.orderNbr, Equal<SOLine.orderNbr>>>,
                Where<SOLine.orderNbr, Equal<Current<orderNbr>>>>))]
            [PXFormula(typeof(orderNbr))]
            public virtual string OrderType { get; set; }
            public abstract class orderType : BqlString.Field<orderType> { }
            #endregion
            #region Warehouse            
            [PXInt]
            [PXUIField(DisplayName = "Warehouse")]
            [PXSelector(typeof(Search<INSite.siteID>),
                DescriptionField = typeof(INSite.siteCD),
                SubstituteKey = typeof(INSite.siteCD))]
            public int? WarehouseID { get; set; }
            public abstract class warehouseID : PX.Data.BQL.BqlInt.Field<warehouseID> { }
            #endregion
            #region Location                   
            [PXInt]
            [PXUIField(DisplayName = "Location", Visibility = PXUIVisibility.SelectorVisible)]
            [PXSelector(typeof(Search2<INLocation.locationID,
            InnerJoin<SOLine, On<SOLine.locationID, Equal<INLocation.locationID>>>,
            Where<SOLine.orderNbr, Equal<Current<orderNbr>>>>),
            DescriptionField = typeof(INLocation.locationCD),
            SubstituteKey = typeof(INLocation.locationCD))]
            public int? Location { get; set; }
            public abstract class location : BqlString.Field<location> { }
            #endregion

        }
        #endregion
    }
}
