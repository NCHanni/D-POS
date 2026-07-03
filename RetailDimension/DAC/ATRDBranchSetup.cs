using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Objects.CA;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.SO;
using RetailDimension.DAC.Base;
using RetailDimension.DAC.Extension;
using RetailDimension.Graph;
using System;
using static RetailDimension.Attributes.Base.AggregateAttribute;

namespace RetailDimension.DAC
{
    //[PXProjection(typeof(SelectFrom<ATRDSetup>
    //        .CrossJoin<Branch>
    //        .InnerJoin<BAccount>.On<BAccount.bAccountID.IsEqual<Branch.bAccountID>>
    //        .InnerJoin<Contact>.On<Contact.contactID.IsEqual<BAccount.defContactID>>), Persistent = true)]
    //[PXProjection(typeof(SelectFrom<BAccount>
    //        .InnerJoin<Branch>.On<Branch.bAccountID.IsEqual<BAccount.bAccountID>>
    //        .InnerJoin<Contact>.On<Contact.contactID.IsEqual<BAccount.defContactID>>
    //        .CrossJoin<ATRDSetup>), new Type[] { typeof(BAccount) }, Persistent = true)]
    [Serializable]
    [PXCacheName("RD-Branch Setup")]
    [PXPrimaryGraph(typeof(ATRDBranchPreference))]
    public class ATRDBranchSetup : Table, IBqlTable
    {
        #region BranchID
        //[Branch(DisplayName = "Setup Branch ID", IsKey = true)]
        [PXDBInt(IsKey =true)]
        [PXUIField(DisplayName = ATRDMessages.BranchID)]
        [PXSelector(typeof(Search<Branch.branchID>), SubstituteKey = typeof(Branch.branchCD), DescriptionField = typeof(Branch.acctName))]
        [PXDefault()]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }
        #endregion

        #region CustomerID
        [CustomerActive]
        [PXUIField(DisplayName = ATRDMessages.Customer, Visibility = PXUIVisibility.Visible, Required = true)]
        [PXDefault(typeof(ATRDSetup.customerID))]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID : BqlInt.Field<customerID> { }
        #endregion

        #region CustomerSCID
        [CustomerActive]
        [PXUIField(DisplayName = ATRDMessages.CustomerSC, Visibility = PXUIVisibility.Visible, Required = true)]
        [PXDefault(typeof(ATRDSetup.customerSCID))]
        public virtual int? CustomerSCID { get; set; }
        public abstract class customerSCID : BqlString.Field<customerSCID> { }
        #endregion

        #region CustomerPWDID
        [CustomerActive]
        [PXUIField(DisplayName = ATRDMessages.CustomerPWD, Visibility = PXUIVisibility.Visible, Required = true)]
        [PXDefault(typeof(ATRDSetup.customerPWDID))]
        public virtual int? CustomerPWDID { get; set; }
        public abstract class customerPWDID : BqlString.Field<customerPWDID> { }
        #endregion

        #region NumberSequenceID
        [ATRDNumberingSequence]
        [PXUIField(DisplayName = ATRDMessages.NumberSequence, Visibility = PXUIVisibility.Visible, Enabled = false)]
        [PXDefault(typeof(ATRDSetup.numberSequenceID))]
        public virtual string NumberSequenceID { get; set; }
        public abstract class numberSequenceID : BqlString.Field<numberSequenceID> { }
        #endregion

        #region MemoNumberSequenceID
        [ATRDNumberingSequence]
        [PXUIField(DisplayName = ATRDMessages.MemoNumberSequence, Visibility = PXUIVisibility.Visible, Enabled = false)]
        [PXDefault(typeof(ATRDSetup.memoNumberSequenceID))]
        public virtual string MemoNumberSequenceID { get; set; }
        public abstract class memoNumberSequenceID : BqlString.Field<memoNumberSequenceID> { }
        #endregion

