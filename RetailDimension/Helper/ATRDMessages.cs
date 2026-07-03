using System;
using PX.Data;
using PX.Common;


namespace RetailDimension
{
    [PXLocalizable]
    public class ATRDMessages
    {
        #region Masks
        #region Any Symbol
        public const string C10 = ">CCCCCCCCCC";
        public const string C15 = ">CCCCCCCCCCCCCCC";
        public const string GreaterThan = ">";
        public const string LessThan = "<";
        public const string Equal = "=";
        #endregion
        #region Digit
        public const string D10 = "0000000000";
        public const string Decimal = "0.0";
        public const string TwoDecimalPlace = "0.00";
        #endregion
        #region Any Letter or Digit
        public const string A3 = "AAA";
        public const string A10 = ">AAAAAAAAAA";
        public const string B3 = "BBB";
        public const string C3 = "CCC";
        public const string D3 = "DDD";
        public const string E3 = "EEE";
        public const string F3 = "FFF";
        public const string G3 = "GGG";
        #endregion
        #region Letter Only
        public const string L5 = ">LLLLL";
        #endregion
        #region ScreenMask
        public const string ScreenMask = "CC.CC.CC.CC";
        #endregion
        #endregion

        #region Display Label
        public const string Customer = "Customer";
        public const string CustomerPWD = "Customer PWD";
        public const string CustomerSC = "Customer SC";
        public const string CustomerClass = "Customer Class";
        public const string Name = "Name";
        public const string Name2 = "Name (2)";
        public const string PermitNumber = "Permit (PTU) No.";
        public const string PermitMin = "Machine ID (MIN)";
        public const string PermitIssued = "Date Issued";
        public const string PermitExpiry = "Valid Until";
        public const string VatRegistrationNo = "VAT Reg. No.";
        public const string BusinessStyle = "Business Style";
        public const string Address = "Address";
        public const string ContactNo = "Contact No.";
        public const string DiscountID = "Discount ID";
        public const string Description = "Description";
        public const string VatExempt = "Vat Exempt";
        public const string DiscountRate = "Discount Rate";
        public const string ClassID = "Class ID";
        public const string TaxCategoryID = "Tax Category ID";
        public const string Vatable = "Vatable";
        public const string ZeroRated = "Zero-rated";
        public const string Discount = "Discount";
        public const string InventoryID = "Inventory ID";
        public const string ReferenceNbr = "Reference Nbr.";
        public const string BatchRefNbr = "Batch Ref. Nbr.";
        public const string AccountType = "Account Type";
        public const string AccountNo = "Account No.";
        public const string Account = "Account";
        public const string AccountName = "Account Name";
        public const string Active = "Active";
        public const string Subaccount = "Subaccount";
        public const string EmployeeID = "Employee ID";
        public const string EmployeeName = "Employee Name";
        public const string Barcode = "Barcode";
        public const string ExpiryDate = "Expiry Date";
        public const string Sold = "Sold";
        public const string Used = "Used";
        public const string TIN = "TIN";
        public const string PaymentMethodID = "Payment Method";
        public const string DicountType = "Discount Type";
        public const string DateIssued = "Date Issued";
        public const string Male = "Male";
        public const string Female = "Female";
        public const string DiscountRegular = "Regular";
        public const string DiscountSenior = "Senior Citizen";
        public const string DiscountPWD = "Person w/ Disability";
        public const string VAT = "VAT";
        public const string IsPOS = "POS Item";
        public const string IsPOSDiscount = "POS Discount";
        public const string IsSCPWDDiscount = "SC/PWD Discount";
        public const string IsPOSSalesperson = "POS Salesperson";
        public const string IsPOSCustomer = "POS Customer";
        public const string Date = "Date";
        public const string OrderType = "Order Type";
        public const string Warehouse = "Default Warehouse";
        public const string OrderNbr = "Order Nbr.";
        public const string LineLimit = "Line Limit";
        public const string Cash = "Cash";
        public const string CreditCard = "Credit Card";
        public const string CreditMemo = "Credit Memo";
        public const string GiftCert = "Gift Certificate";
        public const string Serial = "Serial";
        public const string Lot = "Lot";
        public const string TrackingMethod = "Tracking Method";
        public const string CreditScore = "Credit Score";
        public const string OldVendorID = "Old Vendor ID";
        public const string PortalRole = "Portal Role";
        public const string PortalBranchID = "Portal Branch ID";
        public const string Contact = "Contact";
        public const string FirstName = "First Name";
        public const string LastName = "Last Name";
        public const string Email = "Email";
        public const string Website = "Website";
        public const string Fax = "Fax";
        public const string CreateUser = "Create User";        
        public const string DeleteUser = "Delete User";        
        public const string DisableUser = "Disable User";        
        public const string EnableUser = "Enable User";        
        public const string OutstandingBalance = "Outstanding Balance";        
        public const string CreditLimit = "Credit Limit";        
        public const string AddItem = "Add Item";        
        public const string SubmitOrder = "Submit Order";        
        public const string Total = "Total";
        public const string Acknowledgement = "Acknowledgement";
        public const string Collection = "Collection";
        public const string Official = "Official";
        public const string Provisional = "Provisional";
        public const string ReceiptType = "Receipt Type";
        public const string ReceiptNbr = "Receipt Nbr";
        public const string CheckNbr = "Check Number";
        public const string SyncDate = "Sync Date"; 
        #endregion

