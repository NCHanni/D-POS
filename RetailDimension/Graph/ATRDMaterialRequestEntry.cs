using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Objects.EP;
using PX.Objects.IN;
using PX.Objects.RQ;
using RetailDimension.DAC;
using RetailDimension.Helper;

namespace RetailDimension.Graph
{
    /// <summary>
    /// Screen ID: ATRD3004
    /// </summary>
    public class ATRDMaterialRequestEntry : PXGraph<ATRDMaterialRequestEntry, ATRDMaterialRequest>
    {
        #region ctor
        public ATRDMaterialRequestEntry()
        {
            Action.MenuAutoOpen = true;
            Action.AddMenuAction(CancelRequest);
            Action.AddMenuAction(CreateInventoryIssue);
            Action.AddMenuAction(CreatePurchaseRequest);
        }

        #endregion

        #region View
        public PXSetup<ATRDMaterialRequestSetup> Preferences;
        [PXViewName("Material Requests")]
        public PXSelect<ATRDMaterialRequest> Requests;
        [PXViewName("Material Request Details")]
        public PXSelect<ATRDMaterialRequestDetail, Where<ATRDMaterialRequestDetail.materialRequestRefNbr, Equal<Current<ATRDMaterialRequest.refNbr>>>> DocumentDetails;

        [PXViewName("Setup Approval")]
        public PXSelect<ATRDMaterialRequestSetupApproval> SetupApproval;

        [PXViewName("Approval")]
        public EPApprovalAutomation<ATRDMaterialRequest, ATRDMaterialRequest.approved, ATRDMaterialRequest.rejected, ATRDMaterialRequest.hold, ATRDMaterialRequestSetupApproval> Approval;
        #endregion 

        #region Events
        protected virtual void ATRDMaterialRequest_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            ATRDMaterialRequest row = e.Row as ATRDMaterialRequest;

            //if (Preferences.Current.MaterialRequestRequestApproval != true && row.Approved == true)
            //    PXUIFieldAttribute.SetVisible<ATRDMaterialRequest.approved>(sender, row, false);
            //else if (Preferences.Current.MaterialRequestRequestApproval != true && row.Approved == false)
            //    PXUIFieldAttribute.SetVisible<ATRDMaterialRequest.approved>(sender, row, false);
            //else
            //{
            //    PXUIFieldAttribute.SetVisible<ATRDMaterialRequest.approved>(sender, row, true);
            //}

            if (row.Status == ATRDMaterialRequestStatusAttribute.Open)
            {
                PXUIFieldAttribute.SetEnabled<ATRDMaterialRequest.hold>(sender, row, true);
            }

            if (row == null) { return; }

            PXDefaultAttribute.SetPersistingCheck<ATRDMaterialRequest.finPeriodID>(sender, row, PXPersistingCheck.NullOrBlank);

            sender.RaiseFieldUpdated<ATRDMaterialRequest.date>(row, null);

            //Button Behavior
            bool?[] cancelCondition = new bool?[] { (row.Hold ?? true) == false, string.IsNullOrEmpty(row.IssueRefNbr), row.Status != ATRDMaterialRequestStatusAttribute.CancelledValue };
            CancelRequest.SetEnabled(!cancelCondition.Contains(false));

            bool?[] issueCondition = new bool?[] { (row.Approved ?? false) == true, string.IsNullOrEmpty(row.IssueRefNbr) };
            CreateInventoryIssue.SetEnabled(!issueCondition.Contains(false));

            bool?[] requestCondition = new bool?[] { (row.Approved ?? false) == true, string.IsNullOrEmpty(row.RequestRefNbr) };
            CreatePurchaseRequest.SetEnabled(!requestCondition.Contains(false));

            //Lock UI
            bool lockUI = row.Hold ?? true;
            PXUIFieldAttribute.SetEnabled<ATRDMaterialRequest.date>(sender, row, lockUI);
            PXUIFieldAttribute.SetEnabled<ATRDMaterialRequest.finPeriodID>(sender, row, lockUI);
            PXUIFieldAttribute.SetEnabled<ATRDMaterialRequest.descr>(sender, row, lockUI);
            PXUIFieldAttribute.SetEnabled<ATRDMaterialRequest.requestedByID>(sender, row, lockUI);
            PXUIFieldAttribute.SetEnabled<ATRDMaterialRequest.departmentID>(sender, row, lockUI);

            DocumentDetails.AllowInsert = lockUI;
            DocumentDetails.AllowUpdate = lockUI;
            DocumentDetails.AllowDelete = lockUI;

            //Hold Behavior
            bool lockHold = row.Status == ATRDMaterialRequestStatusAttribute.CancelledValue ? false : row.Status == ATRDMaterialRequestStatusAttribute.ClosedValue ? false : true;
            PXUIFieldAttribute.SetEnabled<ATRDMaterialRequest.hold>(sender, row, lockHold);
        }

