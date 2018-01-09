using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;
using _3dprinter.Components.Connection;
using _3dprinter.Components.Coords;
using _3dprinter.Windows;

namespace _3dprinter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public enum PrinterOperationState
    {
        Idle,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Circle,
        Spiral
    }

    public partial class MainWindow : Window
    {
        private IConnector Connection;
        private Connection_Window ConnectionWindow;
        
        delegate void DataReceivedDel(string data);

        private DataReceivedDel DataReceivedDelegate;
        private object DataReceivedObject;
        private Queue<Coords> CoordsQueue = new Queue<Coords>();

        private PrinterOperationState printerOperationState = PrinterOperationState.Idle;

        public MainWindow()
        {
            InitializeComponent();
            DataReceivedDelegate = FillDataReceivedTextBox;
            ConnectionWindow = new Connection_Window();
            ConnectionWindow.Show();
        }

        private void ConnectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ConnectionWindow = new Connection_Window();
            ConnectionWindow.Show();
            ConnectionWindow.PortOpen += HandlePortOpen;
        }

        private void HandlePortOpen(SerialPort port)
        {
            var conn = new RS232();
            conn.PortOpen(port);
            Connection = conn;
            Connection.DataReceivedEvent += HandleConnectionDataReceived;
        }

        private void HandleConnectionDataReceived(object sender, string e)
        {
            FillDataReceivedTextBox(e);
            SendResponse(e);
        }

        private void SendResponse(string data)
        {
            if (CoordsQueue.Count != 0)
            {
                if (data.Contains("PPR"))
                {
                    Coords Coords = CoordsQueue.Dequeue();
                    Connection.Send("PX" + Coords.X + "\r");
                    Connection.Send("PY" + Coords.Y + "\r");
                    Connection.Send("PZ" + Coords.Z + "\r");
                }
            }
        }

        void FillDataReceivedTextBox(string data)
        {
            if (Dispatcher.CheckAccess())
            {
               // DataReceivedTextBox.Text = data;
            }
            else
            {
                DataReceivedObject = data;
                Dispatcher.Invoke(DataReceivedDelegate, DataReceivedObject);
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            CoordsCalc CC = new CoordsCalc();
            CoordsQueue = CC.GetCircle();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (null != Connection)
            {
                Connection.Close();
            }
            Close();
        }

    }
}
