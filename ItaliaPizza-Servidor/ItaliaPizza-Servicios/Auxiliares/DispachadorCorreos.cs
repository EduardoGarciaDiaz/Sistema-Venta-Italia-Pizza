using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios.Auxiliares
{
    public static class DispachadorCorreos
    {
        public static bool EnviarCorreo(string destinatario, string asunto, string cuerpo, string adjunto)
        {
            bool exito;
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("pizzaitalian80@gmail.com", "blzc palt agjj lfyh")
                };

                MailMessage mensaje = new MailMessage("pizzaitalian80@gmail.com", destinatario, asunto, cuerpo);
                Attachment adjuntoCorreo = new Attachment(adjunto);
                mensaje.Attachments.Add(adjuntoCorreo);
                smtpClient.Send(mensaje);
                exito = true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
                exito = false;
            }
            return exito;
        }


    }
}
