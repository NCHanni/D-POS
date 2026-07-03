using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using RetailDimension.DAC;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE: ATRD1001
    /// </summary>
    public class ATRDPreference : PXGraph<ATRDPreference>
    {
        #region View
        public PXSelect<ATRDSetup> Document;
        #endregion

        #region Actions
        public PXCancel<ATRDSetup> Cancel;
        public PXSave<ATRDSetup> Save;
        #endregion
    }
}