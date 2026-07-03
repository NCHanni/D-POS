using System;
using PX.Data;
using PX.Objects.CS;

namespace RetailDimension.Attributes.Base
{
    public class AggregateAttribute
    {
        #region ATRDNumberingSequenceAttribute
        [PXDBString(15, IsUnicode = true)]
        [PXSelector(typeof(Numbering.numberingID), DescriptionField = typeof(Numbering.descr))]
        [PXDefault]
        public class ATRDNumberingSequenceAttribute : PXAggregateAttribute { }
        #endregion

        #region ATRDDecimalAttribute
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, ATRDMessages.Decimal, PersistingCheck = PXPersistingCheck.Nothing)]
        public class ATRDDecimalAttribute : PXAggregateAttribute { }
        #endregion

        #region ATRDDecimal8Attribute
        [PXDBDecimal(8)]
        [PXDefault(TypeCode.Decimal, ATRDMessages.Decimal, PersistingCheck = PXPersistingCheck.Nothing)]
        public class ATRDDecimal8Attribute : PXAggregateAttribute { }
        #endregion

        #region ATRDBooleanAttribute
        [PXDBBool]
        [PXDefault(TypeCode.Boolean, "False", PersistingCheck = PXPersistingCheck.Nothing)]
        public class ATRDBooleanAttribute : PXAggregateAttribute { }
        #endregion
    }
}
