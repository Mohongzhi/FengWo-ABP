using System;
using System.Collections.Generic;

namespace FengWo.App.Framework
{
    public class ResponseContent
    {
        public Dictionary<string,object> Result { get; set; }

        public string TargetUrl { get; set; }

        public bool Success { get; set; }

        public ResponseError Error { get; set; }

        public bool unAuthorizedRequest { get; set; }

        public bool __abp { get; set; }
    }
}
