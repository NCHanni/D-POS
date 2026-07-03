using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CR;
using PX.Objects.IN;
using RetailDimension.DAC.Extension;

namespace RetailDimension.Graph
{
	/// <summary>
	/// PAGE : ATRD4009
	/// </summary>
    public class ATRDItemSalePrice : PXGraph<ATRDItemSalePrice>
    {
        [Obsolete("Replace or update this query upon upgrade to Acumatica version 2022R1 or higher.")]
        public PXSelectReadonly2<
			ARPriceWorksheetDetail,
			InnerJoin<INSite, 
				On<INSite.siteID, Equal<ARPriceWorksheetDetail.siteID>>,
		    InnerJoin<ARPriceWorksheet, 
		        On<ARPriceWorksheet.refNbr, Equal<ARPriceWorksheetDetail.refNbr>>, 
		    InnerJoin<InventoryItem, 
		        On<InventoryItem.inventoryID, Equal<ARPriceWorksheetDetail.inventoryID>>,
		    LeftJoin<BAccount, 
		        On<BAccount.bAccountID, Equal<ARPriceWorksheetDetail.customerID>>>>>>, 
		    Where<
				ARPriceWorksheet.status, Equal<SPWorksheetStatus.released>,
                And<ATRDInventoryItem.usrATRDIsPOS, Equal<True>,
                And<ARPriceWorksheetDetail.pendingPrice, Less<Current<ATRDInventoryItem.usrATRDOneMillion>>,
                And<Where<ARPriceWorksheet.expirationDate, Greater<Today>,
		            Or<ARPriceWorksheet.expirationDate, IsNull>>>>>>,
		    OrderBy<
				Desc<ARPriceWorksheet.effectiveDate, 
		        Desc<ARPriceWorksheetDetail.refNbr>>>>
            Document;
    }
}

                //InventoryItem.dfltSiteID is obsolete and is going to be removed in 2022R1.
                //Use InventoryItemCurySettings.DfltSiteID instead
                //And<InventoryItem.dfltSiteID, Equal<ARPriceWorksheetDetail.siteID>,
                //And<Where<InventoryItem.dfltSiteID, IsNull,
				//	Or<InventoryItem.dfltSiteID, Equal<ARPriceWorksheetDetail.siteID>>>,
