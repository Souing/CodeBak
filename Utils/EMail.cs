using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// Smtp认证方式
    /// </summary>
    public enum EmailCredential { DefaultCredentials, PassWordCredentials };

    /// <summary>
    /// Email控制类
    /// </summary>
    public class EMail
    {
        /// <summary>
        /// 发送不需要验证的Email
        /// </summary>
        /// <param name="host">smtp地址</param>
        /// <param name="port">端口</param>
        /// <param name="mail">邮件体</param>
        public static void SendEmail(string host, int port, MailMessage mail)
        {
            SendEmail(host, port, "", "", mail, EmailCredential.DefaultCredentials);
        }

        /// <summary>
        /// 发送带用户和验证的Email
        /// </summary>
        /// <param name="host">smtp地址</param>
        /// <param name="port">端口</param>
        /// <param name="username">用户名 </param>
        /// <param name="password">密码</param>
        /// <param name="mail">邮件体</param>
        public static void SendEmail(string host, int port, string username, string password, MailMessage mail)
        {
            SendEmail(host, port, username, password, mail, EmailCredential.PassWordCredentials);
        }

        /// <summary>
        /// 发送不需要验证的Email
        /// </summary>
        /// <param name="host">smtp地址</param>
        /// <param name="port">端口</param>
        /// <param name="senderAddress">发信人地址</param>
        /// <param name="receiverAddress">收件人地址</param>
        /// <param name="emailSubject">邮件标题</param>
        /// <param name="emailBody">邮件内容</param>
        public static void SendEmail(string host, int port, string senderAddress, string receiverAddress, string emailSubject, string emailBody)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(senderAddress);
            mailMessage.To.Add(receiverAddress);
            mailMessage.Subject = emailSubject;
            mailMessage.Body = emailBody;
            mailMessage.Priority = MailPriority.Normal;
            SendEmail(host, port, mailMessage);
        }

        /// <summary>
        /// 发送带用户和验证的Email
        /// </summary>
        /// <param name="host">smtp地址</param>
        /// <param name="port">端口</param>
        /// <param name="username">用户名 </param>
        /// <param name="password">密码</param>
        /// <param name="senderAddress">发信人地址</param>
        /// <param name="receiverAddress">收件人地址</param>
        /// <param name="emailSubject">邮件标题</param>
        /// <param name="emailBody">邮件内容</param>
        public static void SendEmail(string host, int port, string username, string password, string senderAddress, string receiverAddress,
                                     string emailSubject, string emailBody)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(senderAddress);
            mailMessage.To.Add(receiverAddress);
            mailMessage.Subject = emailSubject;
            mailMessage.Body = emailBody;
            mailMessage.Priority = MailPriority.Normal;
            SendEmail(host, port, username, password, mailMessage);
        }

        /// <summary>
        ///  发送邮件
        /// </summary>
        /// <param name="host">smtp地址</param>
        /// <param name="port">端口</param>
        /// <param name="username">用户名 </param>
        /// <param name="password">密码</param>
        /// <param name="mail">邮件体</param>
        /// <param name="credential">认证方式</param>
        private static void SendEmail(string host, int port, string username, string password, MailMessage mail, EmailCredential credential)
        {
            SmtpClient smtpClient = new SmtpClient();
            if (host != null && host.Length > 0)
            {
                smtpClient.Host = host;
            }
            smtpClient.Port = port;
           
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (credential == EmailCredential.PassWordCredentials)
               smtpClient.Credentials = new System.Net.NetworkCredential(username, password);
            else
                smtpClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            smtpClient.Send(mail);
        }
    }
}
