/*
  >>>----- Copyright (c) 2012 zformular -----> 
 |                                            |
 |              Author: zformular             |
 |          E-mail: zformular@163.com         |
 |               Date: 9.27.2012              |
 |                                            |
 ╰==========================================╯
 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mime;

namespace ValueMail.Common
{
    public class MailHelper
    {
        /// <summary>
        ///  将MIME编码格式的字符串解析成原字符
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String AnalyseData(String data)
        {
            String result = String.Empty;
            String[] datas = data.Split('?');
            if (datas.Length > 1)
            {
                String charset = String.Empty;
                String contentTransferEncoding = String.Empty;
                for (int index = 1; index < datas.Length - 1; index += 4)
                {
                    charset = datas[index].ToLower();
                    contentTransferEncoding = datas[index + 1].ToLower();
                    result += MIMEDecode(charset, contentTransferEncoding, datas[index + 2]);
                }
            }
            else
                result = data;
            return result;
        }

        public static void AnaltseFrom(out String name, out String address, String from)
        {
            GroupCollection groups = Regex.Match(from, GlobalVar.FromPattern).Groups;
            name = AnalyseData(groups["name"].ToString());
            address = groups["address"].ToString();
        }

        public static ContentType AnalyseContentType(String cntType)
        {
            ContentType contentType = new System.Net.Mime.ContentType();
            Match match = Regex.Match(cntType, GlobalVar.ContentTypePattern);
            GroupCollection groups = match.Groups;

            contentType.MediaType = groups["type"].ToString();
            contentType.Boundary = groups["boundary"].ToString();
            contentType.CharSet = groups["charset"].ToString();
            contentType.Name = groups["name"].ToString();
            return contentType;
        }

        public static ContentDisposition AnalyseContentDisposition(String cntDisposition)
        {
            ContentDisposition contentDisposition = new ContentDisposition();
            GroupCollection groups = Regex.Match(cntDisposition, GlobalVar.ContentDispositionPattern).Groups;
            contentDisposition.Inline = groups["type"].ToString() == "inline";
            contentDisposition.FileName = AnalyseData(groups["filename"].ToString());
            return contentDisposition;
        }

        public static DateTime ConverToDateTime(String sendTime)
        {
            GroupCollection groups = Regex.Match(sendTime, GlobalVar.DatePattern).Groups;
            String dateString = groups["year"].ToString() + "/" + ConvertMonthNumber(groups["month"].ToString())
                + "/" + groups["day"].ToString() + " " + groups["time"].ToString();
            return DateTime.Parse(dateString);
        }

        public static String MIMEDecode(String charset, String contentTransferEncoding, String context)
        {
            String result = context;
            result = Encrypt.ConvertEncoding(charset, contentTransferEncoding, context);
            return result;
        }

        public static Byte[] MIMEDecode(String contentTransferEncoding, String context)
        {
            return Encrypt.GetBytesByPattern(contentTransferEncoding, context);
        }

        private static String ConvertMonthNumber(String month)
        {
            String number = String.Empty;
            month = month.ToLower();
            switch (month)
            {
                case "jan":
                    return "1";
                case "feb":
                    return "2";
                case "mar":
                    return "3";
                case "apr":
                    return "4";
                case "may":
                    return "5";
                case "jun":
                    return "6";
                case "jul":
                    return "7";
                case "aug":
                    return "8";
                case "sep":
                    return "9";
                case "oct":
                    return "10";
                case "nov":
                    return "11";
                case "dec":
                    return "12";
                default:
                    return month;
            }
        }
    }
}
