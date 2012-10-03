﻿/*
  >>>----- Copyright (c) 2012 zformular -----> 
 |                                            |
 |              Author: zformular             |
 |          E-mail: zformular@163.com         |
 |               Date: 10.1.2012              |
 |                                            |
 ╰==========================================╯

*/

using System;
using ValueMail.Instruction;
using System.Text.RegularExpressions;
using ValueMail.IMAP.Infrastructure;
using System.Collections.Generic;
using ValueMail.Model;

namespace ValueMail.IMAP.MailBase
{
    public class IMAPBase : ValueMail.MailBase.MailBase
    {
        private IMAPInstruction Instruction = null;

        public override void Connect(string server, int port)
        {
            Connect(server, port, false);
        }

        public override void Connect(string server, int port, bool ssl)
        {
            base.Connect(server, port, ssl);
            Instruction = new IMAPInstruction(providerType, streamWriter, streamReader);
        }

        public override Boolean Loging(string account, string password)
        {
            if (String.IsNullOrEmpty(account))
                throw new ArgumentException("账号不能为空");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空");

            try
            {
                Boolean result = Instruction.LOGINResponse(account, password);
                return result;
            }
            catch (ArgumentException aex)
            {
                throw (aex);
            }
            catch (Exception ex)
            {
                throw new Exception("与服务器连接失败");
            }
        }

        /// <summary>
        ///  返回邮箱列表
        /// </summary>
        /// <returns></returns>
        public String ListMailbox()
        {
            //List<String> mailboxList = new List<string>();
            //response = Instruction.LISTResponse();
            //MatchCollection matches = Regex.Matches(response, IMAPHelper.MailBoxPattern);
            //foreach (Match item in matches)
            //{
            //    GroupCollection groups = item.Groups;
            //    mailboxList.Add(groups["mailbox"].ToString());
            //}


            return Instruction.LISTResponse();
        }

        /// <summary>
        ///  进入指定的邮箱
        /// </summary>
        /// <param name="mailboxName"></param>
        public void SelectMailbox(String mailboxName)
        {
            response = Instruction.SELECTResponse(mailboxName);
        }

        public override List<MailHeadModel> GetMailHeaders()
        {
            return GetMailHeaders(SearchType.New);
        }

        public override List<MailHeadModel> GetMailHeaders(SearchType searchType)
        {
            List<MailHeadModel> mailheadList = new List<MailHeadModel>();
            response = Instruction.SEARCHResponse(searchType);
            MatchCollection maches = Regex.Matches(response, IMAPHelper.SearchMailPattern);
            foreach (Match item in maches)
            {
                Int32 index = Int32.Parse(item.Groups["index"].ToString());
                mailheadList.Add(getMailHeader(index));
            }
            return mailheadList;
        }

        private MailHeadModel getMailHeader(Int32 index)
        {
            response = Instruction.FETCHResponse(index, FetchType.UID);
            String uid = Regex.Match(response, IMAPHelper.UIDPattern).Groups["uid"].ToString();
            response = Instruction.FETCHResponse(index, FetchType.Expression("BODY[HEADER.FIELDS (Date From Subject Content-Type Content-Transfer-Encoding)]"));
            String body = Regex.Match(response, IMAPHelper.FetchMailPattern).Groups["body"].ToString();
            return new MailHeadModel(uid, body);
        }

        public override MailModel GetMail(int index)
        {
            response = Instruction.FETCHResponse(index, FetchType.UID);
            String uid = Regex.Match(response, IMAPHelper.UIDPattern).Groups["uid"].ToString();
            response = Instruction.FETCHResponse(index, FetchType.RFC822_HEADER);
            String head = Regex.Match(response, IMAPHelper.FetchMailPattern).Groups["body"].ToString();
            response = Instruction.FETCHResponse(index, FetchType.RFC822_TEXT);
            String body = Regex.Match(response, IMAPHelper.FetchMailPattern).Groups["body"].ToString();
            return new MailModel(uid, head, body);
        }

        public void CopyMail(Int32 index, String mailbox)
        {
            Instruction.COPYResponse(index, mailbox);
        }

        public override void DeleMail(int index)
        {
            Instruction.STOREAddFlagResponse(index, StoreFlags.Deleted);
        }

        public void CloseMailbox()
        {
            Instruction.CLOSEResponse();
        }

        public override void Disconnect()
        {
            Instruction.LOGOUTResponse();
            base.Disconnect();
        }

        public override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (Instruction != null)
            {
                Instruction.Dispose();
                Instruction = null;
            }
        }
    }
}
