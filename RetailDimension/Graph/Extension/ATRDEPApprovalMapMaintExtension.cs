using PX.Data;
using PX.Objects.EP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetailDimension.Graph.Extension
{
    public class ATRDEPApprovalMapMaintExtension : PXGraphExtension<EPApprovalMapMaint>
    {
        public static bool IsActive() => true;

        #region Event Handlers

        public delegate IEnumerable<String> GetEntityTypeScreensDelegate();
        [PXOverride]
        public virtual IEnumerable<string> GetEntityTypeScreens(Func<IEnumerable<string>> method)
        {
            var list = method?.Invoke().ToList(); if (list != null)
            {
                list.Add("ATRD3004");   //Material Request
            }

            return list;
        }
        #endregion
    }
}