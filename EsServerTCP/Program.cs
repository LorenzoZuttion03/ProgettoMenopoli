using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsServerTCP
{
    class Program
    {
        public static int Main(string[] args)
        {
            SynchronousSocketServer.StartServer();
            return 0;
           
        }
        public static IPAddress localhost { get; set; }
    }
    
    public class SynchronousSocketServer
    {
        //Incoming data from the client
        public static string data = null;
        
        public static void StartServer()
        {
            //Data buffer for incoming data
            byte[] bytes = new byte[1024];
            string answer;
            //Establish the local endpoint for the socket
            //Dns.GetHostName returns the name of the
            //host running the application
            IPHostEntry ipHostInfo = Dns.Resolve(hostName: Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 2222);
            //Create a TCP/IP socket
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Bind the socket to the local endpoint and 
            //listen for incoming connections
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                //Start listening for connections
                while (true)
                {
                    Console.WriteLine("SERVER: waiting for a connection... ");
                    //Program is suspended while waiting for an incoming connection
                    Socket handler = listener.Accept();
                    data = null;
                    //An incoming connection needs to be processed
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if(data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    //Show the data on the console
                    Console.WriteLine("Text received : {0}", data);
                    Console.WriteLine("2. REQUEST RECEIVED");
                    Console.WriteLine();
                    answer = DateTime.Now.ToString("dd/MM/yyyy");
                    //Or DateTime.Now.ToString("h:mm:ss")

                    //Echo the data back to the client
                    byte[] msg = Encoding.ASCII.GetBytes(answer);
                    handler.Send(msg);
                    Console.WriteLine("3. ANSWER SENT");
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("\nPress ENTER to continue... ");
            Console.Read();
        }
    }
    public class SynchronousSocketClient
    {
        public static void StartClient()
        {
            // Data buffer for incoming data
            byte[] bytes = new byte[1024];
            //Connect to a remote device
            try
            {
                //Establish the remote endpoint for the socket
                //This example uses port 2222 on the local computer 
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 2222);
                //Create a TCP/IP socket
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //Connect the socket to the remote endpoint. Catch any errors
                try
                {
                    sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}" , sender.RemoteEndPoint.ToString());
                    //Encode the data string into a byte array
                    byte[] msg = Encoding.ASCII.GetBytes("Tell me the date <EOF> ");
                    //Send the data through the socket
                    int bytesSend = sender.Send(msg);
                    Console.WriteLine("1. REQUEST SENT");
                    //Receive the response from the remote device
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine();
                    Console.WriteLine("4. ANSWER RECEIVED = {0}" , Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    //Release the socket
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }catch(ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }catch(SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }catch(Exception e)
                {
                    Console.WriteLine("Unexpcted exception : {0}", e.ToString());

                }
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static int Main1(String[] args)
        {
            SynchronousSocketClient.StartClient();
            Console.WriteLine();
            return 0;
        }
    }
}
