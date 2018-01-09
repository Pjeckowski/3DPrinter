using System;
using System.IO.Ports;

namespace _3dprinter.Components.Connection
{
    public class RS232 : IConnector
    {
        string data;
        private SerialPort port;

        public event EventHandler<string> DataReceivedEvent;

        public bool PortOpen()
        {
            String[] portsAvailable = SerialPort.GetPortNames();

            if (portsAvailable.Length != 0)
            {
                port = new SerialPort()
                {
                    PortName = portsAvailable[0],
                    BaudRate = 57600,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One
                };
                port.RtsEnable = true;
                port.DataReceived += ReceiverHandler;
                port.Open();

                data = "";
                return true;
            }
            return false;
        }

        public bool PortOpen(SerialPort port)
        {
            this.port = port;
            this.port.RtsEnable = true;
            this.port.DataReceived += ReceiverHandler;

            //try
            //{
                this.port.Open();
                data = "";
                return true;
            //}
            /*catch (Exception)
            {

                if (this.port.IsOpen)
                {
                    return true;
                }
                return false;
            }  */       
        }

        void ReceiverHandler(object sender,SerialDataReceivedEventArgs e)
        {
            data += port.ReadExisting();
            
            if (data.Contains("\r"))
            {
                if (DataReceivedEvent != null) DataReceivedEvent(this, data);
                data = "";
            }
        }

        public bool Send(string data)
        {
            try
            {
                port.Write(data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Receive()
        {
            return port.ReadExisting();
        }

         public void Close()
        {
            port.Dispose();
            port.Close();
        }
        
        public bool Isopen()
        {
            return port.IsOpen;
        }

    }
}
