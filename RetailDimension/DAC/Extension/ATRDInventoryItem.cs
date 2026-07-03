using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.IN;
using PX.Objects.AR;

namespace RetailDimension.DAC.Extension
{
    public sealed class ATRDInventoryItem : PXCacheExtension<InventoryItem>
    {
        public static bool IsActive() { return true; }

        #region UsrATRDIsPOS
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.IsPOS)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrATRDIsPOS { get; set; }
        public abstract class usrATRDIsPOS : BqlBool.Field<usrATRDIsPOS> { }
        #endregion

        #region UsrATRDIsVat
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.VAT)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public bool? UsrATRDIsVat { get; set; }
        public abstract class usrATRDIsVat : BqlBool.Field<usrATRDIsVat> { }
        #endregion

        #region UsrATRDIsZeroRated
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.ZeroRated)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public bool? UsrATRDIsZeroRated { get; set; }
        public abstract class usrATRDIsZeroRated : BqlBool.Field<usrATRDIsZeroRated> { }
        #endregion

        #region UsrATRDIsSeniorPWD
        [PXDBBool]
        [PXUIField(DisplayName = ATRDMessages.ScPwdDiscount)]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<usrATRDIsPOS, Equal<True>>))]
        public bool? UsrATRDIsSeniorPWD { get; set; }
        public abstract class usrATRDIsSeniorPWD : BqlBool.Field<usrATRDIsSeniorPWD> { }
        #endregion

        #region UsrATRDDiscountID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.Discount)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        [PXSelector(typeof(Search<DiscountSequence.discountID, Where<ATRDDiscountSequence.usrATRDIsPOS, Equal<True>>>), DescriptionField = typeof(DiscountSequence.description))]
        [PXUIEnabled(typeof(Where<usrATRDIsSeniorPWD, Equal<True>, And<usrATRDIsPOS, Equal<True>>>))]
        [PXUIRequired(typeof(Where<usrATRDIsSeniorPWD, Equal<True>>))]
        public string UsrATRDDiscountID { get; set; }
        public abstract class usrATRDDiscountID : BqlString.Field<usrATRDDiscountID> { }
        #endregion

        #region Unbound

        #region UsrATRDTrackingMethod
        [PXString(1, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.TrackingMethod)]
        [PXUnboundDefault(typeof(Search<INLotSerClass.lotSerTrack, Where<INLotSerClass.lotSerClassID, Equal<Current<InventoryItem.lotSerClassID>>>>))]
        public  string UsrATRDTrackingMethod { get; set; }
        public abstract class usrATRDTrackingMethod : BqlString.Field<usrATRDTrackingMethod> { }
        #endregion

        #region UsrATRDIsSerial
        [PXBool]
        [PXUIField(DisplayName =ATRDMessages.Serial)]
        [PXUnboundDefault(typeof(Switch<Case<Where<usrATRDTrackingMethod, Equal<INLotSerTrack.serialNumbered>>, True>, False>))]
        public  bool? UsrATRDIsSerial { get; set; }
        public abstract class usrATRDIsSerial : BqlBool.Field<usrATRDIsSerial> { }
        #endregion

        #region UsrATRDIsLot
        [PXBool]
        [PXUIField(DisplayName = ATRDMessages.Lot)]
        [PXUnboundDefault(typeof(Switch<Case<Where<usrATRDTrackingMethod, Equal<INLotSerTrack.lotNumbered>>, True>, False>))]
        public bool? UsrATRDIsLot { get; set; }
        public abstract class usrATRDIsLot : BqlBool.Field<usrATRDIsLot> { }
        #endregion

        #region UsrATRDOneMillion
        [PXDecimal(6)]
        [PXUnboundDefault(TypeCode.Decimal, "1000000.0")]
        public decimal? UsrATRDOneMillion { get; set; }
        public abstract class usrATRDOneMillion : BqlDecimal.Field<usrATRDOneMillion> { }
        #endregion
        #endregion
    }
}
