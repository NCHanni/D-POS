using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDimension.Graph
{
    
    public class ICGHInventoryItemOnHand : PXGraph<ICGHInventoryItemOnHand>
    {
        public PXFilter<StoragePlaceFilter> Filter;

        [PXFilterable]
        public
            SelectFrom<StoragePlaceStatus>
            .View storages;
        public virtual IEnumerable Storages()
        {
            var result = new PXDelegateResult();
            result.IsResultSorted = true;
            result.IsResultTruncated = Filter.Current.ExpandByLotSerialNbr == false;

            PXSelectBase<StoragePlaceStatus> byLocationSelect =
                new SelectFrom<StoragePlaceStatus>
                .Where<
                    StoragePlaceFilter.siteID.FromCurrent.NoDefault.IsEqual<StoragePlaceStatus.siteID>
                    .And<StoragePlaceStatus.qty.IsGreater<Zero>>
                    .And<
                        Not<FeatureInstalled<FeaturesSet.wMSCartTracking>>
                        .Or<StoragePlaceFilter.showLocations.FromCurrent.NoDefault.IsEqual<True>>.And<StoragePlaceStatus.isCart.IsEqual<False>>
                        .Or<StoragePlaceFilter.showCarts.FromCurrent.NoDefault.IsEqual<True>>.And<StoragePlaceStatus.isCart.IsEqual<True>>>
                >
                .AggregateTo<
                    GroupBy<StoragePlaceStatus.siteCD>,
                    GroupBy<StoragePlaceStatus.storageCD>,
                    GroupBy<StoragePlaceStatus.isCart>,
                    GroupBy<StoragePlaceStatus.active>,
                    GroupBy<StoragePlaceStatus.inventoryCD>,
                    GroupBy<StoragePlaceStatus.subItemID>,
                    Sum<StoragePlaceStatus.qty>>
                .OrderBy<
                    StoragePlaceStatus.isCart.Asc,
                    StoragePlaceStatus.siteCD.Asc,
                    StoragePlaceStatus.storageCD.Desc,
                    StoragePlaceStatus.active.Desc,
                    StoragePlaceStatus.inventoryCD.Asc,
                    StoragePlaceStatus.subItemID.Asc,
                    StoragePlaceStatus.qty.Desc>
                .View(this);
            if (Filter.Current.StorageID != null)
                byLocationSelect.WhereAnd<Where<StoragePlaceFilter.storageID.FromCurrent.IsEqual<StoragePlaceStatus.storageID>>>();
            if (Filter.Current.InventoryID != null)
                byLocationSelect.WhereAnd<Where<StoragePlaceFilter.inventoryID.FromCurrent.IsEqual<StoragePlaceStatus.inventoryID>>>();
            if (Filter.Current.SubItemID != null)
                byLocationSelect.WhereAnd<Where<StoragePlaceFilter.subItemID.FromCurrent.IsEqual<StoragePlaceStatus.subItemID>>>();

            int startRow = PXView.StartRow;
            int totalRows = 0;
            var byLocation = Filter.Current.ExpandByLotSerialNbr == true
                ? byLocationSelect.SelectMain()
                : byLocationSelect.View.Select(PXView.Currents, PXView.Parameters, PXView.Searches, PXView.SortColumns, PXView.Descendings, PXView.Filters, ref startRow, PXView.MaximumRows, ref totalRows).RowCast<StoragePlaceStatus>().ToArray();

            if (byLocation.Length > 0 && Filter.Current.ExpandByLotSerialNbr == true)
            {
                PXSelectBase<StoragePlaceStatusExpanded> byLotSerialSelect =
                    new SelectFrom<StoragePlaceStatusExpanded>
                    .Where<
                        StoragePlaceFilter.siteID.FromCurrent.NoDefault.IsEqual<StoragePlaceStatusExpanded.siteID>
                        .And<StoragePlaceStatusExpanded.qty.IsGreater<Zero>>
                    >
                    .AggregateTo<
                        GroupBy<StoragePlaceStatusExpanded.siteCD>,
                        GroupBy<StoragePlaceStatusExpanded.storageCD>,
                        GroupBy<StoragePlaceStatusExpanded.active>,
                        GroupBy<StoragePlaceStatusExpanded.inventoryCD>,
                        GroupBy<StoragePlaceStatusExpanded.subItemID>,
                        GroupBy<StoragePlaceStatusExpanded.lotSerialNbr>,
                        Sum<StoragePlaceStatusExpanded.qty>>
                    .OrderBy<
                        StoragePlaceStatusExpanded.siteCD.Asc,
                        StoragePlaceStatusExpanded.storageCD.Desc,
                        StoragePlaceStatusExpanded.active.Desc,
                        StoragePlaceStatusExpanded.inventoryCD.Asc,
                        StoragePlaceStatusExpanded.subItemID.Asc,
                        StoragePlaceStatusExpanded.lotSerialNbr.Asc,
                        StoragePlaceStatusExpanded.qty.Desc>
                    .View(this);
                if (Filter.Current.StorageID != null)
                    byLotSerialSelect.WhereAnd<Where<StoragePlaceFilter.storageID.FromCurrent.IsEqual<StoragePlaceStatusExpanded.storageID>>>();
                if (Filter.Current.InventoryID != null)
                    byLotSerialSelect.WhereAnd<Where<StoragePlaceFilter.inventoryID.FromCurrent.IsEqual<StoragePlaceStatusExpanded.inventoryID>>>();
                if (Filter.Current.SubItemID != null)
                    byLotSerialSelect.WhereAnd<Where<StoragePlaceFilter.subItemID.FromCurrent.IsEqual<StoragePlaceStatusExpanded.subItemID>>>();
                if (Filter.Current.LotSerialNbr != null)
                    byLotSerialSelect.WhereAnd<Where<StoragePlaceFilter.lotSerialNbr.FromCurrent.IsEqual<StoragePlaceStatusExpanded.lotSerialNbr>>>();

                var byLotSerial =
                    byLotSerialSelect
                    .SelectMain()
                    .Select(
                        r => new StoragePlaceStatus
                        {
                            SplittedIcon = r.SplittedIcon,
                            SiteID = r.SiteID,
                            SiteCD = r.SiteCD,
                            LocationID = r.LocationID,
                            CartID = null,
                            StorageID = r.StorageID,
                            StorageCD = r.StorageCD,
                            Descr = r.Descr,
                            IsCart = false,
                            Active = r.Active,
                            InventoryID = r.InventoryID,
                            InventoryCD = r.InventoryCD,
                            SubItemID = r.SubItemID,
                            LotSerialNbr = r.LotSerialNbr,
                            ExpireDate = r.ExpireDate,
                            Qty = r.Qty,
                            BaseUnit = r.BaseUnit
                        })
                    .ToArray();

                if (byLotSerial.Length > 0)
                {
                    int locationIdx = 1;
                    int lotSerIdx = 0;
                    StoragePlaceStatus current = byLocation[0];
                    result.Add(current);
                    while (locationIdx < byLocation.Length || lotSerIdx < byLotSerial.Length)
                    {
                        if (locationIdx >= byLocation.Length
                            || lotSerIdx < byLotSerial.Length
                            && current.SiteID == byLotSerial[lotSerIdx].SiteID
                            && current.StorageID == byLotSerial[lotSerIdx].StorageID
                            && current.InventoryID == byLotSerial[lotSerIdx].InventoryID
                            && current.SubItemID == byLotSerial[lotSerIdx].SubItemID)
                        {
                            result.Add(byLotSerial[lotSerIdx]);
                            lotSerIdx++;
                        }
                        else
                        {
                            current = byLocation[locationIdx];
                            result.Add(current);
                            locationIdx++;
                        }
                    }
                }
                else
                {
                    result.AddRange(byLocation);
                }
            }
            else
            {
                result.AddRange(byLocation);
            }

            PXView.StartRow = 0;
            return result;
        }

        //protected virtual void _(Events.RowSelected<StoragePlaceFilter> e)
        //    => storages.Cache.Adjust<PXUIFieldAttribute>()
        //        .For<StoragePlaceStatus.splittedIcon>(a => a.Visible = e.Row?.ExpandByLotSerialNbr ?? false)
        //        .SameFor<StoragePlaceStatus.lotSerialNbr>()
        //        .SameFor<StoragePlaceStatus.expireDate>();

        public override bool IsDirty => false;

        [PXHidden]
        public class StoragePlaceFilter : IBqlTable
        {
            #region SiteID
            [Site(Required = true)]
            public int? SiteID { get; set; }
            public abstract class siteID : PX.Data.BQL.BqlInt.Field<siteID> { }
            #endregion
            #region StorageID
            [PXInt]
            [PXUIField(DisplayName = "Storage ID")]
            [PXSelector(
                typeof(SearchFor<StoragePlace.storageID>.In<SelectFrom<StoragePlace>.Where<StoragePlace.active.IsEqual<True>.And<StoragePlace.siteID.IsEqual<siteID.FromCurrent>>>>),
                typeof(StoragePlace.siteCD), typeof(StoragePlace.storageCD), typeof(StoragePlace.isCart), typeof(StoragePlace.active),
                SubstituteKey = typeof(StoragePlace.storageCD),
                DescriptionField = typeof(StoragePlace.descr),
                ValidateValue = false)]
            [PXFormula(typeof(Default<siteID>))]
            public int? StorageID { get; set; }
            public abstract class storageID : PX.Data.BQL.BqlInt.Field<storageID> { }
            #endregion
            #region InventoryID
            [StockItem]
            public virtual Int32? InventoryID { get; set; }
            public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
            #endregion
            #region SubItemID
            [SubItem(typeof(inventoryID))]
            [PXFormula(typeof(Default<inventoryID>))]
            public virtual Int32? SubItemID { get; set; }
            public abstract class subItemID : PX.Data.BQL.BqlInt.Field<subItemID> { }
            #endregion
            #region LotSerialNbr
            [LotSerialNbr]
            [PXFormula(typeof(Default<inventoryID>))]
            public virtual String LotSerialNbr { get; set; }
            public abstract class lotSerialNbr : PX.Data.BQL.BqlString.Field<lotSerialNbr> { }
            #endregion
            #region ShowLocations
            [PXBool]
            [PXUnboundDefault(true)]
            [PXUIField(DisplayName = "Show Locations", FieldClass = "Carts")]
            public virtual Boolean? ShowLocations { get; set; }
            public abstract class showLocations : PX.Data.BQL.BqlBool.Field<showLocations> { }
            #endregion
            #region ShowCarts
            [PXBool]
            [PXUnboundDefault(typeof(FeatureInstalled<FeaturesSet.wMSCartTracking>))]
            [PXUIField(DisplayName = "Show Carts", FieldClass = "Carts")]
            public virtual Boolean? ShowCarts { get; set; }
            public abstract class showCarts : PX.Data.BQL.BqlBool.Field<showCarts> { }
            #endregion
            #region ExpandByLotSerialNbr
            [PXBool]
            [PXUnboundDefault(false)]
            [PXUIField(DisplayName = "Expand by Lot/Serial Number", Visibility = PXUIVisibility.Visible)]
            public virtual bool? ExpandByLotSerialNbr { get; set; }
            public abstract class expandByLotSerialNbr : PX.Data.BQL.BqlBool.Field<expandByLotSerialNbr> { }
            #endregion
        }
    }
}

