using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mime;

namespace ValueMail.Model
{
    public class PartialMailModel
    {
        public ContentType ContentType { get; set; }

        public String ContentTransferEncoding { get; set; }

        public ContentDisposition ContentDisposition { get; set; }

        public String Context { get; set; }

        public Boolean HasValue { get; private set; }

        public PartialMailModel(Boolean hasValue)
        {
            this.HasValue = hasValue;
        }
    }
}
