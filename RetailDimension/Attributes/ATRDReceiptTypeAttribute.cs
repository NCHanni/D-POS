using System;
using PX.Data;
using PX.Data.BQL;

namespace RetailDimension.Attributes
{
    public class ATRDReceiptTypeAttribute : PXStringListAttribute
    {
        #region Values
        public const string Blank = "B";
        public const string Acknowledgement = "A";
        public const string Collection = "C";
        public const string Official = "O";
        public const string Provisional = "P";
        #endregion

        #region BQL Accessors
        public class blank : BqlString.Constant<blank>
        {
            public blank() : base(Blank)
            {
            }
        }

        public class acknowledgement : BqlString.Constant<acknowledgement>
        {
            public acknowledgement() : base(Acknowledgement)
            {
            }
        }

        public class collection : BqlString.Constant<collection>
        {
            public collection() : base(Collection)
            {
            }
        }
        public class official : BqlString.Constant<official>
        {
            public official() : base(Official)
            {
            }
        }
        public class provisional : BqlString.Constant<provisional>
        {
            public provisional() : base(Provisional)
            {
            }
        }
        #endregion

        #region Constructor
        public ATRDReceiptTypeAttribute()
            : base(new[] {
            Pair(Acknowledgement, ATRDMessages.Acknowledgement),
            Pair(Collection, ATRDMessages.Collection),
            Pair(Official, ATRDMessages.Official),
            Pair(Provisional, ATRDMessages.Provisional)
            })
        { }
        #endregion
    }
}
