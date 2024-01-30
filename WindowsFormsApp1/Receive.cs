using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaifexEmulator;
using System.Threading;
using System.Xml.Linq;

namespace TaifexEmulator
{
    class Receive
    {
        public static byte[] inString = new byte[4096];
        static Socket serverSocket;
        static Socket mySocket;
        public static ushort fcm_id = 606;
        public static ushort session_id = 88;
        public static byte[] sendBuff = new byte[4096];
        public static ushort sendLen = 0;
        public static byte[] tmpBytes;
        public static byte[] part_id = { 201, 206 };
        public static uint msg_cur_bound_num = 0;
        public static bool isFLEX = false;
        public static Thread hbThread = new Thread(SendHB);
        public static bool hbFlag = false;
        public static Thread receiveThread = new Thread(ReceiveMessage);
        public static XMLControll xCtrl = new XMLControll();

        public static void SocketInit(string ip, int port)
        {
            if (serverSocket != null)
                serverSocket.Close();
            IPAddress tndIp = IPAddress.Parse(ip);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(tndIp, port));
            serverSocket.Listen(10);
            WriteMessage(String.Format("Listen {0} successful!", serverSocket.LocalEndPoint.ToString()));
            if (!receiveThread.IsAlive)
            {
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }            
        }

        public static void WriteMessage(string msg)
        {
            try
            {
                Form1.form1.Invoke(new Action(() =>
                {
                    Form1.form1.msgLabel.AppendText(DateTime.Now.ToString("HH:mm:ss.ffffff") + " " + msg + "\n");
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ReceiveMessage()
        {
            mySocket = serverSocket.Accept();
            WriteMessage(String.Format("連線到{0}", mySocket.RemoteEndPoint.ToString()));

            while (true)
            {
                try
                {
                    int receiveNumber = mySocket.Receive(inString);

                    //Debug用
                    //for (int i = 0; i < receiveNumber; i++)
                    //{
                    //    WriteMessage(String.Format("inString[{0}]:{1}", i, inString[i]));
                    //}

                    if (receiveNumber > 17)
                    {
                        Telegram.myHdr myHdr = new Telegram.myHdr();
                        tmpBytes = new byte[receiveNumber];
                        Array.Copy(inString, 0, tmpBytes, 0, receiveNumber);
                        myHdr.UnPack(tmpBytes);
                        string s = myHdr.msgType == 230 ? "LX" : (myHdr.msgType > 200 ? "RX" : (myHdr.msgType > 100 ? "R" : (myHdr.msgType > 0 ? "L" : "")));
                        WriteMessage(String.Format("收到電文:" + s + "{0}!", (myHdr.msgType % 100).ToString("D2")));
                        //debug用
                        //foreach (var p in myHdr.GetType().GetProperties())
                        //{
                        //	var value = p.GetValue(myHdr, null);
                        //	writeMessage(String.Format("myHdr: {0},{1}", p.Name, value));
                        //}
                        RecvTypeSwitch(tmpBytes, myHdr.msgType);
                    }

                    if (receiveNumber <= 0)
                    {
                        hbFlag = false;
                        mySocket.Close();                        
                        mySocket = serverSocket.Accept();                        
                        WriteMessage(String.Format("斷線! 已重連到{0}", mySocket.RemoteEndPoint.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    WriteMessage(ex.Message);
                    WriteMessage("接收失敗，請確認!");
                    hbFlag = false;
                    mySocket.Close();
                    mySocket = serverSocket.Accept();                    
                    WriteMessage(String.Format("斷線! 已重連到{0}", mySocket.RemoteEndPoint.ToString()));
                }
            }
        }

        public static long UnixTime()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (DateTime.UtcNow - epochStart).Ticks * 100;
        }

        public static void SendMessage(byte[] sendBuff, int sendLen)
        {
            try
            {
                if (mySocket == null)
                {
                    WriteMessage("尚未連線!");
                }
                else
                {
                    //DEBUG用
                    //for (int i = 0; i < sendLen; i++)
                    //{
                    //    writeMessage(String.Format("sendBuff[{0}]:{1}", i, sendBuff[i]));
                    //}
                    mySocket.Send(sendBuff, sendLen, SocketFlags.None);
                    WriteMessage(String.Format("Send OK, SendLen = {0}", sendLen));
                }
            }
            catch (Exception ex)
            {
                WriteMessage(ex.Message);
                WriteMessage("傳送失敗，請確認!");
                hbFlag = false;
                mySocket.Close();
                mySocket = serverSocket.Accept();                
                WriteMessage(String.Format("斷線! 已重連到{0}", mySocket.RemoteEndPoint.ToString()));
            }
        }

        public static void SendHB()
        {
            Thread.Sleep(30000);
            while (true)
            {
                if (!hbFlag)
                {
                    Thread.Sleep(1);
                }
                else
                {                    
                    try
                    {
                        if (mySocket == null)
                        {
                            WriteMessage("尚未連線!");
                        }
                        else
                        {
                            BuildSendBuff(Telegram.R04, null);
                        }
                        if (hbFlag)
                            Thread.Sleep(30000);
                        else
                            Thread.Sleep(1);
                    }
                    catch (Exception ex)
                    {
                        WriteMessage(ex.Message);
                        WriteMessage("傳送HB失敗，請確認!");
                        hbFlag = false;
                    }
                }
            }
        }

        public static void RecvTypeSwitch(byte[] inBuff, byte recvMsgType)
        {
            switch (recvMsgType)
            {
                case Telegram.L10:
                    BuildSendBuff(Telegram.L10, null);
                    break;
                case Telegram.L20:
                    Telegram.myL20 myL20 = new Telegram.myL20();
                    myL20.UnPack(inBuff);
                    fcm_id = myL20.myHdr.fcm_id;
                    session_id = myL20.myHdr.session_id;
                    isFLEX = myL20.statusCode == 1 ? true : false;
                    WriteMessage(String.Format("FLEX VERSION = {0}", myL20.statusCode));
                    BuildSendBuff(Telegram.L30, null);
                    break;
                case Telegram.L40:
                    BuildSendBuff(Telegram.L50, null);
                    break;
                case Telegram.L60:
                    if (!hbThread.IsAlive)
                    {                        
                        hbThread.IsBackground = true;
                        hbThread.Start();
                    }
                    hbFlag = true;
                    break;
                case Telegram.R04:
                    BuildSendBuff(Telegram.R05, null);
                    break;
                case Telegram.R05:
                    break;
                case Telegram.RX07:
                    break;
                case Telegram.RX19:
                    Telegram.myRX19 myRX19 = new Telegram.myRX19();
                    myRX19.UnPack(inBuff);
                    BuildSendBuff(Telegram.RX20, myRX19);
                    break;
            }
        }

        public static void BuildSendBuff(byte sendMsgType, object obj)
        {
            #region build header
            Telegram.myHdr myHdr = new Telegram.myHdr();
            Telegram.mySubHdr mySubHdr = new Telegram.mySubHdr();
            myHdr.msgLen = 0;
            myHdr.msgSeqNum = 0;
            long uTime = UnixTime();
            myHdr.epoch_s = (int)(uTime / 1000000000);
            myHdr.ms = (ushort)(uTime / 1000000 % 1000);
            myHdr.fcm_id = fcm_id;
            myHdr.session_id = session_id;
            

            if (sendMsgType > 200)
            {
                mySubHdr.msgTypeExt = 0;
                mySubHdr.msgTimeNs = (uint)(uTime % 1000000);
            }
            #endregion

            switch (sendMsgType)
            {
                #region Send L10
                case Telegram.L10:
                    Telegram.myL10 myL10 = new Telegram.myL10();
                    myL10.myHdr = myHdr;
                    myL10.myHdr.msgType = 10;
                    myL10.statusCode = 0;
                    myL10.start_in_bound_num = 0;
                    myL10.chksum = 0;
                    sendBuff = myL10.GetBytes();
                    sendLen = (ushort)sendBuff.Length;
                    myL10.myHdr.msgLen = (ushort)(sendLen - 3);
                    myHdr = myL10.myHdr;
                    break;
                #endregion
                #region Send LX30 L30
                case Telegram.L30:
                    if (isFLEX)
                    {
                        Telegram.myLX30 myLX30 = new Telegram.myLX30();
                        foreach (byte b in part_id)
                        {
                            myLX30.myHdr = myHdr;
                            uTime = UnixTime();
                            myLX30.myHdr.epoch_s = (int)(uTime / 1000000000);
                            myLX30.myHdr.ms = (ushort)(uTime / 1000000 % 1000);
                            myLX30.myHdr.msgType = 230;
                            myLX30.mySubHdr = mySubHdr;
                            myLX30.mySubHdr.msgTimeNs = (uint)(uTime % 1000000);
                            myLX30.statusCode = 0;
                            myLX30.part_id = b;
                            myLX30.end_out_bound_num = msg_cur_bound_num;
                            //myLX30.end_out_bound_num = b; for test
                            myLX30.chksum = 0;
                            sendBuff = myLX30.GetBytes();
                            sendLen = (ushort)sendBuff.Length;
                            myLX30.myHdr.msgLen = (ushort)(sendLen - 3);
                            sendBuff[0] = (byte)(myLX30.myHdr.msgLen / 256);
                            sendBuff[1] = (byte)(myLX30.myHdr.msgLen - sendBuff[0] * 256);
                            sendBuff[sendLen - 1] = Telegram.GenChkSum(sendBuff, sendLen - 1);
                            myHdr = myLX30.myHdr;
                            WriteMessage("送出電文:LX30");
                            SendMessage(sendBuff, sendLen);
                        }
                    }
                    Telegram.myL30 myL30 = new Telegram.myL30();
                    myL30.myHdr = myHdr;
                    myL30.myHdr.msgType = 30;
                    myL30.statusCode = 0;
                    myL30.append_no = 312;
                    myL30.end_out_bound_num = msg_cur_bound_num;
                    myL30.systemType = 20;
                    myL30.encryptMethod = 0;
                    myL30.chksum = 0;
                    sendBuff = myL30.GetBytes();
                    sendLen = (ushort)sendBuff.Length;
                    myL30.myHdr.msgLen = (ushort)(sendLen - 3);
                    myHdr = myL30.myHdr;
                    break;
                #endregion
                #region Send L50
                case Telegram.L50:
                    Telegram.myL50 myL50 = new Telegram.myL50();
                    myL50.myHdr = myHdr;
                    myL50.myHdr.msgType = 50;
                    myL50.statusCode = 0;
                    myL50.heartBtInt = 30;
                    myL50.max_flow_ctrl_cnt = 16;
                    myL50.chksum = 0;
                    sendBuff = myL50.GetBytes();
                    sendLen = (ushort)sendBuff.Length;
                    myL50.myHdr.msgLen = (ushort)(sendLen - 3);
                    myHdr = myL50.myHdr;
                    break;
                #endregion
                #region Send R04
                case Telegram.R04:
                    Telegram.myR04 myR04 = new Telegram.myR04();
                    myR04.myHdr = myHdr;
                    myR04.myHdr.msgType = 104;
                    myR04.statusCode = 0;
                    sendBuff = myR04.GetBytes();
                    sendLen = (ushort)sendBuff.Length;
                    myR04.myHdr.msgLen = (ushort)(sendLen - 3);
                    myHdr = myR04.myHdr;
                    break;
                #endregion
                #region Send R05
                case Telegram.R05:
                    Telegram.myR05 myR05 = new Telegram.myR05();
                    myR05.myHdr = myHdr;
                    myR05.myHdr.msgType = 105;
                    myR05.statusCode = 0;
                    sendBuff = myR05.GetBytes();
                    sendLen = (ushort)sendBuff.Length;
                    myR05.myHdr.msgLen = (ushort)(sendLen - 3);
                    myHdr = myR05.myHdr;
                    break;
                #endregion
                #region Send RX08
                case Telegram.RX08:
                    if (isFLEX)
                    {
                        xCtrl.InitXML();
                        XElement xe = xCtrl.GetTelegramX("RX08");
                        Telegram.myRX08 myRX08 = new Telegram.myRX08();
                        myRX08.myHdr = myHdr;
                        myRX08.myHdr.msgType = 208;
                        myRX08.mySubHdr = mySubHdr;
                        myRX08.mySubHdr.msgTypeExt = byte.Parse(xe.Element("msgTypeExt").Value);
                        myRX08.part_id = byte.Parse(xe.Element("part_id").Value);
                        myRX08.statusCode = Convert.ToUInt16(xe.Element("statusCode").Value);
                        Array.Copy(xe.Element("order_no").Value.ToCharArray(), 0, myRX08.order_no, 0, 5);
                        myRX08.ord_id = Convert.ToUInt32(xe.Element("ord_id").Value);
                        myRX08.fcm_id = Convert.ToUInt16(xe.Element("fcm_id").Value);
                        if (myRX08.mySubHdr.msgTypeExt == 1)
                            myRX08.pseq = Convert.ToUInt32(xe.Element("pseq").Value);
                        else
                            Array.Copy(xe.Element("prod_id").Value.ToCharArray(), 0, myRX08.prod_id, 0, xe.Element("prod_id").Value.Length);
                        myRX08.chksum = 0;
                        sendBuff = myRX08.GetBytes();
                        sendLen = (ushort)sendBuff.Length;
                        myRX08.myHdr.msgLen = (ushort)(sendLen - 3);
                        myHdr = myRX08.myHdr;
                    }
                    break;
                #endregion
                #region
                case Telegram.R14:
                    xCtrl.InitXML();
                    XElement xz = xCtrl.GetTelegramX("R14");
                    Telegram.myR14 myR14 = new Telegram.myR14();
                    myR14.myHdr = myHdr;
                    myR14.myHdr.msgType = 114;
                    myR14.statusCode = byte.Parse(xz.Element("statusCode").Value);
                    myR14.fcm_req_id = Convert.ToUInt32(xz.Element("fcm_req_id").Value);
                    myR14.bulletin_seq = Convert.ToUInt32(xz.Element("bulletin_seq").Value);
                    myR14.epoch_s = (uint)myR14.myHdr.epoch_s;
                    myR14.ms = myR14.myHdr.ms;
                    myR14.system_type = byte.Parse(xz.Element("system_type").Value);
                    myR14.data = xz.Element("data").Value;
                    myR14.chksum = 0;
                    sendBuff = myR14.GetBytes();
                    sendLen = (ushort)sendBuff.Length;
                    myR14.myHdr.msgLen = (ushort)(sendLen - 3);
                    myHdr = myR14.myHdr;
                    break;
                #endregion
                #region Send RX14
                case Telegram.RX14:
                    if (isFLEX)
                    {
                        xCtrl.InitXML();
                        XElement xe = xCtrl.GetTelegramX("RX14");
                        Telegram.myRX14 myRX14 = new Telegram.myRX14();
                        myRX14.myHdr = myHdr;
                        myRX14.myHdr.msgType = 214;
                        myRX14.mySubHdr = mySubHdr;
                        if (obj == null)
                            myRX14.mySubHdr.msgTypeExt = byte.Parse(xe.Element("msgTypeExt").Value);
                        else
                            myRX14.mySubHdr.msgTypeExt = (byte)obj.GetType().GetProperty("mySubHdr").GetType().GetProperty("msgTypeExt").GetValue(obj, null);
                        myRX14.statusCode = Convert.ToUInt16(xe.Element("statusCode").Value);
                        myRX14.fcm_req_id = Convert.ToUInt32(xe.Element("fcm_req_id").Value);
                        myRX14.bulletin_seq = Convert.ToUInt32(xe.Element("bulletin_seq").Value);
                        myRX14.epoch_s = (uint)myRX14.myHdr.epoch_s;
                        myRX14.nanosecond = myRX14.mySubHdr.msgTimeNs;
                        myRX14.system_type = byte.Parse(xe.Element("system_type").Value);
                        myRX14.file_code = xe.Element("data").Element("file_code").Value.ToCharArray();
                        myRX14.is_pub = xe.Element("data").Element("is_pub").Value[0];
                        Array.Copy(xe.Element("data").Element("file_path").Value.ToCharArray(),
                                   myRX14.file_path, xe.Element("data").Element("file_path").Value.Length);
                        myRX14.else_data = xe.Element("data").Element("else_data").Value;
                        myRX14.chksum = 0;
                        sendBuff = myRX14.GetBytes();
                        sendLen = (ushort)sendBuff.Length;
                        myRX14.myHdr.msgLen = (ushort)(sendLen - 3);
                        myHdr = myRX14.myHdr;
                    }
                    break;
                #endregion
                #region Send RX20
                case Telegram.RX20:
                    if (isFLEX)
                    {
                        xCtrl.InitXML();
                        XElement xe =  xCtrl.GetTelegramX("RX20");
                        Telegram.myRX20 myRX20 = new Telegram.myRX20();
                        myRX20.myHdr = myHdr;
                        myRX20.myHdr.msgType = 220;
                        myRX20.mySubHdr = mySubHdr;
                        myRX20.mySubHdr.msgTypeExt = byte.Parse(xe.Element("msgTypeExt").Value);
                        if (obj == null)
                            myRX20.securityRequestType = xe.Element("securityRequestType").Value[0];
                        else
                            myRX20.securityRequestType = (char)obj.GetType().GetProperty("securityRequestType").GetValue(obj, null);
                        myRX20.securityReqId = Convert.ToUInt32(xe.Element("securityReqId").Value);
                        myRX20.chksum = 0;
                        sendBuff = myRX20.GetBytes();
                        sendLen = (ushort)sendBuff.Length;
                        myRX20.myHdr.msgLen = (ushort)(sendLen - 3);
                        myHdr = myRX20.myHdr;
                    }
                    break;
                #endregion
            }
            sendBuff[0] = (byte)(myHdr.msgLen / 256);
            sendBuff[1] = (byte)(myHdr.msgLen - sendBuff[0] * 256);
            sendBuff[sendLen - 1] = Telegram.GenChkSum(sendBuff, sendLen - 1);

            string s = myHdr.msgType == 230 ? "LX" : (myHdr.msgType > 200 ? "RX" : (myHdr.msgType > 100 ? "R" : (myHdr.msgType > 0 ? "L" : "")));
            WriteMessage(String.Format("送出電文:" + s + "{0}!", (myHdr.msgType % 100).ToString("D2")));
            SendMessage(sendBuff, sendLen);
            return;
        }
    }
}
