using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Objects.SO;
using RetailDimension.DAC;
using RetailDimension.Helper;
using PX.Api;
using PX.Common;
using System.Linq;
using PX.Objects.EP;
using PX.Objects.CS;
using PX.Objects.IN;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE: ATRD3001
    /// </summary>
    /// 
    public class ATRDSaleEntry : PXGraph<ATRDSaleEntry, ATRDBatchSale>
    {
        #region Views
        public PXSetup<ATRDSetup> Setup;
        public PXSelect<ATRDBatchSale> Document;
        public PXSelect<ATRDSale, Where<ATRDSale.batchRefNbr, Equal<Current<ATRDBatchSale.refNbr>>, And<ATRDSale.branchID, Equal<Current<ATRDBatchSale.branchID>>>>> Sale;
        public PXSelect<ATRDSaleDetail, Where<ATRDSaleDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Detail;
        public PXSelect<ATRDSaleDiscountDetail, Where<ATRDSaleDiscountDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleDiscountDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Discount;
        public PXSelect<ATRDSaleCashDetail, Where<ATRDSaleCashDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleCashDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Cash;
        public PXSelect<ATRDSaleCardDetail, Where<ATRDSaleCardDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleCardDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Card;
        public PXSelect<ATRDSaleMemoDetail, Where<ATRDSaleMemoDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleMemoDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Memo;
        public PXSelect<ATRDSaleGiftDetail, Where<ATRDSaleGiftDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleGiftDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Gift;
        #endregion

        #region Events
        protected virtual void _(Events.RowSelected<ATRDBatchSale> e)
        {
            ATRDBatchSale row = e.Row;
            if (row == null) return;

            Numbering numbering = PXSelect<Numbering, Where<Numbering.numberingID, Equal<Required<Numbering.numberingID>>>>.Select(this, Setup.Current.NumberSequenceID);

            if(Document.Current.RefNbr.Trim() == numbering.NewSymbol.Trim()) Document.Current.SyncDate = DateTime.Now;
        }

        protected virtual void _(Events.FieldUpdated<ATRDBatchSale, ATRDBatchSale.branchCD> e)
        {
            var row = e.Row;
            if (row == null) return;

            if (!string.IsNullOrEmpty((string)e.NewValue))
            {
                Branch branch = PXSelect<Branch, Where<Branch.branchCD, Equal<Required<Branch.branchCD>>>>.Select(this, e.NewValue);
                if (branch != null)
                {
                    e.Cache.SetValue<ATRDBatchSale.branchID>(row, branch.BranchID);
                }
            }
        }

        protected virtual void _(Events.RowUpdated<ATRDSale> e)
        {
            if (e.Row == null) return;

            if (e.Row.BranchID != Document.Current.BranchID)
            {
                e.Cache.SetValue<ATRDSale.branchID>(e.Row, Document.Current.BranchID);
            }
        }

        protected virtual void _(Events.RowSelected<ATRDSaleDetail> e)
        {
            ATRDSaleDetail row = e.Row;
            if (row == null || string.IsNullOrEmpty(row.AlternateID)) return;

            PopulateDescriptionFromXRef(e.Cache, row);
        }

        private void PopulateDescriptionFromXRef(PXCache cache, ATRDSaleDetail row)
        {
            if (string.IsNullOrEmpty(row.AlternateID)) return;

            INItemXRef xref = PXSelectReadonly<INItemXRef,
                Where<INItemXRef.alternateID, Equal<Required<INItemXRef.alternateID>>>>
                .Select(this, row.AlternateID);

            if (xref != null)
            {
                cache.SetValue<ATRDSaleDetail.description>(row, xref.Descr);
            }
        }
        #endregion
    }
}