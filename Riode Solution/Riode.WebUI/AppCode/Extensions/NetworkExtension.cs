﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Extensions
{
    static public partial class Extension
    {
        static public bool SendEmail(this IConfiguration configuration,
            string to,
            string subject,
            string body,
            bool appendCC = false)
        {

            try
            {
                string fromMail = configuration["emailAccount:userName"];
                string displayName = configuration["emailAccount:displayName"];
                string smtpServer = configuration["emailAccount:smtpServer"];
                int smtpPort = Convert.ToInt32(configuration["emailAccount:smtpPort"]);
                string password = configuration["emailAccount:password"];
                string cc = configuration["emailAccount:cc"];

                using (MailMessage message = new MailMessage(new MailAddress(fromMail, displayName), new MailAddress(to))
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true

                })
                {

                    if (!string.IsNullOrWhiteSpace(cc) && appendCC == true)
                        message.CC.Add(cc);

                    SmtpClient smtpclient = new SmtpClient(smtpServer, smtpPort);
                    smtpclient.Credentials = new NetworkCredential(fromMail, password);
                    smtpclient.EnableSsl = true;
                    smtpclient.Send(message);

                }
            }
            catch (Exception)
            {

                return false;
            }




            return true;

        }
    }
}
