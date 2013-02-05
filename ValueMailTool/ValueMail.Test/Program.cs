using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueMail.Receive.POP3;
using ValueMail.Receive.IMAP;
using ValueMail.Receive.IMAP.Infrastructure;
using ValueMail.Receive.Model;
using System.Configuration;
using ValueMail.Send.SMTP;
using ValueMail.Send.Infrastructure;

namespace ValueMail.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //SMTPTest();
            POPTest();
            //IMAPTest();
        }

        static void POPTest()
        {
            mailhost host = new mailhost
            {
                server = "pop.126.com",
                Ssl = false,
                address = "zformular@126.com",
                password = "zy123456",
                port = 110
            };

            using (POP3Client client = new POP3Client())
            {
                try
                {
                    // 建立连接
                    client.Connect(host.server, host.port, host.Ssl);
                    // 登陆账号
                    client.Loging(host.address, host.password);

                    //// 获取当前邮箱邮件个数
                    Int32 count = client.GetMailCount();

                    //client.DeleMail(1);
                    //client.ResetStatus();
                    //client.Disconnect();

                    //// 获得邮件头列表
                    MailHeadList mailheaders = client.GetMailHeaders();
                    var model = client.GetMail(2);

                    // 循环显示邮件信息
                    foreach (MailHeadModel mailheader in mailheaders)
                    {
                        Console.WriteLine("UID: " + mailheader.UID);
                        Console.WriteLine("用户名: " + mailheader.Name);
                        Console.WriteLine("地址: " + mailheader.Address);
                        Console.WriteLine("主题: " + mailheader.Subject);
                        Console.WriteLine("日期: " + mailheader.Date.ToString("yyyy-MM-dd"));
                        Console.WriteLine();
                    }

                    var mails = client.GetMails();
                    foreach (var mail in mails)
                    {
                        Console.WriteLine("UID: " + mail.MailHead.UID);
                        Console.WriteLine("用户名: " + mail.MailHead.Name);
                        Console.WriteLine("地址: " + mail.MailHead.Address);
                        Console.WriteLine("主题: " + mail.MailHead.Subject);
                        Console.WriteLine("日期: " + mail.MailHead.Date.ToString("yyyy-MM-dd"));
                        Console.WriteLine("内容: " + mail.Body);
                        Console.WriteLine();
                        Console.WriteLine("超文本内容: " + mail.BodyHtml);
                        Console.WriteLine("附件:");
                        foreach (var item in mail.Attachments)
                        {
                            //item.Download(@"E:\mailattachments");
                            Console.WriteLine(item.Name);
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                    }


                    client.Disconnect();
                }
                catch (ArgumentException aex)
                {
                    Console.WriteLine(aex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            Console.ReadLine();
        }

        static void IMAPTest()
        {
            //mailhose host = new mailhose
            //{
            //    server = "imap.163.com",
            //    Ssl = true,
            //    address = "zformular@163.com",
            //    password = "zy077614",
            //    port = 993
            //};

            mailhost host = new mailhost
            {
                server = "imap.126.com",
                Ssl = false,
                address = "zformular@126.com",
                password = "zy123456",
                port = 110
            };

            using (IMAPClient client = new IMAPClient())
            {
                client.Connect(host.server, host.port, host.Ssl);
                client.Login(host.address, host.password);
                var mailboxlist = client.ListMailbox();
                Console.WriteLine("邮箱列表:");
                Console.WriteLine(mailboxlist);

                client.SelectINBOX();
                MailHeadList list = client.GetMailHeads(SearchType.All);
                foreach (MailHeadModel mailheader in list)
                {
                    Console.WriteLine("UID: " + mailheader.UID);
                    Console.WriteLine("用户名: " + mailheader.Name);
                    Console.WriteLine("地址: " + mailheader.Address);
                    Console.WriteLine("主题: " + mailheader.Subject);
                    Console.WriteLine("日期: " + mailheader.Date.ToString("yyyy-MM-dd"));
                    Console.WriteLine();
                }

                var mailtest = client.GetMail(1);
                Console.WriteLine("UID: " + mailtest.MailHead.UID);
                Console.WriteLine("用户名: " + mailtest.MailHead.Name);
                Console.WriteLine("地址: " + mailtest.MailHead.Address);
                Console.WriteLine("主题: " + mailtest.MailHead.Subject);
                Console.WriteLine("日期: " + mailtest.MailHead.Date.ToString("yyyy-MM-dd"));
                Console.WriteLine("内容: " + mailtest.Body);
                Console.WriteLine();
                Console.WriteLine("超文本内容: " + mailtest.BodyHtml);
                Console.WriteLine("附件:");
                foreach (var item in mailtest.Attachments)
                {
                    //item.Download(@"E:\mailattachments");
                    Console.WriteLine(item.Name);
                }

                client.Disconnect();
                client.Dispose();
            }

            Console.ReadLine();
        }

        static void SMTPTest()
        {
            using (SmtpClient smtpClient = new SmtpClient())
            {
                SendModel model = new SendModel
                {
                    Body = "测试测试这是测试",
                    Subject = "邮箱测试",
                    From = new System.Net.Mail.MailAddress("zformular@126.com", "ValueMember"),
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Password = "zy123456"
                };
                model.AddTo("283341628@qq.com", "Hey ValueCEO");
                model.AddTo("zformular@163.com", "Hey ValueCEO");

                smtpClient.SendMail(model);
            }
        }
    }

    class mailhost
    {
        public String server { get; set; }

        public Int32 port { get; set; }

        public String address { get; set; }

        public String password { get; set; }

        public Boolean Ssl { get; set; }


    }
}
