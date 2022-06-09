using Basics.Commands;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Basics.Viewmodels
{
    public class AddContacsViewModel : BaseViewModel
    {
        private string ipField = SetIpField();
        private bool _GroupEnabled;
        private bool _IpEnabled;
        private string _GroupName;
        private bool addButtonEnabled;
        private bool createGroupButtonEnabled;

        public event EventHandler AddNewChatroom;
        public DelegateCommand RadioCheckedCommand { get; set; }

        public DelegateCommand Addcommand { get; set; }

        public DelegateCommand Createcommand { get; set; }

        public bool IpEnabled
        {
            get { return _IpEnabled; }
            set
            {
                if (value != _IpEnabled)
                {
                    _IpEnabled = value;
                    RaisePropertyChanged();
                    AddButtonEnabled = !String.IsNullOrEmpty(IpField) && IpEnabled && IpField.Split('.')[3] != "0" && IpField.Split('.')[3] != "255";
                }
            }
        }

        public string IpField
        {
            get { return ipField; }
            set
            {
                ipField = value;
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

        public bool GroupEnabled
        {
            get { return _GroupEnabled; }
            set
            {
                if (value != _GroupEnabled)
                {
                    _GroupEnabled = value;
                    RaisePropertyChanged();
                    CreateGroupButtonEnabled = !String.IsNullOrEmpty(GroupName) && GroupEnabled;
                }
                //RaisePropertyChanged();
            }
        }

        public string GroupName
        {
            get { return _GroupName; }
            set
            {
                if (value != _GroupName)
                {
                    _GroupName = value;
                    this.RaisePropertyChanged();
                    CreateGroupButtonEnabled = !String.IsNullOrEmpty(GroupName);
                }
            }
        }

        public bool CreateGroupButtonEnabled
        {
            get { return createGroupButtonEnabled; }
            set
            {
                createGroupButtonEnabled = value;
                this.RaisePropertyChanged();
            }
        }


        public AddContacsViewModel()
        {
            IpEnabled = true;
            GroupEnabled = false;
            this.RadioCheckedCommand = new DelegateCommand((o) =>
            {
                string selected = (string)o;
                if (selected == "IP")
                {
                    GroupEnabled = false;
                    IpEnabled = true;
                }
                else
                {
                    GroupEnabled = true;
                    IpEnabled = false;
                };
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