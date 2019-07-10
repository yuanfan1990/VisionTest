using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionSystem
{
    public class OmronFinsHelper
    {
        //字段
        private string ip;
        private int port = 9600;
        private string localip;
        private TcpClientHelper tcpclient;

        private byte[] FINS_Header = new byte[] { 0x46, 0x49, 0x4E, 0x53 };       //FINS命令固定包头
        private byte[] FINS_Length = new byte[] { 0x00, 0x00, 0x00, 0x0C };       //数据长度，从功能码数起至数据结尾
        private byte[] FINS_Command_0 = new byte[] { 0x00, 0x00, 0x00, 0x00 };    //功能码，客户端 to 服务端
        private byte[] FINS_Command_1 = new byte[] { 0x00, 0x00, 0x00, 0x01 };    //功能码，服务端 to 客户端
        private byte[] FINS_Command_2 = new byte[] { 0x00, 0x00, 0x00, 0x02 };    //功能码，FINS帧发送命令
        private byte[] FINS_Command_3 = new byte[] { 0x00, 0x00, 0x00, 0x03 };    //功能码，FINS帧发送错误通知命令
        private byte[] FINS_Command_6 = new byte[] { 0x00, 0x00, 0x00, 0x06 };    //功能码，确立通信连接
        private byte[] FINS_ErrorCode_0 = new byte[] { 0x00, 0x00, 0x00, 0x00 };  //错误码，正常
        private byte[] FINS_ErrorCode_1 = new byte[] { 0x00, 0x00, 0x00, 0x01 };  //错误码，数据头不是FINS或ASCII格式
        private byte[] FINS_ErrorCode_2 = new byte[] { 0x00, 0x00, 0x00, 0x02 };  //错误码，数据长度过长
        private byte[] FINS_ErrorCode_3 = new byte[] { 0x00, 0x00, 0x00, 0x03 };  //错误码，命令错误
        private byte[] FINS_ErrorCode_20 = new byte[] { 0x00, 0x00, 0x00, 0x20 }; //错误码，连接/通信被占用

        //构造
        public OmronFinsHelper(string ip, int port, string localip)
        {
            this.ip = ip;
            this.port = port;
            this.localip = localip;
        }

        //方法
        private byte[] Fins_CreateFiled_IP(string ip)
        {
            string[] byte_ip = ip.Split('.');

            return Uint32ToBytes(UInt32.Parse(byte_ip[byte_ip.Length - 1]));
        }

        private byte[] Fins_CreateFiled_Length(UInt32 length)
        {
            return Uint32ToBytes(length);
        }

        private byte Fins_MemoryAreaByte(string area)
        {
            byte b;
            switch (area)
            {
                case "DM":
                    b = 0x82;
                    break;
                case "CIO":
                    b = 0xB0;
                    break;
                case "WR":
                    b = 0xB1;
                    break;
                case "HR":
                    b = 0xB2;
                    break;
                case "AR":
                    b = 0xB3;
                    break;
                default:
                    b = 0x82;
                    break;
            }

            return b;
        }

        private byte[] Uint32ToBytes(UInt32 node)
        {
            byte[] b1 = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            byte[] b2 = BitConverter.GetBytes(node);

            if (b1.Length == b2.Length)
            {
                for (int i = 0; i < b1.Length; i++)
                {
                    b1[i] = b2[b2.Length - 1 - i];
                }
            }

            return b1;
        }

        private byte[] UshortToBytes(ushort node)
        {

            byte[] b1 = new byte[] { 0x00, 0x00 };
            byte[] b2 = BitConverter.GetBytes(node);

            if (b1.Length == b2.Length)
            {
                for (int i = 0; i < b1.Length; i++)
                {
                    b1[i] = b2[b2.Length - 1 - i];
                }
            }

            return b1;
        }

        private UInt32 BytesToUint32(byte[] p_arr)
        {
            if (p_arr.Length < 4)
                return 0;

            byte[] arrDes = new byte[p_arr.Length];
            for (int i = 0; i < 4; ++i)
            {
                arrDes[i] = p_arr[p_arr.Length - 1 - i];//大端转换为小端
            }

            UInt32 nRet = 0;
            try
            {
                nRet = BitConverter.ToUInt32(arrDes, 0);
            }
            catch (Exception)
            {
                nRet = 0;
            }

            return nRet;
        }

        private ushort BytesToUint16(byte[] p_arr)
        {
            ushort nRet = 0;

            if (p_arr.Length < 2)
                return nRet;

            byte[] arrDes = new byte[p_arr.Length];
            for (int i = 0; i < 2; ++i)
            {
                arrDes[i] = p_arr[p_arr.Length - 1 - i];//大端转换为小端
            }

            try
            {
                nRet = BitConverter.ToUInt16(arrDes, 0);
            }
            catch (Exception)
            {

                nRet = 0;
            }
            return nRet;
        }

        private byte[] CreateMessage_HandShake()
        {
            List<byte> list = new List<byte>();

            list.AddRange(FINS_Header);
            list.AddRange(FINS_Length);
            list.AddRange(FINS_Command_0);
            list.AddRange(FINS_ErrorCode_0);
            list.AddRange(Fins_CreateFiled_IP(this.localip));    //Client node address

            return list.ToArray();
        }

        private bool FINS_HandShake_Process(byte[] receivebuff, int length)
        {
            if (FINS_HandShake_Identify(receivebuff) == 0)
            {
                List<Byte> listRecv = new List<byte>();
                for (int i = 0; i < length; ++i)
                {
                    listRecv.Add(receivebuff[i]);
                }
                List<byte> listSub = new List<byte>();
                for (int i = 0; i < length; ++i)
                {
                    if (i >= 8) listSub.Add(listRecv[i]);
                }

                if (FINS_HandShake_Identify2(listSub.ToArray()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private int FINS_HandShake_Identify(byte[] buff)
        {
            //判断FINS消息的字节数长度
            if (buff.Length < (16))
            {
                return 1;
            }

            //判断FINS消息标识符
            if (buff[0] != FINS_Header[0] || buff[1] != FINS_Header[1] || buff[2] != FINS_Header[2] || buff[3] != FINS_Header[3])
            {
                return 2;
            }

            //判断FINS消息的数据字节长度
            byte[] arrFinsLen = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                arrFinsLen[i] = buff[4 + i];
            }
            UInt32 datalength = BytesToUint32(arrFinsLen);
            if (datalength != (buff.Length - 8))
            {
                return 3;
            }

            //判断FINS消息是否有ErrorCode
            if (buff[12] != 0 || buff[13] != 0 || buff[14] != 0 || buff[15] != 0)//接受数据：3.FINS消息errorcode判断
            {
                return 4;
            }

            return 0;
        }

        private bool FINS_HandShake_Identify2(byte[] p_arrRead)
        {
            byte[] m_arrHandshake = new byte[16];

            if (p_arrRead.Length < m_arrHandshake.Length)
            {
                for (int i = 0; i < m_arrHandshake.Length; ++i)
                {
                    m_arrHandshake[i] = 0x00;
                }
                for (int i = 0; i < p_arrRead.Length; ++i)
                {
                    m_arrHandshake[i] = p_arrRead[i];
                }
            }
            else
            {
                for (int i = 0; i < m_arrHandshake.Length; ++i)
                {
                    m_arrHandshake[i] = p_arrRead[i];
                }
            }

            //判断FINS消息中的 Command 和 ErrorCode
            if
            (
                0 == m_arrHandshake[0] && 0 == m_arrHandshake[1] && 0 == m_arrHandshake[2] && 0x01 == m_arrHandshake[3] &&
                0 == m_arrHandshake[4] && 0 == m_arrHandshake[5] && 0 == m_arrHandshake[6] && 0 == m_arrHandshake[7]
            )
            {
                //判断FINS消息中的PLCIP
                byte plcip = Fins_CreateFiled_IP(this.ip)[3];
                if (plcip == m_arrHandshake[15])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private byte[] CreateMessage_Read(string AreaType, ushort Address, ushort StartAddress, ushort ReadLength)
        {
            List<byte> list = new List<byte>();

            list.AddRange(FINS_Header);
            list.AddRange(Fins_CreateFiled_Length(0));
            list.AddRange(FINS_Command_2);
            list.AddRange(FINS_ErrorCode_0);

            list.Add(0x80); //ICF
            list.Add(0x00); //RSV
            list.Add(0x02); //GCT

            list.Add(0x00);
            byte b1 = Fins_CreateFiled_IP(this.ip)[3];    //PLC IP
            list.Add(b1);
            list.Add(0x00);

            list.Add(0x00);
            byte b2 = Fins_CreateFiled_IP(this.localip)[3];    //PC IP
            list.Add(b2);
            list.Add(0x00);

            list.Add(0xFF);

            list.Add(0x01);         //读取功能码
            list.Add(0x01);

            list.Add(Fins_MemoryAreaByte(AreaType));    //寄存器区

            list.AddRange(UshortToBytes(Address));      //字节起始地址

            list.Add((byte)StartAddress);               //位起始地址

            list.AddRange(UshortToBytes(ReadLength));   //读取长度

            byte[] b3 = Uint32ToBytes((UInt32)(list.Count - FINS_Header.Length - 4));   //长度
            for (int i = 0; i < b3.Length; i++)
            {
                list[FINS_Header.Length + i] = b3[i];
            }

            return list.ToArray();
        }

        private bool FINS_Read_Process(byte[] receivebuff, ushort ReadLength, int length, out ushort[] buffer)
        {
            buffer = null;

            if (FINS_Read_Identify(receivebuff) == 0)
            {
                List<Byte> listRecv = new List<byte>();
                for (int i = 0; i < length; ++i)
                {
                    listRecv.Add(receivebuff[i]);
                }
                List<byte> listSub = new List<byte>();
                for (int i = 0; i < length; ++i)
                {
                    if (i >= 8) listSub.Add(listRecv[i]);
                }

                if (FINS_Read_Identify2(listSub.ToArray()))
                {
                    if (FINS_Read_Identify3(receivebuff) == 0)
                    {
                        byte[] b1 = new byte[2];
                        UInt16[] b2 = new UInt16[ReadLength];
                        buffer = b2;

                        for (int i = 0; i < ReadLength; i++)
                        {
                            b1[0] = receivebuff[30 + i * 2];
                            b1[1] = receivebuff[30 + i * 2 + 1];
                            b2[i] = BytesToUint16(b1);
                            buffer[i] = b2[i];
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private int FINS_Read_Identify(byte[] buff)
        {
            //判断FINS消息的字节数长度
            if (buff.Length < (16))
            {
                return 1;
            }

            //判断FINS消息标识符
            if (buff[0] != FINS_Header[0] || buff[1] != FINS_Header[1] || buff[2] != FINS_Header[2] || buff[3] != FINS_Header[3])
            {
                return 2;
            }

            //判断FINS消息的数据字节长度
            byte[] arrFinsLen = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                arrFinsLen[i] = buff[4 + i];
            }
            UInt32 datalength = BytesToUint32(arrFinsLen);
            if (datalength != (buff.Length - 8))
            {
                return 3;
            }

            //判断FINS消息是否有ErrorCode
            if (buff[12] != 0 || buff[13] != 0 || buff[14] != 0 || buff[15] != 0)//接受数据：3.FINS消息errorcode判断
            {
                return 4;
            }

            return 0;
        }

        private bool FINS_Read_Identify2(byte[] p_arrRead)
        {
            byte[] m_arrRead = new byte[26];

            if (p_arrRead.Length < m_arrRead.Length)
            {
                for (int i = 0; i < m_arrRead.Length; ++i)
                {
                    m_arrRead[i] = 0x00;
                }
                for (int i = 0; i < p_arrRead.Length; ++i)
                {
                    m_arrRead[i] = p_arrRead[i];
                }
            }
            else
            {
                for (int i = 0; i < m_arrRead.Length; ++i)
                {
                    m_arrRead[i] = p_arrRead[i];
                }
            }

            //判断FINS消息中的 Command 和 ErrorCode
            if
            (
                0 == m_arrRead[0] && 0 == m_arrRead[1] && 0 == m_arrRead[2] && 0x02 == m_arrRead[3] &&
                0 == m_arrRead[4] && 0 == m_arrRead[5] && 0 == m_arrRead[6] && 0 == m_arrRead[7]
            )
            {
                //判断FINS消息中的PLCIP
                byte plcip = Fins_CreateFiled_IP(this.ip)[3];
                if (plcip == m_arrRead[15])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private int FINS_Read_Identify3(byte[] p_src)
        {
            //字节总数
            if (p_src.Length < (16 + 10 + 2 + 2 * 1))
            {
                return 1;
            }

            //读取模式
            if (p_src[16 + 10 + 0] != 0x01 || p_src[16 + 10 + 1] != 0x01)
            {
                return 2;
            }

            //读取正常
            if (p_src[16 + 10 + 2] != 0x00 || p_src[16 + 10 + 3] != 0x00)//判断最终结果是否正确--或异常代码
            {
                return 3;
            }

            return 0;
        }

        private byte[] CreateMessage_Write(string AreaType, ushort Address, ushort[] WriteValues)
        {
            List<byte> list = new List<byte>();

            list.AddRange(FINS_Header);
            list.AddRange(Fins_CreateFiled_Length(0));
            list.AddRange(FINS_Command_2);
            list.AddRange(FINS_ErrorCode_0);

            list.Add(0x80); //ICF
            list.Add(0x00); //RSV
            list.Add(0x02); //GCT

            list.Add(0x00);
            byte b1 = Fins_CreateFiled_IP(this.ip)[3];    //PLC IP
            list.Add(b1);
            list.Add(0x00);

            list.Add(0x00);
            byte b2 = Fins_CreateFiled_IP(this.localip)[3];    //PC IP
            list.Add(b2);
            list.Add(0x00);

            list.Add(0xFF);

            list.Add(0x01);         //写入功能码
            list.Add(0x02);

            list.Add(Fins_MemoryAreaByte(AreaType));    //寄存器区

            list.AddRange(UshortToBytes(Address));      //字节起始地址

            list.Add(0x00);                             //位起始地址

            list.AddRange(UshortToBytes((ushort)WriteValues.Length));  //写入长度

            for (int i = 0; i < WriteValues.Length; i++)       //写入数据
            {
                list.AddRange(UshortToBytes(WriteValues[i]));
            }

            byte[] b3 = Uint32ToBytes((UInt32)(list.Count - FINS_Header.Length - 4));   //长度
            for (int i = 0; i < b3.Length; i++)
            {
                list[FINS_Header.Length + i] = b3[i];
            }

            return list.ToArray();
        }

        private bool FINS_Write_Process(byte[] receivebuff, int length)
        {
            if (FINS_Write_Identify(receivebuff) == 0)
            {
                List<Byte> listRecv = new List<byte>();
                for (int i = 0; i < length; ++i)
                {
                    listRecv.Add(receivebuff[i]);
                }
                List<byte> listSub = new List<byte>();
                for (int i = 0; i < length; ++i)
                {
                    if (i >= 8) listSub.Add(listRecv[i]);
                }

                if (FINS_Write_Identify2(listSub.ToArray()))
                {
                    if (FINS_Write_Identify3(receivebuff) == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private int FINS_Write_Identify(byte[] buff)
        {
            //判断FINS消息的字节数长度
            if (buff.Length < (16))
            {
                return 1;
            }

            //判断FINS消息标识符
            if (buff[0] != FINS_Header[0] || buff[1] != FINS_Header[1] || buff[2] != FINS_Header[2] || buff[3] != FINS_Header[3])
            {
                return 2;
            }

            //判断FINS消息的数据字节长度
            byte[] arrFinsLen = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                arrFinsLen[i] = buff[4 + i];
            }
            UInt32 datalength = BytesToUint32(arrFinsLen);
            if (datalength != (buff.Length - 8))
            {
                return 3;
            }

            //判断FINS消息是否有ErrorCode
            if (buff[12] != 0 || buff[13] != 0 || buff[14] != 0 || buff[15] != 0)//接受数据：3.FINS消息errorcode判断
            {
                return 4;
            }

            return 0;
        }

        private bool FINS_Write_Identify2(byte[] p_arrRead)
        {
            byte[] m_arrRead = new byte[22];

            if (p_arrRead.Length < m_arrRead.Length)
            {
                for (int i = 0; i < m_arrRead.Length; ++i)
                {
                    m_arrRead[i] = 0x00;
                }
                for (int i = 0; i < p_arrRead.Length; ++i)
                {
                    m_arrRead[i] = p_arrRead[i];
                }
            }
            else
            {
                for (int i = 0; i < m_arrRead.Length; ++i)
                {
                    m_arrRead[i] = p_arrRead[i];
                }
            }

            //判断FINS消息中的 Command 和 ErrorCode
            bool status = false;
            if
            (
                0 == m_arrRead[0] && 0 == m_arrRead[1] && 0 == m_arrRead[2] && 0x02 == m_arrRead[3] &&
                0 == m_arrRead[4] && 0 == m_arrRead[5] && 0 == m_arrRead[6] && 0 == m_arrRead[7]
            )
            {
                //判断FINS消息中的PLCIP
                byte plcip = Fins_CreateFiled_IP(this.ip)[3];
                if (plcip == m_arrRead[15])
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }

            return status;
        }

        private int FINS_Write_Identify3(byte[] p_src)
        {
            //字节总数
            if (p_src.Length < (16 + 10 + 2 + 2))
            {
                return 1;
            }

            //写入模式
            if (p_src[16 + 10 + 0] != 0x01 || p_src[16 + 10 + 1] != 0x02)
            {
                return 1;
            }

            //写入正常
            if (p_src[16 + 10 + 2] != 0x00 || p_src[16 + 10 + 3] != 0x00)//判断最终结果是否正确--或异常代码
            {
                return 1;
            }

            return 0;
        }

        public bool FINS_Connect()
        {
            try
            {
                tcpclient = new TcpClientHelper(this.ip, this.port);
                tcpclient.Start();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FINS_DisConnect()
        {
            if (tcpclient != null)
            {
                tcpclient.Close();
            }
        }

        public bool FINS_HandShake_Command()
        {
            try
            {
                byte[] sendbuffer = CreateMessage_HandShake();
                byte[] receivebuffer = this.tcpclient.SendReceive(sendbuffer);
                return FINS_HandShake_Process(receivebuffer, this.tcpclient.Count);
            }
            catch
            {
                return false;
            }
        }

        public bool FINS_Read_Command(string AreaType, ushort Address, ushort StartAddress, ushort ReadLength, out ushort[] buffer)
        {
            try
            {
                byte[] sendbuffer = CreateMessage_Read(AreaType, Address, StartAddress, ReadLength);
                byte[] receivebuffer = this.tcpclient.SendReceive(sendbuffer);
                bool b = FINS_Read_Process(receivebuffer, ReadLength, this.tcpclient.Count, out buffer);
                return b;
            }
            catch
            {
                buffer = null;
                return false;
            }
        }

        public bool FINS_Write_Command(string AreaType, ushort Address, ushort[] WriteVaules)
        {
            try
            {
                byte[] sendbuffer = CreateMessage_Write(AreaType, Address, WriteVaules);
                byte[] receivebuffer = this.tcpclient.SendReceive(sendbuffer);
                return FINS_Write_Process(receivebuffer, this.tcpclient.Count);
            }
            catch
            {
                return false;
            }
        }
    }
}
