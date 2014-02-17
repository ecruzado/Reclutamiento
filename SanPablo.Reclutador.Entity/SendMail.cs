using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace SanPablo.Reclutador.Entity
{
    public  class SendMail
    {

        public void EnviarCorreo( string accion, bool indSingular, string tipoRequerimiento )
        {
            string saludo = "";
            string cuerpo = "";
            string comunicarle = "";
            string inicio = "";
            //Creamos un nuevo Objeto de mensaje
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
            mmsg.To.Add("j.ccana@conastec.com.pe");

            //Asunto
            mmsg.Subject = "Asunto del correo";
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

            mmsg.Bcc.Add("j.ccana@conastec.com.pe"); //Opcional

            if (indSingular)
            {
                inicio = "Estimado sr(a).";
                saludo = "saludarlo(a)";
                comunicarle = "comunicarle";
            }
            else
            {
                inicio = "Estimados Señores.";
                saludo = "saludarlos";
                comunicarle = "comunicarles";
            }
            //Cuerpo del Mensaje

            cuerpo =  "Luego de " + saludo + "la presente es para " + comunicarle + "que se ha" + accion + "Requerimiento de " + tipoRequerimiento + "Cargo en la Sede ";
            mmsg.Body = cuerpo;
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML


            mmsg.From = new System.Net.Mail.MailAddress("j.ccana@conastec.com.pe");

           
            //Creamos un objeto de cliente de correo
            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();

            cliente.UseDefaultCredentials = true;
            cliente.Credentials = new System.Net.NetworkCredential("j.ccana@conastec.com.pe", "jc6543");

            cliente.Host = "gator3243.hostgator.com";

            cliente.EnableSsl = false;

            
            try
            {
                cliente.Send(mmsg);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //Aquí gestionamos los errores al intentar enviar el correo
            }
        }
    }
}