        #region CustomerClassID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CustomerClass)]
        [PXSelector(typeof(Search<CustomerClass.customerClassID>), typeof(CustomerClass.customerClassID), typeof(CustomerClass.descr), DescriptionField = typeof(CustomerClass.descr))]
        [PXDefault(typeof(ATRDSetup.customerClassID))]
        public virtual string CustomerClassID { get; set; }
        public abstract class customerClassID : BqlString.Field<customerClassID> { }
        #endregion

        #region OrderType
        [PXDBString(2, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.OrderType)]
        [PXSelector(typeof(Search<SOOrderType.orderType, Where<SOOrderType.active, Equal<True>>>), typeof(SOOrderType.orderType), typeof(SOOrderType.descr), DescriptionField = typeof(SOOrderType.descr))]
        [PXDefault(typeof(ATRDSetup.orderType))]
        public virtual string OrderType { get; set; }
        public abstract class orderType : BqlString.Field<orderType> { }
        #endregion

        #region LineLimit
        [PXDBInt]
        [PXUIField(DisplayName = ATRDMessages.LineLimit)]
        [PXDefault(typeof(ATRDSetup.lineLimit), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? LineLimit { get; set; }
        public abstract class lineLimit : BqlInt.Field<lineLimit> { }
        #endregion

        #region SiteID
        [Site(DisplayName = ATRDMessages.Warehouse, DescriptionField = typeof(INSite.descr), Required = true)]
        [PXDefault]
        //[PXDefault(typeof(ATRDSetup.siteID), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? SiteID { get; set; }
        public abstract class siteID : BqlInt.Field<siteID> { }
        #endregion

        #region CompanyInfo

        #region Name
        [PXDBString(200, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Name, Required = true)]
        [PXDefault(typeof(ATRDSetup.name))]
        public virtual string Name { get; set; }
        public abstract class name : BqlString.Field<name> { }
        #endregion

        #region Name2
        [PXDBString(200, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Name2, Required = true)]
        [PXDefault(typeof(BAccount.acctName))]
        public virtual string Name2 { get; set; }
        public abstract class name2 : BqlString.Field<name2> { }
        #endregion

        #region PermitNumber
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.PermitNumber, Required = true)]
        [PXDefault(typeof(ATRDBAccountExtension.usrATRDPermitNumber))]
        public virtual string PermitNumber { get; set; }
        public abstract class permitNumber : BqlString.Field<permitNumber> { }
        #endregion

