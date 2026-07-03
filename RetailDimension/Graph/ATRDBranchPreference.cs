using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using RetailDimension.DAC;
using RetailDimension.DAC.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE: ATRD1004
    /// </summary>
    public class ATRDBranchPreference : PXGraph<ATRDBranchPreference, ATRDBranchSetup>
    {
        #region View
        public SelectFrom<ATRDBranchSetup>.View Document;
        public PXSetup<ATRDSetup> Setup;
        #endregion

        protected virtual void _(Events.RowSelected<ATRDBranchSetup> e)
        {
            if (e.Cache.GetStatus(e.Row) == PXEntryStatus.Inserted)
            {
                Branch branch = SelectFrom<Branch>
                    .Where<Branch.branchID.IsEqual<@P.AsInt>>
                    .View.Select(this, Document.Current.BranchID);
                if (branch == null) return;

                BAccount bAccount = SelectFrom<BAccount>
                    .Where<BAccount.bAccountID.IsEqual<@P.AsInt>>
                    .View.Select(this, branch.BAccountID);

                if (bAccount != null)
                {
                    var bAccountExt = bAccount.GetExtension<ATRDBAccountExtension>();
                    Contact contact = SelectFrom<Contact>
                        .Where<Contact.contactID.IsEqual<@P.AsInt>>
                        .View.Select(this, bAccount.DefContactID);
                    Address address = SelectFrom<Address>
                        .Where<Address.addressID.IsEqual<@P.AsInt>>
                        .View.Select(this, bAccount.DefAddressID);
                    Country country = Address.FK.Country.FindParent(this, address);
                    State state = Address.FK.State.FindParent(this, address);

                    if (bAccount != null && (Document.Current.VatRegistrationNo == null && bAccount.TaxRegistrationID != null))
                    {
                        Document.Current.VatRegistrationNo = bAccount.TaxRegistrationID;
                        e.Cache.IsDirty = true;
                    }
                    if (bAccount != null && Document.Current.Name2 == null && bAccount.AcctName != null)
                    {
                        Document.Current.Name2 = bAccount.AcctName;
                        e.Cache.IsDirty = true;
                    }

                    //POS Settings
                    if (bAccountExt != null && (Document.Current.PermitNumber == null ||
                        Document.Current.PermitMin == null ||
                        Document.Current.PermitIssued == null ||
                        Document.Current.PermitExpiry == null))
                    {
                        Document.Current.PermitNumber = Document.Current.PermitNumber ?? bAccountExt.UsrATRDPermitNumber;
                        Document.Current.PermitMin = Document.Current.PermitMin ?? bAccountExt.UsrATRDPermitMin;
                        Document.Current.PermitIssued = Document.Current.PermitIssued ?? bAccountExt.UsrATRDPermitIssued;
                        Document.Current.PermitExpiry = Document.Current.PermitExpiry ?? bAccountExt.UsrATRDPermitExpiry;
                        e.Cache.IsDirty = true;
                    }


                    //Contact Details
                    if (contact != null && (Document.Current.ContactNo == null ||
                        Document.Current.Website == null ||
                        Document.Current.Email == null ||
                        Document.Current.Fax == null))
                    {
                        Document.Current.ContactNo = Document.Current.ContactNo ?? contact.Phone1;
                        Document.Current.Website = Document.Current.Website ?? contact.WebSite;
                        Document.Current.Email = Document.Current.Email ?? contact.EMail;
                        Document.Current.Fax = Document.Current.Fax ?? contact.Fax;
                        e.Cache.IsDirty = true;
                    }

                    if (address != null && Document.Current.Address == null)
                    {
                        Document.Current.Address = Document.Current.Address ?? string.Concat(address.AddressLine1, !string.IsNullOrEmpty(address.AddressLine2) ? ", " : "",
                                address.AddressLine2, !string.IsNullOrEmpty(address.City) ? ", " : "",
                                address.City, !string.IsNullOrEmpty(address.PostalCode) ? ", " : "",
                                address.PostalCode, !string.IsNullOrEmpty(state?.Name) ? ", " : "",
                                state?.Name ?? "", !string.IsNullOrEmpty(country.Description) ? ", " : "", country.Description) ?? "";
                        e.Cache.IsDirty = true;
                    }
                }
            }
        }

        public override void Persist()
        {
            ATRDBranchSetup branchSetup = Document.Current;
            if (branchSetup != null && Accessinfo.ScreenID == "AT.RD.10.04")
            {
                BranchMaint branchMaint = PXGraph.CreateInstance<BranchMaint>();
                branchMaint.BAccount.Current = SelectFrom<BranchMaint.BranchBAccount>
                    .InnerJoin<Branch>.On<Branch.branchCD.IsEqual<BranchMaint.BranchBAccount.acctCD>>
                    .Where<Branch.branchID.IsEqual<@P.AsInt>>
                    .View.Select(this, branchSetup.BranchID);
                if (branchMaint.BAccount.Current != null)
                {
                    ATRDBAccountExtension bAccountExt = branchMaint.BAccount.Current.GetExtension<ATRDBAccountExtension>();
                    if ((bAccountExt.UsrATRDPermitNumber != branchSetup.PermitNumber && branchSetup.PermitNumber != null) ||
                        (bAccountExt.UsrATRDPermitMin != branchSetup.PermitMin && branchSetup.PermitMin != null) ||
                        (bAccountExt.UsrATRDPermitIssued != branchSetup.PermitIssued && branchSetup.PermitIssued != null) ||
                        (bAccountExt.UsrATRDPermitExpiry != branchSetup.PermitExpiry && branchSetup.PermitExpiry != null))
                    {
                        bAccountExt.UsrATRDPermitNumber = branchSetup.PermitNumber;
                        bAccountExt.UsrATRDPermitMin = branchSetup.PermitMin;
                        bAccountExt.UsrATRDPermitIssued = branchSetup.PermitIssued;
                        bAccountExt.UsrATRDPermitExpiry = branchSetup.PermitExpiry;
                        branchMaint.BAccount.Cache.PersistUpdated(branchMaint.BAccount.Current);
                    }

                    Contact contact = SelectFrom<Contact>
                        .Where<Contact.contactID.IsEqual<@P.AsInt>>
                        .View.Select(this, branchMaint.BAccount.Current.DefContactID);
                    if (contact != null)
                    {
                        if ((contact.Phone1 != branchSetup.ContactNo && branchSetup.ContactNo != null) ||
                            (contact.WebSite != branchSetup.Website && branchSetup.Website != null) ||
                            (contact.EMail != branchSetup.Email && branchSetup.Email != null) ||
                            (contact.Fax != branchSetup.Fax && branchSetup.Fax != null))
                        {
                            contact.Phone1 = branchSetup.ContactNo;
                            contact.WebSite = branchSetup.Website;
                            contact.EMail = branchSetup.Email;
                            contact.Fax = branchSetup.Fax;
                            branchMaint.ContactDummy.Update(contact);
                            branchMaint.ContactDummy.Cache.Persist(contact, PXDBOperation.Update);
                        }
                    }
                }
            }
            base.Persist();
        }
    }
}