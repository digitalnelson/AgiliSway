using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class WiimoteMeasurement
    {
        [XmlAttribute()]
        public String Id;
        [XmlAttribute()]
        public long Ticks;
        [XmlAttribute()]
        public float AccelX;
        [XmlAttribute()]
        public float AccelY;
        [XmlAttribute()]
        public float AccelZ;
        [XmlAttribute()]
        public float GyroX;
        [XmlAttribute()]
        public float GyroY;
        [XmlAttribute()]
        public float GyroZ;
    }

    public class WiimoteMeasurementGroup
    {
        public int StartIndex;
        public WiimoteMeasurement[] Measurements = new WiimoteMeasurement[0];
    }
}