        #region Report Labels
        public const string Preview = "Preview";
        public const string BatchNbr = "BatchNbr";
        public const string PrintOfficial = "Print Official Receipt";
        public const string PrintAcknowledgement = "Print Acknowledgement Receipt";
        public const string PrintProvisional = "Print Provisional Receipt";
        public const string PrintJournalVoucher = "Print Journal Voucher";
        #endregion

        #region Batch Label
        public const string BranchCD = "Branch Code";
        #endregion

        #region Sale Label
        public const string Code = "Code";
        public const string CashierSessionID = "Session ID";
        public const string TerminalCode = "Terminal Code";
        public const string TransactionDate = "Transaction Date";
        public const string CustomerCode = "Customer Code";
        public const string CustomerName = "Customer Name";
        public const string CustomerTin = "Customer TIN";
        public const string CustomerBusinessStyle = "Business Style";
        public const string CustomerAddress = "Customer address";
        public const string SalespersonCode = "Salesperson Code";
        public const string Salesperson = "Salesperson";
        public const string CashierCode = "Cashier Code";
        public const string CashierName = "Cashier Name";
        public const string TotalVat = "Total VAT";
        public const string TotalDiscount = "Total Discount";
        public const string TotalAmount = "Total Amount";
        public const string TotalAmountReturned = "Total Amount Returned";
        public const string VatSales = "VAT Sales";
        public const string VatExemptSales = "VAT Exempt Sales";
        public const string ZeroRatedSales = "Zero Rated Sales";
        public const string LessVat = "Less VAT";
        public const string IsSC = "Senior Citizen";
        public const string IsPWD = "PWD";
        public const string ScPwdIdNo = "SC/PWD Id No.";
        public const string ScPwdName = "SC/PWD Name";
        public const string ScPwdDiscount = "SC/PWD Discount";
        public const string ScPwdLessVat = "SC/PWD VAT";
        public const string IsVoid = "Void";
        public const string VoidCode = "Void Code";
        public const string IsAllReturned = "All Returned";
        #endregion

        #region Sale Detail Label
        public const string ItemLineID = "Line ID";
        public const string SaleCode = "Sale Code";
        public const string ItemCode = "Item Code";
        public const string ClassCode= "Class Code";
        public const string Price = "Price";
        public const string Qty = "Qty";
        public const string QtyReturned = "Qty Returned";
        public const string UnitOfMeasure = "UOM";
        public const string QtyPerUom = "Qty Per UOM";
        public const string IsRegularDiscount = "Regular Discount";
        public const string DiscountPercent = "Discount %";
        public const string DiscountAmount = "Discount Amt.";
        public const string DiscountedPrice = "Discount Price";
        public const string VatPercent = "VAT %";
        public const string VatAmount = "VAT Amt.";
        public const string VatExemptAmount = "VAT Exempt Amt.";
        public const string Amount = "Amount";
        public const string IsVatable = "Vatable";
        public const string IsZeroRated = "Zero Rated";
        public const string IsVatExempt = "VAT Exempt";
        public const string IsGiftCertificate = "Gift Cert.";
        public const string SerialLotNo = "Serial/Lot No.";
        public const string AlternateID = "Alternate ID";
        #endregion

