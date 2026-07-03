using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.EP;
using RetailDimension.DAC.Extension;
using RetailDimension.Helper;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("Material Request Setup Approval")]
    public class ATRDMaterialRequestSetupApproval : Base.Audit, IBqlTable, IAssignedMap
    {
        #region ApprovalID
        /// <summary>
        /// RLMaterialRequestSetupApproval approvedID Bql field.
        /// </summary>
        public abstract class approvalID : PX.Data.IBqlField
        {
        }
        protected int? _ApprovalID;
        /// <summary>
        /// RLMaterialRequestSetupApproval ApprovalID nullable integer property field.
        /// </summary>
        [PXDBIdentity(IsKey = true)]
        public virtual int? ApprovalID
        {
            get
            {
                return this._ApprovalID;
            }
            set
            {
                this._ApprovalID = value;
            }
        }
        #endregion
        #region AssignmentMapID
        /// <summary>
        /// RLMaterialRequestSetupApproval assignmentMapID Bql field.
        /// </summary>
        public abstract class assignmentMapID : PX.Data.IBqlField
        {
        }
        protected int? _AssignmentMapID;
        [PXDefault]
        [PXDBInt()]
        [PXSelector(
            typeof(Search<
                EPAssignmentMap.assignmentMapID,
                Where<EPAssignmentMap.entityType, Equal<EPAssignmentMapMaintExtension.AssignmentMapTypeRLMaterialRequest>,
                    And<EPAssignmentMap.mapType, NotEqual<EPMapType.assignment>>>>),
                DescriptionField = typeof(EPAssignmentMap.name))]
        [PXRestrictor(typeof(Where<EPAssignmentMap.assignmentMapID,
                                NotIn2<Search<ATRDMaterialRequestSetupApproval.assignmentMapID>>>), null, ShowWarning = true)]
        [PXUIField(DisplayName = ATRDMessages.AssignmentMapID)]
        /// <summary>
        /// RLMaterialRequestSetupApproval AssignmentID nullable integer property field.
        /// </summary>
        public virtual int? AssignmentMapID
        {
            get
            {
                return this._AssignmentMapID;
            }
            set
            {
                this._AssignmentMapID = value;
            }
        }
        #endregion
        #region AssignmentNotificationID
        /// <summary>
        /// RLMaterialRequestSetupApproval assignmentNotificationID Bql field.
        /// </summary>
        public abstract class assignmentNotificationID : PX.Data.IBqlField
        {
        }
        protected int? _AssignmentNotificationID;
        [PXDBInt]
        [PXSelector(typeof(PX.SM.Notification.notificationID), SubstituteKey = typeof(PX.SM.Notification.name))]
        [PXUIField(DisplayName = ATRDMessages.AssignmentNotificationID)]
        /// <summary>
        /// RLMaterialRequestSetupApproval AssignmentNotificationID nullable integer property field.
        /// </summary>
        public virtual int? AssignmentNotificationID
        {
            get
            {
                return this._AssignmentNotificationID;
            }
            set
            {
                this._AssignmentNotificationID = value;
            }
        }
        #endregion
        #region IsActive
        /// <summary>
        /// RLMaterialRequestSetupApproval isActive Bql field.
        /// </summary>
        public abstract class isActive : PX.Data.IBqlField
        {
        }
        /// <summary>
        /// RLMaterialRequestSetupApproval IsActive nullable boolean property field.
        /// </summary>
        protected Boolean? _IsActive;
        [PXDBBool()]
        [PXDefault(typeof(Search<ATRDMaterialRequestSetup.materialRequestRequestApproval>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATRDMessages.IsActive)]
        public virtual Boolean? IsActive
        {
            get
            {
                return this._IsActive;
            }
            set
            {
                this._IsActive = value;
            }
        }
        #endregion
    }
}
