using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightAcademy.ApplicationLayer.Services
{
    public interface  IApplicationEmailSender
    {
        Task SendWelcomeEmail(string fname, string toEmail, string text);
        Task SendTutorVarifyEmail(string fname, string toEmail);

		Task ResetConfrimationEmail(string toEmails,string URL);

	}
}
