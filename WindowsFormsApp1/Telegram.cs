using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaifexEmulator
{
    class Telegram
    {
        public const byte L10  =  10;
        public const byte L20  =  20;
        public const byte L30  =  30;
        public const byte LX30 = 230;
        public const byte L40  =  40;
        public const byte L50  =  50;
        public const byte L60  =  60;
        public const byte R04  = 104;
        public const byte R05  = 105;
        public const byte RX07 = 207;
        public const byte RX08 = 208;
        public const byte RX13 = 213;
        public const byte R14  = 114;
        public const byte RX14 = 214;
        public const byte RX19 = 219;
        public const byte RX20 = 220;


        public const int hdrLen = 17;
        public const int flexHdrLen = 23;

        public class myHdr
        {
            public ushort msgLen { get; set; }
            public uint msgSeqNum { get; set; }
            public int epoch_s { get; set; }
            public ushort ms { get; set; }
            public byte msgType { get; set; }
            public ushort fcm_id { get; set; }
            public ushort session_id { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(NumToBytes(msgLen));
                list.AddRange(NumToBytes(msgSeqNum));
                list.AddRange(NumToBytes(epoch_s));
                list.AddRange(NumToBytes(ms));
                list.Add(msgType);
                list.AddRange(NumToBytes(fcm_id));
                list.AddRange(NumToBytes(session_id));
                return list.ToArray();
            }
            public void UnPack(byte[] buffer)
            {
                msgLen = BitConverter.ToUInt16(ReverseArray(buffer, 0, 2), 0);
                msgSeqNum = BitConverter.ToUInt32(ReverseArray(buffer, 2, 4), 0);
                epoch_s = BitConverter.ToInt32(ReverseArray(buffer, 6, 4), 0);
                ms = BitConverter.ToUInt16(ReverseArray(buffer, 10, 2), 0);
                msgType = buffer[12];
                fcm_id = BitConverter.ToUInt16(ReverseArray(buffer, 13, 2), 0);
                session_id = BitConverter.ToUInt16(ReverseArray(buffer, 15, 2), 0);
            }
        }
        public class mySubHdr
        {
            public byte msgTypeExt { get; set; }
            public byte preserve { get; set; }
            public uint msgTimeNs { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.Add(msgTypeExt);
                list.Add(preserve);
                list.AddRange(NumToBytes(msgTimeNs));
                return list.ToArray();
            }
            public void UnPack(byte[] buffer)
            {
                msgTypeExt = buffer[hdrLen];
                preserve = buffer[hdrLen + 1];
                msgTimeNs = BitConverter.ToUInt32(ReverseArray(buffer, hdrLen + 2, 4), 0);
            }
        }

        public static byte[] NumToBytes(short s)
        {
            byte[] tmpBytes = BitConverter.GetBytes(s);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(tmpBytes);
            return tmpBytes;
        }
        public static byte[] NumToBytes(ushort us)
        {
            byte[] tmpBytes = BitConverter.GetBytes(us);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(tmpBytes);
            return tmpBytes;
        }
        public static byte[] NumToBytes(uint i)
        {
            byte[] tmpBytes = BitConverter.GetBytes(i);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(tmpBytes);
            return tmpBytes;
        }
        public static byte[] NumToBytes(int ui)
        {
            byte[] tmpBytes = BitConverter.GetBytes(ui);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(tmpBytes);
            return tmpBytes;
        }
        public static byte[] NumToBytes(long l)
        {
            byte[] tmpBytes = BitConverter.GetBytes(l);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(tmpBytes);
            return tmpBytes;
        }
        public static byte[] NumToBytes(ulong ul)
        {
            byte[] tmpBytes = BitConverter.GetBytes(ul);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(tmpBytes);
            return tmpBytes;
        }
        public static byte GenChkSum(byte[] buffer, long buflen)
        {
            long idx;
            byte cks;

            for (idx = 0L, cks = 0; idx < buflen; cks += (byte)buffer[idx++]);
            return (byte)(cks % 256);
        }

        public static byte[] ReverseArray(byte[] arr, long index, long len)
        {
            byte[] tmpBytes = new byte[len];
            Array.Copy(arr, index, tmpBytes, 0, len);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(tmpBytes);
            return tmpBytes;
        }

        public class myL10
        {
            public myHdr myHdr = new myHdr();
            public byte statusCode { get; set; }
            public uint start_in_bound_num { get; set; }
            public byte chksum { get; set; }
            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.Add(statusCode);
                list.AddRange(NumToBytes(start_in_bound_num));
                list.Add(chksum);
                return list.ToArray();
            }
            public void UnPack(byte[] buffer)
            {
                myHdr.UnPack(buffer);
                statusCode = buffer[hdrLen];
                start_in_bound_num = BitConverter.ToUInt32(ReverseArray(buffer, hdrLen + 1, 4), 0);
                chksum = buffer[hdrLen + 5];
            }
        }
        public class myL20
        {
            public myHdr myHdr = new myHdr();
            public byte statusCode { get; set; }
            public byte chksum { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.Add(statusCode);
                list.Add(chksum);
                return list.ToArray();
            }
            public void UnPack(byte[] buffer)
            {
                myHdr.UnPack(buffer);
                statusCode = buffer[hdrLen];
                chksum = buffer[hdrLen + 1];
            }
        }
        public class myL30
        {
            public myHdr myHdr = new myHdr();
            public byte statusCode { get; set; }
            public ushort append_no { get; set; }
            public uint end_out_bound_num { get; set; }
            public byte systemType { get; set; }
            public byte encryptMethod { get; set; }
            public byte chksum { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.Add(statusCode);
                list.AddRange(NumToBytes(append_no));
                list.AddRange(NumToBytes(end_out_bound_num));
                list.Add(systemType);
                list.Add(encryptMethod);
                list.Add(chksum);
                return list.ToArray();
            }
        }
        public class myLX30
        {
            public myHdr myHdr = new myHdr();
            public mySubHdr mySubHdr = new mySubHdr();
            public ushort statusCode { get; set; }
            public byte part_id { get; set; }
            public uint end_out_bound_num { get; set; }
            public byte[] filler = new byte[8];
            public byte chksum { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.AddRange(mySubHdr.GetBytes());
                list.AddRange(NumToBytes(statusCode));
                list.Add(part_id);
                list.AddRange(NumToBytes(end_out_bound_num));
                list.AddRange(filler);
                list.Add(chksum);
                return list.ToArray();
            }
        }
        public class myL40
        {
            public myHdr myHdr = new myHdr();
            public byte statusCode { get; set; }
            public ushort appendNo { get; set; }
            public ushort fcm_id { get; set; }
            public ushort session_id { get; set; }
            public byte systemType { get; set; }
            public byte apCode { get; set; }
            public byte keyValue { get; set; }
            public uint request_start_seq { get; set; }
            public byte cancel_order_sec { get; set; }
            public byte chksum { get; set; }

            public void UnPack(byte[] buffer)
            {
                myHdr.UnPack(buffer);                
                statusCode = buffer[hdrLen];
                appendNo = BitConverter.ToUInt16(ReverseArray(buffer, hdrLen + 1, 2), 0);
                fcm_id = BitConverter.ToUInt16(ReverseArray(buffer, hdrLen + 3, 2), 0);
                session_id = BitConverter.ToUInt16(ReverseArray(buffer, hdrLen + 5, 2), 0);
                systemType = buffer[hdrLen + 7];
                apCode = buffer[hdrLen + 8];
                keyValue = buffer[hdrLen + 9];
                request_start_seq = BitConverter.ToUInt32(ReverseArray(buffer, hdrLen + 10, 4), 0);
                cancel_order_sec = buffer[hdrLen + 14];
                chksum = buffer[hdrLen + 15];
            }
        }
        public class myL50
        {
            public myHdr myHdr = new myHdr();
            public byte statusCode { get; set; }
            public byte heartBtInt { get; set; }
            public ushort max_flow_ctrl_cnt { get; set; }
            public byte chksum { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.Add(statusCode);
                list.Add(heartBtInt);
                list.AddRange(NumToBytes(max_flow_ctrl_cnt));
                list.Add(chksum);
                return list.ToArray();
            }
        }
        public class myL60
        {
            public myHdr myHdr = new myHdr();
            public byte statusCode { get; set; }
            public byte chksum { get; set; }

            public void UnPack(byte[] buffer)
            {
                myHdr.UnPack(buffer);
                statusCode = buffer[hdrLen];
                chksum = buffer[hdrLen + 1];
            }
        }
        public class myR04
        {
            public myHdr myHdr = new myHdr();
            public byte statusCode { get; set; }
            public byte chksum { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.Add(statusCode);
                list.Add(chksum);
                return list.ToArray();
            }
            public void UnPack(byte[] buffer)
            {
                myHdr.UnPack(buffer);
                statusCode = buffer[hdrLen];
                chksum = buffer[hdrLen + 1];
            }
        }
        public class myR05
        {
            public myHdr myHdr = new myHdr();
            public byte statusCode { get; set; }
            public byte chksum { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.Add(statusCode);
                list.Add(chksum);
                return list.ToArray();
            }
            public void UnPack(byte[] buffer)
            {
                myHdr.UnPack(buffer);
                statusCode = buffer[hdrLen];
                chksum = buffer[hdrLen + 1];
            }
        }
        #region RX02 RX03可測 先不管這塊
        public class myRX02
        {
            public myHdr myHdr = new myHdr();
            public mySubHdr mySubHdr = new mySubHdr();
            public byte part_id { get; set; }
            public byte noLeg { get; set; }
            public ushort statusCode { get; set; }
            public char execType { get; set; }
            public ushort cm_id { get; set; }
            public ushort fcm_id { get; set; }
            public char[] order_no = new char[5];
            public uint ord_id { get; set; }
            public char[] user_define = new char[8];
            #region 商品代號部分 Noleg == 1
            public uint pseq { get; set; }
            public char[] prod_id = new char[20];
            #endregion
            public int price { get; set; }
            public ushort qty { get; set; }
            public uint investor_acno { get; set; }
            public char investor_flag { get; set; }
            public byte side { get; set; }
            public byte ordType { get; set; }
            public byte timeInForce { get; set; }
            public char positionEffect { get; set; }
            public int lastPx { get; set; }
            public ushort lastQty { get; set; }
            


            public byte chksum { get; set; }
        }
        public class myRX03
        {

        }
        #endregion
        public class myRX08
        {
            public myHdr myHdr = new myHdr();
            public mySubHdr mySubHdr = new mySubHdr();
            public byte part_id { get; set; }
            public ushort statusCode { get; set; }
            public char[] order_no = new char[5];
            public uint ord_id { get; set; }
            public ushort fcm_id { get; set; }
            #region 商品代號部分 MsgTypeExt==1 ? pseq : prod_id
            public uint pseq { get; set; }
            public char[] prod_id = new char[20];
            #endregion
            public byte[] filler = new byte[8];
            public byte chksum { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.AddRange(mySubHdr.GetBytes());
                list.Add(part_id);
                list.AddRange(NumToBytes(statusCode));                
                list.AddRange(Encoding.ASCII.GetBytes(order_no));
                list.AddRange(NumToBytes(ord_id));
                list.AddRange(NumToBytes(fcm_id));
                if (mySubHdr.msgTypeExt == 1)
                    list.AddRange(NumToBytes(pseq));
                else
                    list.AddRange(Encoding.ASCII.GetBytes(prod_id));
                list.AddRange(filler);
                list.Add(chksum);
                return list.ToArray();
            }
        }
        public class myRX13
        {

        }
        public class myR14
        {
            public myHdr myHdr = new myHdr();
            public byte statusCode { get; set; }
            public uint fcm_req_id { get; set; }
            public uint bulletin_seq { get; set; }
            public uint epoch_s { get; set; }
            public ushort ms { get; set; }
            public byte system_type { get; set; }
            public string data { get; set; }
            public byte chksum { get; set; }
            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.Add(statusCode);
                list.AddRange(NumToBytes(fcm_req_id));
                list.AddRange(NumToBytes(bulletin_seq));
                list.AddRange(NumToBytes(epoch_s));
                list.AddRange(NumToBytes(ms));
                list.Add(system_type);
                list.AddRange(Encoding.ASCII.GetBytes(data));
                list.Add(chksum);
                return list.ToArray();
            }
        }
        public class myRX14
        {
            public myHdr myHdr = new myHdr();
            public mySubHdr mySubHdr = new mySubHdr();
            public ushort statusCode { get; set; }
            public uint fcm_req_id { get; set; }
            public uint bulletin_seq { get; set; }
            public uint epoch_s { get; set; }
            public uint nanosecond { get; set; }
            public byte system_type { get; set; }
            public char[] file_code = new char[4]; 
            public char is_pub { get; set; }
            public char[] file_path = new char[128];
            public string else_data { get; set; }
            public byte chksum { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.AddRange(mySubHdr.GetBytes());
                list.AddRange(NumToBytes(statusCode));
                list.AddRange(NumToBytes(fcm_req_id));
                list.AddRange(NumToBytes(bulletin_seq));
                list.AddRange(NumToBytes(epoch_s));
                list.AddRange(NumToBytes(nanosecond));
                list.Add(system_type);
                list.AddRange(Encoding.ASCII.GetBytes(file_code));
                list.Add((byte)is_pub);
                list.AddRange(Encoding.ASCII.GetBytes(file_path));
                if (else_data != "")
                    list.AddRange(Encoding.ASCII.GetBytes(else_data));
                list.Add(chksum);
                return list.ToArray();
            }
        }
        public class myRX19
        {
            public myHdr myHdr = new myHdr();
            public mySubHdr mySubHdr = new mySubHdr();
            public char securityRequestType { get; set; }
            public uint securityReqId { get; set; }
            public char type { get; set; }
            public ushort fcm_id { get; set; }
            public uint investor_acno { get; set; }
            public char application_type { get; set; }
            public char[] root_symbol = new char[3];
            public char expiry_type { get; set; }
            public char[] contract_date = new char[8];
            public uint strike_price { get; set; }
            public char call_put_code { get; set; }
            public char[] filler = new char[20];
            public byte chksum { get; set; }

            public void UnPack(byte[] buffer)
            {
                myHdr.UnPack(buffer);
                mySubHdr.UnPack(buffer);
                securityRequestType = (char)buffer[flexHdrLen];
                securityReqId = BitConverter.ToUInt32(ReverseArray(buffer, flexHdrLen + 1, 4), 0);
                type = (char)buffer[flexHdrLen + 5];
                fcm_id = BitConverter.ToUInt16(ReverseArray(buffer, flexHdrLen + 6, 2), 0);
                investor_acno = BitConverter.ToUInt32(ReverseArray(buffer, flexHdrLen + 8, 4), 0);
                application_type = (char)buffer[flexHdrLen + 12];
                byte[] tmp = new byte[3];
                Array.Copy(buffer, flexHdrLen + 13, tmp, 0, 3);
                root_symbol = Encoding.ASCII.GetString(tmp).ToCharArray();
                expiry_type = (char)buffer[flexHdrLen + 16];
                tmp = new byte[8];
                Array.Copy(buffer, flexHdrLen + 17, tmp, 0, 8);
                contract_date = Encoding.ASCII.GetString(tmp).ToCharArray();
                strike_price = BitConverter.ToUInt32(ReverseArray(buffer, flexHdrLen + 25, 4), 0);
                call_put_code = (char)buffer[flexHdrLen + 29];
                chksum = buffer[flexHdrLen + 30];
            }
        }
        public class myRX20
        {
            public myHdr myHdr = new myHdr();
            public mySubHdr mySubHdr = new mySubHdr();
            public char securityRequestType { get; set; }
            public uint securityReqId { get; set; }
            public ushort statusCode { get; set; }
            public byte chksum { get; set; }

            public byte[] GetBytes()
            {
                List<byte> list = new List<byte>();
                list.AddRange(myHdr.GetBytes());
                list.AddRange(mySubHdr.GetBytes());
                list.Add((byte)securityRequestType);
                list.AddRange(NumToBytes(securityReqId));
                list.AddRange(NumToBytes(statusCode));
                list.Add(chksum);
                return list.ToArray();
            }
        }
        public class myRX41
        {

        }
    }
}
