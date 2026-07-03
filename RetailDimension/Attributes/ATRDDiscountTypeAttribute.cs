using System;
using PX.Data;
using PX.Data.BQL;

namespace RetailDimension.Attributes
{
    public class ATRDDiscountTypeAttribute : PXStringListAttribute
    {
        #region Values
        public const string DiscountRegular = "R";
        public const string DiscountSenior = "S";
        public const string DiscountPWD = "P";
        #endregion

        #region BQL Accessors
        public class discountRegular : BqlString.Constant<discountRegular>
        {
            public discountRegular() : base(DiscountRegular)
            {
            }
        }

        public class discountSenior : BqlString.Constant<discountSenior>
        {
            public discountSenior() : base(DiscountSenior)
            {
            }
        }

        public class discountPWD : BqlString.Constant<discountPWD>
        {
            public discountPWD() : base(DiscountPWD)
            {
            }
        }
        #endregion

        #region Constructor
        public ATRDDiscountTypeAttribute()
            : base(new[] {
            Pair(DiscountRegular, ATRDMessages.DiscountRegular),
            Pair(DiscountSenior, ATRDMessages.DiscountSenior),
            Pair(DiscountPWD, ATRDMessages.DiscountPWD)
            })
        { }
        #endregion
    }
}
