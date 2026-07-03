using System;
using PX.TM;
using PX.Data;
using PX.Data.EP;
using PX.Data.BQL;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.EP;
using PX.Objects.CR;
using PX.Objects.GL;
using RetailDimension.Graph;
using PX.Objects.Common;

namespace RetailDimension.DAC
{
    [Serializable]
    //[PXEMailSource]
    [PXPrimaryGraph(typeof(ATRDMaterialRequestEntry))]
    [PXCacheName("Material Request")]
    public class ATRDMaterialRequest : Base.Audit, IBqlTable, IAssign
    {
        #region RefNbr                
        [PXDBString(15, IsKey = true, InputMask = ">CCCCCCCCCCCCCCC", IsUnicode = true)]
        [PXDefault()]
        [PXUIField(DisplayName = ATRDMessages.RefNbr)]
        [PXSelector(typeof(refNbr))]
        [AutoNumber(typeof(ATRDMaterialRequestSetup.materialRequestNumberingID), typeof(AccessInfo.businessDate))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.IBqlField { }
        #endregion

        #region Status
        [PXDBString]
        [ATRDMaterialRequestStatusAttribute.ATRDMaterialRequestStatus]
        [PXDefault(ATRDMaterialRequestStatusAttribute.HoldValue)]
        [PXUIField(DisplayName = ATRDMessages.Status, Enabled = false)]
        public virtual string Status { get; set; }
        public abstract class status : IBqlField { }
        #endregion

        #region Hold
        [PXDBBool]
        [PXDefault(true, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATRDMessages.Hold)]
        public virtual bool? Hold { get; set; }
        public abstract class hold : IBqlField { }
        #endregion

        #region Date
        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = ATRDMessages.Date, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual DateTime? Date { get; set; }
        public abstract class date : PX.Data.IBqlField { }
        #endregion

        #region FinPeriodID
        [INOpenPeriod(
            sourceType: typeof(date),
            branchSourceType: typeof(branchID),
            masterFinPeriodIDType: typeof(tranPeriodID),
            IsHeader = true)]
        [PXUIField(DisplayName = ATRDMessages.FinPeriodID, Visibility = PXUIVisibility.SelectorVisible)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual String FinPeriodID { get; set; }
        public abstract class finPeriodID : IBqlField { }
        #endregion

        #region Descr
        [PXDBString(255, IsUnicode = true)]
        [PXDefault()]
        [PXUIField(DisplayName = ATRDMessages.Description)]
        public virtual string Descr { get; set; }
        public abstract class descr : IBqlField { }
        #endregion

        #region RequestedByID
        [PXDBInt]
        [PXDefault(typeof(Search<EPEmployee.bAccountID, Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>))]
        [PXUIField(DisplayName = ATRDMessages.RequestedByID)]
        [PXSelector(typeof(Search2<EPEmployee.bAccountID, LeftJoin<Location, On<Location.bAccountID, Equal<EPEmployee.bAccountID>>>>), typeof(EPEmployee.bAccountID), typeof(EPEmployee.acctCD), typeof(EPEmployee.acctName), typeof(EPEmployee.departmentID), typeof(Location.vTaxZoneID), DescriptionField = typeof(EPEmployee.acctName), SubstituteKey = typeof(EPEmployee.acctCD))]
        [PXRestrictor(typeof(Where<EPEmployee.userID, IsNotNull>), ATRDMessages.EmployeeNotUser)]
        public virtual int? RequestedByID { get; set; }
        public abstract class requestedByID : IBqlField { }
        #endregion

        #region DepartmentID
        [PXDBString()]
        [PXDefault(typeof(Selector<requestedByID, EPEmployee.departmentID>))]
        [PXFormula(typeof(Default<requestedByID>))]
        [PXUIField(DisplayName = ATRDMessages.DepartmentID)]
        [PXSelector(typeof(Search<EPDepartment.departmentID>), typeof(EPDepartment.departmentID), typeof(EPDepartment.description), DescriptionField = typeof(EPDepartment.description))]
        public virtual string DepartmentID { get; set; }
        public abstract class departmentID : IBqlField { }
        #endregion

        #region BranchID
        [PXDBInt()]
        [PXDefault(typeof(AccessInfo.branchID))]
        [PXUIField(DisplayName = ATRDMessages.BranchID)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.IBqlField { }
        #endregion

        #region TranPeriodID
        [PeriodID]
        public virtual String TranPeriodID { get; set; }
        public abstract class tranPeriodID : IBqlField { }
        #endregion

        #region IssueRefNbr
        [PXDBString(15)]
        [PXUIField(DisplayName = ATRDMessages.IssueRefNbr)]
        public virtual string IssueRefNbr { get; set; }
        public abstract class issueRefNbr : IBqlField { }
        #endregion

        #region RequestRefNbr
        [PXDBString(15)]
        [PXUIField(DisplayName = ATRDMessages.RequestRefNbr)]
        public virtual string RequestRefNbr { get; set; }
        public abstract class requestRefNbr : IBqlField { }
        #endregion

        #region OwnerID
        [Owner(typeof(workgroupID))]
        [PXDefault(typeof(Search<EPEmployee.defContactID, Where<EPEmployee.userID, Equal<Current<AccessInfo.userID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATRDMessages.OwnerID)]
        public virtual int? OwnerID { get; set; }
        public abstract class ownerID : IBqlField { }
        #endregion

        #region WorkgroupID
        [PXDBInt]
        [PXDefault(typeof(EPCompanyTree.workGroupID), PersistingCheck = PXPersistingCheck.Nothing)]
        [PX.TM.PXCompanyTreeSelector]
        [PXUIField(DisplayName = ATRDMessages.WorkgroupID, Enabled = false)]
        public virtual int? WorkgroupID { get; set; }
        public abstract class workgroupID : PX.Data.IBqlField { }
        #endregion

        #region Approved
        [PXDBBool()]
        [PXDefault(false)]
        [PXUIField(DisplayName = ATRDMessages.Approved, Visibility = PXUIVisibility.Visible, Enabled = false)]
        [PXUIVisible(typeof(Where<GetSetupValue<ATRDMaterialRequestSetup.materialRequestRequestApproval>, Equal<True>>))]
        public virtual bool? Approved { get; set; }
        public abstract class approved : IBqlField { }
        #endregion

        #region Rejected
        public abstract class rejected : IBqlField { }
        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? Rejected { get; set; }
        #endregion

        #region NoteID  
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : IBqlField { }
        #endregion

        #region IAssign Members
        int? IAssign.WorkgroupID
        {
            get { return WorkgroupID; }
            set { WorkgroupID = value; }
        }

        int? IAssign.OwnerID
        {
            get { return OwnerID; }
            set { OwnerID = value; }
        }
        #endregion

        #region Unbound

        #region RequestApproval
        [PXBool]
        [PXUnboundDefault(typeof(Search<ATRDMaterialRequestSetup.materialRequestRequestApproval>), PersistingCheck = PXPersistingCheck.Null)]
        [PXFormula(typeof(Default<ATRDMaterialRequestSetup.materialRequestRequestApproval>))]
        [PXUIField(DisplayName = ATRDMessages.RequestApproval, Visible = false)]
        public virtual bool? RequestApproval { get; set; }

        public abstract class requestApproval : IBqlField { }
        #endregion

        #region BAccountID
        /// <summary>
        /// The ID of the workgroup which was assigned to approve the transaction.
        /// </summary>
        [PXInt]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? BAccountID { get; set; }
        public abstract class bAccountID : IBqlField { }
        #endregion

        #region ApprovalWorkgroupID
        public abstract class approvalWorkgroupID : PX.Data.IBqlField
        {
        }
        protected int? _ApprovalWorkgroupID;
        [PXInt]
        [PXSelector(typeof(Search<EPCompanyTree.workGroupID>), SubstituteKey = typeof(EPCompanyTree.description))]
        [PXUIField(DisplayName = ATRDMessages.ApprovalWorkgroupID, Enabled = false)]
        public virtual int? ApprovalWorkgroupID
        {
            get
            {
                return this._ApprovalWorkgroupID;
            }
            set
            {
                this._ApprovalWorkgroupID = value;
            }
        }
        #endregion

        #region ApprovalOwnerID
        public abstract class approvalOwnerID : IBqlField
        {
        }
        protected int? _ApprovalOwnerID;
        [Owner(IsDBField = false)]
        [PXUIField(DisplayName = ATRDMessages.ApprovalOwnerID, Enabled = false)]
        public virtual int? ApprovalOwnerID
        {
            get
            {
                return this._ApprovalOwnerID;
            }
            set
            {
                this._ApprovalOwnerID = value;
            }
        }
        #endregion

        #endregion
    }

    public class ATRDMaterialRequestStatusAttribute : Attributes.Base.Status
    {
        public const string Cancelled = "Cancelled";
        public const string CancelledValue = "X";

        public class cancelledValue : BqlType<IBqlInt, string>.Constant<cancelledValue>
        {
            public cancelledValue() : base(CancelledValue) { }
        }

        public class ATRDMaterialRequestStatus : PXStringListAttribute
        {
            public ATRDMaterialRequestStatus()
                : base(
                    new[]
                    {
                        OpenValue, PendingValue,ClosedValue, RejectedValue,HoldValue,ReleaseValue,CancelledValue
                    },
                    new[]
                    {
                        Open,Pending,Closed,Rejected,Hold,Release,Cancelled
                    })
            {

            }
        }
    }
}
