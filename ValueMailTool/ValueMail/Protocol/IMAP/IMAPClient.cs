using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueMail.IMAP.MailBase;
using ValueMail.IMAP.Infrastructure;
using ValueMail.Model;

namespace ValueMail.IMAP
{
    public class IMAPClient : IDisposable
    {
        private IMAPBase imapBase = new IMAPBase();

        public void Connect(String server, Int32 port)
        {
            imapBase.Connect(server, port);
        }

        public void Connect(String server, Int32 port, Boolean ssl)
        {
            imapBase.Connect(server, port, ssl);
        }

        public void Login(String account, String password)
        {
            imapBase.Loging(account, password);
        }

        public String ListMailbox()
        {
            return imapBase.ListMailbox();
        }

        public void SelectINBOX()
        {
            imapBase.SelectMailbox("INBOX");
        }

        public void SelectMailbox(String mailboxName)
        {
            imapBase.SelectMailbox(mailboxName);
        }

        public MailHeadList GetMailHeads(SearchType searchType)
        {
            return new MailHeadList(imapBase, searchType.ToString());
        }

        public MailHeadList GetMailHeads(String expression)
        {
            return new MailHeadList(imapBase, expression);
        }

        public MailModel GetMail(Int32 index)
        {
            return imapBase.GetMail(index);
        }

        public void NoBackupDelete(Int32 mailIndex)
        {
            imapBase.DeleMail(mailIndex);
        }

        public void DeleteMail(Int32 index, String mailbox)
        {
            imapBase.CopyMail(index, mailbox);
            imapBase.DeleMail(index);
        }

        public void Disconnect()
        {
            imapBase.Disconnect();
        }

        private Boolean disposed = false;
        public void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (imapBase != null)
                    {
                        imapBase.Dispose();
                        imapBase = null;
                    }
                }
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 析构函数

        ~IMAPClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
