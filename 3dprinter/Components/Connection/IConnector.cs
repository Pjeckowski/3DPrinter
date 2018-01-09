using System;

namespace _3dprinter.Components.Connection
{
    public interface IConnector
    {
        bool Send(string data);
        string Receive();
        void Close();
        event EventHandler<string> DataReceivedEvent;
    }
}
