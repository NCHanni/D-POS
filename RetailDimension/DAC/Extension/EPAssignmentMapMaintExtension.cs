using PX.Data;
using PX.Data.BQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDimension.DAC.Extension
{
     public class EPAssignmentMapMaintExtension 
    {
        public class AssignmentMapTypeRLMaterialRequest : BqlType<IBqlInt, string>.Constant<AssignmentMapTypeRLMaterialRequest>
        {
            public AssignmentMapTypeRLMaterialRequest() : base(typeof(ATRDMaterialRequest).FullName) { }
        }
    }
}
