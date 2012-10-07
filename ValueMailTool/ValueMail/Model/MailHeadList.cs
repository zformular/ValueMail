using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ValueMail.MailBase;
using ValueMail.IMAP.Infrastructure;

namespace ValueMail.Model
{
    public class MailHeadList : ReadOnlyCollection<MailHeadModel>
    {
        public MailHeadList(MailBase.MailBase mailBase)
            : base(new List<MailHeadModel>())
        {
            List<MailHeadModel> headerList = mailBase.GetMailHeaders();
            foreach (MailHeadModel header in headerList)
            {
                base.Items.Add(header);
            }
        }

        public MailHeadList(MailBase.MailBase mailBase, String expression)
            : base(new List<MailHeadModel>())
        {
            List<MailHeadModel> headerList = mailBase.GetMailHeaders(expression);
            foreach (MailHeadModel header in headerList)
            {
                base.Items.Add(header);
            }
        }
    }
}
