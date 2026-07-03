using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using RetailDimension.DAC;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE : ATRD2007
    /// </summary>
    public class ATRDGiftCertMaint : PXGraph<ATRDGiftCertMaint>
    {
        public PXSave<ATRDGiftCert> Save;

        public PXCancel<ATRDGiftCert> Cancel;

        [PXImport]
        public PXSelect<ATRDGiftCert> Document;

        #region Events

        //protected virtual void _(Events.RowSelected<ATRDGiftCert> e)
        //{
        //    ATRDGiftCert current = e.Row;

        //    if (current == null) return;

        //    PXUIFieldAttribute.SetEnabled<ATRDGiftCert.isUsed>(e.Cache, current, false);
        //}

        #endregion
    }
}