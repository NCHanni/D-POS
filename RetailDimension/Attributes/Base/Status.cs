using PX.Data;
using PX.Data.BQL;

namespace RetailDimension.Attributes.Base
{
    public abstract class Status
    {
        #region Display
        public const string Open = "Open";
        public const string Pending = "Pending Approval";
        public const string Closed = "Closed";
        public const string Rejected = "Rejected";
        public const string Hold = "On Hold";
        public const string Release = "Pending Release";
        public const string Approved = "Approved";
        #endregion

        #region Values
        public const string OpenValue = "O";
        public const string PendingValue = "P";
        public const string ClosedValue = "C";
        public const string RejectedValue = "R";
        public const string HoldValue = "H";
        public const string ReleaseValue = "E";
        public const string ApprovedValue = "A";
        #endregion

        public class openValue : BqlType<IBqlInt, string>.Constant<openValue>
        {
            public openValue() : base(OpenValue) { }
        }

        public class pendingValue : BqlType<IBqlInt, string>.Constant<pendingValue>
        {
            public pendingValue() : base(PendingValue) { }
        }

        public class closedValue : BqlType<IBqlInt, string>.Constant<closedValue>
        {
            public closedValue() : base(ClosedValue) { }
        }

        public class rejectedValue : BqlType<IBqlInt, string>.Constant<rejectedValue>
        {
            public rejectedValue() : base(RejectedValue) { }
        }

        public class holdValue : BqlType<IBqlInt, string>.Constant<holdValue>
        {
            public holdValue() : base(HoldValue) { }
        }

        public class releaseValue : BqlType<IBqlInt, string>.Constant<releaseValue>
        {
            public releaseValue() : base(ReleaseValue) { }
        }

        public class approvedValue : BqlType<IBqlInt, string>.Constant<approvedValue>
        {
            public approvedValue() : base(ApprovedValue) { }
        }
    }
}
