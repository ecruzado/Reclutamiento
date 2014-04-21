

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
    using System.Web.Security;
  
    
    
    public class ContrataController : BaseController
    {

        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ICvPostulanteRepository _cvPostulanteRepository;
        private IReclutamientoPersonaRepository _reclutamientoPersonaRepository;
        private IPostulanteRepository _postulanteRepository;
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;
        private IReemplazoRepository _reemplazoRepository;



        public ContrataController(IDetalleGeneralRepository detalleGeneralRepository,
                                         ISolReqPersonalRepository solReqPersonalRepository,
                                         ICvPostulanteRepository cvPostulanteRepository,
                                        IReclutamientoPersonaRepository reclutamientoPersonaRepository,
            IPostulanteRepository postulanteRepository,
            ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
            IReemplazoRepository reemplazoRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _cvPostulanteRepository = cvPostulanteRepository;
            _reclutamientoPersonaRepository = reclutamientoPersonaRepository;
            _postulanteRepository = postulanteRepository;
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _reemplazoRepository = reemplazoRepository;
        }
        
        /// <summary>
        /// obtiene los postulantes seleccionados
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

            if (Indicador.Si.Equals(rpta))
            {
                _reclutamientoPersonaRepository.FinalizaContratacion(objReCluta);

                objJson.Resultado = true;
                objJson.Mensaje = "Finalizo la solicitud";
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

            objReCluta.IdeSol = model.ReclutaPersonal.IdeSol;
            objReCluta.TipSol = model.ReclutaPersonal.TipSol;
            objReCluta.TipPuesto = model.ReclutaPersonal.TipPuesto;
            objReCluta.IdSede = model.ReclutaPersonal.IdSede;
            objReCluta.IdeCargo = model.ReclutaPersonal.IdeCargo;
            objReCluta.MotivoCierre = model.ReclutaPersonal.MotivoCierre;

            //actualiza el motivo de cierre
            objReCluta.MotivoCierre = model.ReclutaPersonal.MotivoCierre;

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

            objJson.Resultado = true;
            objJson.Mensaje = "Finalizo la solicitud";
            
            return Json(objJson);
        }


    }
}
