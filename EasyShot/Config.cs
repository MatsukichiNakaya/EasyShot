using System;
using System.Xml.Serialization;

namespace EasyShot
{
    [Serializable]
    public class Config
    {
        [XmlElement(ElementName = "SaveDir")]
        public String SaveDir { get; set; }

        [XmlElement(ElementName = "PrintKeyCode")]
        public Int32 PrintKeyCode { get; set; }

        [XmlElement(ElementName = "BootMode")]
        public Int32 BootMode { get; set; }

        [XmlElement(ElementName = "Effect")]
        public Int32 Effect { get; set; }
    }
}
