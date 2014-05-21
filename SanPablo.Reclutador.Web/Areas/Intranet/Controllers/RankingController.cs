

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{

    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using System.Configuration;
    using CrystalDecisions.Shared;
    using CrystalDecisions.CrystalReports.Engine;
    using CrystalDecisions.CrystalReports;
    using CrystalDecisions.Web;

    using iTextSharp;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    
    public class RankingController : BaseController
    {
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ICvPostulanteRepository _cvPostulanteRepository;
        private IReclutamientoPersonaRepository _reclutamientoPersonaRepository;
        private IPostulanteRepository _postulanteRepository;
        

        public RankingController(IDetalleGeneralRepository detalleGeneralRepository,
                                         ISolReqPersonalRepository solReqPersonalRepository,
                                         ICvPostulanteRepository cvPostulanteRepository,
                                        IReclutamientoPersonaRepository reclutamientoPersonaRepository,
                                        
            IPostulanteRepository postulanteRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _cvPostulanteRepository = cvPostulanteRepository;
            _reclutamientoPersonaRepository = reclutamientoPersonaRepository;
            _postulanteRepository = postulanteRepository;
           
        }

        /// <summary>
        /// inicializa el ranking
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Index(int id, string tipSol, string pagina)
        {
            RankingViewModel model;
            
            model = new RankingViewModel();
            model.Solicitud = new SolReqPersonal();
            model.ReclutaPersonal = new ReclutamientoPersona();


            model.Solicitud.IdeSolReqPersonal = id;
            model.Solicitud.Tipsol = tipSol;
            List<SolReqPersonal> listaSol = _solReqPersonalRepository.GetDatosSol(model.Solicitud);

            if (listaSol!=null && listaSol.Count>0)
            {
                model.Solicitud = (SolReqPersonal)listaSol[0];
            }
            // se incializa
            model.Solicitud.IdeSolReqPersonal = id;
            model.Solicitud.Tipsol = tipSol;

            model.listaEstaPost =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoPostulante));
            model.listaEstaPost.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.pagina = pagina;

            // consulta que obtiene los datos de la solicitud por id y Tipo de Puesto



            //Permisos de los botones por roll
            Int32 idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);

            if (Roles.Analista_Seleccion.Equals(idRol))
            {
                model.btnAgregar ="S";
                model.btnEditar = "S";
                model.btnEliminar = "S";
                model.btnCitado = "S";
                model.btnAsistio = "S";
                model.btnBuscar = "S";
                model.btnAprobado = "S";
                model.btnCvPreseleccion = "S";

            }
            else if (Roles.Encargado_Seleccion.Equals(idRol))
            {
                model.btnAgregar = "S";
                model.btnEditar = "S";
                model.btnEliminar = "S";
                model.btnCitado = "S";
                model.btnAsistio = "S";
                model.btnBuscar = "S";
                model.btnAprobado = "S";
                model.btnCvPreseleccion = "S";

            }
            else if (Roles.Administrador_Sistema.Equals(idRol))
            {
                model.btnAgregar = "S";
                model.btnEditar = "S";
                model.btnEliminar = "S";
                model.btnCitado = "S";
                model.btnAsistio = "S";
                model.btnBuscar = "S";
                model.btnAprobado = "S";
                model.btnCvPreseleccion = "S";

            }
            else 
            { 
                model.btnAgregar = "N";
                model.btnEditar = "N";
                model.btnEliminar = "N";
                model.btnCitado = "N";
                model.btnAsistio = "N";
                model.btnBuscar = "N";
                model.btnAprobado = "N";
                model.btnCvPreseleccion = "N";
            }

            return View("Index",model);
        }

       /// <summary>
       /// Inicializa el popup para ingresa el cv de los postulantes
       /// </summary>
       /// <param name="id"></param>
       /// <param name="idPos"></param>
       /// <param name="tipSol"></param>
       /// <returns></returns>
        public ActionResult inicoPopupCv(int id, int idPos, string tipSol) 
        {
            JsonMessage objJson = new JsonMessage();
            RankingViewModel model = new RankingViewModel();
            model.Solicitud = new SolReqPersonal();
            model.CvPostulanteEx = new CvPostulante();
            

            if (idPos>0)
            {

                var ObjCvPostulante = _cvPostulanteRepository.GetSingle(x => x.IdCvPostulante == idPos);
                if (ObjCvPostulante!=null)
                {
                    model.CvPostulanteEx = ObjCvPostulante;
                    model.Solicitud.IdeSolReqPersonal = id;
                    model.Solicitud.Tipsol = tipSol;
                    model.CvPostulanteEx.IdCvPostulante = idPos;
                }

            }
            else
            {
                model.Solicitud.IdeSolReqPersonal = id;
                model.Solicitud.Tipsol = tipSol;
                model.CvPostulanteEx.IdCvPostulante = idPos;

            }

            
            return View("PopupCvPostulante", model);

        
        }
        /// <summary>
        /// guarda los datos del postulante relacionados a una solicitud
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetCvPostulante(RankingViewModel model)
        {
            JsonMessage objJson;
            objJson = new JsonMessage();
            int idPostulante=0;
            int idSolicitud = 0;
            string tipSol = null;
            CvPostulante objCvPostulante = null;
            if (model !=null)
            {

                DateTime hoy = DateTime.Today;
              
               idPostulante = model.CvPostulanteEx.IdCvPostulante;

               tipSol = model.Solicitud.Tipsol;
               idSolicitud = (int)model.Solicitud.IdeSolReqPersonal;

               if (idPostulante > 0)
               {
                   objCvPostulante = new CvPostulante();    
                   //actualiza
                   objCvPostulante = _cvPostulanteRepository.GetSingle(x => x.IdCvPostulante == idPostulante);
                   objCvPostulante.Nombre = model.CvPostulanteEx.Nombre;
                   objCvPostulante.ApePaterno = model.CvPostulanteEx.ApePaterno;
                   objCvPostulante.ApeMaterno = model.CvPostulanteEx.ApeMaterno;
                   objCvPostulante.Fechacita = model.CvPostulanteEx.Fechacita;
                   objCvPostulante.HoraCita = model.CvPostulanteEx.HoraCita;
                   objCvPostulante.Dni = model.CvPostulanteEx.Dni;
                   objCvPostulante.Telefono = model.CvPostulanteEx.Telefono;
                   objCvPostulante.FecModificacion = hoy;
                   objCvPostulante.UsrModifcacion = UsuarioActual.NombreUsuario;
                   
                   _cvPostulanteRepository.Update(objCvPostulante);
                   objJson.Resultado = true;
                   objJson.Mensaje = "Se actualizaron los datos del postulante";

               }
               else
               {
                   //inserta
                   objCvPostulante = new CvPostulante();

                   objCvPostulante.IdSolicitud = idSolicitud;
                   objCvPostulante.TipSol = tipSol;
                   objCvPostulante.Nombre = model.CvPostulanteEx.Nombre;
                   objCvPostulante.ApePaterno = model.CvPostulanteEx.ApePaterno;
                   objCvPostulante.ApeMaterno = model.CvPostulanteEx.ApeMaterno;
                   objCvPostulante.Fechacita = model.CvPostulanteEx.Fechacita;
                   objCvPostulante.HoraCita = model.CvPostulanteEx.HoraCita;
                   objCvPostulante.Dni = model.CvPostulanteEx.Dni;
                   objCvPostulante.Telefono = model.CvPostulanteEx.Telefono;
                   objCvPostulante.FecCreacion = hoy;
                   objCvPostulante.UsrCreacion = UsuarioActual.NombreUsuario;

                   _cvPostulanteRepository.Add(objCvPostulante);

                   objJson.Resultado = true;
                   objJson.Mensaje = "Se registro el postulante";
               }

            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se puede registrar el postulante";
            }

            

            return Json(objJson);

        }



        /// <summary>
        /// muestra la lista de postulantes
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="idCriterio"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListaCvPostulante(GridTable grid)
        {
            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;
              
                int idSolicitud = Convert.ToInt32(grid.rules[0].data);
                string tipoSol = Convert.ToString(grid.rules[1].data);

                DetachedCriteria where = DetachedCriteria.For<CvPostulante>();


                where.Add(Expression.Eq("IdSolicitud", idSolicitud));
                where.Add(Expression.Eq("TipSol", tipoSol));


                var generic = Listar(_cvPostulanteRepository,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdCvPostulante.ToString(),
                        cell = new string[]
                            {
                                
                                item.IdCvPostulante.ToString(),
                                item.IdSolicitud.ToString(),
                                item.TipSol,
                                item.ApePaterno.ToString(),
                                item.ApeMaterno.ToString(),
                                item.Nombre,
                                item.Dni,
                                item.Telefono,
                                item.Fechacita==null?"":String.Format("{0:dd/MM/yyyy}", item.Fechacita),
                                item.HoraCita==null?"":String.Format("{0:hh:mm tt}", item.HoraCita),
                                item.IndicadorCitado.ToString(),
                                item.IndicadorAsistio.ToString()
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }

        /// <summary>
        /// elimina el postulante cv externo 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult EliminarPopupCv(int id)
        {
            JsonMessage objJson = new JsonMessage();

            if (id>0)
            {

                var objCvPost = _cvPostulanteRepository.GetSingle(x => x.IdCvPostulante == id);
                _cvPostulanteRepository.Remove(objCvPost);

                objJson.Resultado = true;
                objJson.Mensaje = "Se elimino el resgistro";

            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se puede eliminiar el usuario";
            }


            return Json(objJson);
        }


        /// <summary>
        /// Realiza la Aprobacion del postulante cambia el estado a preseleccion manual
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult ApruebaPostulante(int id, int idPostulante, int idSede)
        {
            JsonMessage objJson = new JsonMessage();
            string estado = PostulanteEstado.PRESELECCIONADO_MANUAL;
            string IndPostulacion = null;
            ReclutamientoPersona objReclutamientoPersona;
            
            objReclutamientoPersona = new ReclutamientoPersona();
            objReclutamientoPersona.IdePostulante = idPostulante;
            objReclutamientoPersona.IdSede = idSede;

            IndPostulacion = _postulanteRepository.ValidaSeleccion(objReclutamientoPersona);

            if (Indicador.No.Equals(IndPostulacion))
            {
               if (id > 0)
                {

                    objReclutamientoPersona = new ReclutamientoPersona();
                    objReclutamientoPersona.IdeReclutaPersona = id;
                    objReclutamientoPersona.EstPostulante = estado;


                    _postulanteRepository.UpdateEstadoPostulante(objReclutamientoPersona);

                    objJson.Resultado = true;
                    objJson.Mensaje = "Se actualizo el resgistro";

                }
                else
                {
                    objJson.Resultado = false;
                    objJson.Mensaje = "No se pudo actualiza el registro";
                }
            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "El postulante se encuentra preseleccionado en otra solicitud";
            }


            return Json(objJson);
        }

        


        /// <summary>
        /// Marca si un postulante externo tiene una cita
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CitaPostulanteCv(int id)
        {
            JsonMessage objJson = new JsonMessage();

            if (id>0)
            {

                var objCvPost = _cvPostulanteRepository.GetSingle(x => x.IdCvPostulante == id);

                if (Indicador.Si.Equals(objCvPost.Citado))
                {
                    objCvPost.Citado = Indicador.No;
                    _cvPostulanteRepository.Update(objCvPost);
                    objJson.Mensaje = "Se anulo la cita";
                }
                else
                {
                    objCvPost.Citado = Indicador.Si;
                    _cvPostulanteRepository.Update(objCvPost);
                    objJson.Mensaje = "Se relizo la cita";
                }
               
                objJson.Resultado = true;
              

            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se pudo marcar la cita";
            }


            return Json(objJson);
        }

        /// <summary>
        /// Marca si el postulante asistio a ala cita
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AsistioPostulanteCv(int id)
        {
            JsonMessage objJson = new JsonMessage();

            if (id > 0)
            {

                var objCvPost = _cvPostulanteRepository.GetSingle(x => x.IdCvPostulante == id);

                if (Indicador.Si.Equals(objCvPost.Asistio))
                {
                    objCvPost.Asistio = Indicador.No;
                    _cvPostulanteRepository.Update(objCvPost);
                    objJson.Mensaje = "Se anulo la asistencia";
                }
                else
                {
                    objCvPost.Asistio = Indicador.Si;
                    _cvPostulanteRepository.Update(objCvPost);
                    objJson.Mensaje = "Se marco la asistencia";
                }

                objJson.Resultado = true;


            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se pudo marcar la asistencia";
            }


            return Json(objJson);
        }

        /// <summary>
        /// inicializa el popup de exclucion de postulante
        /// </summary>
        /// <returns></returns>
        
        public ActionResult ExclucionPostulante(int id, int idPos, string tipSol) 
        {
            RankingViewModel model = new RankingViewModel();

            model.ReclutaPersonal = new ReclutamientoPersona();
            model.Solicitud = new SolReqPersonal();
            model.CvPostulanteEx = new CvPostulante();

            model.ReclutaPersonal.IdeSol = id;
            model.ReclutaPersonal.IdePostulante = idPos;
            model.ReclutaPersonal.TipSol = tipSol;

            return View("PopupExclucion", model);

        }

        [HttpPost]
        public ActionResult AsignaExclucionPost(RankingViewModel model)
        {

            JsonMessage objJson = new JsonMessage();

            var objReclutaPersona = _reclutamientoPersonaRepository.GetSingle(x => x.IdeSol == model.ReclutaPersonal.IdeSol &&
                                                       x.TipSol == model.ReclutaPersonal.TipSol
                                                       && x.IdePostulante == model.ReclutaPersonal.IdePostulante);
            
            objReclutaPersona.EstPostulante = PostulanteEstado.EXCLUIDO;
            objReclutaPersona.Comentario = model.ReclutaPersonal.Comentario.Trim();


            _reclutamientoPersonaRepository.Update(objReclutaPersona);
            
            objJson.Resultado = true;
            objJson.Mensaje = "Se excluyo al postulante";

            return Json(objJson);

        }



        /// <summary>
        /// Relaliza el contacto del postulante
        /// </summary>
        /// <param name="id">id de reclutamiento persona</param>
        /// <param name="indContacto">indicador de contacto</param>
        /// <returns></returns>
         [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult ContactaPostulante(int id, string indContacto)
        {
            JsonMessage objJson = new JsonMessage();

            var contacto = indContacto;
           

            if (id > 0)
            {

                var objReclutamientoPer = _reclutamientoPersonaRepository.GetSingle(x => x.IdeReclutaPersona == id);

                if (Indicador.Si.Equals(objReclutamientoPer.IndContactado))
                {
                    objReclutamientoPer.IndContactado = Indicador.No;
                    _reclutamientoPersonaRepository.Update(objReclutamientoPer);
                    objJson.Mensaje = "Se actualizo el contacto";
                }
                else
                {
                    objReclutamientoPer.IndContactado = Indicador.Si;
                    _reclutamientoPersonaRepository.Update(objReclutamientoPer);
                    objJson.Mensaje = "Se actualizo el contacto";
                }

                objJson.Resultado = true;


            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se puede actualizar el contacto";
            }


            return Json(objJson);
        }

        /// <summary>
        /// obtiene los postulantes para la solicitud
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
         [HttpPost]
        public ActionResult ListPostulantesRanking(GridTable grid)
        {

            ReclutamientoPersona reclutamientoPersona;
            List<ReclutamientoPersona> lista = new List<ReclutamientoPersona>();
            try
            {

                reclutamientoPersona = new ReclutamientoPersona();

                reclutamientoPersona.IdeSol = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
                reclutamientoPersona.TipSol = (grid.rules[1].data == null ? "" : Convert.ToString(grid.rules[1].data));
                reclutamientoPersona.ApePaterno = (grid.rules[2].data == null ? "" : Convert.ToString(grid.rules[2].data));
                reclutamientoPersona.ApeMaterno = (grid.rules[3].data == null ? "" : Convert.ToString(grid.rules[3].data));
                reclutamientoPersona.Nombre = (grid.rules[4].data == null ? "" : Convert.ToString(grid.rules[4].data));
                reclutamientoPersona.EstPostulante = (grid.rules[5].data == null ? "" : Convert.ToString(grid.rules[5].data));

                if ("0".Equals(reclutamientoPersona.EstPostulante))
                {
                    reclutamientoPersona.EstPostulante = "";
                }

                lista = _postulanteRepository.GetPostulantesRanking(reclutamientoPersona);

                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = (item.IdeReclutaPersona.ToString()),
                    cell = new string[]
                            {
                                item.IdeReclutaPersona==null?"":item.IdeReclutaPersona.ToString(),
                                item.IdePostulante==null?"":item.IdePostulante.ToString(),
                                item.IdeSol==null?"":item.IdeSol.ToString(),
                                item.IdSede==null?"":item.IdSede.ToString(),
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.Apellidos==null?"":item.Apellidos,
                                item.Nombres==null?"":item.Nombres,
                                "FormatoCv",
                                item.EstPostulante==null?"":item.EstPostulante.ToString(),
                                item.DesEstadoPostulante==null?"":item.DesEstadoPostulante,
                                item.Comentario==null?"":item.Comentario,
                                item.PtoTotal==null?"":item.PtoTotal.ToString(),
                                item.PostulacionParalelo ==null?"":item.PostulacionParalelo
                                
                            }
                }).ToArray();

                return Json(generic.Value);

            }
            catch (Exception ex)
            {

                return MensajeError();
            }
        }


         /// <summary>
         /// Inicializa la pantalla de preseleccion de postulantes
         /// </summary>
         /// <param name="id"></param>
         /// <param name="tipSol"></param>
         /// <returns></returns>
   [ValidarSesion]
         public ActionResult Preseleccionado(int id, string tipSol,string pagina,string ind)
         {
             RankingViewModel model;

             model = new RankingViewModel();
             model.Solicitud = new SolReqPersonal();
             model.ReclutaPersonal = new ReclutamientoPersona();


             model.Solicitud.IdeSolReqPersonal = id;
             model.Solicitud.Tipsol = tipSol;
             List<SolReqPersonal> listaSol = _solReqPersonalRepository.GetDatosSol(model.Solicitud);

             if (listaSol != null && listaSol.Count > 0)
             {
                 model.Solicitud = (SolReqPersonal)listaSol[0];
             }
             // se incializa
             model.Solicitud.IdeSolReqPersonal = id;
             model.Solicitud.Tipsol = tipSol;

             model.listaEstaPost =
             new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoPostulante));
             model.listaEstaPost.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
             
             model.pagina = pagina;
             model.indPagina = ind;
             
            // consulta que obtiene los datos de la solicitud por id y Tipo de Puesto

            //accesos por botones
       
             Int32 idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);

             if (Roles.Administrador_Sistema.Equals(idRol))
             {
                 model.btnEvaluaciones = "S";
                 model.btnSeleccionar = "S";
                 model.btnExcluir = "S";
                 model.btnContactado = "S";
                 model.btnContratar = "S";
               
             }
             else if (Roles.Analista_Seleccion.Equals(idRol))
             {
                 model.btnEvaluaciones = "S";
                 model.btnSeleccionar = "S";
                 model.btnExcluir = "S";
                 model.btnContactado = "S";
                 model.btnContratar = "S";

             }
             else if (Roles.Encargado_Seleccion.Equals(idRol))
             {
                 model.btnEvaluaciones = "S";
                 model.btnSeleccionar = "S";
                 model.btnExcluir = "S";
                 model.btnContactado = "S";
                 model.btnContratar = "S";

             }else if(Roles.Gerente_General_Adjunto.Equals(idRol))
             {
                 model.btnEvaluaciones = "S";
                 model.btnSeleccionar = "S";
                 model.btnContratar = "S";
                
             }
             else if (Roles.Gerente.Equals(idRol))
             {
                 model.btnEvaluaciones = "S";
                 model.btnSeleccionar = "S";
                 model.btnContratar = "S";

             }
             else if (Roles.Jefe.Equals(idRol))
             {
                 model.btnEvaluaciones = "S";
                 model.btnSeleccionar = "S";
                 model.btnContratar = "S";

             }
             else
             {
                 model.btnEvaluaciones = "N";
                 model.btnSeleccionar = "N";
                 model.btnExcluir = "N";
                 model.btnContactado = "N";
                 model.btnContratar = "N";
             }


             return View("PostulantesPreSeleccionados", model);
         }

         [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
         [HttpPost]
         public ActionResult SeleccionaPost(int id,string promedio) 
         {
             JsonMessage objJson = new JsonMessage();

             var objReclutaPer = _reclutamientoPersonaRepository.GetSingle(x => x.IdeReclutaPersona == id);
             objReclutaPer.EstPostulante = PostulanteEstado.SELECCIONADO;
             
             if (promedio!=null)
             {
                 objReclutaPer.PromedioExamen = Convert.ToDouble(promedio);    
             }
             
             _reclutamientoPersonaRepository.Update(objReclutaPer);


             objJson.Resultado = true;
             objJson.Mensaje = "Se actualizo el registro";

             return Json(objJson);
         }

         /// <summary>
         /// lista de postulantes preseleccionados
         /// </summary>
         /// <param name="grid"></param>
         /// <returns></returns>
 
         [HttpPost]
         public ActionResult ListPostulantesPre(GridTable grid)
         {

             ReclutamientoPersona reclutamientoPersona;
             List<ReclutamientoPersona> lista = new List<ReclutamientoPersona>();
             try
             {

                 reclutamientoPersona = new ReclutamientoPersona();

                 reclutamientoPersona.IdeSol = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
                 reclutamientoPersona.TipSol = (grid.rules[1].data == null ? "" : Convert.ToString(grid.rules[1].data));
               

                 if ("0".Equals(reclutamientoPersona.EstPostulante))
                 {
                     reclutamientoPersona.EstPostulante = "";
                 }

                 lista = _postulanteRepository.GetPostulantesPreseleccionado(reclutamientoPersona);

                 var generic = GetListar(lista,
                                          grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                 generic.Value.rows = generic.List.Select(item => new Row
                 {
                     id = (item.IdeReclutaPersona.ToString()),
                     cell = new string[]
                            {
                                item.IdeReclutaPersona==null?"":item.IdeReclutaPersona.ToString(),
                                item.IdePostulante==null?"":item.IdePostulante.ToString(),
                                item.IdeSol==null?"":item.IdeSol.ToString(),
                                item.IdSede==null?"":item.IdSede.ToString(),
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.Apellidos==null?"":item.Apellidos,
                                item.Nombres==null?"":item.Nombres,
                                item.FonoFijo==null?"":item.FonoFijo,
                                item.FonoMovil==null?"":item.FonoMovil,
                                item.IndicadorContactado==null?"":item.IndicadorContactado.ToString(),
                                "formatoCv",
                                item.EstPostulante==null?"":item.EstPostulante.ToString(),
                                item.DesEstadoPostulante==null?"":item.DesEstadoPostulante,
                                item.EvalPostulante==null?"":item.EvalPostulante,
                                item.PtoTotal==null?"":item.PtoTotal.ToString(),
                                item.IndAprobacion==null?"":item.IndAprobacion
                                
                            }
                 }).ToArray();

                 return Json(generic.Value);

             }
             catch (Exception ex)
             {

                 return MensajeError();
             }
         }

        /// <summary>
        /// obtiene el Cv del postulante en formato PDF
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         public ActionResult GetCvPDF(string id)
         {
             JsonMessage objJsonMessage = new JsonMessage();
             string fullPath = null;
             ReportDocument rep = new ReportDocument();
             MemoryStream mem;
             Postulante objPostulante;

             try
             {
                 CvPostulante objCvPostulante;

                     objCvPostulante = new CvPostulante();
                 objCvPostulante.IdCvPostulante = Convert.ToInt32(id);
                 List<CvPostulante> listaCvPostulante = new List<CvPostulante>();

                 listaCvPostulante = _postulanteRepository.ListaCvPostulante(objCvPostulante);

                 objCvPostulante = new CvPostulante();

                 objCvPostulante = listaCvPostulante[0];

                 Font normal = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL));
                 BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                 Font estiloBlanco = new Font(bfTimes, 10, Font.ITALIC, BaseColor.WHITE);
                 Font negrita = new Font(FontFactory.GetFont("Arial", 10, Font.BOLD));
                 Font subRayado = new Font(FontFactory.GetFont("Arial", 10, Font.UNDERLINE));

                 using (MemoryStream ms = new MemoryStream())
                 {
                     Document doc = new Document(PageSize.A4, 30, 30, 30, 30);
                     PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                     //se abre el documento antes de empezar
                     doc.Open();
                     PdfPTable tableCabecera = new PdfPTable(2);
                     float[] anchosCabecera = new float[] { 70.0f, 30.0f };
                     tableCabecera.SetWidths(anchosCabecera);

                     PdfPTable tableCabTexto = new PdfPTable(5);
                     float[] anchosCabTexto = new float[] { 40.0f, 30.0f, 20.0f, 5.0f, 5.0f };
                     tableCabTexto.SetWidths(anchosCabTexto);


                     string nombreCompleto = (objCvPostulante.Nombrecompleto == null ? "" : objCvPostulante.Nombrecompleto);
                     string desEdad = (objCvPostulante.Desedad == null ? "" : objCvPostulante.Desedad);
                     string edad = (objCvPostulante.Edad == null ? "" : objCvPostulante.Edad);
                     string desEstadoCivil = (objCvPostulante.Desestadocivil == null ? "" : objCvPostulante.Desestadocivil);
                     string desdir = (objCvPostulante.Desdir == null ? "" : objCvPostulante.Desdir);
                     string fonoFijo = (objCvPostulante.Telfijo == null ? "" : objCvPostulante.Telfijo);
                     string fonoMovil = (objCvPostulante.Telmovil == null ? "" : objCvPostulante.Telmovil);
                     string fonoConcatenado = "";
                     
                     if (fonoFijo != "" && fonoMovil!="")
                     {
                         fonoConcatenado = fonoFijo + " / " + fonoMovil;
                     }

                     if (fonoFijo != "" && fonoMovil=="")
                     {
                         fonoConcatenado = fonoFijo;
                     }

                     if (fonoMovil != "" && fonoFijo == "")
                     {
                         fonoConcatenado = fonoMovil;
                     }

                     string correo = (objCvPostulante.Correo == null ? "" : objCvPostulante.Correo);
                     string observacion = (objCvPostulante.Observacion == null ? "" : objCvPostulante.Observacion);

                     // celda 01 fila 01
                     PdfPCell CellNombreCompleto = new PdfPCell(new Phrase(nombreCompleto, negrita));
                     CellNombreCompleto.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellNombreCompleto.Border = Rectangle.NO_BORDER;
                     CellNombreCompleto.Colspan = 5;
                     CellNombreCompleto.SpaceCharRatio = 4f;
                     tableCabTexto.AddCell(CellNombreCompleto);
                    
                     // celda 01 fila 01
                     PdfPCell CellDesEdad = new PdfPCell(new Phrase(desEdad, normal));
                     CellDesEdad.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellDesEdad.Border = Rectangle.NO_BORDER;
                     
                     tableCabTexto.AddCell(CellDesEdad);


                     // celda 01 fila 02
                     PdfPCell CellEdad = new PdfPCell(new Phrase(edad +" años", normal));
                     CellEdad.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellEdad.Border = Rectangle.NO_BORDER;
                     tableCabTexto.AddCell(CellEdad);
                     // celda 01 fila 03
                     PdfPCell CellEstadoCivil = new PdfPCell(new Phrase(desEstadoCivil, normal));
                     CellEstadoCivil.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellEstadoCivil.Border = Rectangle.NO_BORDER;
                     CellEstadoCivil.Colspan = 3;
                     CellEstadoCivil.SpaceCharRatio = 4f;
                     tableCabTexto.AddCell(CellEstadoCivil);

                     PdfPCell CellDireccion = new PdfPCell(new Phrase(desdir, normal));
                     CellDireccion.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                     CellDireccion.Border = Rectangle.NO_BORDER;
                     CellDireccion.Colspan = 5;
                     CellDireccion.SpaceCharRatio = 4f;
                     tableCabTexto.AddCell(CellDireccion);


                     PdfPCell CellFono = new PdfPCell(new Phrase(fonoConcatenado, normal));
                     CellFono.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                     CellFono.Border = Rectangle.NO_BORDER;
                     CellFono.Colspan = 5;
                     CellFono.SpaceCharRatio = 4f;
                     tableCabTexto.AddCell(CellFono);

                     PdfPCell CellaCorreo = new PdfPCell(new Phrase(correo, normal));
                     CellaCorreo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                     CellaCorreo.Border = Rectangle.NO_BORDER;
                     CellaCorreo.Colspan = 5;
                     CellaCorreo.SpaceCharRatio = 4f;
                     tableCabTexto.AddCell(CellaCorreo);

                     PdfPCell CellObserv = new PdfPCell(new Phrase(observacion, normal));
                     CellObserv.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                     CellObserv.Border = Rectangle.NO_BORDER;
                     CellObserv.Colspan = 5;
                     CellObserv.SpaceCharRatio = 4f;
                     tableCabTexto.AddCell(CellObserv);



                     PdfPTable tableCabImage = new PdfPTable(1);
                     float[] anchosCabImage= new float[] { 90.0f};
                     tableCabImage.SetWidths(anchosCabImage);

                     PdfPCell CellFotoPostulante = null;

                     if (objCvPostulante.Fotopostulante != null)
                     {
                         Image FotoPostulante = Image.GetInstance(objCvPostulante.Fotopostulante);
                         FotoPostulante.ScaleToFit(130f, 130f);

                         CellFotoPostulante = new PdfPCell(FotoPostulante);
                         CellFotoPostulante.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellFotoPostulante.Border = Rectangle.NO_BORDER;
                         // se le indica que ocupe las 3 columnas

                     }
                     else
                     {
                         CellFotoPostulante = new PdfPCell(new Phrase("", normal));

                     }

                     CellFotoPostulante.Border = Rectangle.NO_BORDER;

                     tableCabImage.AddCell(CellFotoPostulante);
                     
                     PdfPCell CellCabTexto = new PdfPCell(tableCabTexto);
                     PdfPCell CellCabImage = new PdfPCell(tableCabImage);

                     CellCabTexto.Border = Rectangle.NO_BORDER;
                     CellCabImage.Border = Rectangle.NO_BORDER;

                     tableCabecera.AddCell(CellCabTexto);
                     tableCabecera.AddCell(CellCabImage);

                     tableCabecera.SpacingBefore = 20f;
                     tableCabecera.SpacingAfter = 30f;

                     doc.Add(tableCabecera);


                     PdfPTable tableExperiencia = new PdfPTable(3);
                     float[] anchosExperiencia = new float[] { 10.0f,60.0f, 30.0f };
                     tableExperiencia.SetWidths(anchosExperiencia);

                     PdfPCell CellCabExperiencia = new PdfPCell(new Phrase("Experiencia", negrita));
                     CellCabExperiencia.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellCabExperiencia.Border = Rectangle.NO_BORDER;
                     CellCabExperiencia.Colspan = 3;
                     tableExperiencia.AddCell(CellCabExperiencia);

                     doc.Add(tableExperiencia);


                     //se cierra el documento pdf
                     doc.Close();


                     writer.Close();

                     return File(ms.ToArray(), "application/pdf"); 

                 }
                 //DataTable dtNivelAcademico = _postulanteRepository.getDataCvNivelAcademico(objPostulante);
                 //DataTable dtExperiencias = _postulanteRepository.getDataCvExperiencias(objPostulante);
                 //DataTable dtConOfimatica = _postulanteRepository.getDataCvConOfimatica(objPostulante);
                 //DataTable dtConIdiomas = _postulanteRepository.getDataCvConIdiomas(objPostulante);
                 //DataTable dtConOtros = _postulanteRepository.getDataCvConOtros(objPostulante);
                 //DataTable dtParientes = _postulanteRepository.getDataCvParientes(objPostulante);
                 //DataTable dtCvDiscapacidad = _postulanteRepository.getDataCvDiscapacidad(objPostulante);


                 //string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                 //string directoryPath = ConfigurationManager.AppSettings["ReportIntranetPath"];
                 //string nomReporte = "CvPostulanteReport.rpt";
                 //fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, nomReporte));

                 //rep.Load(fullPath);
                 //rep.Database.Tables["DtPostulante"].SetDataSource(dtPostulante);
                 //rep.Database.Tables["DtNivelAcademico"].SetDataSource(dtNivelAcademico);
                 //rep.Database.Tables["DtExperiencia"].SetDataSource(dtExperiencias);
                 //rep.Database.Tables["DtConOfimatica"].SetDataSource(dtConOfimatica);
                 //rep.Database.Tables["DtConIdioma"].SetDataSource(dtConIdiomas);
                 //rep.Database.Tables["DtConOtro"].SetDataSource(dtConOtros);
                 //rep.Database.Tables["DtPariente"].SetDataSource(dtParientes);
                 //rep.Database.Tables["DtDiscapacidad"].SetDataSource(dtCvDiscapacidad);

                 //mem = (MemoryStream)rep.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

             }
             catch (Exception)
             {
                 return MensajeError();
             }
             //return File(mem, "application/pdf");

         }


    }
}
