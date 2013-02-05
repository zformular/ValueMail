using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueMail.Send.Infrastructure
{
    public class HostModel
    {
        /// <summary>
        ///  服务器名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///  地址
        /// </summary>
        public String Host { get; set; }

        /// <summary>
        ///  端口
        /// </summary>
        public Int32 Port { get; set; }

        /// <summary>
        ///  Ssl加密
        /// </summary>
        public Boolean Ssl { get; set; }
    }
}
