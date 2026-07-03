using PX.Data;
using PX.Data.BQL;
using PX.Objects.GL;
using RetailDimension.DAC.Base;
using System;

namespace RetailDimension.DAC
{
    [Serializable]
    [PXCacheName("Portal Account")]
    public class ATRDPortalAccount : Audit, IBqlTable
    {
        [PXDBIdentity]
        public virtual int? ID { get; set; }
        public abstract class id : BqlInt.Field<id> { }

        [PXDBGuid(IsKey = true)]
        public virtual Guid? UserID { get; set; }
        public abstract class userID : BqlGuid.Field<userID> { }

        [PXDBInt(IsKey = true)]
        public virtual int? CustomerID { get; set; }
        public abstract class customerID : BqlInt.Field<customerID> { }

        [PXDBInt(IsKey = true)]
        public virtual int? ContactID { get; set; }
        public abstract class contactID : BqlInt.Field<contactID> { }

        [Branch()]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : BqlInt.Field<branchID> { }

    }
}
