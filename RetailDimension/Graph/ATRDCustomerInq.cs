using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using PX.Objects.AR;
using RetailDimension.DAC;
using PX.Objects.CR;
using RetailDimension.DAC.Extension;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE : ATRD4002
    /// </summary>
    public class ATRDCustomerInq : PXGraph<ATRDCustomerInq>
    {
        public PXSelectReadonly2<
            Customer, 
            LeftJoin<Address, 
                On<Address.addressID, Equal<Customer.defAddressID>, 
                And<Address.bAccountID, Equal<Customer.bAccountID>>>, 
            LeftJoin<Contact, 
                On<Contact.contactID, Equal<Customer.defContactID>, 
                And<Contact.bAccountID, Equal<Customer.bAccountID>>>>>, 
            Where<ATRDCustomer.usrATRDIsPOS, Equal<True>>> 
            Document;
    }
}