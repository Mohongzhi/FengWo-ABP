using System;
using System.Collections.Generic;
using System.Text;

namespace FengWo.App.Framework
{
    public class ResponseError
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public dynamic ValidationErrors { get; set; }
    }
}
