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
        public virtual string Usuario {get; set;}
        public virtual string Rol {get; set;}
        public virtual string Area { get; set; }
        public virtual string Sede { get; set; }

       
        public void EnviarMail(string destinatario, string asunto, string conCopia, string cuerpo )
        {
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
            //destinatario
            mmsg.To.Add(destinatario);
            
            mmsg.Subject = asunto;
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;


            mmsg.Bcc.Add(conCopia); //Opcional

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

        public List<string> cuerpoMail(string tipo,string tipoRequerimiento, string Responsable1,
                                       string Observacion,string suceso, string cargo, string codCargo )
        {
            string cuerpo = "";
            string asunto = "";
            switch (tipo)
            {
                case EtapasSolicitud.PendienteAprobacionGerenteGralAdj:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha solicitado un " +
                           " Requerimiento de " + tipoRequerimiento + " en la Sede " + Sede + " y se requiere la elaboración del Perfil para continuar con el procedimiento.";
                        asunto = "Aprobacion/Rechazo " + tipoRequerimiento + " de " + cargo + " - Nro " + codCargo + " - Sede:" + Sede;
                        break;
                    }
                case EtapasSolicitud.PendienteAprobacionGerenteArea:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha solicitado un " +
                                 "Requerimiento de " + tipoRequerimiento + " en la Sede " + Sede + " y se requiere de su Aprobación o Rechazo como Gerente de Area" +
                                 " para continuar con el procedimiento.";
                        asunto = "Aprobacion/Rechazo " + tipoRequerimiento + " de " + cargo + " - Nro " + codCargo + " - Sede:" + Sede;
                        break;
                    }
                case EtapasSolicitud.PendienteElaboracionPerfil:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha solicitado un " +
                        " Requerimiento de " + tipoRequerimiento + " en la Sede " + Sede + " y se requiere la elaboración del Perfil para continuar con el procedimiento.";
                        asunto = "Elaboracion de Perfil de " + tipoRequerimiento + " de " + cargo + " - Nro " + codCargo + " - Sede:" + Sede;
                        break;
                    }
                case EtapasSolicitud.PendienteAprobacionPerfilEncargSeleccion:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha elaborado el perfil para el Requerimiento de "
                                   + tipoRequerimiento + " en la Sede " + Sede + " y se requiere de su Aprobación/Rechazo como Encargado de Seleccion para continuar con el procedimiento.";
                        asunto = "Aprobacion/Rechazo " + tipoRequerimiento + " de " + cargo + " - Nro " + codCargo + " - Sede:" + Sede;
                        break;
                    }
                case EtapasSolicitud.PendienteAprobacionPerfilJefeArea:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha elaborado el perfil para el Requerimiento de "
                                    + tipoRequerimiento + " en la Sede " + Sede + " y se requiere de su Aprobación/Rechazo como Jefe de Area para continuar con el procedimiento.";
                        asunto = "Aprobacion/Rechazo " + tipoRequerimiento + " de " + cargo + " - Nro " + codCargo + " - Sede:" + Sede;
                        break;
                    }
                case EtapasSolicitud.Finalizado:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarles que se ha Rechazado el Requerimiento de " + tipoRequerimiento +
                                   " en la Sede " + Sede + " por el motivo de " + Observacion;
                        asunto = "Aprobacion/Rechazo " + tipoRequerimiento + " de " + cargo + " - Nro " + codCargo + " - Sede:" + Sede;
                        break;
                    }
                case EtapasSolicitud.PendientePublicacion:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarles que se ha Aprobado el perfil para el Requerimiento de " + tipoRequerimiento +
                                   " en la Sede " + Sede + " y se requiere de la publicación como Destinatario de Recursos Humanos";
                        asunto = "Pendiente de publicación " + tipoRequerimiento + " de " + cargo + " - Nro " + codCargo + " - Sede:" + Sede;
                        break;
                    }
            }

            if (suceso == SucesoSolicitud.Rechazado)
            {
                cuerpo = "Luego de saludarlo(a), la presente es para comunicarles que se ha Rechazado el Requerimiento de " + tipoRequerimiento +
                                 " en la Sede " + Sede + " por el motivo de " + Observacion;
                asunto = "Aprobacion/Rechazo " + tipoRequerimiento + " de " + cargo + " - Nro " + codCargo + " - Sede:" + Sede;
            }

            List<string> mail = new List<string>();
            mail.Add(cuerpo);
            mail.Add(asunto);
            return mail;
 
        }

        public void EnviarCorreo(string dir, string etapa,string responsable, string tipoRequerimiento, string observacion,
                                string cargo, string codCargo, string destinatario, string suceso)
        {

            List<string> mail = cuerpoMail(etapa, tipoRequerimiento, responsable, observacion, suceso, cargo, codCargo);

            string body = ObtenerCuerpoCorreo(dir, new List<string> { "cuerpo", "usuario", "rol", "area", "sede" },
                                                   new List<string> { mail[0], Usuario, Rol, Area, Sede });

            EnviarMail(destinatario, mail[1], "", body);

        }
       
    }
}
