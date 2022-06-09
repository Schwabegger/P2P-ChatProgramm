using Basics.Commands;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Basics.Viewmodels
{
    public class AddUserToGroupChatViewModel : BaseViewModel
    {
        private string ip = SetIpField();
        private bool addButtonEnabled;

        public DelegateCommand AddUserCommand { get; set; }

        public string IpField
        {
            get { return ip; }
            set
            {
                ip = value;
                RaisePropertyChanged();
                AddButtonEnabled = !String.IsNullOrEmpty(IpField) && IpField.Split('.')[3] != "0" && IpField.Split('.')[3] != "255";
            }
        }


        public bool AddButtonEnabled
        {
            get { return addButtonEnabled; }
            set
            {
                addButtonEnabled = value;
                RaisePropertyChanged();
            }
        }


        public AddUserToGroupChatViewModel()
        {
            AddUserCommand = new DelegateCommand(
                _ =>
                {
                    return IpField.Split('.')[3] != "0" && IpField.Split('.')[3] != "255";
                },
                _ =>
                {

                });
        }

        static string SetIpField()
        {
            string ip = GetIpAddressFromHost();
            if (ip != null)
            {
                string mask = GetSubnetMask(ip);
                if (mask != null)
                {
                    string setIp = "";
                    string[] ipParts = ip.Split('.');
                    string[] maskParts = mask.Split('.');
                    for (int i = 0; i < maskParts.Length; i++)
                    {
                        if (maskParts[i] == "255")
                        {
                            setIp += ipParts[i] + ".";
                        }
                        else
                        {
                            for (int c = i; c < maskParts.Length; c++)
                                setIp += "0.";
                            return setIp.Substring(0, setIp.Length - 1);
                        }
                    }
                }
            }
            return "0.0.0.0";
        }

        private static string GetIpAddressFromHost()
        {
            string hostname = Dns.GetHostName();
            //Get the Ip
            try
            {
                return Dns.GetHostByName(hostname).AddressList[1].ToString();
            }
            catch
            {
                try
                {
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                    {
                        socket.Connect("8.8.8.8", 65530);
                        IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
                        return endPoint?.Address.ToString();
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        static string GetSubnetMask(string ip)
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface Interface in Interfaces)
            {
                if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                Console.WriteLine(Interface.Description);
                UnicastIPAddressInformationCollection UnicastIPInfoCol = Interface.GetIPProperties().UnicastAddresses;
                foreach (UnicastIPAddressInformation UnicatIPInfo in UnicastIPInfoCol)
                {
                    if (UnicatIPInfo.Address.ToString() == ip)
                    {
                        return UnicatIPInfo.IPv4Mask.ToString();
                    }
                }
            }
            return null;
        }

    }

}
