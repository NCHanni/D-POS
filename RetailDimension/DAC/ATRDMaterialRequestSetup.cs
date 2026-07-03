using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.EP;
using PX.Objects.CS;
using RetailDimension.Helper;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("RL Setup")]
    public class ATRDMaterialRequestSetup : Base.Audit, IBqlTable
    {
        #region MaterialRequestNumberingID
        [PXDBString(10, IsUnicode = true)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXUIField(DisplayName = ATRDMessages.MaterialRequestNumberingID, Visibility = PXUIVisibility.Visible)]
        public string MaterialRequestNumberingID { get; set; }
        public abstract class materialRequestNumberingID : PX.Data.IBqlField { }
        #endregion

        #region MaterialRequestRequestApproval
        [EPRequireApproval]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATRDMessages.MaterialRequestRequestApproval)]
        public virtual bool? MaterialRequestRequestApproval { get; set; }
        public abstract class materialRequestRequestApproval : BqlBool.Field<materialRequestRequestApproval> { }
        #endregion
    }
}
