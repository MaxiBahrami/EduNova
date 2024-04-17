using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static System.Net.Mime.MediaTypeNames;

namespace InsightAcademy.ApplicationLayer.Services
{
    public class EmailService : IEmailSender,IApplicationEmailSender  
    {

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webEnvironment;
        private const string WelcomeEmailePath = @"EmailTemplate/Welcome.html";
        private const string TutorVarifyEmailePath = @"EmailTemplate/TutorVarify.html";
        private const string ResetPasswordPath = @"EmailTemplate/ResetPassword.html";
        public EmailService(IConfiguration configuration, IWebHostEnvironment webEnvironment)
        {
            _config = configuration;
            _webEnvironment = webEnvironment;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var host = _config["SMTPConfig:Host"];
            var port = Convert.ToInt32(_config["SMTPConfig:Port"]);
            var username = _config["SMTPConfig:UserName"];
            var password = _config["SMTPConfig:Password"];
            var displayName = _config["SMTPConfig:SenderDisplayName"];
     
            try
            {
                var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    //EnableSsl = true,
                    //UseDefaultCredentials = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(username, displayName),
                    To = { new MailAddress(email) },
                    Subject = subject,
                    Body =htmlMessage,
                    IsBodyHtml = true,
                };

                await client.SendMailAsync(mailMessage);
              
            }
            catch (Exception e)
            {
                //_logger.LogError(e.Message);
             
            }
        }

        public async Task SendWelcomeEmail(string fName, string toEmail, string text)
        {

            var host = _config["SMTPConfig:Host"];
            var port = Convert.ToInt32(_config["SMTPConfig:Port"]);
            var username = _config["SMTPConfig:UserName"];
            var password = _config["SMTPConfig:Password"];
            var cc = _config["SMTPConfig:CCEmail"];
            var displayName = _config["SMTPConfig:SenderDisplayName"];
            try
            {
                var filepath = Path.Combine(_webEnvironment.WebRootPath, WelcomeEmailePath);

                string html = File.ReadAllText(filepath);
                html = html.Replace("{text}", text);
                // Include Order Items in the email template

                var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    //EnableSsl = true,
                    //UseDefaultCredentials = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(username, displayName),
                    To = { new MailAddress(toEmail) },
                    Subject = "Welcome to Our Platform!",
                    Body = html,
                    IsBodyHtml = true,
                };
                if (!string.IsNullOrEmpty(cc))
                {
                    mailMessage.CC.Add(cc);
                }
                await client.SendMailAsync(mailMessage);

            }
            catch (Exception e)
            {


            }
        }
        public async Task SendTutorVarifyEmail(string fName, string toEmail)
        {

            var host = _config["SMTPConfig:Host"];
            var port = Convert.ToInt32(_config["SMTPConfig:Port"]);
            var username = _config["SMTPConfig:UserName"];
            var password = _config["SMTPConfig:Password"];
            var cc = _config["SMTPConfig:CCEmail"];
            var displayName = _config["SMTPConfig:SenderDisplayName"];
            try
            {
                var filepath = Path.Combine(_webEnvironment.WebRootPath, TutorVarifyEmailePath);

                string html = File.ReadAllText(filepath);
             

                var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    //EnableSsl = true,
                    //UseDefaultCredentials = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(username, displayName),
                    To = { new MailAddress(toEmail) },
                    Subject = "Congratulations! You're Verified as a Tutor",
                    Body = html,
                    IsBodyHtml = true,
                };
                if (!string.IsNullOrEmpty(cc))
                {
                    mailMessage.CC.Add(cc);
                }
                await client.SendMailAsync(mailMessage);

            }
            catch (Exception e)
            {


            }
        }

        public async Task ResetConfrimationEmail(string toEmails, string URL)
		{
			var host = _config["SMTPConfig:Host"];
			var port = Convert.ToInt32(_config["SMTPConfig:Port"]);
			var username = _config["SMTPConfig:UserName"];
			var password = _config["SMTPConfig:Password"];
			var cc = _config["SMTPConfig:CCEmail"];
			var displayName = _config["SMTPConfig:SenderDisplayName"];
			try
			{
				var filepath = Path.Combine(_webEnvironment.WebRootPath, ResetPasswordPath);

				string html = File.ReadAllText(filepath);
				html = html.Replace("{URL}", URL);
				// Include Order Items in the email template

				var client = new SmtpClient(host, port)
				{
					Credentials = new NetworkCredential(username, password),
					//EnableSsl = true,
					//UseDefaultCredentials = true,
					DeliveryMethod = SmtpDeliveryMethod.Network,
				};

				var mailMessage = new MailMessage
				{
					From = new MailAddress(username, displayName),
					To = { new MailAddress(toEmails) },
					Subject = "Reset Your Password",
					Body = html,
					IsBodyHtml = true,
				};
				if (!string.IsNullOrEmpty(cc))
				{
					mailMessage.CC.Add(cc);
				}
				await client.SendMailAsync(mailMessage);

			}
			catch (Exception e)
			{


			}
		}
	}
}
