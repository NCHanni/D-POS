using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using RetailDimension.DAC;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE : ATRD2005
    /// </summary>
    public class ATRDCreditCardMaint : PXGraph<ATRDCreditCardMaint, ATRDCreditCard>
    {
        public PXSelect<ATRDCreditCard> Document;
    }
}