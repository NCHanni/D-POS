using PX.Data;
using PX.Data.BQL;
using PX.EP;
using PX.Objects.AR;
using PX.Objects.CR;
using PX.Objects.GL;
using PX.SM;
using RetailDimension.DAC;
using RetailDimension.Helper;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RetailDimension.Graphs
{
    /// <summary>
    /// Screen ID: ATRD2008
    /// </summary>
    public class ATRDRECustomerAccountRegistrationMaint : PXGraph<ATRDRECustomerAccountRegistrationMaint>
    {
        public PXSetup<ATRDSetup> Setup;

        public PXFilter<ATRDCustomerAccount> Filter;

        public PXSelect<ATRDPortalAccount> PortalAccounts;

        public PXCancel<ATRDCustomerAccount> Cancel;

        public PXAction<ATRDCustomerAccount> CreateUserAccount;

        public PXAction<ATRDCustomerAccount> CreateUser;
        public PXAction<ATRDCustomerAccount> DisableUserAccount;
        public PXAction<ATRDCustomerAccount> EnableUserAccount;

        [PXButton]
        [PXUIField(DisplayName = ATRDMessages.CreateUser)]
        protected IEnumerable createUser(PXAdapter adapter)
        {
            List<ATRDCustomerAccount> result = new List<ATRDCustomerAccount>();

            foreach (ATRDCustomerAccount item in adapter.Get<ATRDCustomerAccount>())
            {
                result.Add(item);
            }

            PXLongOperation.StartOperation(this, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATRDCustomerAccount filter = Filter.Current;

                    ATRDSetup settings = Setup.Current;

                    if (settings.PortalRole == null)
                        throw new Exception(ATRDMessages.AccountRegisterFillupTheRequiredFields);
                    if (filter.CustomerID == null || filter.ContactID == null)
                        throw new Exception(ATRDMessages.AccountRegisterFillupTheRequiredFields);
                    if (filter.Email == null)
                        throw new Exception(ATRDMessages.AccountRegisterEmailIsRequired);

                    #region Create Portal User

                    Tuple<Guid?, string> res = CreatePortalUser(filter, settings.PortalRole);

                    if (res.Item1 == null) throw new Exception(res.Item2);

                    #endregion

                    #region Send Email

                    #endregion

                    #region Save to Portal Accounts

                    ATRDPortalAccount account = new ATRDPortalAccount
                    {
                        ContactID = filter.ContactID,
                        CustomerID = filter.CustomerID,
                        UserID = res.Item1,
                        BranchID = filter.BranchID
                    };

                    PortalAccounts.Insert(account);

                    PortalAccounts.UpdateCurrent();

                    this.Actions.PressSave();
                    #endregion                    

                    ts.Complete();
                }
            });

            return result;
        }


        [PXButton]
        [PXUIField(DisplayName = ATRDMessages.DisableUser, Visible = false)]
        protected IEnumerable disableUserAccount(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATRDCustomerAccount filter = Filter.Current;

                    AccessUsers graph = PXGraph.CreateInstance<AccessUsers>();

                    Users user = graph.UserList.Search<Users.username>(filter.Email);

                    if (user == null) return;

                    graph.UserList.Current = user;

                    graph.UserList.DisableLogin.Press();

                    ts.Complete();
                }
            });

            return adapter.Get();
        }

        [PXButton]
        [PXUIField(DisplayName = ATRDMessages.EnableUser, Visible = false)]
        protected IEnumerable enableUserAccount(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, delegate ()
            {
                using (PXTransactionScope ts = new PXTransactionScope())
                {
                    ATRDCustomerAccount filter = Filter.Current;

                    AccessUsers graph = PXGraph.CreateInstance<AccessUsers>();

                    Users user = graph.UserList.Search<Users.username>(filter.Email);

                    if (user == null) return;

                    graph.UserList.Current = user;

                    graph.UserList.EnableLogin.Press();

                    ts.Complete();
                }
            });

            return adapter.Get();
        }

        public void _(Events.RowSelected<ATRDCustomerAccount> e)
        {
            ATRDCustomerAccount row = e.Row;

            if (row == null) return;

            var portalUserID = row.UserID;

            bool hasPortalUserID = portalUserID != null;

            var portalUserIsActive = row.IsActive ?? false;

            Filter.Cache.RaiseFieldUpdated<ATRDCustomerAccount.email>(row, null);

            PXUIFieldAttribute.SetEnabled<ATRDCustomerAccount.branchID>(e.Cache, row, !hasPortalUserID);

            EnableUserAccount.SetVisible((hasPortalUserID && !portalUserIsActive));

            DisableUserAccount.SetVisible((hasPortalUserID && portalUserIsActive));

            CreateUser.SetEnabled(!hasPortalUserID);
        }

        public void _(Events.FieldUpdated<ATRDCustomerAccount, ATRDCustomerAccount.email> e)
        {
            ATRDCustomerAccount row = e.Row;

            if (row == null) return;

            if (row.Email == null) return;

            Users user = PXSelect<Users, Where<Users.email, Equal<Required<ATRDCustomerAccount.email>>>>.Select(this, row.Email);

            if (user == null)
            {
                row.UserID = null;
                return;
            }

            if (user.PKID == null)
            {
                row.UserID = null;
                return;
            }

            row.UserID = user.PKID;

            row.IsActive = user.IsApproved;
        }

        #region Methods

        public Tuple<Guid?, string> CreatePortalUser(ATRDCustomerAccount filter, string role)
        {

            string message = string.Empty;

            AccessUsers graph = PXGraph.CreateInstance<AccessUsers>();

            try
            {
                graph.Clear();

                Users u = new Users();
                u.Username = filter.Email;
                u = graph.UserList.Insert(u);
                graph.UserList.Current.GeneratePassword = false;
                graph.UserList.Current.Password = ATRDConstants.DEFAULT_PASSWORD;
                graph.UserList.Current.Email = filter.Email;
                graph.UserList.Current.Comment = ATRDMessages.GeneratedFromCustomerAccountRegistration;
                graph.UserList.Current.FirstName = filter.FirstName;
                graph.UserList.Current.LastName = filter.LastName;

                u = graph.UserList.UpdateCurrent();

                foreach (EPLoginTypeAllowsRole r in graph.AllowedRoles.Select())
                {
                    if (r.Rolename == role)
                    {
                        r.Selected = true;

                        graph.AllowedRoles.Update(r);
                    }
                }

                graph.Save.Press();

                return new Tuple<Guid?, string>(u.PKID, message);

            }
            catch (Exception e)
            {

                message = string.Format(ATRDMessages.ErrorProcessingTransaction, e.Message);

                return new Tuple<Guid?, string>(null, message);
            }
        }

        #endregion



        #region Internal Types
        [Serializable]
        [PXCacheName("Customer Accounts")]
