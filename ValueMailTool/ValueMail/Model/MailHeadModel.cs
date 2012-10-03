using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueMail.Common;
using System.Net.Mime;
using ValueMail.Infrastructure;

namespace ValueMail.Model
{
    public class MailHeadModel
    {
        #region 邮件头属性

        public String UID { get; private set; }

        public String Name { get; private set; }

        public String Address { get; private set; }

        public String Subject { get; private set; }

        public DateTime Date { get; private set; }

        public ContentType ContentType { get; private set; }

        public String ContentTransferEncoding { get; private set; }

        #endregion

        public MailHeadModel(String uid, String mailHead)
        {
            this.UID = uid;
            MailDecompose mailDecompose = new MailDecompose();
            mailDecompose.DecomposeHead(mailHead);
            fillField(mailDecompose);
        }

        public MailHeadModel(String uid, MailDecompose mailDecompose)
        {
            this.UID = uid;
            fillField(mailDecompose);
        }

        private void fillField(MailDecompose mailDecompose)
        {
            this.Date = MailHelper.ConverToDateTime(mailDecompose.Date);
            this.Subject = MailHelper.AnalyseData(mailDecompose.Subject.Substring(GlobalVar.SubjectString.Length));
            this.ContentType = MailHelper.AnalyseContentType(mailDecompose.Content_Type);
            this.ContentTransferEncoding = mailDecompose.Content_Transfer_Encoding == null ? null :
                mailDecompose.Content_Transfer_Encoding.Substring(GlobalVar.ContentTransferEncodingString.Length);

            String name, address;
            MailHelper.AnaltseFrom(out name, out address, mailDecompose.From);
            this.Name = name;
            this.Address = address;
        }
    }
}
