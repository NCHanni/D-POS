using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AR;
using RetailDimension.Attributes;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDCustomer : PXCacheExtension<Customer>
    {
        public static bool IsActive() { return true; }

        #region UsrATRDIsPOS
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.IsPOSCustomer)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATRDIsPOS { get; set; }
        public abstract class usrATRDIsPOS : BqlBool.Field<usrATRDIsPOS> { }
        #endregion
                
        #region UsrATRDTin
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.TIN)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public string UsrATRDTin { get; set; }
        public abstract class usrATRDTin : BqlString.Field<usrATRDTin> { }
        #endregion

        #region UsrATRDBusinessStyle
        [PXDBString(250, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.BusinessStyle)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public string UsrATRDBusinessStyle { get; set; }
        public abstract class usrATRDBusinessStyle : BqlString.Field<usrATRDBusinessStyle> { }
        #endregion

        #region UsrATRDGender
        [PXDBString(1, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Gender)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        [ATRDGender]
        public string UsrATRDGender { get; set; }
        public abstract class usrATRDGender : BqlString.Field<usrATRDGender> { }
        #endregion

        #region UsrATRDBirthDate
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.BirthDate)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public DateTime? UsrATRDBirthDate { get; set; }
        public abstract class usrATRDBirthDate : BqlDateTime.Field<usrATRDBirthDate> { }
        #endregion

        #region UsrATRDDiscountType
        [PXDBString(1, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.DiscountType)]
        [PXDefault(typeof(ATRDDiscountTypeAttribute.discountRegular), PersistingCheck = PXPersistingCheck.Nothing)]
        [ATRDDiscountType]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public string UsrATRDDiscountType { get; set; }
        public abstract class usrATRDDiscountType : BqlString.Field<usrATRDDiscountType> { }
        #endregion

        #region UsrATRDScPwdIdNo
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.ScPwdIdNo)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public string UsrATRDScPwdIdNo { get; set; }
        public abstract class usrATRDScPwdIdNo : BqlString.Field<usrATRDScPwdIdNo> { }
        #endregion

        #region UsrATRDDateIssued
        [PXDBDate]
        [PXUIField(DisplayName = ATRDMessages.DateIssued)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public DateTime? UsrATRDDateIssued { get; set; }
        public abstract class usrATRDDateIssued : BqlDateTime.Field<usrATRDDateIssued> { }
        #endregion

        #region UsrATRDCreditScore
        [PXDBString(25, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CreditScore)]
        public string UsrATRDCreditScore { get; set; }
        public abstract class usrATRDCreditScore : BqlString.Field<usrATRDCreditScore> { }
        #endregion

    }
}
