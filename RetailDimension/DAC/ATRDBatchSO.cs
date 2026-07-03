using System;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.SO;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("RD-Batch SO")]
    public class ATRDBatchSO : Base.Table, IBqlTable
    {
        #region Id
        [PXDBLongIdentity(IsKey = true)]
        public virtual long? Id { get; set; }
        public abstract class id : BqlLong.Field<id> { }
        #endregion

        #region OrderNbr
        [PXDBString(15, IsUnicode = true)]
        [PXDBDefault(typeof(SOOrder.orderNbr))]
        [PXParent(typeof(Select<SOOrder, Where<SOOrder.orderNbr, Equal<Current<ATRDBatchSO.orderNbr>>>>))]
        public virtual string OrderNbr { get; set; }
        public abstract class orderNbr : BqlString.Field<orderNbr> { }
        #endregion

        #region SaleCode
        [PXDBString(12, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.SaleCode)]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string SaleCode { get; set; }
        public abstract class saleCode : BqlString.Field<saleCode> { }
        #endregion
    }
}
