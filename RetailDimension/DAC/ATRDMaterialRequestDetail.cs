using System;
using PX.Data;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.CT;
using PX.Objects.PM;
using PX.Data.BQL;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("Material Request Details")]
//#if Version25R1
//    [Obsolete]
//#endif
    public class ATRDMaterialRequestDetail : Base.Audit, IBqlTable
    {
        #region DetailID
        [PXDBLongIdentity(IsKey = true)]
        public virtual long? DetailID { get; set; }
        public abstract class detailID : IBqlField { }
        #endregion

        #region MaterialRequestRefNbr
        [PXDBString(15)]
        [PXDBDefault(typeof(ATRDMaterialRequest.refNbr))]
        [PXParent(typeof(Select<ATRDMaterialRequest,
                                Where<ATRDMaterialRequest.refNbr, Equal<Current<ATRDMaterialRequestDetail.materialRequestRefNbr>>>>))]
        public virtual string MaterialRequestRefNbr { get; set; }
        public abstract class materialRequestRefNbr : IBqlField { }
        #endregion

        #region BranchID
        [Branch(DisplayName = ATRDMessages.Branch)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : IBqlField { }
        #endregion

        #region InventoryID
        [StockItem(DisplayName = ATRDMessages.InventoryID)]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual Int32? InventoryID { get; set; }
        public abstract class inventoryID : IBqlField { }
        #endregion

        #region SiteID
        [PX.Objects.IN.SiteAvail(typeof(inventoryID))]
        [PXDefault(typeof(siteID), PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual Int32? SiteID { get; set; }
        public abstract class siteID : IBqlField { }
        #endregion

        #region LocationID
        [PX.Objects.IN.Location(typeof(siteID), DisplayName = ATRDMessages.LocationID, KeepEntry = false, ResetEntry = false, DescriptionField = typeof(INLocation.descr))]
        [PXDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
        public virtual int? LocationID { get; set; }
        public abstract class locationID : IBqlField { }
        #endregion

        #region Qty
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Null)]
        [PXUIField(DisplayName = ATRDMessages.Qty)]
        public virtual decimal? Qty { get; set; }
        public abstract class qty : PX.Data.IBqlField { }
        #endregion

        #region UOM
        [PXDBString(6)]
        [PXUIField(DisplayName = ATRDMessages.UOM)]
        //[PXFormula(typeof(Default<inventoryID>))]
        [PXSelector(typeof(Search<INUnit.fromUnit, Where<INUnit.inventoryID, Equal<Current<inventoryID>>>>))]
        //[PXDefault(typeof(Selector<inventoryID, InventoryItem.purchaseUnit>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string UOM { get; set; }
        public abstract class uOM : IBqlField { }
        #endregion

        #region ProjectID
        [PXDBInt]
        [PXDefault(typeof(PX.Objects.PM.NonProject), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATRDMessages.ProjectID)]
        [PXSelector(typeof(Search<Contract.contractID>), typeof(Contract.contractID), typeof(Contract.contractCD), typeof(Contract.description), DescriptionField = typeof(Contract.description), SubstituteKey = typeof(Contract.contractCD))]
        public virtual int? ProjectID { get; set; }
        public abstract class projectID : PX.Data.IBqlField { }
        #endregion

        #region ProjectTaskID
        [PXDBInt]
        [PXDefault()]
        [PXUIField(DisplayName = ATRDMessages.ProjectTaskID)]
        [PXSelector(typeof(Search<
            PMTask.taskID,
            Where<PMTask.projectID, Equal<Current<projectID>>>>), typeof(PMTask.taskCD), typeof(PMTask.description), DescriptionField = typeof(PMTask.description), SubstituteKey = typeof(PMTask.taskCD))]
        [PXUIRequired(typeof(Where<projectID, NotEqual<PX.Objects.PM.NonProject>>))]
        [PXUIEnabled(typeof(Where<projectID, NotEqual<PX.Objects.PM.NonProject>>))]
        public virtual int? ProjectTaskID { get; set; }
        public abstract class projectTaskID : PX.Data.IBqlField { }
        #endregion

        #region CostCodeID
        [PXDefault()]
        [CostCode(typeof(InventoryItem.cOGSAcctID), typeof(projectTaskID), PX.Objects.GL.AccountType.Expense, DisplayName = "Cost Code")]
        [PXUIRequired(typeof(Where<projectID, NotEqual<PX.Objects.PM.NonProject>>))]
        [PXUIEnabled(typeof(Where<projectID, NotEqual<PX.Objects.PM.NonProject>>))]
        public virtual Int32? CostCodeID { get; set; }
        public abstract class costCodeID : PX.Data.BQL.BqlInt.Field<costCodeID> { }
        #endregion

        #region Description
        [PXString]
        [PXUIField(DisplayName = ATRDMessages.Description, IsReadOnly = true)]
        [PXUnboundDefault(typeof(Selector<inventoryID, InventoryItem.descr>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<inventoryID>))]
        public virtual string Description { get; set; }
        public abstract class description : IBqlField { }
        #endregion

        #region NoteID  
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : IBqlField { }
        #endregion

        #region AccountID
        public abstract class accountID : BqlInt.Field<accountID> { }
        [Account(DisplayName = ATRDMessages.Account, Visibility = PXUIVisibility.Visible, DescriptionField = typeof(Account.description))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? AccountID { get; set; }
        #endregion

        #region SubaccountID
        public abstract class subaccountID : BqlInt.Field<subaccountID> { }
        [SubAccount(typeof(accountID), DisplayName = ATRDMessages.Subaccount, Visibility = PXUIVisibility.Visible, DescriptionField = typeof(Sub.description))]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? SubaccountID { get; set; }
        #endregion
    }
}
