

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
        private ILogSolicitudNuevoCargoRepository _logSolNuevoRepository;
        private ILogSolicitudRequerimientoRepository _logSolReqRepository;
        

        public RankingController(IDetalleGeneralRepository detalleGeneralRepository,
                                 ISolReqPersonalRepository solReqPersonalRepository,
                                 ICvPostulanteRepository cvPostulanteRepository,
                                 IReclutamientoPersonaRepository reclutamientoPersonaRepository,
                                 IPostulanteRepository postulanteRepository,
                                 ILogSolicitudNuevoCargoRepository logSolNuevoRepository,
                                 ILogSolicitudRequerimientoRepository logSolReqRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _cvPostulanteRepository = cvPostulanteRepository;
            _reclutamientoPersonaRepository = reclutamientoPersonaRepository;
            _postulanteRepository = postulanteRepository;
            _logSolNuevoRepository = logSolNuevoRepository;
            _logSolReqRepository = logSolReqRepository;

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
                objJson.Mensaje = "Se elimino el registro";

            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se puede eliminiar el usuario";
            }


            return Json(objJson);
        }
        

        /// <summary>
        /// marca al postulante como no apto y no sera considerado 
        /// para el mismo puesto por un periodo de 6 meses
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult PostulanteNoApto(int id, int idPostulante, int idSede)
        {
            JsonMessage objJson = new JsonMessage();
            ReclutamientoPersona reclutaPersona = new ReclutamientoPersona();

            try
            {
                if (id > 0)
                {
                    reclutaPersona.IdePostulante = idPostulante;
                    reclutaPersona.IdSede = idSede;
                    reclutaPersona.IdeReclutaPersona = id;
                    reclutaPersona.EstPostulante = PostulanteEstado.NO_APTO;

                    _postulanteRepository.UpdateEstadoPostulante(reclutaPersona);

                    objJson.Resultado = true;
                    objJson.Mensaje = "Registro actualizado correctamente";

                }
                else
                {
                    objJson.Resultado = false;
                    objJson.Mensaje = "No se pudo actualizar el registro";
                }
                return Json(objJson);
            }
            catch (Exception ex)
            {
                objJson.Mensaje = "ERROR: "+ ex;
                objJson.Resultado = false;
                return Json(objJson);
            }
        }


        /// <summary>
        /// Realiza la validación del postulante que no esta como preSeleccionado en otra 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult ValidacionPreSeleccion(int id, int idPostulante, int idSede)
        {
            JsonMessage objJson = new JsonMessage();
            ReclutamientoPersona reclutaPersona = new ReclutamientoPersona();
            string indicadorPreSelec = "";

            try
            {
                reclutaPersona.IdePostulante = idPostulante;
                reclutaPersona.IdSede = idSede;

                indicadorPreSelec = _postulanteRepository.ValidaSeleccion(reclutaPersona);

                if (Indicador.No.Equals(indicadorPreSelec))
                {
                    objJson.Resultado = true;
                }
                else
                {
                    objJson.Resultado = false;
                    objJson.Mensaje = "El postulante se encuentra preseleccionado en otra solicitud";
                }


                return Json(objJson);
            }
            catch (Exception ex)
            {
                objJson.Resultado = false;
                objJson.Mensaje = "ERROR: "+ex;
                return Json(objJson);
            }
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
                    objJson.Mensaje = "Se actualizo el registro";

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

                if ((objCvPost.HoraCita != null) && (objCvPost.Fechacita != null))
                {
                    if (objCvPost.Asistio == Indicador.Si)
                    {
                        objJson.Resultado = false;
                        objJson.Mensaje = "No puede desmarcar la cita, el  postulante ya asistió";
                    }
                    else
                    {
                        if (Indicador.Si.Equals(objCvPost.Citado))
                        {
                            objCvPost.Citado = Indicador.No;
                            _cvPostulanteRepository.Update(objCvPost);
                            objJson.Mensaje = "Se anuló la cita satisfactoriamente";
                        }
                        else
                        {
                            objCvPost.Citado = Indicador.Si;
                            _cvPostulanteRepository.Update(objCvPost);
                            objJson.Mensaje = "Se realizó la cita satisfactoriamente";
                        }

                        objJson.Resultado = true;
                    }
                }
                else
                {
                    objJson.Resultado = false;
                    objJson.Mensaje = "No se puede realizar la cita debido a que no se cuenta con los datos de fecha y hora de cita";
                }
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

                if (objCvPost.Citado == Indicador.Si)
                {

                    if (Indicador.Si.Equals(objCvPost.Asistio))
                    {
                        objCvPost.Asistio = Indicador.No;
                        _cvPostulanteRepository.Update(objCvPost);
                        objJson.Mensaje = "Se anuló la asistencia satisfactoriamente";
                    }
                    else
                    {
                        objCvPost.Asistio = Indicador.Si;
                        _cvPostulanteRepository.Update(objCvPost);
                        objJson.Mensaje = "Se marcó la asistencia correctamente";
                    }

                    objJson.Resultado = true;
                }
                else
                {
                    objJson.Resultado = false;
                    objJson.Mensaje = "No se puede marcar debido a que el postulante no fue Citado";
                }

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
                    objJson.Mensaje = "Se anulo la marca en contactado";
                }
                else
                {
                    objReclutamientoPer.IndContactado = Indicador.Si;
                    _reclutamientoPersonaRepository.Update(objReclutamientoPer);
                    objJson.Mensaje = "Postulante contactado";
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

             int idUsuarioSession = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
             model = new RankingViewModel();
             model.Solicitud = new SolReqPersonal();
             model.ReclutaPersonal = new ReclutamientoPersona();

             LogSolicitudNuevoCargo logNuevo = new LogSolicitudNuevoCargo();
             LogSolReqPersonal logReqCargo = new LogSolReqPersonal();
        
             if (tipSol == TipoSolicitud.Nuevo)
             {
                 logNuevo = _logSolNuevoRepository.getFirthValue(x => x.IdeSolicitudNuevoCargo == id);
             }
             else
             {
                 logReqCargo = _logSolReqRepository.getFirthValue(x => x.IdeLogSolReqPersonal == id);
             }

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
                 model.btnSeleccionar = "N";
                 model.btnExcluir = "S";
                 model.btnContactado = "S";
                 model.btnContratar = "S";

             }
             else if (Roles.Encargado_Seleccion.Equals(idRol))
             {
                 model.btnEvaluaciones = "S";
                 model.btnSeleccionar = "N";
                 model.btnExcluir = "S";
                 model.btnContactado = "S";
                 model.btnContratar = "S";

             }else if(Roles.Gerente_General_Adjunto.Equals(idRol))
             {
                 model.btnEvaluaciones = "S";
                 model.btnContratar = "S";
                
             }
             else if (Roles.Gerente.Equals(idRol))
             {
                 model.btnEvaluaciones = "S";
                 model.btnContratar = "S";

             }
             else if (Roles.Jefe.Equals(idRol))
             {
                 model.btnEvaluaciones = "S";
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

             if (tipSol == TipoSolicitud.Nuevo)
             {
                 if ((logNuevo.RolResponsable == idRol) && (logNuevo.UsuarioResponsable == idUsuarioSession))
                 {
                     model.btnSeleccionar = "S";
                 }
                 else
                 {
                     model.btnSeleccionar = "N";
                 }
             }
             else if ((tipSol == TipoSolicitud.Ampliacion) || (tipSol == TipoSolicitud.Remplazo))
             {
                 if ((logReqCargo.RolResponsable == idRol) && (logReqCargo.UsResponsable == idUsuarioSession))
                 {
                     model.btnSeleccionar = "S";
                 }
                 else
                 {
                     model.btnSeleccionar = "N";
                 }
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
                                (item.PtoTotal==-1)||(item.PtoTotal==null)?"0":item.PtoTotal.ToString(),
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
                 List<CvPostulante> ListaCvExperiencia = new List<CvPostulante>();
                 List<CvPostulante> ListaCvEst = new List<CvPostulante>();
                 List<CvPostulante> ListaCvConOfi = new List<CvPostulante>();
                 

                 listaCvPostulante = _postulanteRepository.ListaCvPostulante(objCvPostulante);
                 ListaCvExperiencia = _postulanteRepository.ListaCvExperiencia(objCvPostulante);
                 ListaCvEst = _postulanteRepository.ListaCvEstudios(objCvPostulante);
                 ListaCvConOfi = _postulanteRepository.ListaCvConocOfimatica(objCvPostulante);
                 


                 objCvPostulante = new CvPostulante();

                 objCvPostulante = listaCvPostulante[0];

                 Font normal = new Font(FontFactory.GetFont("Arial", 10, Font.NORMAL));
                 Font normal8 = new Font(FontFactory.GetFont("Arial", 8, Font.NORMAL));
                 
                 Font cursiva = new Font(FontFactory.GetFont("Arial", 10, Font.ITALIC));
                 BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                 Font estiloBlanco = new Font(bfTimes, 10, Font.ITALIC, BaseColor.WHITE);
                 Font negrita = new Font(FontFactory.GetFont("Arial", 10, Font.BOLD));

                 Font negrita12 = new Font(FontFactory.GetFont("Arial", 12, Font.BOLD));

                 Font negritaGray = new Font(FontFactory.GetFont("Arial", 10, Font.BOLD,BaseColor.GRAY));

                 Font subRayado = new Font(FontFactory.GetFont("Arial", 10, Font.UNDERLINE));

                 using (MemoryStream ms = new MemoryStream())
                 {
                     Document doc = new Document(PageSize.A4, 5, 5, 30, 30);
                     PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                     //se abre el documento antes de empezar
                     doc.Open();
                     
                                          
                     PdfPTable tableCabecera = new PdfPTable(3);
                     float[] anchosCabecera = new float[] { 5.0f,65.0f, 30.0f };
                     tableCabecera.SetWidths(anchosCabecera);

                     PdfPTable tableCabTexto = new PdfPTable(5);
                     float[] anchosCabTexto = new float[] { 50.0f, 30.0f, 10.0f, 5.0f, 5.0f };
                     tableCabTexto.SetWidths(anchosCabTexto);


                     string nombreCompleto = (objCvPostulante.Nombrecompleto == null ? "" : objCvPostulante.Nombrecompleto);
                     string desEdad = (objCvPostulante.Desedad == null ? "" : objCvPostulante.Desedad);
                     string edad = (objCvPostulante.Edad == null ? "" : objCvPostulante.Edad);
                     string desEstadoCivil = (objCvPostulante.Desestadocivil == null ? "" : objCvPostulante.Desestadocivil);
                     string desdir = (objCvPostulante.Desdir == null ? "" : objCvPostulante.Desdir);
                     string fonoFijo = (objCvPostulante.Telfijo == null ? "" : objCvPostulante.Telfijo);
                     string fonoMovil = (objCvPostulante.Telmovil == null ? "" : objCvPostulante.Telmovil);
                     string fonoConcatenado = "";
                     string salario = (objCvPostulante.Salario == null ? "" : objCvPostulante.Salario);
                     //datos personales
                     string NumDoc = (objCvPostulante.Numdocumento == null ? "" : objCvPostulante.Numdocumento);
                     string email = (objCvPostulante.Correo == null ? "" : objCvPostulante.Correo);
                     string modificacion = (objCvPostulante.Modificacion == null ? "" : objCvPostulante.Modificacion);

                     
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


                     // Modificacion
                     PdfPTable tableInicial = new PdfPTable(1);
                     float[] anchosInicial = new float[] { 100.0f };
                     tableInicial.SetWidths(anchosInicial);

                     string imagepath = Server.MapPath(@"~/Content/images");
                     Image img = Image.GetInstance(imagepath + "/logo_sanpablo -prueba.png");
                     img.ScalePercent(80f);
                     PdfPCell logo = new PdfPCell(img);
                     logo.HorizontalAlignment = Element.ALIGN_LEFT;
                     // se quitan los bordes de la celda
                     logo.Border = Rectangle.NO_BORDER;
                     // se agrega la celda a la tabla
                     tableInicial.AddCell(logo);


                     PdfPCell CellInicialBlanco = new PdfPCell(new Phrase("xD", estiloBlanco));
                     CellInicialBlanco.Border = Rectangle.NO_BORDER;
                     CellInicialBlanco.HorizontalAlignment = Element.ALIGN_LEFT;
                     tableInicial.AddCell(CellInicialBlanco);
                     
                     
                     PdfPCell CellInicial = new PdfPCell(new Phrase("ACTUALIZADO AL "+modificacion, normal8));
                     CellInicial.Border = Rectangle.NO_BORDER;
                     CellInicial.HorizontalAlignment = Element.ALIGN_LEFT;
                     tableInicial.AddCell(CellInicial);






                     doc.Add(tableInicial);



                     // celda 01 fila 01
                     PdfPCell CellNombreCompleto = new PdfPCell(new Phrase(nombreCompleto, negrita12));
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
                     PdfPCell CellEdad = new PdfPCell(new Phrase(edad + " AÑOS " + desEstadoCivil, normal));
                     CellEdad.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellEdad.Border = Rectangle.NO_BORDER;
                     CellEdad.Colspan = 4;
                     tableCabTexto.AddCell(CellEdad);
                     // celda 01 fila 03
                     //PdfPCell CellEstadoCivil = new PdfPCell(new Phrase(desEstadoCivil, normal));
                     //CellEstadoCivil.HorizontalAlignment = Element.ALIGN_LEFT;
                     //CellEstadoCivil.Border = Rectangle.NO_BORDER;
                     //CellEstadoCivil.Colspan = 3;
                     //CellEstadoCivil.SpaceCharRatio = 4f;
                     //tableCabTexto.AddCell(CellEstadoCivil);

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

                     //PdfPCell CellObserv = new PdfPCell(new Phrase(observacion, normal));
                     //CellObserv.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                     //CellObserv.Border = Rectangle.NO_BORDER;
                     //CellObserv.Colspan = 5;
                     //CellObserv.SpaceCharRatio = 4f;
                     //tableCabTexto.AddCell(CellObserv);



                     PdfPTable tableCabImage = new PdfPTable(1);
                     float[] anchosCabImage= new float[] { 100.0f};
                     tableCabImage.SetWidths(anchosCabImage);

                     PdfPCell CellFotoPostulante = null;

                     if (objCvPostulante.Fotopostulante != null)
                     {
                         Image FotoPostulante = Image.GetInstance(objCvPostulante.Fotopostulante);
                         FotoPostulante.ScaleToFit(100f, 100f);

                         CellFotoPostulante = new PdfPCell(FotoPostulante);
                         CellFotoPostulante.HorizontalAlignment = Element.ALIGN_CENTER;
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

                     PdfPCell CellBlanco = new PdfPCell(new Phrase("xD", estiloBlanco));
                     CellBlanco.Border = Rectangle.NO_BORDER;

                     tableCabecera.AddCell(CellBlanco);
                     tableCabecera.AddCell(CellCabTexto);
                     tableCabecera.AddCell(CellCabImage);

                     tableCabecera.SpacingBefore = 5f;
                     tableCabecera.SpacingAfter = 5f;

                     doc.Add(tableCabecera);


                     //tabla obserbaciones inicio

                     PdfPTable tableObservaciones = new PdfPTable(1);
                     float[] anchosObservaciones = new float[] { 100.0f };
                     tableObservaciones.SetWidths(anchosObservaciones);

                     PdfPCell CellCabObservaciones = new PdfPCell(new Phrase(observacion, normal));
                     CellCabObservaciones.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                     CellCabObservaciones.Border = Rectangle.NO_BORDER;
                     tableObservaciones.AddCell(CellCabObservaciones);
                     tableObservaciones.SpacingBefore = 5f;
                     tableObservaciones.SpacingAfter = 5f;

                     doc.Add(tableObservaciones);

                     //tabla observacion fin


                     // incicio experiencias
                     PdfPTable tableExperiencia = new PdfPTable(3);
                     float[] anchosExperiencia = new float[] { 5.0f,65.0f, 30.0f };
                     tableExperiencia.SetWidths(anchosExperiencia);

                     PdfPCell CellCabExperiencia = new PdfPCell(new Phrase("EXPERIENCIA", negrita));
                     CellCabExperiencia.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellCabExperiencia.Border = Rectangle.NO_BORDER;
                     CellCabExperiencia.Colspan = 3;
                     tableExperiencia.AddCell(CellCabExperiencia);


                     foreach (CvPostulante item in ListaCvExperiencia)
                     {


                         PdfPCell CellBlanco1 = new PdfPCell(new Phrase("xD", estiloBlanco));
                         CellBlanco1.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellBlanco1.Border = Rectangle.NO_BORDER;
                         CellBlanco1.Colspan = 3;
                         tableExperiencia.AddCell(CellBlanco1);

                         PdfPCell CellExpBlanco = new PdfPCell(new Phrase("xD", estiloBlanco));
                         CellExpBlanco.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellExpBlanco.Border = Rectangle.NO_BORDER;
                         tableExperiencia.AddCell(CellExpBlanco);

                         PdfPCell CellNombreEmpresa = new PdfPCell(new Phrase(item.Nomempresa, negrita));
                         CellNombreEmpresa.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellNombreEmpresa.Border = Rectangle.NO_BORDER;
                         tableExperiencia.AddCell(CellNombreEmpresa);

                         PdfPCell CellFechaEmpresa = new PdfPCell(new Phrase(item.Fectrabajo, cursiva));
                         CellFechaEmpresa.HorizontalAlignment = Element.ALIGN_RIGHT;
                         CellFechaEmpresa.Border = Rectangle.NO_BORDER;
                         tableExperiencia.AddCell(CellFechaEmpresa);


                         PdfPCell CellExpBlanco2 = new PdfPCell(new Phrase("", negrita));
                         CellExpBlanco2.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellExpBlanco2.Border = Rectangle.NO_BORDER;
                         tableExperiencia.AddCell(CellExpBlanco2);


                         PdfPCell CellExpCargo = new PdfPCell(new Phrase(item.CargoExp, negritaGray));
                         CellExpCargo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellExpCargo.Border = Rectangle.NO_BORDER;
                         CellExpCargo.Colspan = 2;
                         tableExperiencia.AddCell(CellExpCargo);


                         PdfPCell CellExpBlanco3 = new PdfPCell(new Phrase("", negrita));
                         CellExpBlanco3.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellExpBlanco3.Border = Rectangle.NO_BORDER;
                         tableExperiencia.AddCell(CellExpBlanco3);


                         PdfPCell CellExpFunciones = new PdfPCell(new Phrase(item.Fucniones, normal));
                         CellExpFunciones.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellExpFunciones.Border = Rectangle.NO_BORDER;
                         CellExpFunciones.Colspan = 2;
                         tableExperiencia.AddCell(CellExpFunciones);

                     }

                     doc.Add(tableExperiencia);

                     PdfPTable tableEstudio = new PdfPTable(3);
                     float[] anchosEstudio = new float[] { 5.0f, 65.0f, 30.0f };
                     tableEstudio.SetWidths(anchosEstudio);

                     PdfPCell CellBlancoEstudio = new PdfPCell(new Phrase("xD", estiloBlanco));
                     CellBlancoEstudio.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellBlancoEstudio.Border = Rectangle.NO_BORDER;
                     CellBlancoEstudio.Colspan = 3;
                     tableEstudio.AddCell(CellBlancoEstudio);


                     PdfPCell CellCabEstudio = new PdfPCell(new Phrase("ESTUDIOS", negrita));
                     CellCabEstudio.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellCabEstudio.Border = Rectangle.NO_BORDER;
                     CellCabEstudio.Colspan = 3;
                     tableEstudio.AddCell(CellCabEstudio);


                     foreach (CvPostulante ItemEstudio in ListaCvEst)
	                    {
                            PdfPCell CellBlanco1 = new PdfPCell(new Phrase("xD", estiloBlanco));
                            CellBlanco1.HorizontalAlignment = Element.ALIGN_LEFT;
                            CellBlanco1.Border = Rectangle.NO_BORDER;
                            CellBlanco1.Colspan = 3;
                            tableEstudio.AddCell(CellBlanco1);

                            PdfPCell CellEstBlanco = new PdfPCell(new Phrase("xD", estiloBlanco));
                            CellEstBlanco.HorizontalAlignment = Element.ALIGN_LEFT;
                            CellEstBlanco.Border = Rectangle.NO_BORDER;
                            tableEstudio.AddCell(CellEstBlanco);

                            PdfPCell CellEstInstitucion = new PdfPCell(new Phrase(ItemEstudio.Institucion, negrita));
                            CellEstInstitucion.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                            CellEstInstitucion.Border = Rectangle.NO_BORDER;
                            tableEstudio.AddCell(CellEstInstitucion);

                            PdfPCell CellFechaEstudio = new PdfPCell(new Phrase(ItemEstudio.Fecestudio, cursiva));
                            CellFechaEstudio.HorizontalAlignment = Element.ALIGN_RIGHT;
                            CellFechaEstudio.Border = Rectangle.NO_BORDER;
                            tableEstudio.AddCell(CellFechaEstudio);


                            PdfPCell CellExpBlanco2 = new PdfPCell(new Phrase("", negrita));
                            CellExpBlanco2.HorizontalAlignment = Element.ALIGN_LEFT;
                            CellExpBlanco2.Border = Rectangle.NO_BORDER;
                            tableEstudio.AddCell(CellExpBlanco2);


                            PdfPCell CellNivelEstudio = new PdfPCell(new Phrase(ItemEstudio.Nivelestudio, negritaGray));
                            CellNivelEstudio.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                            CellNivelEstudio.Border = Rectangle.NO_BORDER;
                            CellNivelEstudio.Colspan = 2;
                            tableEstudio.AddCell(CellNivelEstudio);


                            PdfPCell CellExpBlanco3 = new PdfPCell(new Phrase("", negrita));
                            CellExpBlanco3.HorizontalAlignment = Element.ALIGN_LEFT;
                            CellExpBlanco3.Border = Rectangle.NO_BORDER;
                            tableEstudio.AddCell(CellExpBlanco3);


                            PdfPCell CellAreaEstudio = new PdfPCell(new Phrase(ItemEstudio.Areaestudio, normal));
                            CellAreaEstudio.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                            CellAreaEstudio.Border = Rectangle.NO_BORDER;
                            CellAreaEstudio.Colspan = 2;
                            tableEstudio.AddCell(CellAreaEstudio);


                            PdfPCell CellExpBlanco4 = new PdfPCell(new Phrase("", negrita));
                            CellExpBlanco4.HorizontalAlignment = Element.ALIGN_LEFT;
                            CellExpBlanco4.Border = Rectangle.NO_BORDER;
                            tableEstudio.AddCell(CellExpBlanco4);


                            PdfPCell CellEstNivelAlcanzado = new PdfPCell(new Phrase(ItemEstudio.Nivelalcanzado, normal));
                            CellEstNivelAlcanzado.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                            CellEstNivelAlcanzado.Border = Rectangle.NO_BORDER;
                            CellEstNivelAlcanzado.Colspan = 2;
                            tableEstudio.AddCell(CellEstNivelAlcanzado);
	                    }

                     doc.Add(tableEstudio);

                     //Salario
                     PdfPTable tableSalario = new PdfPTable(3);
                     float[] anchosSalario = new float[] { 5.0f, 65.0f, 30.0f };
                     tableSalario.SetWidths(anchosSalario);

                     PdfPCell CellBlancoSalario = new PdfPCell(new Phrase("xD", estiloBlanco));
                     CellBlancoSalario.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellBlancoSalario.Border = Rectangle.NO_BORDER;
                     CellBlancoSalario.Colspan = 3;
                     tableSalario.AddCell(CellBlancoSalario);

                     PdfPCell CellCabSalario = new PdfPCell(new Phrase("PREFERENCIAS SALARIALES", negrita));
                     CellCabSalario.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellCabSalario.Border = Rectangle.NO_BORDER;
                     CellCabSalario.Colspan = 3;
                     tableSalario.AddCell(CellCabSalario);


                     PdfPCell CellExpBlanco5 = new PdfPCell(new Phrase("xD", estiloBlanco));
                     CellExpBlanco5.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellExpBlanco5.Border = Rectangle.NO_BORDER;
                     CellExpBlanco5.Colspan = 3;
                     tableSalario.AddCell(CellExpBlanco5);


                     if (salario!=null && salario!="")
                     {
                         PdfPCell CellSalarioInicio = new PdfPCell(new Phrase("", normal));
                         CellSalarioInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellSalarioInicio.Border = Rectangle.NO_BORDER;
                         tableSalario.AddCell(CellSalarioInicio);

                         PdfPCell CellSalario = new PdfPCell(new Phrase("SALARIO NETO: " + salario, normal));
                         CellSalario.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellSalario.Border = Rectangle.NO_BORDER;
                         CellSalario.Colspan = 2;
                         tableSalario.AddCell(CellSalario);
                     }

                  

                     doc.Add(tableSalario);
                     // fin salario

                     // conocimientos
                     PdfPTable tableConc = new PdfPTable(3);
                     float[] anchosConc = new float[] { 5.0f, 65.0f, 30.0f };
                     tableConc.SetWidths(anchosConc);

                     PdfPCell CellBlancoConc = new PdfPCell(new Phrase("xD", estiloBlanco));
                     CellBlancoConc.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellBlancoConc.Border = Rectangle.NO_BORDER;
                     CellBlancoConc.Colspan = 3;
                     tableConc.AddCell(CellBlancoConc);


                     PdfPCell CellCabConc = new PdfPCell(new Phrase("CONOCIMIENTOS", negrita));
                     CellCabConc.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellCabConc.Border = Rectangle.NO_BORDER;
                     CellCabConc.Colspan = 3;
                     tableConc.AddCell(CellCabConc);

                     foreach (CvPostulante itemCon in ListaCvConOfi)
                     {

                         PdfPCell CellBlanco1 = new PdfPCell(new Phrase("xD", estiloBlanco));
                         CellBlanco1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellBlanco1.Border = Rectangle.NO_BORDER;
                         CellBlanco1.Colspan = 3;
                         tableConc.AddCell(CellBlanco1);

                         PdfPCell CellEstBlanco = new PdfPCell(new Phrase("xD", estiloBlanco));
                         CellEstBlanco.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellEstBlanco.Border = Rectangle.NO_BORDER;
                         tableConc.AddCell(CellEstBlanco);

                         PdfPCell CellTipConc = new PdfPCell(new Phrase(itemCon.TipoConocimiento, negrita));
                         CellTipConc.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellTipConc.Border = Rectangle.NO_BORDER;
                         CellTipConc.Colspan = 3;
                         tableConc.AddCell(CellTipConc);


                         PdfPCell CellEstBlanco2 = new PdfPCell(new Phrase("xD", estiloBlanco));
                         CellEstBlanco2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellEstBlanco2.Border = Rectangle.NO_BORDER;
                         tableConc.AddCell(CellEstBlanco2);

                         PdfPCell CellDescConc = new PdfPCell(new Phrase(itemCon.DescripcionConocimiento, normal));
                         CellDescConc.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellDescConc.Border = Rectangle.NO_BORDER;
                         CellDescConc.Colspan = 2;
                         tableConc.AddCell(CellDescConc);

                         PdfPCell CellEstBlanco3 = new PdfPCell(new Phrase("xD", estiloBlanco));
                         CellEstBlanco3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellEstBlanco3.Border = Rectangle.NO_BORDER;
                         tableConc.AddCell(CellEstBlanco3);

                         PdfPCell CellTipNivelConc = new PdfPCell(new Phrase(itemCon.Tipnivelconocimiento, normal));
                         CellTipNivelConc.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellTipNivelConc.Border = Rectangle.NO_BORDER;
                         CellTipNivelConc.Colspan = 2;
                         tableConc.AddCell(CellTipNivelConc);

                     }

                     doc.Add(tableConc);

                     //datos personales
                     PdfPTable tableDatosPer = new PdfPTable(3);
                     float[] anchosDatosPer = new float[] { 5.0f, 65.0f, 30.0f };
                     tableDatosPer.SetWidths(anchosDatosPer);

                     PdfPCell CellBlancoPersona = new PdfPCell(new Phrase("xD", estiloBlanco));
                     CellBlancoPersona.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellBlancoPersona.Border = Rectangle.NO_BORDER;
                     CellBlancoPersona.Colspan = 3;
                     tableDatosPer.AddCell(CellBlancoPersona);

                     PdfPCell CellCabDatPersona = new PdfPCell(new Phrase("DATOS PERSONALES", negrita));
                     CellCabDatPersona.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellCabDatPersona.Border = Rectangle.NO_BORDER;
                     CellCabDatPersona.Colspan = 3;
                     tableDatosPer.AddCell(CellCabDatPersona);


                     PdfPCell CellCabBlanco1 = new PdfPCell(new Phrase("xD", estiloBlanco));
                     CellCabBlanco1.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellCabBlanco1.Border = Rectangle.NO_BORDER;
                     CellCabBlanco1.Colspan = 3;
                     tableDatosPer.AddCell(CellCabBlanco1);

                     if (NumDoc!=null && NumDoc!="")
                     {
                         PdfPCell CellDatDocumentoInicio = new PdfPCell(new Phrase("", normal));
                         CellDatDocumentoInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellDatDocumentoInicio.Border = Rectangle.NO_BORDER;
                         tableDatosPer.AddCell(CellDatDocumentoInicio);

                         PdfPCell CellDatDocumento = new PdfPCell(new Phrase("DOCUMENTO: " + NumDoc, normal));
                         CellDatDocumento.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellDatDocumento.Border = Rectangle.NO_BORDER;
                         CellDatDocumento.Colspan = 2;
                         tableDatosPer.AddCell(CellDatDocumento);
                     }
                    

                     if (desdir!=null && desdir!="")
                     {
                         PdfPCell CellDatDirInicio = new PdfPCell(new Phrase("", normal));
                         CellDatDirInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellDatDirInicio.Border = Rectangle.NO_BORDER;
                         tableDatosPer.AddCell(CellDatDirInicio);

                         PdfPCell CellDatDir = new PdfPCell(new Phrase("DIRECCION: " + desdir, normal));
                         CellDatDir.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellDatDir.Border = Rectangle.NO_BORDER;
                         CellDatDir.Colspan = 2;
                         tableDatosPer.AddCell(CellDatDir);
                     }
                   


                     if (fonoMovil!=null && fonoMovil!="")
                     {
                         PdfPCell CellDatFonoMovilInicio = new PdfPCell(new Phrase("", normal));
                         CellDatFonoMovilInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellDatFonoMovilInicio.Border = Rectangle.NO_BORDER;
                         tableDatosPer.AddCell(CellDatFonoMovilInicio);

                         PdfPCell CellDatFonoMovil = new PdfPCell(new Phrase("TELEFONO CELULAR: " + fonoMovil, normal));
                         CellDatFonoMovil.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellDatFonoMovil.Border = Rectangle.NO_BORDER;
                         CellDatFonoMovil.Colspan = 2;
                         tableDatosPer.AddCell(CellDatFonoMovil);
                     }

                    

                     if (fonoFijo!=null && fonoFijo!="")
                     {
                         PdfPCell CellDatFonoInicio = new PdfPCell(new Phrase("", normal));
                         CellDatFonoInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellDatFonoInicio.Border = Rectangle.NO_BORDER;
                         tableDatosPer.AddCell(CellDatFonoInicio);

                         PdfPCell CellDatFono = new PdfPCell(new Phrase("TELEFONO FIJO: " + fonoFijo, normal));
                         CellDatFono.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellDatFono.Border = Rectangle.NO_BORDER;
                         CellDatFono.Colspan = 2;
                         tableDatosPer.AddCell(CellDatFono);
                     }
                     

                     if (desEstadoCivil!=null && desEstadoCivil!="")
                     {
                         PdfPCell CellDatEstCivilInicio = new PdfPCell(new Phrase("", normal));
                         CellDatEstCivilInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellDatEstCivilInicio.Border = Rectangle.NO_BORDER;
                         tableDatosPer.AddCell(CellDatEstCivilInicio);


                         PdfPCell CellDatEstCivil = new PdfPCell(new Phrase("ESTADO CIVIL: " + desEstadoCivil, normal));
                         CellDatEstCivil.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellDatEstCivil.Border = Rectangle.NO_BORDER;
                         CellDatEstCivil.Colspan = 2;
                         tableDatosPer.AddCell(CellDatEstCivil);
                     }
                    

                     if (email!=null && email!="")
                     {
                         PdfPCell CellDatEmailInicio = new PdfPCell(new Phrase("", normal));
                         CellDatEmailInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellDatEmailInicio.Border = Rectangle.NO_BORDER;
                         tableDatosPer.AddCell(CellDatEmailInicio);


                         PdfPCell CellDatEmail = new PdfPCell(new Phrase("EMAIL: " + email, normal));
                         CellDatEmail.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellDatEmail.Border = Rectangle.NO_BORDER;
                         CellDatEmail.Colspan = 2;
                         tableDatosPer.AddCell(CellDatEmail);
                     }
                     


                     doc.Add(tableDatosPer);
                     //Fin datos Personales

                     //Inicio Referencias

                     PdfPTable tableReferencia = new PdfPTable(3);
                     float[] anchosReferencia = new float[] { 5.0f, 65.0f, 30.0f };
                     tableReferencia.SetWidths(anchosReferencia);

                     PdfPCell CellCabRefInicial = new PdfPCell(new Phrase("xD", estiloBlanco));
                     CellCabRefInicial.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellCabRefInicial.Border = Rectangle.NO_BORDER;
                     CellCabRefInicial.Colspan = 3;
                     tableReferencia.AddCell(CellCabRefInicial);


                     PdfPCell CellCabReferencia = new PdfPCell(new Phrase("REFERENCIA", negrita));
                     CellCabReferencia.HorizontalAlignment = Element.ALIGN_LEFT;
                     CellCabReferencia.Border = Rectangle.NO_BORDER;
                     CellCabReferencia.Colspan = 3;
                     tableReferencia.AddCell(CellCabReferencia);


                     foreach (CvPostulante item3 in ListaCvExperiencia)
                     {


                         PdfPCell CellBlanco1 = new PdfPCell(new Phrase("xD", estiloBlanco));
                         CellBlanco1.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellBlanco1.Border = Rectangle.NO_BORDER;
                         CellBlanco1.Colspan = 3;
                         tableReferencia.AddCell(CellBlanco1);

                         PdfPCell CellExpBlanco = new PdfPCell(new Phrase("xD", estiloBlanco));
                         CellExpBlanco.HorizontalAlignment = Element.ALIGN_LEFT;
                         CellExpBlanco.Border = Rectangle.NO_BORDER;
                         tableReferencia.AddCell(CellExpBlanco);

                         PdfPCell CellNombreEmpresa = new PdfPCell(new Phrase(item3.Nomempresa, negrita));
                         CellNombreEmpresa.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                         CellNombreEmpresa.Border = Rectangle.NO_BORDER;
                         tableReferencia.AddCell(CellNombreEmpresa);

                         PdfPCell CellFechaEmpresa = new PdfPCell(new Phrase("xD", estiloBlanco));
                         CellFechaEmpresa.HorizontalAlignment = Element.ALIGN_RIGHT;
                         CellFechaEmpresa.Border = Rectangle.NO_BORDER;
                         tableReferencia.AddCell(CellFechaEmpresa);

                         if (item3.Nomreferente != null && item3.Nomreferente != "")
                         {
                             PdfPCell CellRefBlanco2 = new PdfPCell(new Phrase("", negrita));
                             CellRefBlanco2.HorizontalAlignment = Element.ALIGN_LEFT;
                             CellRefBlanco2.Border = Rectangle.NO_BORDER;
                             tableReferencia.AddCell(CellRefBlanco2);


                             PdfPCell CellRefNombre = new PdfPCell(new Phrase(item3.Nomreferente, normal));
                             CellRefNombre.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                             CellRefNombre.Border = Rectangle.NO_BORDER;
                             CellRefNombre.Colspan = 2;
                             tableReferencia.AddCell(CellRefNombre);
                         }

                         if (item3.Cargoreferente!=null && item3.Cargoreferente!="")
                         {
                             PdfPCell CellRefCargoInicio = new PdfPCell(new Phrase("", negrita));
                             CellRefCargoInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                             CellRefCargoInicio.Border = Rectangle.NO_BORDER;
                             tableReferencia.AddCell(CellRefCargoInicio);

                             PdfPCell CellRefCargo = new PdfPCell(new Phrase(item3.Cargoreferente, normal));
                             CellRefCargo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                             CellRefCargo.Border = Rectangle.NO_BORDER;
                             CellRefCargo.Colspan = 2;
                             tableReferencia.AddCell(CellRefCargo);
                         }


                         if (item3.Fonoreferente!=null && item3.Fonoreferente!="")
                         {
                             PdfPCell CellRefCelularInicio = new PdfPCell(new Phrase("", negrita));
                             CellRefCelularInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                             CellRefCelularInicio.Border = Rectangle.NO_BORDER;
                             tableReferencia.AddCell(CellRefCelularInicio);


                             PdfPCell CellRefCelular = new PdfPCell(new Phrase(item3.Fonoreferente, normal));
                             CellRefCelular.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                             CellRefCelular.Border = Rectangle.NO_BORDER;
                             CellRefCelular.Colspan = 2;
                             tableReferencia.AddCell(CellRefCelular);
                         }

                         


                         if (item3.Correoreferente!=null && item3.Correoreferente!="")
                         {
                             PdfPCell CellRefCorreoInicio = new PdfPCell(new Phrase("", negrita));
                             CellRefCorreoInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                             CellRefCorreoInicio.Border = Rectangle.NO_BORDER;
                             tableReferencia.AddCell(CellRefCorreoInicio);

                             PdfPCell CellRefCorreo = new PdfPCell(new Phrase(item3.Correoreferente, normal));
                             CellRefCorreo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                             CellRefCorreo.Border = Rectangle.NO_BORDER;
                             CellRefCorreo.Colspan = 2;
                             tableReferencia.AddCell(CellRefCorreo);
                         }
                       


                         if (item3.Fonoinst!=null && item3.Fonoinst!="")
                         {
                             PdfPCell CellRefInstitucionInicio = new PdfPCell(new Phrase("", negrita));
                             CellRefInstitucionInicio.HorizontalAlignment = Element.ALIGN_LEFT;
                             CellRefInstitucionInicio.Border = Rectangle.NO_BORDER;
                             tableReferencia.AddCell(CellRefInstitucionInicio);

                             PdfPCell CellRefInstitucion = new PdfPCell(new Phrase(item3.Fonoinst, normal));
                             CellRefInstitucion.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                             CellRefInstitucion.Border = Rectangle.NO_BORDER;
                             CellRefInstitucion.Colspan = 2;
                             tableReferencia.AddCell(CellRefInstitucion);
                         }
                         

                         



                     }

                     doc.Add(tableReferencia);





                     //fin
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
