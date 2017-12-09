using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Com.Yst.Framework.Model;

namespace Com.Yst.Framework.Utils
{
    public class ServiceConfigHelper
    {
        public static BasicHttpBinding GetBasicHttpBinding( WcfServiceInfo config)
        {
            BasicHttpBinding result = new BasicHttpBinding();
            result.MaxReceivedMessageSize = config.MaxReceivedMessageSize;
            result.MaxBufferPoolSize = config.MaxReceivedMessageSize;
            result.ReaderQuotas.MaxStringContentLength = config.MaxArrayLength;
            result.ReaderQuotas.MaxArrayLength = config.MaxArrayLength;
            result.CloseTimeout = new TimeSpan(0, 0, config.TimeOut.CloseTimeout / 1000);
            result.OpenTimeout = new TimeSpan(0, 0, config.TimeOut.OpenTimeout / 1000);
            result.ReceiveTimeout = new TimeSpan(0, 0, config.TimeOut.ReceiveTimeout / 1000);
            result.SendTimeout = new TimeSpan(0, 0, config.TimeOut.SendTimeout / 1000);
            return result;
        }
        public static WSHttpBinding GetWSHttpBinding( WcfServiceInfo config)
        {
            WSHttpBinding result = new WSHttpBinding(SecurityMode.None);
            result.MaxReceivedMessageSize = config.MaxReceivedMessageSize;
            result.MaxBufferPoolSize = config.MaxReceivedMessageSize;
            result.ReaderQuotas.MaxArrayLength = config.MaxArrayLength;
            result.ReaderQuotas.MaxStringContentLength = config.MaxArrayLength;
            result.CloseTimeout = new TimeSpan(0, 0, config.TimeOut.CloseTimeout / 1000);
            result.OpenTimeout = new TimeSpan(0, 0, config.TimeOut.OpenTimeout / 1000);
            result.ReceiveTimeout = new TimeSpan(0, 0, config.TimeOut.ReceiveTimeout / 1000);
            result.SendTimeout = new TimeSpan(0, 0, config.TimeOut.SendTimeout / 1000);
            return result;
        }

        public static BasicHttpBinding GetBasicHttpBinding(WebCacheServerInfo config)
        {
            WcfServiceInfo info = new WcfServiceInfo();
            BasicHttpBinding result = new BasicHttpBinding();
            result.MaxReceivedMessageSize = info.MaxReceivedMessageSize;
            result.MaxBufferPoolSize = info.MaxReceivedMessageSize;
            result.ReaderQuotas.MaxArrayLength = info.MaxArrayLength;
            result.ReaderQuotas.MaxStringContentLength = info.MaxArrayLength;
            result.CloseTimeout = new TimeSpan(0, 0, config.CloseTimeout / 1000);
            result.OpenTimeout = new TimeSpan(0, 0, config.OpenTimeout / 1000);
            result.ReceiveTimeout = new TimeSpan(0, 0, config.ReceiveTimeout / 1000);
            result.SendTimeout = new TimeSpan(0, 0, config.SendTimeout / 1000);
            return result;
        }

        public static WSHttpBinding GetCacheHttpBinding(WebCacheServerInfo config)
        {
          
            WcfServiceInfo info = new WcfServiceInfo();
            WSHttpBinding result = new WSHttpBinding(SecurityMode.None);
            result.MaxReceivedMessageSize = info.MaxReceivedMessageSize;
            result.MaxBufferPoolSize = info.MaxReceivedMessageSize;
            result.ReaderQuotas.MaxArrayLength = info.MaxArrayLength;
            result.ReaderQuotas.MaxStringContentLength = info.MaxArrayLength;
            result.CloseTimeout = new TimeSpan(0, 0, config.CloseTimeout / 1000);
            result.OpenTimeout = new TimeSpan(0, 0, config.OpenTimeout / 1000);
            result.ReceiveTimeout = new TimeSpan(0, 0, config.ReceiveTimeout / 1000);
            result.SendTimeout = new TimeSpan(0, 0, config.SendTimeout / 1000);
            return result;
        }
    }
}
