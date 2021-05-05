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
        static GestoreConnessione gestore;
        public static void Main(string[] args)
        {
            gestore = new GestoreConnessione();
            GestoreConnessione.StartServer();
           
        }
        
    }
    
}
