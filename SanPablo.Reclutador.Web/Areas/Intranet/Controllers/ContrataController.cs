

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using FluentValidation;
    using FluentValidation.Results;
    using Newtonsoft.Json;
    using NHibernate.Criterion;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
   // using System.Web.Security;
    using SanPablo.Reclutador.Entity;
  
    
    
    public class ContrataController : BaseController
    {

        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ICvPostulanteRepository _cvPostulanteRepository;
        private IReclutamientoPersonaRepository _reclutamientoPersonaRepository;
        private IPostulanteRepository _postulanteRepository;
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;
        private IReemplazoRepository _reemplazoRepository;
        private ICargoRepository _cargoRepository;
        private IUsuarioRepository _usuarioRepository;
        private ISedeNivelRepository _sedeNivelRepository;
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;



        public ContrataController(IDetalleGeneralRepository detalleGeneralRepository,
                                         ISolReqPersonalRepository solReqPersonalRepository,
                                         ICvPostulanteRepository cvPostulanteRepository,
                                        IReclutamientoPersonaRepository reclutamientoPersonaRepository,
            IPostulanteRepository postulanteRepository,
            ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
            IReemplazoRepository reemplazoRepository,
            ICargoRepository cargoRepository,
            IUsuarioRepository usuarioRepository,
            ISedeNivelRepository sedeNivelRepository,
            IUsuarioRolSedeRepository usuarioRolSedeRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _cvPostulanteRepository = cvPostulanteRepository;
            _reclutamientoPersonaRepository = reclutamientoPersonaRepository;
            _postulanteRepository = postulanteRepository;
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _reemplazoRepository = reemplazoRepository;
            _cargoRepository = cargoRepository;
            _usuarioRepository = usuarioRepository;
            _sedeNivelRepository = sedeNivelRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
        }
        
        /// <summary>
        /// Inicializa la pantalla de contratacion, obtiene los postulantes seleccionados
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipSol"></param>
        /// <returns></returns>
        public ActionResult Index(int id, string tipSol, string pagina, string indPagina)
        {
            
           RankingViewModel model = new RankingViewModel();

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
           model.indPagina = indPagina;
           model.pagina = pagina;


           //accesos a los botones
           Int32 idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);

           if (Roles.Analista_Seleccion.Equals(idRol))
           {
               model.btnContratarPost = Visualicion.SI;
               model.btnFinalizarSol = Visualicion.SI;
               

           }
           else if (Roles.Encargado_Seleccion.Equals(idRol))
           {
               model.btnContratarPost = Visualicion.SI;
               model.btnFinalizarSol = Visualicion.SI;
           

           }
           else if (Roles.Administrador_Sistema.Equals(idRol))
           {
               model.btnContratarPost = Visualicion.SI;
               model.btnFinalizarSol = Visualicion.SI;

           }
           else
           {
               model.btnContratarPost = Visualicion.NO;
               model.btnFinalizarSol = Visualicion.NO;
             
           }





           return View("Index", model);
        }



        /// <summary>
        /// lista de postulantes seleccionados listos para contratacion
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListPostulantesSel(GridTable grid)
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

                lista = _postulanteRepository.GetPostulantesSeleccionados(reclutamientoPersona);

                
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
                                item.EstPostulante==null?"":item.EstPostulante.ToString(),
                                item.DesEstadoPostulante==null?"":item.DesEstadoPostulante
                                

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
        /// Valida la contratacion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="numVac"></param>
        /// <param name="idSol"></param>
        /// <param name="tipSol"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult ValidaContrata(int id, int numVac, int idSol, string tipSol)
        {
            JsonMessage objJson = new JsonMessage();
            try
            {
                List<ReclutamientoPersona> lista = (List<ReclutamientoPersona>)_reclutamientoPersonaRepository.GetBy(x => x.IdeSol == idSol
                                                            && x.TipSol == tipSol
                                                            && x.EstPostulante == PostulanteEstado.CONTRATADO);

                if (lista!=null)
                {
                    if (numVac>lista.Count)
                    {
                        objJson.Resultado = true;

                    }
                    else
                    {
                        objJson.Resultado = false;
                    }
                }
                else
                {
                    objJson.Resultado = false;
                }
                
                
            
            }
            catch (Exception)
            {

                objJson.Resultado = false;
                objJson.Mensaje = "Error";
            }
            return Json(objJson);
        
        }



        /// <summary>
        /// Realiza el cambio de estado a contratado validado por el numero de vacantes
        /// el numero de contrataciones no puede sobrepasar el numero de vacantes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult ContrataPost(int id, int numVac,int idSol,string tipSol)
        {
            JsonMessage objJson = new JsonMessage();

            try
            {
                List<ReclutamientoPersona> lista = (List<ReclutamientoPersona>)_reclutamientoPersonaRepository.GetBy(x => x.IdeSol == idSol
                                                            && x.TipSol == tipSol
                                                            && x.EstPostulante == PostulanteEstado.CONTRATADO);

                if (numVac >= lista.Count())
                {
                    var objReclutaPer = _reclutamientoPersonaRepository.GetSingle(x => x.IdeReclutaPersona == id);
                    objReclutaPer.EstPostulante = PostulanteEstado.CONTRATADO;
                    objReclutaPer.FecModifica = FechaModificacion;
                    objReclutaPer.UsrModifica = UsuarioActual.NombreUsuario;
                    _reclutamientoPersonaRepository.Update(objReclutaPer);


                    //actualiza a el reemplazo
                    if (TipoSolicitud.Remplazo.Equals(objReclutaPer.TipSol))
                    {
                        List<Reemplazo> ListaReemplazo = (List<Reemplazo>)_reemplazoRepository.GetBy(x => x.IdPostulante == null && x.IdeSolReqPersonal == objReclutaPer.IdeSol);
                        if (ListaReemplazo!=null && ListaReemplazo.Count>0)
                        {
                            Reemplazo objReemplazo = new Reemplazo();
                            objReemplazo = (Reemplazo)ListaReemplazo[0];

                            var obj = _reemplazoRepository.GetSingle(x => x.IdeSolReqPersonal == objReemplazo.IdeSolReqPersonal && x.IdPersona == objReemplazo.IdPersona);

                            obj.IdPostulante = objReclutaPer.IdePostulante;
                            _reemplazoRepository.Update(obj);
                        }

                    }



                    objJson.Resultado = true;
                    objJson.Mensaje = "Se actualizo el registro";
                }
                else
                {
                    objJson.Resultado = true;
                    objJson.Mensaje = "A excedido el número de vacantes";
                }    
            }
            catch (Exception)
            {

                objJson.Resultado = false;
                objJson.Mensaje = "Error";
            }
          


            return Json(objJson);
        }

        /// <summary>
        /// Finaliza el proceso de contratacion de postulante y cierra la solicitud,
        /// si hay solicitudes en cola realiza una migracion
        /// </summary>
        /// <param name="idSol"></param>
        /// <param name="tipSol"></param>
        /// <param name="tipPuesto"></param>
        /// <param name="idSede"></param>
        /// <param name="idCargo"></param>
        /// <returns></returns>
        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult FinalizaSol(int idSol, string tipSol, string tipPuesto, int idSede, int idCargo,int numVac)
        {

            JsonMessage objJson = new JsonMessage();
            string rpta = null;

            List<String> listSends = null;
            List<String> listCopys = null;
            string desRol = Convert.ToString(Session[ConstanteSesion.RolDes]);
            
            Int32 idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);

            ReclutamientoPersona objReCluta = new ReclutamientoPersona();

            objReCluta.IdeSol = idSol;
            objReCluta.TipSol = tipSol;
            objReCluta.TipPuesto = tipPuesto;
            objReCluta.IdSede = idSede;
            objReCluta.IdeCargo = idCargo;
            objReCluta.NumVacantes = numVac;

            objReCluta.idResponsable = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            objReCluta.idRolResponsable = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            objReCluta.idSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            objReCluta.idRolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);


            rpta = _reclutamientoPersonaRepository.validaFinSolicitud(objReCluta);

            System.Collections.ArrayList lista = new System.Collections.ArrayList();

            if (TipoSolicitud.Nuevo.Equals(objReCluta.TipSol))
            {
                var objSolNuevo1 = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == idSol);
                lista = listaEmail(Convert.ToInt32(objSolNuevo1.IdeSolicitudNuevoCargo), idRol, AccionEnvioEmail.Finalizar, idSede, TipoSolicitud.Nuevo);
            }
            else
            {
                var objSolReq1 = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == idSol);

                if (TipoSolicitud.Remplazo.Equals(objReCluta.TipSol))
                {
                    lista = listaEmail(Convert.ToInt32(objSolReq1.IdeSolReqPersonal), idRol, AccionEnvioEmail.Finalizar, idSede, TipoSolicitud.Remplazo);
                }

                if (TipoSolicitud.Ampliacion.Equals(objReCluta.TipSol))
                {
                    lista = listaEmail(Convert.ToInt32(objSolReq1.IdeSolReqPersonal), idRol, AccionEnvioEmail.Finalizar, idSede, TipoSolicitud.Ampliacion);
                }

            }

            listSends = new List<String>();
            listSends = (List<String>)lista[0];

            listCopys = new List<String>();
            listCopys = (List<String>)lista[1];


            if (Indicador.Si.Equals(rpta))
            {
                _reclutamientoPersonaRepository.FinalizaContratacion(objReCluta);

                objJson.Resultado = true;
                objJson.Mensaje = "Finalizo la solicitud";
                string plantilla = Server.MapPath(@"~/TemplateEmail/EnviarSolicitudFinalizada.htm");

                if (TipoSolicitud.Nuevo.Equals(objReCluta.TipSol))
                {
                    var objSolNuevo2 = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == idSol);
                    bool flag = EnviarCorreo(desRol, Etapa.Finalizado, "Nuevo", objSolNuevo2.NombreCargo, "" + idSol, listSends, listCopys, "",plantilla);

                }
                else
                {
                    var objSolReq2 = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == idSol);

                    if (TipoSolicitud.Remplazo.Equals(objReCluta.TipSol))
                    {
                        bool flag = EnviarCorreo(desRol, Etapa.Finalizado, "Reemplazo", objSolReq2.nombreCargo, "" + idSol, listSends, listCopys, "", plantilla);

                    }

                    if (TipoSolicitud.Ampliacion.Equals(objReCluta.TipSol))
                    {
                        bool flag = EnviarCorreo(desRol, Etapa.Finalizado, "Ampliación", objSolReq2.nombreCargo, "" + idSol, listSends, listCopys, "", plantilla);

                    }

                }

                //actualiza estado de la solicitud

                List<ReclutamientoPersona> listaReclutamiento = new List<ReclutamientoPersona>();

                listaReclutamiento = (List<ReclutamientoPersona>)_reclutamientoPersonaRepository.GetBy(x => x.IdeSol == objReCluta.IdeSol && x.TipSol == objReCluta.TipSol && x.TipPuesto == objReCluta.TipPuesto && x.IdSede == objReCluta.IdSede && x.IdeCargo == objReCluta.IdeCargo);

                Usuario objUsuario;
                SedeNivel objSedeNivel;
                if (listaReclutamiento!=null)
                {
                    if (listaReclutamiento.Count>0)
                    {
                        foreach (ReclutamientoPersona item in listaReclutamiento)
	                        {
		                        objUsuario = new Usuario();
                                
                                
                                var objUsuario2 = _usuarioRepository.GetSingle(x => x.IdePostulante == item.IdePostulante && x.TipUsuario == TipUsuario.Instranet);
                                objUsuario2.FlgEstado = IndicadorActivo.Inactivo;

                                _usuarioRepository.Update(objUsuario2);

                                objSedeNivel = new SedeNivel();
                                objSedeNivel.IDUSUARIO = objUsuario2.IdUsuario;

                                var objSedeNivel2 = _sedeNivelRepository.GetBy(x => x.IDUSUARIO == objSedeNivel.IDUSUARIO);

                                foreach (SedeNivel item2 in objSedeNivel2)
                                {
                                    _sedeNivelRepository.Remove(item2);
                                }


                                var objSedeRolUsuario = _usuarioRolSedeRepository.GetBy(x => x.IdUsuario == objUsuario2.IdUsuario);

                                foreach (UsuarioRolSede item3 in objSedeRolUsuario)
                                {

                                    item3.IdSede = 0;

                                    _usuarioRolSedeRepository.Update(item3);
                                }
                                
                                
	                        }

                        

                    }
                    
                }




            }
            else
            {
                objJson.Resultado = false;
            }
            

            return Json(objJson);
        }


        /// <summary>
        /// Inicializa al popup de las observaciones
        /// </summary>
        /// <param name="idSol"></param>
        /// <param name="tipSol"></param>
        /// <param name="tipPuesto"></param>
        /// <param name="idSede"></param>
        /// <param name="idCargo"></param>
        /// <param name="numVac"></param>
        /// <returns></returns>
        public ActionResult popupObserv(int id, string tipSol, string tipPuesto, int idSede, int idCargo)
        {

            RankingViewModel model = new RankingViewModel();
            model.ReclutaPersonal = new ReclutamientoPersona();

            model.ReclutaPersonal.IdeSol = id;
            model.ReclutaPersonal.TipSol = tipSol;
            model.ReclutaPersonal.TipPuesto = tipPuesto;
            model.ReclutaPersonal.IdeCargo = idCargo;
            model.ReclutaPersonal.IdSede = idSede;
           
            return View("popupObserv", model);
        }
        
        

        /// <summary>
        /// finaliza la solicitud con el comentario ingresado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FinalizaSolicitudObs(RankingViewModel model)
        {

            JsonMessage objJson = new JsonMessage();
            ReclutamientoPersona objReCluta = new ReclutamientoPersona();
            List<String> listSends = null;
                List<String> listCopys = null;
                string desRol = Convert.ToString(Session[ConstanteSesion.RolDes]);
             Int32 idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
             Int32 idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);

            objReCluta.IdeSol = model.ReclutaPersonal.IdeSol;
            objReCluta.TipSol = model.ReclutaPersonal.TipSol;
            objReCluta.TipPuesto = model.ReclutaPersonal.TipPuesto;
            objReCluta.IdSede = model.ReclutaPersonal.IdSede;
            objReCluta.IdeCargo = model.ReclutaPersonal.IdeCargo;
            objReCluta.MotivoCierre = model.ReclutaPersonal.MotivoCierre;

            //actualiza el motivo de cierre
            objReCluta.MotivoCierre = model.ReclutaPersonal.MotivoCierre;


            System.Collections.ArrayList lista = new System.Collections.ArrayList();

            if (TipoSolicitud.Nuevo.Equals(objReCluta.TipSol))
            {
                var objSolNuevo1 = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == model.ReclutaPersonal.IdeSol);
                lista = listaEmail(Convert.ToInt32(objSolNuevo1.IdeSolicitudNuevoCargo), idRol, AccionEnvioEmail.Finalizar, idSede, TipoSolicitud.Nuevo);
            }
            else
            {
                var objSolReq1 = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == model.ReclutaPersonal.IdeSol);

                if (TipoSolicitud.Remplazo.Equals(objReCluta.TipSol))
	            {
                    lista = listaEmail(Convert.ToInt32(objSolReq1.IdeSolReqPersonal), idRol, AccionEnvioEmail.Finalizar, idSede, TipoSolicitud.Remplazo);
	            }

                if (TipoSolicitud.Ampliacion.Equals(objReCluta.TipSol))
	            {
                    lista = listaEmail(Convert.ToInt32(objSolReq1.IdeSolReqPersonal), idRol, AccionEnvioEmail.Finalizar, idSede, TipoSolicitud.Ampliacion);
	            }

            }

            
            listSends = new List<String>();
            listSends = (List<String>)lista[0];

            listCopys = new List<String>();
            listCopys = (List<String>)lista[1];


            if (TipoSolicitud.Nuevo.Equals(objReCluta.TipSol))
            {
                var objSolNuevo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == model.ReclutaPersonal.IdeSol);
                objSolNuevo.MotivoCierre = model.ReclutaPersonal.MotivoCierre;
                _solicitudNuevoCargoRepository.Update(objSolNuevo);

            }
            else
            {
                var objSolReq =_solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == model.ReclutaPersonal.IdeSol);
                objSolReq.MotivoCierre=model.ReclutaPersonal.MotivoCierre;
                _solReqPersonalRepository.Update(objSolReq);

            }

            objReCluta.idResponsable = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            objReCluta.idRolResponsable = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            objReCluta.idSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            objReCluta.idRolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);


            _reclutamientoPersonaRepository.FinalizaContratacion(objReCluta);

            string plantilla = Server.MapPath(@"~/TemplateEmail/EnviarSolicitudFinalizada.htm");
            string comentario = model.ReclutaPersonal.MotivoCierre;



            if (TipoSolicitud.Nuevo.Equals(objReCluta.TipSol))
            {
                var objSolNuevo2 = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == model.ReclutaPersonal.IdeSol);
                bool flag = EnviarCorreo(desRol, Etapa.Finalizado, "Nuevo", objSolNuevo2.NombreCargo, "" + model.ReclutaPersonal.IdeSol, listSends, listCopys, comentario, plantilla);

            }
            else
            {
                var objSolReq2 = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == model.ReclutaPersonal.IdeSol);

                if (TipoSolicitud.Remplazo.Equals(objReCluta.TipSol))
                {
                    bool flag = EnviarCorreo(desRol, Etapa.Finalizado, "Reemplazo", objSolReq2.nombreCargo, "" + model.ReclutaPersonal.IdeSol, listSends, listCopys, comentario, plantilla);

                }

                if (TipoSolicitud.Ampliacion.Equals(objReCluta.TipSol))
                {
                    bool flag = EnviarCorreo(desRol, Etapa.Finalizado, "Ampliación", objSolReq2.nombreCargo, "" + model.ReclutaPersonal.IdeSol, listSends, listCopys, comentario, plantilla);

                }

            }

            List<ReclutamientoPersona> listaReclutamiento = new List<ReclutamientoPersona>();

            listaReclutamiento = (List<ReclutamientoPersona>)_reclutamientoPersonaRepository.GetBy(x => x.IdeSol == objReCluta.IdeSol && x.TipSol == objReCluta.TipSol && x.TipPuesto == objReCluta.TipPuesto && x.IdSede == objReCluta.IdSede && x.IdeCargo == objReCluta.IdeCargo);

            Usuario objUsuario;
            SedeNivel objSedeNivel;
            if (listaReclutamiento != null)
            {
                if (listaReclutamiento.Count > 0)
                {
                    foreach (ReclutamientoPersona item in listaReclutamiento)
                    {
                        objUsuario = new Usuario();


                        var objUsuario2 = _usuarioRepository.GetSingle(x => x.IdePostulante == item.IdePostulante && x.TipUsuario == TipUsuario.Instranet);
                        objUsuario2.FlgEstado = IndicadorActivo.Inactivo;

                        _usuarioRepository.Update(objUsuario2);

                        objSedeNivel = new SedeNivel();
                        objSedeNivel.IDUSUARIO = objUsuario2.IdUsuario;

                        var objSedeNivel2 = _sedeNivelRepository.GetBy(x => x.IDUSUARIO == objSedeNivel.IDUSUARIO);

                        foreach (SedeNivel item2 in objSedeNivel2)
                        {
                            _sedeNivelRepository.Remove(item2);
 
                        }

                        
                        var objSedeRolUsuario = _usuarioRolSedeRepository.GetBy(x => x.IdUsuario == objUsuario2.IdUsuario);

                        foreach (UsuarioRolSede item3 in objSedeRolUsuario)
                        {

                            item3.IdSede = 0;

                            _usuarioRolSedeRepository.Update(item3);
                        }

                    }



                }

            }


            objJson.Resultado = true;
            objJson.Mensaje = "Se finalizó exitosamente la solicitud de requerimiento";
            
            return Json(objJson);
        }

        /// <summary>
        /// obtiene la lista de Emails
        /// </summary>
        /// <param name="idSol">id de la solicitud</param>
        /// <param name="idRolSuceso">id del rol de la persona logueada</param>
        /// <param name="btnAccion">codigo de la accion del boton</param>
        /// <param name="idSede">id de la sede de la solicitud</param>
        /// <param name="TipoSol">tipo de solicitud</param>
        /// <returns></returns>
        public System.Collections.ArrayList listaEmail(int idSol, int idRolSuceso, string btnAccion, int idSede, string TipoSol)
        {
            EmailSol objEmailSol;
            List<EmailSol> listaRolxEmail;
            //List<EmailSol> listaEmialSend;
            //List<EmailSol> listaEmialCopy;

            List<String> listaSend;
            List<String> listaCopy;
            SolReqPersonal objSolReqPersonal;
            System.Collections.ArrayList ListaEmailEnvio = new System.Collections.ArrayList();


            objEmailSol = new EmailSol();
            listaRolxEmail = new List<EmailSol>();

            objEmailSol.IdSol = idSol;
            objEmailSol.IdRolSuceso = idRolSuceso;
            objEmailSol.TipSol = TipoSol;
            objEmailSol.AccionBoton = btnAccion;
            objEmailSol.idSede = idSede;

            //obtiene los roles de para  el envio de correo
            listaRolxEmail = _solReqPersonalRepository.GetRolxEmial(objEmailSol);
            listaSend = new List<String>();
            listaCopy = new List<String>();
            Boolean ind = false;

            string tipoReq = null;
            if (listaRolxEmail != null)
            {
                if (listaRolxEmail.Count > 0)
                {
                    foreach (EmailSol item in listaRolxEmail)
                    {
                        //obtiene la lista de send
                        ind = false;

                        if (item.RolSend != null)
                        {

                            if (item.RolSend.Equals("**"))
                            {
                                if (TipoSolicitud.Nuevo.Equals(TipoSol))
                                {
                                    var objSolNuevo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == idSol && x.EstadoActivo == IndicadorActivo.Activo);
                                    var idCargo = objSolNuevo.IdeCargo;

                                    var objCargo = _cargoRepository.GetSingle(x => x.IdeCargo == idCargo && x.EstadoActivo == IndicadorActivo.Activo);

                                    tipoReq = objCargo.TipoRequerimiento;
                                }
                                else
                                {
                                    var objSolReq = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == idSol && x.EstadoActivo == IndicadorActivo.Activo);
                                    if (objSolReq != null)
                                    {
                                        tipoReq = objSolReq.TipoRequerimiento;
                                    }


                                }

                                if (tipoReq != null)
                                {
                                    objSolReqPersonal = new SolReqPersonal();
                                    objSolReqPersonal = _solReqPersonalRepository.GetResponsable("U", idSede, tipoReq);
                                    var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == objSolReqPersonal.idUsuarioResp && x.FlgEstado == IndicadorActivo.Activo);

                                    ind = listaSend.Contains(objUsuario.Email);
                                    if (!ind)
                                    {
                                        listaSend.Add(objUsuario.Email);
                                    }
                                }

                            }
                            else
                            {

                                ind = listaSend.Contains(item.RolSend);
                                if (!ind)
                                {
                                    listaSend.Add(item.RolSend);
                                }

                            }



                        }

                        ind = false;
                        ind = listaCopy.Contains(item.RolCopy1);
                        if (!ind)
                        {
                            if (item.RolCopy1 != null && item.RolCopy1 != "")
                            {
                                listaCopy.Add(item.RolCopy1);
                            }
                        }

                        ind = false;
                        ind = listaCopy.Contains(item.RolCopy2);

                        if (!ind)
                        {
                            if (item.RolCopy2 != null && item.RolCopy2 != "")
                            {
                                listaCopy.Add(item.RolCopy2);
                            }
                        }

                        ind = false;
                        ind = listaCopy.Contains(item.RolCopy3);

                        if (!ind)
                        {
                            if (item.RolCopy3 != null && item.RolCopy3 != "")
                            {
                                listaCopy.Add(item.RolCopy3);
                            }
                        }

                        // obtiene la lista para las copias

                    }
                }
            }

            ListaEmailEnvio.Add(listaSend);
            ListaEmailEnvio.Add(listaCopy);
            return ListaEmailEnvio;

        }


        /// <summary>
        /// envia correo electronico a los responsables de la solicitud de requerimiento de personal
        /// </summary>
        /// <param name="usuarioDestinatario"></param>
        /// <param name="rolResponsable"></param>
        /// <param name="etapa"></param>
        /// <param name="observacion"></param>
        /// <param name="cargoDescripcion"></param>
        /// <param name="codCargo"></param>
        /// <returns></returns>
        public bool EnviarCorreo(string rolResponsable, string etapa, string tipoRq, string cargoDescripcion, string codCargo, List<String> Sends, List<String> Copys, string comentario, string plantilla)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            var usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            //var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitudFinalizada.htm");

            var dir = plantilla;

            try
            {
                SendMail enviarMail = new SendMail();
                enviarMail.Area = usuarioSession.AREADES;
                enviarMail.Sede = usuarioSession.SEDEDES;
                enviarMail.Rol = Session[ConstanteSesion.RolDes].ToString();

                var objUsuario = (Usuario)Session[ConstanteSesion.ObjUsuario];
 
                //enviarMail.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();
                if (objUsuario!=null)
                {
                    enviarMail.Usuario = objUsuario.DscNombres + " " + objUsuario.DscApePaterno + " " + objUsuario.DscApeMaterno;    
                }
                
                enviarMail.EnviarCorreoFinal(dir, etapa, rolResponsable, tipoRq, "", cargoDescripcion, codCargo, Sends, "suceso", Copys,comentario);
                return true;
            }
            catch (Exception Ex)
            {
                return false;

            }

        }

    }
}
