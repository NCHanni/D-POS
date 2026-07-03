using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using RetailDimension.DAC;
using RetailDimension.DAC.Extension;

namespace RetailDimension.Graph.Extension
{
    public class ATRDBranchMaintExtension : PXGraphExtension<BranchMaint>
    {
        public static bool IsActive() => true;

        #region Overrides
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            var bAccount = Base.BAccount.Current;

            if (bAccount != null && Base.Accessinfo.ScreenID == "CS.10.20.00")
            {
                ATRDBranchPreference aTRDBranchPreference = PXGraph.CreateInstance<ATRDBranchPreference>();
                aTRDBranchPreference.Document.Current = SelectFrom<ATRDBranchSetup>
                    .InnerJoin<Branch>.On<Branch.branchID.IsEqual<ATRDBranchSetup.branchID>>
                    .Where<Branch.branchCD.IsEqual<@P.AsString>>
                    .View.Select(Base, bAccount.AcctCD);

                if (aTRDBranchPreference.Document.Current != null)
                {
                    ATRDBAccountExtension bAccountExt = bAccount.GetExtension<ATRDBAccountExtension>();
                    if ((bAccountExt.UsrATRDPermitNumber != aTRDBranchPreference.Document.Current.PermitNumber && bAccountExt.UsrATRDPermitNumber != null) ||
                        (bAccountExt.UsrATRDPermitMin != aTRDBranchPreference.Document.Current.PermitMin && bAccountExt.UsrATRDPermitNumber != null) ||
                        (bAccountExt.UsrATRDPermitIssued != aTRDBranchPreference.Document.Current.PermitIssued && bAccountExt.UsrATRDPermitNumber != null) ||
                        (bAccountExt.UsrATRDPermitExpiry != aTRDBranchPreference.Document.Current.PermitExpiry && bAccountExt.UsrATRDPermitNumber != null) ||
                        (bAccount.TaxRegistrationID != aTRDBranchPreference.Document.Current.VatRegistrationNo && bAccount.TaxRegistrationID != null))
                    {
                        aTRDBranchPreference.Document.Current.PermitNumber = bAccountExt.UsrATRDPermitNumber;
                        aTRDBranchPreference.Document.Current.PermitMin = bAccountExt.UsrATRDPermitMin;
                        aTRDBranchPreference.Document.Current.PermitIssued = bAccountExt.UsrATRDPermitIssued;
                        aTRDBranchPreference.Document.Current.PermitExpiry = bAccountExt.UsrATRDPermitExpiry;
                        aTRDBranchPreference.Document.Current.VatRegistrationNo = bAccount.TaxRegistrationID;
                        aTRDBranchPreference.Document.Cache.PersistUpdated(aTRDBranchPreference.Document.Current);
                    }

                    Contact contact = SelectFrom<Contact>
                        .Where<Contact.contactID.IsEqual<@P.AsInt>>
                        .View.Select(Base, bAccount.DefContactID);
                    //Address address = SelectFrom<Address>
                    //       .Where<Address.addressID.IsEqual<@P.AsInt>>
                    //       .View.Select(Base, bAccount.DefAddressID);
                    //Country country = Address.FK.Country.FindParent(Base, address);
                    //State state = Address.FK.State.FindParent(Base, address);
                    if (contact != null)
                    {
                        if ((contact.Phone1 != aTRDBranchPreference.Document.Current.ContactNo && contact.Phone1 != null) ||
                                (contact.WebSite != aTRDBranchPreference.Document.Current.Website && contact.WebSite != null) ||
                                (contact.EMail != aTRDBranchPreference.Document.Current.Email && contact.EMail != null) ||
                                (contact.Fax != aTRDBranchPreference.Document.Current.Fax && contact.Fax != null))
                        {
                            aTRDBranchPreference.Document.Current.ContactNo = contact.Phone1;
                            aTRDBranchPreference.Document.Current.Website = contact.WebSite;
                            aTRDBranchPreference.Document.Current.Email = contact.EMail;
                            aTRDBranchPreference.Document.Current.Fax = contact.Fax;
                            aTRDBranchPreference.Document.Cache.PersistUpdated(aTRDBranchPreference.Document.Current);
                        }
                    }
                }
            }
            baseMethod();
        }
        #endregion
    }
}
