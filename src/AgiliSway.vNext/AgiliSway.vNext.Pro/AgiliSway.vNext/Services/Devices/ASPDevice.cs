using AgiliSway.vNext.Models;
using AgiliSway.vNext.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiliSway
{
    public class ASPDevice : IDevice
    {
		public ASPDevice()
        {}

		public string PortName { get; set; }

        public void LoadDigitalCalibration()
        {
            using (StreamReader sr = new StreamReader("C:\\Users\\Brent\\Desktop\\ASP\\Digital0983_1.acl"))
            {
                var line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    if (line.StartsWith("COEFFROW"))
                    {
                        var itms = line.Split(':');
                        var lbl = itms[0];
                        var vals = itms[1].Split(',');

                        var dVals = new List<double>();
                        foreach (var val in vals)
                            dVals.Add(double.Parse(val));

                        cal.Add(dVals);
                    }

                    line = sr.ReadLine();
                }
            }
        }

        public void Connect()
        {
			//_port = new SerialPort(PortName);
			//_port.DataBits = 8;
			//_port.StopBits = StopBits.One;
			//_port.Handshake = Handshake.None;
			//_port.BaudRate = 57600;
			//_port.ReadBufferSize = 1024 * 1000;

			//_port.ErrorReceived += new SerialErrorReceivedEventHandler(port_ErrorReceived);

			//_port.Open();
        }

        public void Start()
        {
            _port.Write(new byte[] { 0x51 }, 0, 1);

            int resp = _port.ReadByte(); // Ack
            if (resp != 0x55)
                throw new Exception("Error starting data transmission: " + resp.ToString());

            _port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        public void SetDataRate(DataRate rate)
        {
            switch (rate)
            {
                case DataRate.Hz50B57600:
                    {
                        _port.Write(new byte[] { 0x72 }, 0, 1); // Set baud and data rate to 57.6 and 50hz
                        int resp = _port.ReadByte();
                        if (resp != 0x72)
                            throw new Exception("Error setting data rate: " + resp.ToString());
                        break;
                    }
                case DataRate.Hz100B115200:
                    {
                        _port.Write(new byte[] { 0x66 }, 0, 1); // Set baud and data rate to 115.2 and 100hz
                        int resp = _port.ReadByte();
                        if (resp != 0x66)
                            throw new Exception("Error setting data rate: " + resp.ToString());
                        break;
                    }
            }
        }

        public string GetSerialNumber()
        {
            _port.Write(new byte[] { 0x58 }, 0, 1);

            int resp = _port.ReadByte();
            if (resp != 0x78)
                throw new Exception("Error getting serialnumber: " + resp.ToString());

            byte[] buf = readFromPort(_port, 4);
            return Encoding.ASCII.GetString(buf, 0, 4);
        }

        public byte[] Autozero()
        {
            _port.Write(new byte[] { 0x53 }, 0, 1);

            int resp = _port.ReadByte();
            if (resp != 0x54)
                throw new Exception("Error with autozero: " + resp.ToString());

            byte[] buf = readFromPort(_port, 12);

            return buf;
        }

        public void Stop()
        {
            _port.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
            _port.Write(new byte[] { 0x52 }, 0, 1);  // Does not ack
        }

        public void Disconnect()
        {
            try
            {
                if (_port != null && _port.IsOpen)
                    _port.Close();
            }
            catch (Exception)
            { }
        }

        public object o = new object();
        public byte[] nbuf = new byte[128];
        public Queue<byte> frameBuf = new Queue<byte>(24);
        public Queue<AccuSwayPlusMeasurement> Tare = new Queue<AccuSwayPlusMeasurement>();
        
        private byte[] readFromPort(SerialPort port, int cbToRead)
        {
            int nRead = 0;
            byte[] buf = new byte[cbToRead];
            byte[] tmpBuf = new byte[cbToRead];

            while (nRead < cbToRead)
            {
                tmpBuf.Initialize();

                int n = port.Read(tmpBuf, 0, cbToRead - nRead);

                Array.Copy(tmpBuf, 0, buf, nRead, n);

                nRead += n;
            }

            return buf;
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (o)
                {
                    int nRead = _port.Read(nbuf, 0, 128);

                    while (nRead > 0)
                    {
                        for (int i = 0; i < nRead; i++)
                        {
                            byte b = nbuf[i];

                            if (frameBuf.Count == 24)
                                frameBuf.Dequeue();

                            frameBuf.Enqueue(b);

                            if (frameBuf.Count == 24)
                            {
                                var lst = new List<int>();
                                int idx = 0;
                                foreach (var itm in frameBuf)
                                {
                                    if (idx % 2 != 0)
                                    {
                                        var msk = itm & 0xF0;
                                        var ch = msk >> 4;

                                        lst.Add(ch);
                                    }

                                    idx++;
                                }

                                bool bGood = true;
                                for (int j = 0; j < lst.Count; j++)
                                {
                                    if (lst[j] != j)
                                        bGood = false;
                                }

                                if (bGood)
                                {
                                    List<int> finalVals = new List<int>();
                                    int datum = 0;
                                    foreach (var itm in frameBuf)
                                    {
                                        if (idx % 2 != 0)
                                        {
                                            var val = itm & 0x0F;
                                            var vals = val << 8;

                                            var msk = itm & 0xF0;
                                            var ch = msk >> 4;

                                            datum |= val;
                                            finalVals.Add(datum - 2048);
                                        }
                                        else
                                            datum = itm;

                                        idx++;
                                    }

                                    AccuSwayPlusMeasurement meas = new AccuSwayPlusMeasurement()
                                    {
                                        Ax = finalVals[0],
                                        Ay = finalVals[1],
                                        Az = finalVals[2],
                                        Bx = finalVals[3],
                                        By = finalVals[4],
                                        Bz = finalVals[5],
                                        Cx = finalVals[6],
                                        Cy = finalVals[7],
                                        Cz = finalVals[8],
                                        Dx = finalVals[9],
                                        Dy = finalVals[10],
                                        Dz = finalVals[11]
                                    };

                                    var fx = meas.Calc(cal[0]);
                                    var fy = meas.Calc(cal[1]);
                                    var fz = meas.Calc(cal[2]);
                                    var mx = meas.Calc(cal[3]);
                                    var my = meas.Calc(cal[4]);
                                    var mz = meas.Calc(cal[5]);

                                    StringBuilder sbValues = new StringBuilder();

                                    //Console.WriteLine("Meas: " + meas.ToString());

                                    var dz = 41.3;
                                    var x = -1 * (my + fx * dz) / fz;
                                    var y = (mx - fy * dz) / fz;

                                    Console.WriteLine(string.Format("x {6} y {7} Fx {0} Fy {1} Fz {2} Mx {3} My {4} Mz {5}", fx, fy, fz, mx, my, mz, x, y));
                                }
                            }
                        }

                        nRead = _port.Read(nbuf, 0, 128);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new Exception("Error received on port: " + e.EventType.ToString());
        }

        private SerialPort _port;
        List<List<double>> cal = new List<List<double>>();


		public vNext.Models.CollectionDataPoint GetDataPoint()
		{
			return new vNext.Models.CollectionDataPoint();
		}

		public DeviceState DeviceState
		{
			get
			{
				return DeviceState.Disconnected;
			}
			set { }
		}

        public Task<vNext.Models.CollectionDataSet> Collect(TimeSpan time, IProgress<vNext.Models.CollectProgress> progress, System.Threading.CancellationToken ct, CollectionDataPoint calibration)
		{
            return null;
		}
	}

    public enum DataRate
    {
        Hz50B57600,
        Hz100B115200
    }

    public enum PortState
    {
        None,
        SetCommSpeed,
        Autozero,
        DataTransmission
    }

    public class AccuSwayPlusMeasurement
    {
        public long Ticks;
        public int Ax;
        public int Ay;
        public int Az;
        public int Bx;
        public int By;
        public int Bz;
        public int Cx;
        public int Cy;
        public int Cz;
        public int Dx;
        public int Dy;
        public int Dz;

        public override string ToString()
        {
            return string.Format(
                "Ticks {0} Ax {1} Ay {2} Az {3}",
                Ticks,
                Ax,
                Ay,
                Az
                );
        }

        public double Calc(List<double> cal)
        {
            var val = (Ax * cal[0]) + (Ay * cal[1]) + (Az * cal[2]);
            val += (Bx * cal[3]) + (By * cal[4]) + (Bz * cal[5]);
            val += (Cx * cal[6]) + (Cy * cal[7]) + (Cz * cal[8]);
            val += (Dx * cal[9]) + (Dy * cal[10]) + (Dz * cal[11]);

            return val;
        }
    }
}
