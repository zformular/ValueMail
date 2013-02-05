/*
  >>>----- Copyright (c) 2012 zformular -----> 
 |                                            |
 |              Author: zformular             |
 |          E-mail: zformular@163.com         |
 |               Date: 2.4.2013               |
 |                                            |
 ╰==========================================╯
 
 */

using System;
using System.ComponentModel;
using ValueMail.Send.Infrastructure;

namespace ValueMail.Send.SMTP
{
    public class SmtpClient : IDisposable
    {
        private System.Net.Mail.SmtpClient client = null;

        public SmtpClient()
        {
            SendHelper.SetHosts();
            client = new System.Net.Mail.SmtpClient();
        }

        public void SendMail(SendModel model)
        {
            var msg = SendHelper.PacketMessage(model);
            var host = SendHelper.GetHost(model.From.Address);

            client.Credentials = new System.Net.NetworkCredential(model.From.Address, model.Password);
            client.Port = host.Port;
            client.Host = host.Host;
            client.EnableSsl = host.Ssl;

            try
            {
                client.Send(msg);
            }
            catch (System.Net.Mail.SmtpException sex)
            {
                throw (sex);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private Boolean disposed = false;
        public void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    client = null;
                    SendHelper.Dispose();

                    disposed = true;
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

        ~SmtpClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
