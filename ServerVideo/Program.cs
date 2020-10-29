using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ServerVideo
{
    class Program
    {
        static void Main(string[] args)
        {

            int udpSize = 65507;    // 65535 - 8 udpHeader - 20 ipHeader
            byte[] data = new byte[udpSize];    //buffer

            IPEndPoint ipep = new IPEndPoint(IPAddress.Loopback, 7777);
            UdpClient newsock = new UdpClient(ipep);
            IPEndPoint clientAddress = new IPEndPoint(IPAddress.Any, 0);

            data = newsock.Receive(ref clientAddress);
            //Console.WriteLine("client connected!" + sender.ToString());

            // "C:\\Users\\HARF\\Desktop\\test.mp4"
            string fileName = "C:\\Users\\urkmez\\Desktop\\IMG_3257.MOV";
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            long numBytes = new FileInfo(fileName).Length;
            long okBytes = 0;
            //reads file one udp frame size by one
            while(okBytes <= numBytes)
            {
                data = br.ReadBytes(udpSize);
          
                newsock.Send(data, data.Length, clientAddress);
                okBytes += udpSize;
            }

            //data[0] = Convert.ToByte('Z');
            //newsock.Send(data, 0, clientAddress);


            br.Close();
            fs.Close();
            newsock.Close();

            Console.WriteLine(okBytes + " " + numBytes);
            Console.ReadKey();
        }

        
    }
}