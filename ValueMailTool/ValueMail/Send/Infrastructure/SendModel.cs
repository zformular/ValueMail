using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace ValueMail.Send.Infrastructure
{
    public class SendModel
    {
        public SendModel()
        {
            Priority = MailPriority.Normal;
            To = new MailAddressCollection();
        }

        /// <summary>
        ///  邮件编号
        /// </summary>
        public String UID { get; set; }

        /// <summary>
        ///  发件箱
        /// </summary>
        public MailAddress From { get; set; }

        /// <summary>
        ///  密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        ///  收件箱
        /// </summary>
        public MailAddressCollection To { get; private set; }

        /// <summary>
        ///  主题
        /// </summary>
        public String Subject { get; set; }

        /// <summary>
        ///  内容
        /// </summary>
        public String Body { get; set; }

        /// <summary>
        ///  优先级
        /// </summary>
        public MailPriority Priority { get; set; }

        /// <summary>
        ///  发送日期
        /// </summary>
        public String Date { get; set; }

        /// <summary>
        ///  附件
        /// </summary>
        public IList<Attachment> Attachments { get; set; }

        public void AddTo(String address, String displayName)
        {
            this.To.Add(new MailAddress(address, displayName));
        }
    }
}
