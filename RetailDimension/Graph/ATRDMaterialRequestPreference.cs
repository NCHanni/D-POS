using System;
using PX.SM;
using PX.Data;
using RetailDimension.DAC;

namespace RetailDimension.Graph
{
    public class ATRDMaterialRequestPreference : PXGraph<ATRDMaterialRequestPreference>
    {
        public PXSave<ATRDMaterialRequestSetup> Save;
        public PXCancel<ATRDMaterialRequestSetup> Cancel;

        #region View
        public PXSelect<ATRDMaterialRequestSetup> Preferences;
        public PXSelect<ATRDMaterialRequestSetupApproval> MaterialRequestApproval;
        #endregion

        #region Events
        protected virtual void ATRDMaterialRequestSetup_MaterialRequestRequestApproval_FieldUpdating(PXCache sender, PXFieldUpdatingEventArgs e)
        {
            PXCache cache = this.Caches[typeof(ATRDMaterialRequestSetupApproval)];
            PXResultset<ATRDMaterialRequestSetupApproval> setups = PXSelect<ATRDMaterialRequestSetupApproval>.Select(sender.Graph, null);
            foreach (ATRDMaterialRequestSetupApproval setup in setups)
            {
                setup.IsActive = (bool?)e.NewValue;
                cache.Update(setup);

            }
        }
        #endregion
    }
}