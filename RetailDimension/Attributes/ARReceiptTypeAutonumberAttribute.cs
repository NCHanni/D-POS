using PX.Data;
using PX.Objects.AR;
using RetailDimension.DAC.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDimension.Attributes
{
    public class ARReceiptTypeAutonumberAttribute : PX.Objects.CS.AutoNumberAttribute
    {
        public ARReceiptTypeAutonumberAttribute(Type doctypeField, Type dateField)
      : base(doctypeField, dateField)
        {
        }
        public ARReceiptTypeAutonumberAttribute(Type doctypeField, Type dateField, string[] doctypeValues, Type[] setupFields)
          : base(doctypeField, dateField, doctypeValues, setupFields)
        {
        }

        public override void RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            ARPayment row = e.Row as ARPayment;
            ATRDARPaymentExtension rowExt = row.GetExtension<ATRDARPaymentExtension>();

            if (rowExt.UsrReceiptType.Equals(ATRDReceiptTypeAttribute.Blank))
            {
                sender.SetValue(e.Row, _FieldName, String.Empty);
            }
            else base.RowPersisting(sender, e);
        }
    }
}
