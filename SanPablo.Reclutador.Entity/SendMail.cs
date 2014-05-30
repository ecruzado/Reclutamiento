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
    using System.Collections.Generic;
    using System.Configuration;

    public  class SendMail
    {
        public virtual string Usuario {get; set;}
        public virtual string Rol {get; set;}
        public virtual string Area { get; set; }
        public virtual string Sede { get; set; }

       /// <summary>
       /// envia el Email de forma generica
       /// </summary>
       /// <param name="destinatario">destino</param>
       /// <param name="asunto">asunto</param>
       /// <param name="conCopia">No funciona xD</param>
       /// <param name="cuerpo">texto que visualiza el destinatario</param>
        public void EnviarMail(string destinatario, string asunto, string conCopia, string cuerpo )
        {
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
            //destinatario
            mmsg.To.Add(destinatario);
            
            mmsg.Subject = asunto;
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;


            //mmsg.Bcc.Add(conCopia); //Opcional

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
                var dato = 0;
            }
        }


        /// <summary>
        /// Enviar emial a todos
        /// </summary>
        /// <param name="destinatario"></param>
        /// <param name="asunto"></param>
        /// <param name="conCopia"></param>
        /// <param name="cuerpo"></param>
        public void EnviarMailAll(List<String> Sends,List<String> Copys, string asunto, string cuerpo)
        {
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
            //destinatario

            for (int i = 0; i < Sends.Count; i++)
            {
                mmsg.To.Add(Sends[i]);
            }

            for (int x = 0; x < Copys.Count; x++)
            {
                mmsg.CC.Add(Copys[x]);
            }

            mmsg.Subject = asunto;
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;


            //mmsg.Bcc.Add(conCopia); //Opcional

            mmsg.Body = cuerpo;
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML


            string CorreoHost = ConfigurationManager.AppSettings["CorreoHost"];
            string PassHost = ConfigurationManager.AppSettings["PassHost"];
            string ClienteHost = ConfigurationManager.AppSettings["ClienteHost"];

            //mmsg.From = new System.Net.Mail.MailAddress("j.ccana@conastec.com.pe");
            mmsg.From = new System.Net.Mail.MailAddress(CorreoHost);

            //Creamos un objeto de cliente de correo
            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();

            cliente.UseDefaultCredentials = true;
            //cliente.Credentials = new System.Net.NetworkCredential(CorreoHost, "jc6543");
            cliente.Credentials = new System.Net.NetworkCredential(CorreoHost, PassHost);

            //cliente.Host = "gator3243.hostgator.com";
            cliente.Host = ClienteHost;

            cliente.EnableSsl = false;

            try
            {

                cliente.Send(mmsg);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //Aquí gestionamos los errores al intentar enviar el correo
                var dato = 0;
            }
        }

        /// <summary>
        /// Se obtiene el html con los campos
        /// </summary>
        /// <param name="path">ruta del archivo html</param>
        /// <param name="parametros">los parametros del html</param>
        /// <param name="valores">datos de los parametros</param>
        /// <returns></returns>
        public string ObtenerCuerpoCorreo(string path, List<string> parametros, List<string> valores)
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
                case Etapa.Pendiente:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha solicitado un " +
                                 "requerimiento de " + tipoRequerimiento + " de " + cargo + " en la Sede " + Sede + " y se requiere de su Aprobación o Rechazo" +
                                 " para continuar con el procedimiento.";
                        asunto = "Aprobación/Rechazo de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                        break;
                    }
                case Etapa.Validado:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha solicitado un " +
                                 "requerimiento de " + tipoRequerimiento + " de " + cargo + " en la Sede " + Sede + " y se requiere de su Aprobación o Rechazo" +
                                 " para continuar con el procedimiento.";
                        asunto = "Aprobación/Rechazo de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                        break;
                    }
                case Etapa.Aprobado:
                    {
                        if (tipoRequerimiento == TipoRequerimientoEmail.Ampliacion)
                        {
                            cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha Aprobado el requerimiento de " + tipoRequerimiento + " de " + cargo +
                                     " en la Sede " + Sede + " y se requiere de su Aceptación.";
                            asunto = "Pendiente de aceptación de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                            break;
                        }

                        if (tipoRequerimiento == TipoRequerimientoEmail.Reemplazo)
                        {
                            cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha Aprobado el requerimiento de " + tipoRequerimiento + " de " + cargo +
                                " en la Sede " + Sede + " y se requiere que proceda con la publicación para continuar con el procedimiento.";
                            asunto = "Pendiente de publicación de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                            break;
                        }

                        if (tipoRequerimiento == TipoRequerimientoEmail.Nuevo)
                        {
                            cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha solicitado un " +
                            " requerimiento de " + tipoRequerimiento + " en la Sede " + Sede + " y se requiere la elaboración del Perfil para continuar con el procedimiento.";
                            asunto = "Elaboración de Perfil de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                            break;
                        }

                        break;
                    }

                case Etapa.Generacion_Perfil:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha elaborado el perfil para el requerimiento de "
                                  + tipoRequerimiento + " en la Sede " + Sede + " y se requiere de su Aprobación/Observación como solicitante para continuar con el procedimiento.";
                        asunto = "Aprobación/Observación de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                        break;
                    }
                case Etapa.Aprobacion_Perfil:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha Aprobado el perfil para el requerimiento de " + tipoRequerimiento + " de " + cargo +
                                   " en la Sede " + Sede + " y se requiere de su aceptación.";
                        asunto = "Aceptación de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                        break;
                    }
                case Etapa.Observado:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha Observado el perfil de " + tipoRequerimiento + " de " + cargo +
                                   " en la Sede " + Sede + " por el motivo: " + Observacion;
                        asunto = "Observación de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                        break;
                    }
                case Etapa.Rechazado:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha Rechazado el requerimiento de " + tipoRequerimiento + " de " + cargo +
                                   " en la Sede " + Sede + " por el motivo: " + Observacion;
                        asunto = "Rechazo de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                        break;
                    }
                case Etapa.Publicado:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha publicado el requerimiento de " + tipoRequerimiento + " de " + cargo +
                                 " en la Bolsa de Oportunidades.";
                        asunto = "Publicación de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                        break;
                    }
                case Etapa.Finalizado:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que se ha Finalizado el requerimiento de " + tipoRequerimiento + " de " + cargo +
                                 " en la Bolsa de Oportunidades.";
                        asunto = "Finalización de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                        break;
                    }
                case Etapa.Aceptado:
                    {
                        cuerpo = "Luego de saludarlo(a), la presente es para comunicarle que el requerimiento de " + tipoRequerimiento + " de " + cargo +
                                   " en la Sede " + Sede + " requiere de su aceptación para continuar el procedimiento.";
                        asunto = "Aceptación de " + tipoRequerimiento + " de " + cargo + " - Cargo " + codCargo + " - Sede: " + Sede;
                        break;
                    }

            }

            List<string> mail = new List<string>();
            mail.Add(cuerpo);
            mail.Add(asunto);
            return mail;
 
        }
        /// <summary>
        /// Envia Correo
        /// </summary>
        /// <param name="dir">ruta</param>
        /// <param name="etapa">etapa</param>
        /// <param name="responsable">responsable</param>
        /// <param name="tipoRequerimiento">tipo de requerimiento: reemplazo, ampliacion  y Nuevo</param>
        /// <param name="observacion">Observacion</param>
        /// <param name="cargo">Nombre del cargo</param>
        /// <param name="codCargo">Codigo del cargo</param>
        /// <param name="destinatario">destino</param>
        /// <param name="suceso">suceso</param>
        public void EnviarCorreo(string dir, string etapa,string responsable, string tipoRequerimiento, string observacion,
                                string cargo, string codCargo, string destinatario, string suceso)
        {
            

                //Dar formato a textos
                Usuario = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Usuario.ToLower());
                Rol = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Rol.ToLower());
                Area = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Area.ToLower());
                Sede = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Sede.ToLower());
            

            List<string> mail = cuerpoMail(etapa, tipoRequerimiento, responsable, observacion, suceso, cargo, codCargo);

            string body = ObtenerCuerpoCorreo(dir, new List<string> { "cuerpo", "usuario", "rol", "area", "sede" },
                                                   new List<string> { mail[0], Usuario, Rol, Area, Sede });

            EnviarMail(destinatario, mail[1], "", body);

        }

        /// <summary>
        /// Envia correo a todos
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="etapa"></param>
        /// <param name="responsable"></param>
        /// <param name="tipoRequerimiento"></param>
        /// <param name="observacion"></param>
        /// <param name="cargo"></param>
        /// <param name="codCargo"></param>
        /// <param name="destinatario"></param>
        /// <param name="suceso"></param>
        public void EnviarCorreoVarios(string dir, string etapa, string responsable, string tipoRequerimiento, string observacion,
                               string cargo, string codCargo, List<String> sends, string suceso,List<String> copy)
        {


            //Dar formato a textos
            //Usuario = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Usuario.ToLower());
            //Rol = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Rol.ToLower());
            //Area = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Area.ToLower());
            //Sede = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Sede.ToLower());
            Usuario = Usuario;
            Rol = Rol;
            Area = Area;
            Sede = Sede;

            List<string> mail = cuerpoMail(etapa, tipoRequerimiento, responsable, observacion, suceso, cargo, codCargo);

            string body = ObtenerCuerpoCorreo(dir, new List<string> { "cuerpo", "usuario", "rol", "area", "sede" },
                                                   new List<string> { mail[0], Usuario, Rol, Area, Sede });

            EnviarMailAll(sends, copy,mail[1], body);

        }

        public void enviarCorreoProgramacion(string dir, string destinatario, string entrevistaDesc, string fecha, string hora, string postulante, string cargo)
        {
            //Dar formato a textos
            Usuario = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Usuario.ToLower());
            Rol = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Rol.ToLower());
            Area = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Area.ToLower());
            Sede = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Sede.ToLower());


            List<string> mail = new List<string>();
            string cuerpoMail = "Luego de saludarlo(a), la presente es para comunicarle que se ha programado la "+entrevistaDesc+" a "+postulante+ " , para el cargo de "+cargo+
                                ",  para el dia "+fecha+" a las "+hora+" horas. ";
            string asuntoMail = "Programación de entrevista para el cargo " + cargo;

            mail.Add(cuerpoMail);
            mail.Add(asuntoMail);

            string body = ObtenerCuerpoCorreo(dir, new List<string> { "cuerpo", "usuario", "rol", "area", "sede" },
                                                   new List<string> { mail[0], Usuario, Rol, Area, Sede });

            EnviarMail(destinatario, mail[1], "", body);
        }
       
    }
}
