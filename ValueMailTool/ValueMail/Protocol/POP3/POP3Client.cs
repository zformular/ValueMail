﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueMail.Model;
using ValueMail.POP3.MailBase;

namespace ValueMail.POP3
{
    public class POP3Client : IDisposable
    {
        private POP3Base pop3Base = new POP3Base();
        private Boolean disposed = false;

        public void Connect(string server, Int32 port)
        {
            pop3Base.Connect(server, port);
        }

        public void Connect(string server, Int32 port, bool ssl)
        {
            pop3Base.Connect(server, port, ssl);
        }

        public void Loging(string account, string password)
        {
            pop3Base.Loging(account, password);
        }

        public Int32 GetMailCount()
        {
            return pop3Base.GetMailCount();
        }

        public MailHeadList GetMailHeaders()
        {
            return new MailHeadList(pop3Base);
        }

        public MailList GetMails()
        {
            return new MailList(pop3Base);
        }

        public MailModel GetMail(Int32 index)
        {
            return pop3Base.GetMail(index);
        }

        public void DeleMail(Int32 index)
        {
            pop3Base.DeleMail(index);
        }

        public void ResetStatus()
        {
            pop3Base.ResetStatus();
        }

        public void Disconnect()
        {
            pop3Base.Disconnect();
        }

        public void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (pop3Base != null)
                    {
                        pop3Base.Dispose();
                        pop3Base = null;
                    }

                    disposed = true;
                }
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {

        }

        #endregion
    }
}
