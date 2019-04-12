using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Linq;

namespace MyWebApi.Core.Utility
{
    /// <summary>
    /// 获取服务器ip
    /// </summary>
  public static class IpExtensions
    {
        /// <summary>
        /// ipv4
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetLocalIPv4(NetworkInterfaceType type)
        {
            string ipresult = string.Empty;
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                     IPInterfaceProperties iPInterfaceProperties=  item.GetIPProperties();
                    if (iPInterfaceProperties.GatewayAddresses.FirstOrDefault() != null)
                    {
                        foreach (UnicastIPAddressInformation ip in iPInterfaceProperties.UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                ipresult = ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            return ipresult;
        }
    }
}
