using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Test.Networking
{
    public class TestNetTcp : MonoBehaviour
    {
        private const int Port = 8080;
        private IPEndPoint localAddress = new IPEndPoint(IPAddress.Parse(""), Port);

        private TcpListener server = null;

        // Start is called before the first frame update
        void Start()
        {
            server = new TcpListener(localAddress);
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
            }
        }
    }   
}