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

        public void EnviarCorreo(string dir, string asunto, string sedeDescripcion, string responsable, string tipoRequerimiento, string observacion)
        {
            

            //Creamos un nuevo Objeto de mensaje
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
            mmsg.To.Add("j.ccana@conastec.com.pe");

            //Asunto
            mmsg.Subject = asunto + " de Requerimiento de "+tipoRequerimiento+" - Nro "+ "XYZ"+"- Sede:"+ sedeDescripcion;
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

            mmsg.Bcc.Add("j.ccana@conastec.com.pe"); //Opcional

            
            //Cuerpo del Mensaje

            string cuerpo = cuerpoMail(asunto, tipoRequerimiento, sedeDescripcion, responsable, observacion);
            
            
            
            //string dir = MapPath("~") + @"\TemplateEmail\EnviarSolicitud.htm";
            string body = ObtenerCuerpoCorreo(dir, new List<string> { "cuerpo", "usuario", "rol", "area", "sede" },
                                                   new List<string> { cuerpo, "Usuario Actual", "Rol de usuario", "Area de Usuario", "Sede Actual" });

            mmsg.Body = body;
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

        public string cuerpoMail(string tipo,string tipoRequerimiento, string SedeDescripcion, string Responsable1, string Observacion)
        {
            string cuerpo = "";
            switch (tipo)
            {
                case  EtapasSolicitud.ElaboracionPerfil:
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha solicitado un "+
                        " Requerimiento de "+ tipoRequerimiento+" en la Sede " + SedeDescripcion + " y se requiere la elaboración del Perfil para continuar con el procedimiento.";
                        break;
                case EtapasSolicitud.PendienteAprobacion:
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha solicitado un "+
                                 "Requerimiento de " + tipoRequerimiento + " en la Sede " + SedeDescripcion + " y se requiere de su Aprobación o Rechazo como "+
                                  Responsable1+" para continuar con el procedimiento.";
                        break;
                case EtapasSolicitud.Finalizado:
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarles que se ha Rechazado el Requerimiento de "+tipoRequerimiento+
                                 " en la Sede "+SedeDescripcion+" por el motivo de "+Observacion;
                        break;
                case EtapasSolicitud.PendienteAprobacionPerfil:
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha elaborado el perfil para el Requerimiento de "
                                 +tipoRequerimiento+" en la Sede "+SedeDescripcion+" y se requiere de su Aprobación/Rechazo como "+Responsable1+" para continuar con el procedimiento.";
                        break;
            }

            return cuerpo;
 
        }
       
    }
}
