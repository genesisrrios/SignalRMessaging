using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulapi.ReturnObjects
{
    public class GenericReturnObject<TValue>
    {
        public string Message { get; set; }
        public TValue Values { get; set; }
        public bool Success { get; set; }
    }
}