        protected virtual void _(Events.RowInserting<ATRDMaterialRequestDetail> e) 
        {
            ATRDMaterialRequestDetail row = e.Row;

            if (row == null) return;

            EPDepartment department = PXSelect<EPDepartment, 
                                        Where<EPDepartment.departmentID, 
                                        Equal<Current<ATRDMaterialRequest.departmentID>>>>.Select(this);

            if (department == null) return;

            row.AccountID = department.ExpenseAccountID;

            row.SubaccountID = department.ExpenseSubID;
        }
        #endregion

        #region Actions

        public PXAction<ATRDMaterialRequest> Action;
        [PXButton]
        [PXUIField(DisplayName = ATRDMessages.Action, MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable action(PXAdapter adapter,
                 [PXInt][PXIntList(new int[] { 1, 2 }, new string[] { "Persist", "Update" })] int? actionID,
                 [PXBool] bool refresh,
                 [PXString] string actionName
             )
        {
            List<ATRDMaterialRequest> result = new List<ATRDMaterialRequest>();
            if (actionName != null)
            {
                PXAction a = this.Actions[actionName];
                if (a != null)
                    foreach (PXResult<ATRDMaterialRequest> e in a.Press(adapter))
                        result.Add(e);
            }
            else
                foreach (ATRDMaterialRequest e in adapter.Get<ATRDMaterialRequest>())
                    result.Add(e);

            if (refresh)
            {
                foreach (ATRDMaterialRequest MyView in result)
                    Requests.Search<ATRDMaterialRequest.refNbr>(MyView.RefNbr);
            }
            if (actionID == 1) Save.Press();

            return result;
        }

        public PXAction<ATRDMaterialRequest> Hold;
        [PXUIField(DisplayName = ATRDMessages.Hold, Visible = false)]
        [PXButton(ImageKey = PX.Web.UI.Sprite.Main.DataEntryF)]
        protected virtual IEnumerable hold(PXAdapter adapter)
        {
            return adapter.Get();
        }

        public PXAction<ATRDMaterialRequest> CancelRequest;
        [PXProcessButton()]
        [PXUIField(DisplayName = ATRDMessages.CancelRequest, Visible = false)]
        public IEnumerable cancelRequest(PXAdapter adapter)
        {
            if (Requests.Current != null)
            {
                Requests.Current.Status = ATRDMaterialRequestStatusAttribute.CancelledValue;
                Requests.UpdateCurrent();

                this.Save.Press();
            }

            return adapter.Get();
        }

        public PXAction<ATRDMaterialRequest> CreateInventoryIssue;
        [PXProcessButton()]
        [PXUIField(DisplayName = ATRDMessages.CreateInventoryIssue, Visible = false)]
        public IEnumerable createInventoryIssue(PXAdapter adapter)
        {
#if Version25R1 || Version25R2
            Actions.PressSave();

            var graph = this;
            ATRDMaterialRequest request = Requests.Current;
#endif

            PXLongOperation.StartOperation(this, () =>
            {
#if Version23R1
                Actions.PressSave();
#endif

                using (PXTransactionScope ts = new PXTransactionScope())
                {
#if Version23R1
                    ATRDMaterialRequest request = Requests.Current;
#endif

                    INIssueEntry entry = PXGraph.CreateInstance<INIssueEntry>();
                    INRegister register = new INRegister();

                    register = (INRegister)entry.issue.Insert(register);

                    register.BranchID = request.BranchID;
                    register.DocType = INDocType.Issue;
                    register.TranDate = request.Date;
                    register.FinPeriodID = request.FinPeriodID;

                    register.ExtRefNbr = request.RefNbr;
                    register.TranDesc = request.Descr;

                    foreach (PXResult<ATRDMaterialRequestDetail> detail in DocumentDetails.Select())
                    {
                        ATRDMaterialRequestDetail doc = detail;
                        INTran tran = new INTran();
                        tran = (INTran)entry.transactions.Insert(tran);

                        tran.TranType = INTranType.Issue;
                        tran.DocType = INDocType.Issue;

                        entry.transactions.SetValueExt<INTran.tranType>(tran, INTranType.Issue);
                        entry.transactions.SetValueExt<INTran.inventoryID>(tran, doc.InventoryID);
                        entry.transactions.SetValueExt<INTran.siteID>(tran, doc.SiteID);
                        entry.transactions.SetValueExt<INTran.qty>(tran, doc.Qty);

                        tran.BranchID = doc.BranchID;
                        tran.LocationID = doc.LocationID;
                        tran.ProjectID = doc.ProjectID;
                        tran.TaskID = doc.ProjectTaskID;
                        tran.CostCodeID = doc.CostCodeID;
                        tran.COGSAcctID = doc.AccountID;
                        tran.COGSSubID = doc.SubaccountID;

                        tran = entry.transactions.Update(tran);
                    }

                    register = entry.issue.Update(register);
                    entry.Actions.PressSave();

#if Version23R1
                    request.IssueRefNbr = register.RefNbr;
                    request.Status = ATRDMaterialRequestStatusAttribute.ClosedValue;
                    Requests.Update(request);
                    this.Save.Press();
#endif
#if Version25R1 || Version25R2
                    request.IssueRefNbr = register.RefNbr;
                    request.Status = ATRDMaterialRequestStatusAttribute.ClosedValue;
                    graph.Requests.Update(request);
                    graph.Save.Press();
#endif

                    ts.Complete();
                }
            });

            return adapter.Get();
        }

        public PXAction<ATRDMaterialRequest> CreatePurchaseRequest;
        [PXProcessButton()]
        [PXUIField(DisplayName = ATRDMessages.CreatePurchaseRequest, Visible = false)]
        public IEnumerable createPurchaseRequest(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, () =>
            {
                Actions.PressSave();
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATRDMaterialRequest request = Requests.Current;

                    RQRequestEntry entry = PXGraph.CreateInstance<RQRequestEntry>();
                    RQRequest req = new RQRequest();

                    req = (RQRequest)entry.Document.Insert(req);

                    req.BranchID = request.BranchID;
                    req.OrderDate = request.Date;
                    req.FinPeriodID = request.FinPeriodID;
                    req.EmployeeID = request.RequestedByID;
                    req.Priority = 1;

                    entry.Document.SetValueExt<RQRequest.employeeID>(req, request.RequestedByID);

                    req.DepartmentID = request.DepartmentID;
                    req.Description = request.Descr;

                    foreach (PXResult<ATRDMaterialRequestDetail> detail in DocumentDetails.Select())
                    {
                        ATRDMaterialRequestDetail doc = detail;
                        RQRequestLine tran = new RQRequestLine();
                        tran = (RQRequestLine)entry.Lines.Insert(tran);

                        entry.Lines.SetValueExt<RQRequestLine.inventoryID>(tran, doc.InventoryID);
                        entry.Lines.SetValueExt<RQRequestLine.orderQty>(tran, doc.Qty);
                      
                        tran.BranchID = doc.BranchID;
                        
                        tran = entry.Lines.Update(tran);
                    }

                    req = entry.Document.Update(req);
                    entry.Actions.PressSave();

                    request.RequestRefNbr = req.OrderNbr;
                    Requests.Update(request);
                    this.Save.Press();

                    ts.Complete();
                }
            });

            return adapter.Get();
        }

