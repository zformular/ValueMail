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
using System.Net.Mail;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ValueMail.Send.Infrastructure;

namespace ValueMail.Send
{
    public class SendHelper
    {
        private static HostModel[] Host = null;

        /// <summary>
        ///  获得邮箱配置信息
        /// </summary>
        public static void SetHosts()
        {
            try
            {
                var sectionNames = (IDictionary)ConfigurationManager.GetSection("SectionNames");
                var names = new String[sectionNames.Values.Count];
                sectionNames.Values.CopyTo(names, 0);
                Host = new HostModel[names.Length];
                for (int i = 0; i < names.Length; i++)
                {
                    var confHost = (IDictionary)ConfigurationManager.GetSection(names[i]);
                    Host[i] = convertHost(confHost);
                }
            }
            catch
            {
                throw new ArgumentException("请确保邮箱配置正确");
            }
        }

        /// <summary>
        ///  包装发送的邮件的相关信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static MailMessage PacketMessage(SendModel model)
        {
            var mail = new MailMessage();
            mail.From = model.From;
            mail.Subject = model.Subject;
            mail.Body = model.Body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = model.Priority;
            if (model.To != null)
                foreach (var item in model.To)
                    mail.To.Add(item);
            if (model.Attachments != null)
                foreach (var item in model.Attachments)
                    mail.Attachments.Add(item);
            return mail;
        }

        /// <summary>
        ///  获得邮件服务器参数
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static HostModel GetHost(String address)
        {
            var mark = Regex.Match(address, @"@(?<mark>.*)\.").Groups["mark"].Value;
            if (mark == String.Empty)
                throw new ArgumentException("请输入正确的邮箱地址");

            for (int i = 0; i < SendHelper.Host.Length; i++)
            {
                if (SendHelper.Host[i].Host.IndexOf(mark) > 0)
                    return SendHelper.Host[i];
            }
            throw new ArgumentException("不支持的邮箱地址,请先配置服务器信息");
        }

        public static void Dispose()
        {
            Host = null;
        }

        private static HostModel convertHost(IDictionary host)
        {
            var keys = new String[host.Keys.Count];
            var values = new String[host.Values.Count];
            host.Keys.CopyTo(keys, 0);
            host.Values.CopyTo(values, 0);

            var model = new HostModel();
            var props = TypeDescriptor.GetProperties(model);
            for (int i = 0; i < keys.Length; i++)
            {
                String propName = keys[i];
                Object value = values[i];

                var type = props[propName].PropertyType;
                if (type == typeof(Int32))
                    value = Int32.Parse(values[i]);
                else if (type == typeof(Boolean))
                    value = Boolean.Parse(values[i]);

                props[propName].SetValue(model, value);

            }
            return model;
        }
    }
}
