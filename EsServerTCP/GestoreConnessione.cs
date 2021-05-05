using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EsServerTCP
{
   
    class GestoreConnessione
    {
        public static IPAddress localhost { get; set; }

        //Incoming data from the client
        public static string data = null;


        
        static GestoreConnessione()
        {

        }

        const int LOCAL_PORT = 50000;
        const int REMOTE_PORT = 50001;

        const string REMOTE_IP = "10.73.0.4";
        static bool connesso = false;
        public static void StartServer()
        {
            //Data buffer for incoming data
            byte[] bytes = new byte[1400];
            string answer;


            IPAddress ipAddress = IPAddress.Parse("10.73.0.3");
            
            //Create a TCP socket
            

            //Bind the socket to the local endpoint and listen for incoming connections
            try
            {
                loadAnimation();
                TcpListener server = new TcpListener(ipAddress, LOCAL_PORT);
                server.Start();
                TcpClient client = new TcpClient();
                
               
                    bool tmp = true;
                    while(tmp)
                    {
                        if(server.Pending() == true)
                        {
                            client = server.AcceptTcpClient();
                            tmp = false;
                            connesso = true;
                        }
                        
                    }



                    Thread.Sleep(2200);
                    Console.WriteLine("Connesso");

                    NetworkStream stream = client.GetStream();
                    stream.Read(bytes,0,1400);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("\nPress ENTER to continue... ");
            Console.Read();
        }

        private async static void loadAnimation()
        {
            await Task.Run(() => {
                bool tmp = true;
                while(tmp)
                {
                    
                    Console.WriteLine("SERVER: waiting for a connection... \\ ");                  
                    Thread.Sleep(500);
                    Console.Clear();
                    Console.WriteLine("SERVER: waiting for a connection... | ");                   
                    Thread.Sleep(500);
                    Console.Clear();
                    Console.WriteLine("SERVER: waiting for a connection... / ");                   
                    Thread.Sleep(500);
                    Console.Clear();
                    Console.WriteLine("SERVER: waiting for a connection... | ");                   
                    Thread.Sleep(500);
                    Console.Clear();
                    if (connesso == true)
                        tmp = false;
                }
            });
        }

        public static void StartClient()
        {
            // Data buffer for incoming data
            byte[] bytes = new byte[1400];
            //Connect to a remote device
            try
            {
                //Establish the remote endpoint for the socket
                

                IPAddress ipAddress = IPAddress.Parse(REMOTE_IP);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, REMOTE_PORT);
                //Create a TCP socket
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //Connect the socket to the remote endpoint. Catch any errors
                try
                {
                    sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                    //Encode the data string into a byte array
                    byte[] msg = Encoding.ASCII.GetBytes("Tell me the date <EOF> ");
                    //Send the data through the socket
                    int bytesSend = sender.Send(msg);
                    Console.WriteLine("1. REQUEST SENT");
                    //Receive the response from the remote device
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine();
                    Console.WriteLine("4. ANSWER RECEIVED = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    //Release the socket
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpcted exception : {0}", e.ToString());

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
       
    
}
