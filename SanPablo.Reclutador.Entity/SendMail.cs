namespace SanPablo.Reclutador.Entity
{
    using System;
    using System.Web;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net.Mail;
    using System.IO;
    using System.Web.Http;
    using System.Web.Util;

    public  class SendMail
    {

        public void EnviarCorreo(string asunto, string accion, bool indSingular, string tipoRequerimiento )
        {
            string saludo = "";
            string cuerpo = "";
            string comunicarle = "";
            string inicio = "";
            string requerimiento = "";

                
                inicio = "Estimado sr(a)."; //1.4.5.6
                requerimiento = "se requiere de su Aprobacion/Rechazo como ";//1.4.5
                saludo = "saludarlo(a)";//1.4.5.6
                comunicarle = "comunicarle";//1.4.5.6


                requerimiento = " por el motivo "; //3.4
                requerimiento = " y se requiere la elaboracion del Perfil para continuar con el procedimiento.";//2


            //Creamos un nuevo Objeto de mensaje
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
            mmsg.To.Add("j.ccana@conastec.com.pe");

            //Asunto
            mmsg.Subject = asunto + " de Requerimiento de "+accion+" - Nro "+ "XYZ"+"- Sede:"+"SURCO" ;
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

            mmsg.Bcc.Add("j.ccana@conastec.com.pe"); //Opcional

            
            //Cuerpo del Mensaje

            cuerpo =  "Luego de " + saludo + "la presente es para " + comunicarle + "que se ha" + accion + "Requerimiento de " + tipoRequerimiento + "Cargo en la Sede (x)" +
                       requerimiento;
            
            //var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            //string dir = MapPath("~") + @"\TemplateEmail\EnviarSolicitud.htm";
            //string body = ObtenerCuerpoCorreo(dir, new List<string> { "cuerpo", "usuario", "rol", "area", "sede" },
            //                                       new List<string> { cuerpo, "Usuario Actual", "Rol de usuario", "Area de Usuario", "Sede Actual" });

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

        public static string ObtenerCuerpoCorreo(string path, List<string> parametros, List<string> valores)
        {
            StreamReader sr = new StreamReader(path);
            string cuerpo = sr.ReadToEnd();
            for (int i = 0; i < parametros.Count; i++)
            {
                cuerpo = cuerpo.Replace("{{" + parametros[i] + "}}", valores[i]);
            }
            return cuerpo;
        }
       
    }
}
