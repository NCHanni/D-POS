using System;
using PX.Data;
using PX.Data.BQL;

namespace RetailDimension.Attributes
{
    public class ATRDGenderAttribute : PXStringListAttribute
    {
        #region Values
        public const string Male = "M";
        public const string Female = "F";
        #endregion

        #region BQL Accessors
        public class male : BqlString.Constant<male>
        {
            public male() : base(Male)
            {
            }
        }

        public class female : BqlString.Constant<female>
        {
            public female() : base(Female)
            {
            }
        }
        #endregion

        #region Constructor
        public ATRDGenderAttribute()
            : base(new[] {
            Pair(Male, ATRDMessages.Male),
            Pair(Female, ATRDMessages.Female)
            })
        { }
        #endregion
    }
}
