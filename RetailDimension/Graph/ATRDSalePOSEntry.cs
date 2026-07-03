using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using RetailDimension.DAC;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE: ATRD3003
    /// </summary>
    public class ATRDSalePOSEntry : PXGraph<ATRDSalePOSEntry,ATRDSale>
    {
        public PXSelect<ATRDSale> Document;
        public PXSelect<ATRDSaleDetail, Where<ATRDSaleDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Detail;
        public PXSelect<ATRDSaleDiscountDetail, Where<ATRDSaleDiscountDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleDiscountDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Discount;
        public PXSelect<ATRDSaleCashDetail, Where<ATRDSaleCashDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleCashDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Cash;
        public PXSelect<ATRDSaleCardDetail, Where<ATRDSaleCardDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleCardDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Card;
        public PXSelect<ATRDSaleMemoDetail, Where<ATRDSaleMemoDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleMemoDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Memo;
        public PXSelect<ATRDSaleGiftDetail, Where<ATRDSaleGiftDetail.saleCode, Equal<Current<ATRDSale.code>>, And<ATRDSaleGiftDetail.branchID, Equal<Current<ATRDSale.branchID>>>>> Gift;

        public virtual void _(Events.RowSelected<ATRDSale> e)
        {
            ATRDSale document = e.Row;

            if (document == null) return;

            bool isProcessed = (document.OrderNbr != null);

            Delete.SetEnabled(!isProcessed);

            //Document.AllowUpdate = !isProcessed;
            PXUIFieldAttribute.SetEnabled<ATRDSale.isVoid>(e.Cache, document, !isProcessed);
            PXUIFieldAttribute.SetEnabled<ATRDSale.isAllReturned>(e.Cache, document, !isProcessed);
            PXUIFieldAttribute.SetEnabled<ATRDSale.isSC>(e.Cache, document, !isProcessed);
            PXUIFieldAttribute.SetEnabled<ATRDSale.isPWD>(e.Cache, document, !isProcessed);

            Detail.AllowUpdate = !isProcessed;
            Detail.AllowInsert = !isProcessed;
            Detail.AllowDelete = !isProcessed;

            Discount.AllowUpdate = !isProcessed;
            Discount.AllowInsert = !isProcessed;
            Discount.AllowDelete = !isProcessed;

            Cash.AllowUpdate = !isProcessed;
            Cash.AllowInsert = !isProcessed;
            Cash.AllowDelete = !isProcessed;

            Card.AllowUpdate = !isProcessed;
            Card.AllowInsert = !isProcessed;
            Card.AllowDelete = !isProcessed;

            Memo.AllowUpdate = !isProcessed;
            Memo.AllowInsert = !isProcessed;
            Memo.AllowDelete = !isProcessed;

            Gift.AllowUpdate = !isProcessed;
            Gift.AllowInsert = !isProcessed;
            Gift.AllowDelete = !isProcessed;
        }


        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXSelector(typeof(Search<ATRDSale.code>))]
        protected virtual void ATRDSale_Code_CacheAttached(PXCache sender) { }

    }
}