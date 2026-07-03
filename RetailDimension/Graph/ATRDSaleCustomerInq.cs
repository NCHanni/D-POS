using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AR;
using RetailDimension.DAC;
using RetailDimension.DAC.Extension;
using RetailDimension.DAC.Base;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE : ATRD4010
    /// </summary>
    public class ATRDSaleCustomerInq : PXGraph<ATRDSaleCustomerInq>
    {
        public PXFilter<ATRDCustomerFilter> Filter;
        public PXSelectReadonly<ATRDSale, Where<ATRDSale.customerCode, Equal<Current<ATRDCustomerFilter.code>>>> Detail;

        public PXAction<ATRDCustomerFilter> ViewTransaction;
        [PXButton]
        [PXUIField]
        protected virtual void viewTransaction()
        {
            if(Detail.Current != null)
            {
                ATRDSalePOSEntry graph = PXGraph.CreateInstance<ATRDSalePOSEntry>();
                graph.Document.Current = graph.Document.Search<ATRDSale.code>(Detail.Current.Code);

                throw new PXRedirectRequiredException(graph, true, string.Empty);
            }
        }
    }

    #region InternalTypes

    [Serializable]
    [PXHidden]
#if Version23R1
    public partial class ATRDCustomerFilter : IBqlTable
#endif
#if Version25R1 || Version25R2
        public partial class ATRDCustomerFilter : PXBqlTable, IBqlTable
#endif
    {

        #region Code
        [PXString(30, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CustomerCode)]
        [PXSelector(typeof(Search<Customer.acctCD, Where<ATRDCustomer.usrATRDIsPOS, Equal<True>>>), typeof(Customer.acctCD),typeof(Customer.acctName), DescriptionField = typeof(Customer.acctName))]
        public virtual string Code { get; set; }
        public abstract class code : BqlString.Field<code> { }
        #endregion

        #region Name
        [PXString(60, IsUnicode = true)]
        [PXUIField(DisplayName = ATRDMessages.CustomerName, Enabled = false)]
        [PXFormula(typeof(Selector<code, Customer.acctName>))]
        [PXFormula(typeof(Default<code>))]
        public virtual string Name { get; set; }
        public abstract class name : BqlString.Field<name> { }
        #endregion
    }

    #endregion
}