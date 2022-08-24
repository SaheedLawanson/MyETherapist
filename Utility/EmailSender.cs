using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace etherapist.Utility;

public class EmailSender: IEmailSender {
    public Task SendEmailAsync(String email, String subject, String htmlMessage) {
        MimeMessage emailToSend = new MimeMessage();
        emailToSend.From.Add(MailboxAddress.Parse("Etherapist@gmail.com"));
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html){
            Text = htmlMessage
        };

        // Send email
        using (SmtpClient emailClient = new SmtpClient()) {
            emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            emailClient.Authenticate("saheedlawanson47@gmail.com", "bupxcfltllvkfuqk");
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);
        }
        
        return Task.CompletedTask;
    }
}