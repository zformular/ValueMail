using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueMail.Infrastructure
{
    /// <summary>
    ///  用来确定未能解码成功的数据的处理
    /// </summary>
    public enum AnalyseType
    {
        /// <summary>
        ///  显示所有数据
        /// </summary>
        None,
        /// <summary>
        ///  取消未解码的数据的显示
        /// </summary>
        IgnoreUnDecoded
    }
}
