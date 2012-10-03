using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueMail.Common
{
    public class GlobalVar
    {
        public const String DateString = "Date: ";
        public const String FromString = "From: ";
        public const String SubjectString = "Subject: ";
        public const String ContentTransferEncodingString = "Content-Transfer-Encoding: ";
        public const String MIMEVersionString = "MIME-Version: ";

        // Content-Type及其参数
        public const String ContentTypeString = "Content-Type:";
        public const String BoundaryString = "boundary=";
        public const String CharsetString = "charset=";
        public const String NameString = "name=";

        // Content-Disposition及其参数
        public const String ContentDispositionString = "Content-Disposition: ";
        public const String InlineString = "inline";
        public const String FileNameString = "filename=";

        /// <summary>
        ///  Content-Type内元素的正则匹配模板
        ///  <para name='type'>type-MediaType</para>
        ///  <para name='boundary'>boundary-Boundary</para>
        ///  <para name='charset'>charset-Charset</para>
        ///  <para name='name'>name-Name</para>
        /// </summary>
        public const String ContentTypePattern = @"\bContent-Type:\s*(?<type>\S+/[^;]+);(\s*charset=(?("")""(?<charset>\S+)""" +
                @"|(?<charset>\S+)))?(\s*boundary=(?("")""(?<boundary>\S+)""|(?<boundary>\S+)))?(\s*name=(?("")""(?<name>\S+)""|(?<name>\S+)))?";

        /// <summary>
        ///  Content-Disposition内元素的正则匹配模板
        ///  <para name='type'>type-Inline/!Inline</para>
        ///  <para name='filename'>filename-FileName</para>
        /// </summary>
        public const String ContentDispositionPattern = @"\bContent-Disposition:\s*(?<type>\w+)(;\s*filename=(?("")""(?<filename>\S+)""|(?<filename>\S+)))?";

        /// <summary>
        ///  Date内元素的正则匹配模板
        ///  <para name='day'>day-天</para>
        ///  <para name='month'>month-月</para>
        ///  <para name='year'>year-年</para>
        ///  <para name='time'>time-时刻</para>
        /// </summary>
        public const String DatePattern = @"\bDate:\s*\w+,\s+(?<day>\d{1,2})\s*(?<month>\w+)\s*(?<year>\d+)\s(?<time>(\d{2}:){2}\d{2})";

        /// <summary>
        ///  From内元素的正则匹配模板
        ///  <para name='name'>name-用户名</para>
        ///  <para name='address'>address-邮箱地址</para>
        /// </summary>
        public const String FromPattern = @"\bFrom:\s*(?("")""(?<name>.+)""|(?<name>.+))\s*<(?<address>.*)>";
    }
}