        #region Sale Discount Detail Label
        public const string DiscountType = "Discount Type";
        public const string IdNo = "Id No.";
        public const string Gender = "Gender";
        public const string BirthDate = "Birthdate";
        public const string IssuedDate = "Issue Date";
        public const string TotalLessVat = "Total Less VAT";
        #endregion

        #region Sale Other Detail Label
        public const string Change = "Change";
        public const string TypeCode = "Type Code";
        public const string TypeName = "Type Name";
        public const string CardNo = "Card No.";
        public const string CardHolder = "Card Holder";
        public const string BankName = "Bank Name";
        public const string ApprovalCode = "Approval Code";
        public const string MemoCode = "Memo Code";
        public const string GcCode = "Gift Cert.";
        #endregion

        #region Setup Label
        public const string NumberSequence = "Number Sequence";
        public const string MemoNumberSequence = "Memo Number Sequence";
        public const string AcknowledgementReceiptNumberingSequence = "Acknowledgement Receipt Numbering Sequence";
        public const string CollectionReceiptNumberingSequence = "Collection Receipt Numbering Sequence";
        public const string OfficialReceiptNumberingSequence = "Official Numbering Sequence";
        public const string ProvisionalReceiptNumberingSequence = "Provisional Receipt Numbering Sequence";
        #endregion

        #region Material Request
        public const string MaterialRequestNumberingID = "Material Request Numbering Sequence";
        public const string MaterialRequestRequestApproval = "Require Approval for Material Request";
        public const string RefNbr = "Reference Nbr";
        public const string Status = "Status";
        public const string Hold = "Hold";
        public const string FinPeriodID = "Post Period";
        public const string RequestedByID = "Requested By";
        public const string DepartmentID = "Department";
        public const string EmployeeNotUser = "Employee must be a user.";
        public const string BranchID = "Branch ID";
        public const string Branch = "Branch";
        public const string IssueRefNbr = "Issue Ref Nbr.";
        public const string RequestRefNbr = "Request Ref Nbr.";
        public const string OwnerID = "Owner";
        public const string WorkgroupID = "Workgroup";
        public const string Approved = "Approved";
        public const string RequestApproval = "Request Approval";
        public const string ApprovalWorkgroupID = "Approval Workgroup ID";
        public const string ApprovalOwnerID = "Approver";
        public const string AssignmentMapID = "Approval Map";
        public const string AssignmentNotificationID = "Pending Approval Notification";
        public const string IsActive = "Active";
        public const string UOM = "UOM";
        public const string LocationID = "Location";
        public const string ProjectID = "Project";
        public const string ProjectTaskID = "Task";
        public const string CostCodeID = "Cost Code";
        public const string Action = "Actions";
        public const string CancelRequest = "Cancel Request";
        public const string CreateInventoryIssue = "Create Inventory Issue";
        public const string CreatePurchaseRequest = "Create Purchase Request";
        public const string OpenIssue = "Open Issue";
        public const string OpenRequest = "Open Request";
        public const string FromDate = "From Date";
        public const string ToDate = "To Date";
        public const string DocumentTotal = "Document Total";
        public const string Balance = "Balance";
        public const string Type = "Type";
        public const string PrintInvoiceMemo = "Print Invoice Memo";
        #endregion

        public const string AccountRegisterFillupTheRequiredFields = "Fill-up the required fields.\nAccount registration failed.";
        public const string AccountRegisterEmailIsRequired = "Email is required.\nAccount registration failed.";
        public const string AccountRegistrationFailed = "Unable to create portal user.\nAccount registration failed.";
        public const string GeneratedFromCustomerAccountRegistration = "Generated from Customer Account Registration Page.";
        public const string ErrorProcessingTransaction = "Error Processing Transaction: {0}";
        public const string NoTransactionToProcess = "Error Processing Transaction. Batch: {0} has no sale detail to process.";
        public const string UserAlreadyHasAccount = "Error Processing Transaction. User is already registered.";
        public const string RestrictBatchByBranch = "Batch is restricted by branch.";
    }
}
