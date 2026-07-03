using PX.Data;
using System;

namespace RetailDimension.DAC.Base
{
#if Version23R1
    public abstract class Audit
    {
        #region CreatedByID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class createdByID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        #endregion

        #region CreatedByScreenID
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        protected String _CreatedByScreenID;
        [PXDBCreatedByScreenID()]
        public virtual String CreatedByScreenID
        {
            get
            {
                return this._CreatedByScreenID;
            }
            set
            {
                this._CreatedByScreenID = value;
            }
        }
        #endregion

        #region CreatedDateTime
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class createdDateTime : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBCreatedDateTime()]
        [PXUIField(DisplayName = PXDBLastModifiedByIDAttribute.DisplayFieldNames.CreatedDateTime, Enabled = false, IsReadOnly = true)]
        public virtual DateTime? CreatedDateTime { get; set; }
        #endregion

        #region LastModifiedByID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class lastModifiedByID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        #endregion

        #region LastModifiedByScreenID
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        protected String _LastModifiedByScreenID;
        [PXDBLastModifiedByScreenID()]
        public virtual String LastModifiedByScreenID
        {
            get
            {
                return this._LastModifiedByScreenID;
            }
            set
            {
                this._LastModifiedByScreenID = value;
            }
        }
        #endregion

        #region LastModifiedDateTime
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class lastModifiedDateTime : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBLastModifiedDateTime()]
        [PXUIField(DisplayName = PXDBLastModifiedByIDAttribute.DisplayFieldNames.LastModifiedDateTime, Enabled = false, IsReadOnly = true)]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        #endregion

        #region tstamp
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class Tstamp : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBTimestamp()]
        public virtual Byte[] tstamp { get; set; }
        #endregion
    }
#endif
#if Version25R1 || Version25R2
    public abstract class Audit : PXBqlTable
    {
        #region CreatedByID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class createdByID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        #endregion

        #region CreatedByScreenID
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        protected String _CreatedByScreenID;
        [PXDBCreatedByScreenID()]
        public virtual String CreatedByScreenID
        {
            get
            {
                return this._CreatedByScreenID;
            }
            set
            {
                this._CreatedByScreenID = value;
            }
        }
        #endregion

        #region CreatedDateTime
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class createdDateTime : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBCreatedDateTime()]
        [PXUIField(DisplayName = PXDBLastModifiedByIDAttribute.DisplayFieldNames.CreatedDateTime, Enabled = false, IsReadOnly = true)]
        public virtual DateTime? CreatedDateTime { get; set; }
        #endregion

        #region LastModifiedByID
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class lastModifiedByID : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        #endregion

        #region LastModifiedByScreenID
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        protected String _LastModifiedByScreenID;
        [PXDBLastModifiedByScreenID()]
        public virtual String LastModifiedByScreenID
        {
            get
            {
                return this._LastModifiedByScreenID;
            }
            set
            {
                this._LastModifiedByScreenID = value;
            }
        }
        #endregion

        #region LastModifiedDateTime
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class lastModifiedDateTime : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBLastModifiedDateTime()]
        [PXUIField(DisplayName = PXDBLastModifiedByIDAttribute.DisplayFieldNames.LastModifiedDateTime, Enabled = false, IsReadOnly = true)]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        #endregion

        #region tstamp
        /// <summary>
        /// Audit Bql field.
        /// </summary>
        public abstract class Tstamp : IBqlField { }
        /// <summary>
        /// Audit Bql property field.
        /// </summary>
        [PXDBTimestamp()]
        public virtual Byte[] tstamp { get; set; }
        #endregion
    }
#endif
}
