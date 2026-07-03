using System;
using System.Collections;
using System.Collections.Generic;
using PX.SM;
using PX.Data;
using RetailDimension.DAC;

namespace RetailDimension.Graph
{
    /// <summary>
    /// PAGE : ATRD3002
    /// </summary>
    public class ATRDMemoEntry : PXGraph<ATRDMemoEntry, ATRDMemo>
    {
        public PXSetup<ATRDSetup> Setup;
        public PXSelect<ATRDMemo> Document;
    }
}