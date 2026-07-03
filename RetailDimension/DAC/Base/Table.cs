using System;
using PX.Data;
using PX.Objects.Common;


namespace RetailDimension.DAC.Base
{
    [PXHidden]
    public abstract class Table : Base.Audit
    {
        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : IBqlField { }
        #endregion
    }
}
