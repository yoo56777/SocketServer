namespace TaifexEmulator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    class XMLControll
    {
        string XMLpath = "TelegramXML.xml";
        static XDocument xdoc;

        public void InitXML()
        {
            try
            {
                xdoc = XDocument.Load(XMLpath);
            }
            catch (Exception ex)
            {
                Receive.WriteMessage(ex.Message);
                Receive.WriteMessage("載入XML失敗，請確認!");
            }
        }

        public XElement GetTelegramX(string type)
        {
            XElement xe = xdoc.Root.Element(type);
            return xe;
        }
    }
}