        public PXAction<ATRDMaterialRequest> openTransactionIssue;
        [PXButton(Tooltip = "Open Issue", CommitChanges = true)]
        [PXUIField(DisplayName = ATRDMessages.OpenIssue)]
        public virtual void OpenTransactionIssue()
        {
            ATRDMaterialRequest row = Requests.Current;

            INIssueEntry graph = PXGraph.CreateInstance<INIssueEntry>();
            INRegister data = PXSelect<
                INRegister,
                Where<INRegister.refNbr, Equal<Required<ATRDMaterialRequest.issueRefNbr>>>>
                .Select(this, row.IssueRefNbr);
            graph.issue.Current = data;

            throw new PXRedirectRequiredException(graph, true, "Issues") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }

        public PXAction<ATRDMaterialRequest> openTransactionRequest;
        [PXButton(Tooltip = "Open Request", CommitChanges = true)]
        [PXUIField(DisplayName = ATRDMessages.OpenRequest)]
        public virtual void OpenTransactionRequest()
        {
            ATRDMaterialRequest row = Requests.Current;

            RQRequestEntry graph = PXGraph.CreateInstance<RQRequestEntry>();
            RQRequest data = PXSelect<
                RQRequest,
                Where<RQRequest.orderNbr, Equal<Required<ATRDMaterialRequest.requestRefNbr>>>>
                .Select(this, row.RequestRefNbr);
            graph.Document.Current = data;

            throw new PXRedirectRequiredException(graph, true, "Request") { Mode = PXBaseRedirectException.WindowMode.NewWindow };
        }

#endregion

        #region EPApproval Cache Attached
        [PXDBDate()]
        [PXDefault(typeof(ATRDMaterialRequest.date), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_DocDate_CacheAttached(PXCache sender)
        {
        }

        [PXDBInt()]
        [PXDefault(typeof(ATRDMaterialRequest.requestedByID), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_BAccountID_CacheAttached(PXCache sender)
        {
        }

        [PXDBString(60, IsUnicode = true)]
        [PXDefault(typeof(ATRDMaterialRequest.descr), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_Descr_CacheAttached(PXCache sender)
        {
        }

        #endregion
    }
}