#if Version23R1
    public partial class ATRDCustomerAccount : IBqlTable
#endif
#if Version25R1 || Version25R2
        public partial class ATRDCustomerAccount : PXBqlTable, IBqlTable
#endif
        {
            #region CustomerID
            public abstract class customerID : BqlInt.Field<customerID> { }

            [CustomerActive(DisplayName = ATRDMessages.Customer)]
            [PXUnboundDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
            public virtual int? CustomerID { get; set; }
            #endregion

            #region Contact ID
            [PXInt]
            [PXSelector(typeof(Search<ContactExtAddress.contactID,
                                Where<ContactExtAddress.contactBAccountID, Equal<Current<ATRDCustomerAccount.customerID>>,
                                And<ContactExtAddress.contactType, NotEqual<ContactTypesAttribute.bAccountProperty>>>>),
                        typeof(ContactExtAddress.contactDisplayName),
                        typeof(ContactExtAddress.eMail), SubstituteKey = typeof(ContactExtAddress.contactDisplayName)
                        )]
            [PXUIField(DisplayName = ATRDMessages.Contact)]
            [PXFormula(typeof(Default<customerID>))]
            [PXUnboundDefault(PersistingCheck = PXPersistingCheck.NullOrBlank)]
            public virtual int? ContactID { get; set; }
            public abstract class contactID : BqlInt.Field<contactID> { }
            #endregion

            #region FirstName
            public abstract class firstName : BqlString.Field<firstName> { }
            [PXString(50)]
            [PXUnboundDefault(typeof(Selector<contactID, ContactExtAddress.firstName>), PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(typeof(Default<contactID>))]
            [PXUIField(DisplayName = ATRDMessages.FirstName, Enabled = false)]
            public virtual string FirstName { get; set; }
            #endregion

            #region LastName
            public abstract class lastName : BqlString.Field<lastName> { }
            [PXString(50)]
            [PXUnboundDefault(typeof(Selector<contactID, ContactExtAddress.lastName>), PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(typeof(Default<contactID>))]
            [PXUIField(DisplayName = ATRDMessages.LastName, Enabled = false)]
            public virtual string LastName { get; set; }
            #endregion

            #region Email
            public abstract class email : BqlString.Field<email> { }
            [PXString(50)]
            [PXUnboundDefault(typeof(Selector<contactID, ContactExtAddress.eMail>), PersistingCheck = PXPersistingCheck.Nothing)]
            [PXFormula(typeof(Default<contactID>))]
            [PXUIField(DisplayName = ATRDMessages.Email, Enabled = false)]
            public virtual string Email { get; set; }
            #endregion

            #region UserID
            [PXGuid]
            //[PXUnboundDefault(typeof(Search<Users.pKID, Where<Users.username, Equal<Current<ATRDCustomerAccount.email>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
            //[PXFormula(typeof(Default<ATRDCustomerAccount.email>))]
            [PXUIField(Visible = false, Enabled = false)]
            public virtual Guid? UserID { get; set; }
            public abstract class userID : BqlGuid.Field<userID> { }
            #endregion

            [Branch()]
            public virtual int? BranchID { get; set; }
            public abstract class branchID : BqlInt.Field<branchID> { }

            [PXBool]
            [PXUIField(DisplayName = ATRDMessages.IsActive, Enabled = false)]
            public virtual bool? IsActive { get; set; }
            public abstract class isActive : BqlBool.Field<isActive> { }
        }
        #endregion
    }
}