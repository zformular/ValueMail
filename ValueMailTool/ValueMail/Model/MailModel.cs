using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValueMail.Common;
using System.Collections.ObjectModel;
using ValueMail.Infrastructure;
using System.Net.Mime;

namespace ValueMail.Model
{
    public class MailModel
    {
        private String body;
        public String Body { get { return body; } }

        private String bodyHtml;
        public String BodyHtml { get { return bodyHtml; } }

        public MailHeadModel MailHead { get; private set; }

        public List<Attachment> Attachments { get; private set; }

        private AnalyseType analyseType;

        public MailModel(String uid, String mailHead, String mailBody)
        {
            initialModel(uid, mailHead, mailBody);
            this.analyseType = AnalyseType.None;
        }

        public MailModel(String uid, String mailHead, String mailBody, AnalyseType analyseType)
        {
            initialModel(uid, mailHead, mailBody);
            this.analyseType = analyseType;
        }

        private void initialModel(String uid, String mailHead, String mailBody)
        {
            this.body = String.Empty;
            this.Attachments = new List<Attachment>();

            MailDecompose mailDecompose = new MailDecompose();
            mailDecompose.DecomposeMail(mailHead, mailBody);
            MailHead = new MailHeadModel(uid, mailDecompose);
            this.analyseBody(MailHead.ContentType, null, MailHead.ContentTransferEncoding, mailDecompose.Body);
        }

        private void analyseBody(ContentType contentType, ContentDisposition contentDispostion, String contentTransferEncoding, String context)
        {
            String[] mediaType = contentType.MediaType.Split('/');
            if (mediaType[0] == MediaType.multipart.ToString())
            {
                String bounary = "--" + contentType.Boundary;
                String[] datas = context.Split(new String[] { bounary }, StringSplitOptions.RemoveEmptyEntries);
                for (int index = 0; index < datas.Length; index++)
                {
                    PartialMailModel partialMailModel = MailDecompose.DecomposePartialBody(datas[index]);
                    if (partialMailModel.HasValue)
                        analyseBody(partialMailModel.ContentType, partialMailModel.ContentDisposition, partialMailModel.ContentTransferEncoding, partialMailModel.Context);
                }
            }
            else
            {
                if (canDisplay(contentType))
                {
                    fillField(contentType, contentDispostion, contentTransferEncoding, context);
                }
            }
        }

        private Boolean canDisplay(ContentType contentType)
        {
            if (analyseType == AnalyseType.None)
                return true;
            else if (analyseType == AnalyseType.IgnoreUnDecoded)
            {
                String[] mediaType = contentType.MediaType.Split('/');
                if (mediaType[0] == MediaType.multipart.ToString())
                    return true;
                else if (mediaType[0] == MediaType.text.ToString())
                    return true;
                else
                    return false;
            }
            else
                throw new ArgumentException("出现未知的分析类型,照常理应该不会出现");
        }

        private void fillField(ContentType contentType, ContentDisposition disposition, String contentTransferEncoding, String context)
        {
            String[] types = contentType.MediaType.Split('/');
            if (disposition == null || disposition.Inline)
            {
                if (types[0] == MediaType.text.ToString())
                {
                    String result = MailHelper.MIMEDecode(contentType.CharSet, contentTransferEncoding, context);
                    if (types[1] == TextSubtype.plain.ToString())
                        this.body += result;
                    else if (types[1] == TextSubtype.html.ToString())
                        this.bodyHtml += result;
                }
                else
                    throw new NotImplementedException("暂不支持");
            }
            else
            {
                Byte[] result = MailHelper.MIMEDecode(contentTransferEncoding, context);
                this.Attachments.Add(new Attachment
                {
                    Data = result,
                    Name = disposition.FileName
                });
            }
        }
    }
}
