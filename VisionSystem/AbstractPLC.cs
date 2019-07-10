using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionSystem
{
    public abstract class AbstractPLC
    {
        /// <summary>
        /// 打开设备
        /// </summary>
        /// <returns>=true 成功 =false 失败</returns>
        public abstract bool Open();
        /// <summary>
        /// 关闭设备
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="block">数据块</param>
        /// <param name="start">起始地址</param>
        /// <param name="len">长度</param>
        /// <param name="buffer">读取的信息</param>
        /// <returns>=true 读取成功 =false 读取失败</returns>
        public abstract bool Read(int start, int len, out ushort[] buffer);
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="block">数据块</param>
        /// <param name="start">起始地址</param>
        /// <param name="buffer">写入的数据</param>
        /// <returns>=true 写入成功 =false 写入失败</returns>
        public abstract bool Write(int start, ushort[] buffer);
    }
}
