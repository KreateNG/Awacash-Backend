using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.OpenApi
{
    public class SwaggerFileUploadAttribute : OpenApiOperationProcessorAttribute
    {
        
        public SwaggerFileUploadAttribute() : base(typeof(SwaggerFilChunkUploadOperationProcessor))
        {
        }
    }
}
