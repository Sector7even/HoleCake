

using System.IO;

using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Drawing;

using System.Net.Sockets;

using System.Linq;

using System.Threading;

using System.Net;

using System.Text;

using System.Threading.Tasks;

using System.Windows.Forms;


         

namespace HoleCake_Client
{
    public partial class Form1 : Form
    {
        Thread Verbindungcheck;

        Thread ShellClient;

        TcpListener TCPL;
    
        Socket SS;

        StringBuilder SIT;

        NetworkStream NS;
     
        StreamWriter SW;
    
        StreamReader SR;
    


        public Form1()
      
        {
           
            InitializeComponent();
      
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
          
            Verbindungcheck = new Thread(new ThreadStart(StartListen));
          
            Verbindungcheck.Start();          
        }

        private void StartListen()
        {
            TCPL = new TcpListener(System.Net.IPAddress.Any, 7511);
          
            TCPL.Start();
         
            toolStripLabel1.Text = "Online auf Port:7511";
          
            for (; ; )
            {
                SS = TCPL.AcceptSocket();
              
                IPEndPoint ipend = (IPEndPoint)SS.RemoteEndPoint;
              
                toolStripLabel1.Text = "Verbunden mit Server..." + IPAddress.Parse(ipend.Address.ToString());
             
                ShellClient = new Thread(new ThreadStart(RunClient));
            
                ShellClient.Start();
            
            }
        }

        private void RunClient()
     
        {
          
            NS = new NetworkStream(SS);
          
            SR = new StreamReader(NS);
         
            SW = new StreamWriter(NS);


            SIT = new StringBuilder();

            while (true)
            {
                try
             
                {
                    SIT.Append(SR.ReadLine());
                  
                    SIT.Append("\r\n");
                }
                catch (Exception err)
             
                {
                    ausmisten();
              
                    break;
                }
              
                Application.DoEvents();
               
                DisplayMessage(SIT.ToString());
                
                SIT.Remove(0, SIT.Length);
            }

        }

        private void ausmisten()
      
        {
            try
          
            {
            
                SR.Close();
          
                SW.Close();
          
                NS.Close();
          
                SS.Close();
         
            }
         
            catch (Exception err) { }
        
            toolStripLabel1.Text = "Verbindungsabbruch...";
            
        }

        private delegate void DisplayDelegate(string message);

        private void DisplayMessage(string message)
     
        {
         
            if (textBox1.InvokeRequired)
         
            {
                Invoke(new DisplayDelegate(DisplayMessage), new object[] { message });
            }
       
            else
          
            {
                textBox1.AppendText(message);
         
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            Verbindungcheck = new Thread(new ThreadStart(StartListen));

            Verbindungcheck.Start();
            
        }

        private void button2_Click(object sender, EventArgs e)
      
        {
            progressBar1.Value = 10;
         
            SIT.Append(textBox2.Text.ToString());
            
            progressBar1.Value = 25;
            
            SW.WriteLine(SIT);
            
            progressBar1.Value = 35;
            
            SW.Flush();
            
            progressBar1.Value = 45;
            
            SIT.Remove(0, SIT.Length);
            
            progressBar1.Value = 50;
            
            if (textBox2.Text == "reset") ausmisten();
            
            progressBar1.Value = 65;
            
            if (textBox2.Text == "kill") ausmisten();
            
            progressBar1.Value = 75;
            
            if (textBox2.Text == "cls") textBox1.Text = "";
            
            progressBar1.Value = 85;
            
            textBox2.Text = "";
            
            progressBar1.Value = 100;

                }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            System.Environment.Exit(System.Environment.ExitCode);
        }
    }
}
