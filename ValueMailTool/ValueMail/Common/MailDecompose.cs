using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mime;
using ValueMail.Model;
using ValueMail.Infrastructure;

namespace ValueMail.Common
{
    public partial class MailDecompose
    {
        private String from;
        public String From
        {
            get
            {
                checkDecomposed();
                return from;
            }
            private set
            {
                from = value;
            }
        }

        private String subject;
        public String Subject
        {
            get
            {
                checkDecomposed();
                return subject;
            }
            private set
            {
                subject = value;
            }
        }

        private String date;
        public String Date
        {
            get
            {
                checkDecomposed();
                return date;
            }
            private set
            {
                date = value;
            }
        }

        private String content_Type;
        public String Content_Type
        {
            get
            {
                checkDecomposed();
                return content_Type;
            }
            private set
            {
                content_Type = value;
            }
        }

        private String content_Transfer_encoding;
        public String Content_Transfer_Encoding
        {
            get
            {
                checkDecomposed();
                return content_Transfer_encoding;
            }
            private set
            {
                content_Transfer_encoding = value;
            }
        }

        private String mime_Version;
        public String MIME_Version
        {
            get
            {
                checkDecomposed();
                return mime_Version;
            }
            private set
            {
                mime_Version = value;
            }
        }

        private string body;
        public String Body
        {
            get
            {
                if (body == null)
                    throw new ArgumentNullException("Please Execute DecomposeMail() First");
                return body;
            }
            set
            {
                body = value;
            }
        }

        private Boolean ifDecomposed = false;

        public void DecomposeHead(String mailHead)
        {
            decomposeHead(mailHead);
        }

        public void DecomposeMail(String mailHead, String mailBody)
        {
            decomposeHead(mailHead);
            this.body = decomposeBody(mailBody);
        }

        private void decomposeHead(String mailHead)
        {
            String data = mailHead;
            data = Regex.Replace(data, "\r\n\t", new MatchEvaluator(delegate(Match match)
            {
                return String.Empty;
            }), RegexOptions.Compiled);

            data = Regex.Replace(data, "\r\n ", new MatchEvaluator(delegate(Match match)
            {
                return String.Empty;
            }), RegexOptions.Compiled);

            String[] datas = ValueHelper.StringHelper.SplitByCRLF(data, StringSplitOptions.RemoveEmptyEntries);
            this.fillHeadField(datas);

            ifDecomposed = true;
        }

        private void fillHeadField(String[] datas)
        {
            for (int index = 0; index < datas.Length; index++)
            {
                String data = datas[index];
                if (data.IndexOf(GlobalVar.DateString) == 0)
                {
                    this.date = data;
                }
                else if (data.IndexOf(GlobalVar.FromString) == 0)
                {
                    this.from = data;
                }
                else if (data.IndexOf(GlobalVar.SubjectString) == 0)
                {
                    this.subject = data;
                }
                else if (data.IndexOf(GlobalVar.ContentTypeString) == 0)
                {
                    this.content_Type = data;
                }
                else if (data.IndexOf(GlobalVar.ContentTransferEncodingString) == 0)
                {
                    this.content_Transfer_encoding = data;
                }
                else if (data.IndexOf(GlobalVar.MIMEVersionString) == 0)
                {
                    this.mime_Version = data;
                }
            }
        }

        private void checkDecomposed()
        {
            if (!ifDecomposed)
                throw new ArgumentNullException("Please Execute DecomposeHead() First");
        }
    }

    public partial class MailDecompose
    {
        public static String DesomposeBody(String mailBody)
        {
            return decomposeBody(mailBody);
        }

        private static String decomposeBody(String mailBody)
        {
            String data = mailBody;
            data = Regex.Replace(data, "\r\n\t", new MatchEvaluator(delegate(Match match)
            {
                return String.Empty;
            }), RegexOptions.Compiled);
            data = Regex.Replace(data, "\r\n ", new MatchEvaluator(delegate(Match match)
            {
                return String.Empty;
            }), RegexOptions.Compiled);

            return data;
        }

        //public static Boolean DecomposePartialBody(
        //    out String contentType,
        //    out String contentTransferEncoding,
        //    out String contentDisposition,
        //    out String context,
        //    String partialBody)
        //{
        //    contentType = String.Empty;
        //    contentTransferEncoding = String.Empty;
        //    contentDisposition = String.Empty;
        //    context = String.Empty;

        //    String[] datas = ValueHelper.StringHelper.SplitByCRLF(partialBody);
        //    if (datas.Length < 2)
        //        return false;

        //    for (int index = 0; index < datas.Length; index++)
        //    {
        //        String data = datas[index];
        //        if (data.IndexOf(GlobalVar.ContentTypeString) == 0)
        //        {
        //            contentType = data;
        //        }
        //        else if (data.IndexOf(GlobalVar.ContentTransferEncodingString) == 0)
        //        {
        //            contentTransferEncoding = data.Substring(GlobalVar.ContentTransferEncodingString.Length);
        //        }
        //        else if (data.IndexOf(GlobalVar.ContentDispositionString) == 0)
        //        {
        //            contentDisposition = data.Substring(GlobalVar.ContentDispositionString.Length);
        //        }
        //        else
        //        {
        //            context += data;
        //        }
        //    }
        //    return true;
        //}

        public static PartialMailModel DecomposePartialBody(String partialBody)
        {
            if (partialBody.StartsWith("\r\n"))
                partialBody = partialBody.Remove(0, 2);

            ContentType contentType = new ContentType();

            PartialMailModel partialMailModel;

            Int32 contentCount = Regex.Matches(partialBody, GlobalVar.ContentTypeString, RegexOptions.Compiled).Count;

            if (contentCount == 0)
            {
                partialMailModel = new PartialMailModel(false);
                return partialMailModel;
            }

            partialMailModel = new PartialMailModel(true);
            if (contentCount == 1)
            {
                String contentTransferEncoding = String.Empty;
                ContentDisposition contentDisposition = new ContentDisposition { Inline = true };

                String[] datas = partialBody.Split(new String[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                String[] confDatas = ValueHelper.StringHelper.SplitByCRLF(datas[0], StringSplitOptions.RemoveEmptyEntries);
                for (int index = 0; index < confDatas.Length; index++)
                {
                    String data = confDatas[index];
                    if (data.IndexOf(GlobalVar.ContentTypeString) == 0)
                    {
                        contentType = MailHelper.AnalyseContentType(data);
                    }
                    else if (data.IndexOf(GlobalVar.ContentTransferEncodingString) == 0)
                    {
                        contentTransferEncoding = data.Substring(GlobalVar.ContentTransferEncodingString.Length);
                    }
                    else if (data.IndexOf(GlobalVar.ContentDispositionString) == 0)
                    {
                        contentDisposition = MailHelper.AnalyseContentDisposition(data);
                    }
                }
                partialMailModel.ContentType = contentType;
                partialMailModel.ContentTransferEncoding = contentTransferEncoding;
                partialMailModel.ContentDisposition = contentDisposition;
                partialMailModel.Context = datas[1];
            }
            else
            {
                Int32 newlineIndex = partialBody.IndexOf("\r\n");
                partialMailModel.ContentType = MailHelper.AnalyseContentType(partialBody.Substring(0, newlineIndex));
                partialMailModel.Context = partialBody.Substring(newlineIndex + 2);
            }

            return partialMailModel;
        }
    }
}