        #region PermitMin
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.PermitMin, Required = true)]
        [PXDefault(typeof(ATRDBAccountExtension.usrATRDPermitMin))]
        public virtual string PermitMin { get; set; }
        public abstract class permitMin : BqlString.Field<permitMin> { }
        #endregion

        #region PermitIssued
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.PermitIssued, Required = true)]
        [PXDefault(typeof(ATRDBAccountExtension.usrATRDPermitIssued))]
        public virtual DateTime? PermitIssued { get; set; }
        public abstract class permitIssued : BqlDateTime.Field<permitIssued> { }
        #endregion

        #region PermitExpiry
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.PermitExpiry, Required = false)]
        [PXDefault(typeof(ATRDBAccountExtension.usrATRDPermitExpiry), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual DateTime? PermitExpiry { get; set; }
        public abstract class permitExpiry : BqlDateTime.Field<permitExpiry> { }
        #endregion

        #region VatRegistrationNo
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.VatRegistrationNo, Required = true)]
        [PXDefault(typeof(BAccount.taxRegistrationID))]
        public virtual string VatRegistrationNo { get; set; }
        public abstract class vatRegistrationNo : BqlString.Field<vatRegistrationNo> { }
        #endregion

        #region BusinessStyle
        [PXDBString(50, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.BusinessStyle)]
        [PXDefault(typeof(ATRDSetup.businessStyle), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string BusinessStyle { get; set; }
        public abstract class businessStyle : BqlString.Field<businessStyle> { }
        #endregion

        #region Address
        [PXDBString(250, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Address, Required = true)]
        //[PXDefault(typeof(ATRDSetup.address))]
        public virtual string Address { get; set; }
        public abstract class address : BqlString.Field<address> { }
        #endregion

        #region ContactNo
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.ContactNo, Required = true)]
        [PXDefault(typeof(Contact.phone1))]
        public virtual string ContactNo { get; set; }
        public abstract class contactNo : BqlString.Field<contactNo> { }
        #endregion

        #region Email
        [PXDBString(200, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Email, Required = true)]
        [PXDefault(typeof(Contact.eMail))]
        public virtual string Email { get; set; }
        public abstract class email : BqlString.Field<email> { }
        #endregion

        #region Website
        [PXDBString(200, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Website, Required = true)]
        [PXDefault(typeof(Contact.webSite))]
        public virtual string Website { get; set; }
        public abstract class website : BqlString.Field<website> { }
        #endregion

        #region Fax
        [PXDBString(200, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Fax, Required = true)]
        [PXDefault(typeof(Contact.fax))]
        public virtual string Fax { get; set; }
        public abstract class fax : BqlString.Field<fax> { }
        #endregion

        #endregion

        #region CashPaymentMethodID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Cash)]
        [PXSelector(typeof(Search<PaymentMethod.paymentMethodID>), typeof(PaymentMethod.paymentMethodID), typeof(PaymentMethod.descr), DescriptionField = typeof(PaymentMethod.descr))]
        [PXDefault(typeof(ATRDSetup.cashPaymentMethodID))]
        public virtual string CashPaymentMethodID { get; set; }
        public abstract class cashPaymentMethodID : BqlString.Field<cashPaymentMethodID> { }
        #endregion

        #region CardPaymentMethodID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CreditCard)]
        [PXSelector(typeof(Search<PaymentMethod.paymentMethodID>), typeof(PaymentMethod.paymentMethodID), typeof(PaymentMethod.descr), DescriptionField = typeof(PaymentMethod.descr))]
        [PXDefault(typeof(ATRDSetup.cardPaymentMethodID))]
        public virtual string CardPaymentMethodID { get; set; }
        public abstract class cardPaymentMethodID : BqlString.Field<cardPaymentMethodID> { }
        #endregion

        #region MemoPaymentMethodID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CreditMemo, Visible = false)] //Hide by Cherry V.
        [PXSelector(typeof(Search<PaymentMethod.paymentMethodID>), typeof(PaymentMethod.paymentMethodID), typeof(PaymentMethod.descr), DescriptionField = typeof(PaymentMethod.descr))]
        //[PXDefault(typeof(ATRDSetup.memoPaymentMethodID))]
        public virtual string MemoPaymentMethodID { get; set; }
        public abstract class memoPaymentMethodID : BqlString.Field<memoPaymentMethodID> { }
        #endregion

        //#region GiftPaymentMethodID
        //[PXDBString(10, IsUnicode = true)]
        //[PXUIField(DisplayName = ATRDMessages.GiftCert)]
        //[PXSelector(typeof(Search<PaymentMethod.paymentMethodID>), typeof(PaymentMethod.paymentMethodID), typeof(PaymentMethod.descr), DescriptionField = typeof(PaymentMethod.descr))]
        //[PXDefault]
        //public virtual string GiftPaymentMethodID { get; set; }
        //public abstract class giftPaymentMethodID : BqlString.Field<giftPaymentMethodID> { }
        //#endregion

        [PXDBString(64, IsUnicode = true)]
        [PX.SM.Roles.RolesSelector]
        [PXDefault(typeof(ATRDSetup.portalRole), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = ATRDMessages.PortalRole)]
        public virtual string PortalRole { get; set; }
        public abstract class portalRole : BqlString.Field<portalRole> { }

        [PXDBInt]
        [PXUIField(DisplayName = ATRDMessages.PortalBranchID)]
        [PXSelector(typeof(Search<Branch.branchID>), SubstituteKey =typeof(Branch.branchCD), DescriptionField = typeof(Branch.acctName))]
        [PXDefault(typeof(ATRDSetup.branchID))]
        public virtual int? PortalBranchID { get; set; }
        public abstract class portalBranchID : BqlInt.Field<portalBranchID> { }
    }
}
