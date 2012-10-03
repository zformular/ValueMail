using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueMail.Infrastructure
{
    /// <summary>
    ///  Content-Type：{主类型}/{子类型}
    /// </summary>
    public class ContentMediaType
    {
        public const String multipart = "multipart";
        public const String mixed = "mixed";
        public const String alternative = "alternative";

        public const String text_plain = "text/plain";
        public const String text_html = "text/html";
    }

    /// <summary>
    ///  MediaType的主类型
    /// </summary>
    public enum MediaType
    {
        multipart,
        text,
        image,
        application
    };

    /// <summary>
    ///  multipart主类型包含的子类型
    /// </summary>
    public enum MultipartSubtype
    {
        /// <summary>
        ///  用于标识邮件附件 boundary属性分割
        /// </summary>
        mixed,
        /// <summary>
        ///  用于发一份纯文本和一份超文本 boundary属性分割
        /// </summary>
        alternative
    };

    /// <summary>
    ///  text主类型包含的子类型
    /// </summary>
    public enum TextSubtype
    {
        /// <summary>
        ///  纯文本
        /// </summary>
        plain,
        /// <summary>
        ///  超文本
        /// </summary>
        html
    };
}
