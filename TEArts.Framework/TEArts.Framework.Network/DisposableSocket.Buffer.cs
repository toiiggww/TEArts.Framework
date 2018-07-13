using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEArts.Framework.Network
{
    partial class DisposableSocket
    {
        /// <summary>
        /// 索引，开始位置，结束位置，长度
        /// </summary>
        private SortedList<int, Tuple<int, int, int>> BufferInfo = new SortedList<int, Tuple<int, int, int>>();

    }
    //class 
}
