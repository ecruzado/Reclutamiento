﻿

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Areas.Intranet.Models;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Results;
    using NHibernate.Criterion;
    using NHibernate.Proxy;
   
    
    
    [Authorize]
    public class SolicitudCargoController : BaseController
    {


        /// <summary>
        /// Se inicializa los repositorios de base de datos
        /// </summary>
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IAreaRepository _areaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;
        private ITipoRequerimiento _tipoRequerimiento;
        private ICargoRepository _cargoRepository;
        private IConocimientoGeneralRequerimientoRepository _ConocimientoGeneralRequerimientoRepository;
        private INivelAcademicoRequerimientoRepository _nivelAcademicoRequerimientoRepository;
        private ICompetenciaRequerimientoRepository _competenciaRequerimientoRepository;
        private IExperienciaRequerimientoRepository _experienciaRequerimientoRepository;
        private IOfrecemosRequerimientoRepository _ofrecemosRequerimientoRepository;
        private IUsuarioRepository _usuarioRepository;
        private IRolRepository _rolRepository;
        private IReemplazoRepository _reemplazoRepository;
      
        public SolicitudCargoController( IDetalleGeneralRepository detalleGeneralRepository,
                                         ISolReqPersonalRepository solReqPersonalRepository,
                                         IDependenciaRepository dependenciaRepository,
                                         IAreaRepository areaRepository,
                                         IDepartamentoRepository departamentoRepository,
                                         IUsuarioRolSedeRepository usuarioRolSedeRepository,
                                         ITipoRequerimiento tipoRequerimiento,
                                         ICargoRepository cargoRepository,
            IConocimientoGeneralRequerimientoRepository conocimientoGeneralRequerimientoRepository,
            INivelAcademicoRequerimientoRepository nivelAcademicoRequerimientoRepository,
            ICompetenciaRequerimientoRepository CompetenciaRequerimientoRepository,
            IExperienciaRequerimientoRepository experienciaRequerimientoRepository,
            IOfrecemosRequerimientoRepository ofrecemosRequerimientoRepository,
            IUsuarioRepository usuarioRepository,
            IRolRepository rolRepository,
            IReemplazoRepository reemplazoRepository,
            ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _dependenciaRepository = dependenciaRepository;
            _areaRepository = areaRepository;
            _departamentoRepository = departamentoRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            _tipoRequerimiento = tipoRequerimiento;
            _cargoRepository = cargoRepository;
            _ConocimientoGeneralRequerimientoRepository = conocimientoGeneralRequerimientoRepository;
            _nivelAcademicoRequerimientoRepository = nivelAcademicoRequerimientoRepository;
            _competenciaRequerimientoRepository = CompetenciaRequerimientoRepository;
            _experienciaRequerimientoRepository = experienciaRequerimientoRepository;
            _ofrecemosRequerimientoRepository = ofrecemosRequerimientoRepository;
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            _reemplazoRepository = reemplazoRepository;
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
        }

        




      

        #region Reemplazo

        /// <summary>
        /// inicializa la busqueda de lista de reemplazo
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        [ValidarSesion]
        public ActionResult ListaReemplazo()
        {
            SolicitudRempCargoViewModel model;
            try
            {
                model = new SolicitudRempCargoViewModel();
                SedeNivel usuarioSede = new SedeNivel();

                

                var sede = Session[ConstanteSesion.Sede];
                if (sede!=null)
                {
                  
                  model = InicializarListaReemplazo(Convert.ToInt32(sede));
                  model.SolReqPersonal = new SolReqPersonal();

                  Cargo objCargo1 = new Cargo();

                  objCargo1.IdeSede = Convert.ToInt32(sede);


                  model.listaTipCargo = new List<Cargo>(_solReqPersonalRepository.GetCargoxSede(objCargo1));
                  model.listaTipCargo.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

                  var objUsuarioSede = Session[ConstanteSesion.UsuarioSede];
                  int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);

                  if (objUsuarioSede != null)
                  {
                      
                      usuarioSede = (SedeNivel)objUsuarioSede;
                  }


                  if (Roles.Jefe.Equals(idRol) || Roles.Gerente.Equals(idRol))
                  {

                      model.Departamentos = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == usuarioSede.IDEDEPENDENCIA
                                                                                             && x.EstadoActivo == IndicadorActivo.Activo));


                      model.Areas = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == usuarioSede.IDEDEPARTAMENTO
                                                                                && x.EstadoActivo == IndicadorActivo.Activo));

                      
                      
                      model.SolReqPersonal.IdeDependencia = usuarioSede.IDEDEPENDENCIA;
                      model.SolReqPersonal.IdeDepartamento = usuarioSede.IDEDEPARTAMENTO;
                      model.SolReqPersonal.IdeArea = usuarioSede.IDEAREA;

                      Cargo objCargo = new Cargo();

                      objCargo.IdeSede = Convert.ToInt32(sede);
                      objCargo.IdeDependencia = usuarioSede.IDEDEPENDENCIA;
                      objCargo.IdeDepartamento = usuarioSede.IDEDEPARTAMENTO;
                      objCargo.IdeArea = usuarioSede.IDEAREA;

                      model.listaTipCargo = new List<Cargo>(_solReqPersonalRepository.GetCargoxSede(objCargo));
                      model.listaTipCargo.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });


                  }



                  // accesos de botones
                  
                  model.idRol = idRol;
                    
                  if (idRol>0)
                  {
                      if (Roles.Analista_Seleccion.Equals(idRol))
                      {
                          model.btnRanking = "S";
                          model.btnPreselec = "S";
                          model.btnNuevo = "N";
                          model.btnRequerimiento = "S";
                          model.CampoEtapa = "S";

                          //model.CampoPopEliminar = "N";
                          //model.CampoPopEditar = "N";
                          //model.CampoTipPuesto = "N";
                          //model.Vacantes = "N";

                          
                      }

                      if (Roles.Encargado_Seleccion.Equals(idRol))
                      {
                          model.btnRanking = "S";
                          model.btnPreselec = "S";
                          model.btnNuevo = "N";
                          model.btnRequerimiento = "S";
                          model.CampoEtapa = "S";

                          //model.CampoPopEliminar = "S";
                          //model.CampoPopEditar = "S";
                          //model.CampoTipPuesto = "S";
                          //model.Vacantes = "S";
                         
                      }

                      if (Roles.Gerente_General_Adjunto.Equals(idRol))
                      {

                          model.btnRanking = "N";
                          model.btnPreselec = "S";
                          model.btnNuevo = "S";
                          model.btnRequerimiento = "S";
                          model.CampoEtapa = "N";

                          //model.CampoPopEliminar = "S";
                          //model.CampoPopEditar = "S";
                          //model.CampoTipPuesto = "S";
                          //model.Vacantes = "S";
                        
                      }

                      if (Roles.Jefe_Corporativo_Seleccion.Equals(idRol))
                      {

                          model.btnRanking = "N";
                          model.btnPreselec = "N";
                          model.btnNuevo = "N";
                          model.btnRequerimiento = "N";
                          model.CampoEtapa = "N";

                          //model.CampoPopEliminar = "N";
                          //model.CampoPopEditar = "N";
                          //model.CampoTipPuesto = "N";
                          //model.Vacantes = "N";

                      }

                      if (Roles.Gerente.Equals(idRol))
                      {

                          model.btnRanking = "N";
                          model.btnPreselec = "S";
                          model.btnNuevo = "S";
                          model.btnRequerimiento = "S";
                          model.CampoEtapa = "N";

                          //model.CampoPopEliminar = "S";
                          //model.CampoPopEditar = "S";
                          //model.CampoTipPuesto = "S";
                          //model.Vacantes = "S";

                      }

                      if (Roles.Jefe.Equals(idRol))
                      {

                          model.btnRanking = "N";
                          model.btnPreselec = "N";
                          model.btnNuevo = "S";
                          model.btnRequerimiento = "N";
                          model.CampoEtapa = "N";

                          //model.CampoPopEliminar = "S";
                          //model.CampoPopEditar = "S";
                          //model.CampoTipPuesto = "S";
                          //model.Vacantes = "S";

                      }

                      if (Roles.Administrador_Sistema.Equals(idRol))
                      {

                          model.btnRanking = "S";
                          model.btnPreselec = "S";
                          model.btnNuevo = "S";
                          model.btnRequerimiento = "S";
                          model.CampoEtapa = "S";

                          //model.CampoPopEliminar = "S";
                          //model.CampoPopEditar = "S";
                          //model.CampoTipPuesto = "S";
                          //model.Vacantes = "S";

                      }

                  }


                }
            }
            catch (Exception ex)
            {

                return MensajeError();
            }
            
            return View("ListaReemplazo", model);
        }

        /// <summary>
        /// lista de departamentos
        /// </summary>
        /// <param name="ideDependencia"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaDepartamentos(int ideDependencia)
        {
            ActionResult result = null;
            Dependencia objDepencia = new Dependencia();

            int idSede = (Session[ConstanteSesion.Sede] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Sede]));
            
            List<Departamento> listaResultado = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == ideDependencia && x.EstadoActivo == IndicadorActivo.Activo));

            foreach (Departamento item in listaResultado)
            {
                item.Dependencia = null;
            }

            result = Json(listaResultado);
            return result;
        }



        /// <summary>
        /// Elimina una Solicitud que ha sido creada y no trabajada
        /// </summary>
        /// <param name="idSol">id de la solicitud</param>
        /// <param name="cTipSol">tipo de solicitud</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EliminaSolicitud(int id, string Tipsol)
        {
            JsonMessage objJson = new JsonMessage();
           

            SolReqPersonal objSol = new SolReqPersonal();

            objSol.IdeSolReqPersonal = id;
            objSol.Tipsol = Tipsol;
            objSol.idRolSuceso = (Session[ConstanteSesion.Rol] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Rol]));
            objSol.idUsuarioSuceso = (Session[ConstanteSesion.Usuario] == null ? 0 : Convert.ToInt32(Session[ConstanteSesion.Usuario]));

            try
            {
                objSol = _solReqPersonalRepository.EliminaSol(objSol);
            
            }
            catch (Exception ex)
            {
               MensajeError(ex.Message);
            }

            objJson.Mensaje = objSol.Mensaje;

            if (objSol.IdRespuesta == 0)
            {
                objJson.Resultado = false;
            }
            if (objSol.IdRespuesta == 1)
            {
                objJson.Resultado = true;
            }

            return Json(objJson);
        }


        /// <summary>
        /// obtiene los cargos x sede
        /// </summary>
        /// <param name="idDependencia"></param>
        /// <param name="idDepartamento"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaCargoxSede(string idDependencia, string idDepartamento, string idArea)
        {
            ActionResult result = null;

           
            Cargo objCargo = new Cargo();

            var idSede = Session[ConstanteSesion.Sede];


            objCargo.IdeSede = Convert.ToInt32(idSede);
            objCargo.IdeDependencia = (idDependencia==null?0:Convert.ToInt32(idDependencia));
            objCargo.IdeDepartamento = (idDepartamento == null ? 0 : Convert.ToInt32(idDepartamento));
            objCargo.IdeArea = (idArea == null ? 0 : Convert.ToInt32(idArea));


            //var listaCargoxSede = new List<Cargo>(_reemplazoRepository.GetCargoxSede(objCargo));
            var listaCargoxSede = new List<Cargo>(_cargoRepository.GetCargoxSede(objCargo));

            result = Json(listaCargoxSede);
            return result;
        }


        /// <summary>
        /// lista de areas
        /// </summary>
        /// <param name="ideDepartamento"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult listaAreas(int ideDepartamento)
        {
            ActionResult result = null;

            var listaResultado = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == ideDepartamento));

            foreach (Area item in listaResultado)
            {
                item.Departamento = null;
            }
            
            result = Json(listaResultado);
            return result;
        }

        /// <summary>
        /// iniciliza la pantalla de busqueda de reemplazo
        /// </summary>
        /// <param name="idSel"></param>
        /// <returns></returns>
        public SolicitudRempCargoViewModel InicializarListaReemplazo(int idSel)
        {
            var model = new SolicitudRempCargoViewModel();

            model.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                         && x.IdeSede == idSel));
            model.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            model.Departamentos = new List<Departamento>();
            model.Departamentos.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            model.Areas = new List<Area>();
            model.Areas.Add(new Area { IdeArea = 0, NombreArea = "Seleccionar" });

            model.listaTipCargo = new List<Cargo>(_solReqPersonalRepository.GetTipCargo(0));
            model.listaTipCargo.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

            model.listaRol = new List<Rol>(_usuarioRolSedeRepository.GetListaRol(0));
            model.listaRol.Insert(0, new Rol { IdRol = 0, CodRol = "Seleccionar" });

            model.listaEtapas =
             new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEtapa));
            model.listaEtapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.listaEstados =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadosSolicitud));
            model.listaEstados.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.listaTipPuesto =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            model.listaTipPuesto.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);

            if (Roles.Analista_Seleccion.Equals(idRol))
            {
                model.listaEtapas = new List<DetalleGeneral>(_detalleGeneralRepository.GetBy(x => x.General.IdeGeneral == 50 && ((x.Valor == "03") || (x.Valor == "04") || (x.Valor == "08"))));
                model.listaEtapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            }

            if (Roles.Encargado_Seleccion.Equals(idRol))
            {
                model.listaEtapas = new List<DetalleGeneral>(_detalleGeneralRepository.GetBy(x => x.General.IdeGeneral == 50 && ((x.Valor == "03") || (x.Valor == "01") || (x.Valor == "04") || (x.Valor == "08"))));
                model.listaEtapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            }


            return model;
        }


        /// <summary>
        /// Lista de busqueda de reemplazo
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListBusquedaReemplazo(GridTable grid)
        {

            SolReqPersonal solReqPersonal;
            List<SolReqPersonal> lista = new List<SolReqPersonal>();
            try
            {
               
                    solReqPersonal = new SolReqPersonal();

                    solReqPersonal.IdeCargo = (grid.rules[1].data==null?0:Convert.ToInt32(grid.rules[1].data));
                    solReqPersonal.IdeDependencia = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));
                    solReqPersonal.IdeArea = (grid.rules[3].data == null ? 0 : Convert.ToInt32(grid.rules[3].data));
                    solReqPersonal.TipResponsable = (grid.rules[4].data == null ? "" : grid.rules[4].data);

                    if (grid.rules[5].data!=null && grid.rules[6].data!=null)
                    {
                        solReqPersonal.FechaInicioBus = Convert.ToDateTime(grid.rules[5].data);
                        solReqPersonal.FechaFinBus = Convert.ToDateTime(grid.rules[6].data);      
                    }
                  
                    solReqPersonal.IdeDepartamento = (grid.rules[7].data == null ? 0 : Convert.ToInt32(grid.rules[7].data));
                    solReqPersonal.TipEtapa = (grid.rules[8].data == null ? "" : grid.rules[8].data);
                    solReqPersonal.TipEstado = (grid.rules[9].data == null ? "" : grid.rules[9].data);

                    solReqPersonal.Tipsol = TipoSolicitud.Remplazo;


                    var IdRolResp = Session[ConstanteSesion.Rol];
                    solReqPersonal.IdRolResp = Convert.ToInt32(IdRolResp);

                    var idUsuarioResp = Session[ConstanteSesion.Usuario];
                    solReqPersonal.idUsuarioResp = Convert.ToInt32(idUsuarioResp);
    
                    var idSede = Session[ConstanteSesion.Sede];
                    
                    solReqPersonal.IdeSede = Convert.ToInt32(idSede);
                    lista = _solReqPersonalRepository.GetListaSolReqPersonal(solReqPersonal);
               

                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);
               
                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeSolReqPersonal.ToString(),
                    cell = new string[]
                            {
                               
                                "1",
                                item.TipEstado==null?"":item.TipEstado,
                                item.IdeSolReqPersonal==null?"":item.IdeSolReqPersonal.ToString(),
                                item.CodSolReqPersonal==null?"":item.CodSolReqPersonal.ToString(),
                                item.IdeCargo==null?"":item.IdeCargo.ToString(),
                                item.DesCargo==null?"":item.DesCargo,
                                item.IdeDependencia==null?"":item.IdeDependencia.ToString(),
                                item.Dependencia_des==null?"":item.Dependencia_des,
                                item.IdeDepartamento==null?"":item.IdeDepartamento.ToString(),
                                item.Departamento_des==null?"":item.Departamento_des,
                                item.IdeArea==null?"":item.IdeArea.ToString(),
                                item.Area_des==null?"":item.Area_des,
                                item.NumVacantes==null?"":item.NumVacantes.ToString(),
                                item.CantPostulante==null?"":item.CantPostulante.ToString(),
                                item.CantPreSelec==null?"":item.CantPreSelec.ToString(),
                                item.CantEvaluados==null?"":item.CantEvaluados.ToString(),
                                item.CantSeleccionados==null?"":item.CantSeleccionados.ToString(),
                                item.CantContratados==null?"":item.CantContratados.ToString(),
                                item.Feccreacion==null?"":String.Format("{0:dd/MM/yyyy}", item.Feccreacion),
                                item.FecExpiracacion==null?"":String.Format("{0:dd/MM/yyyy}", item.FecExpiracacion),
                               
                                item.idRolSuceso==null?"":item.idRolSuceso.ToString(),
                                item.DesRolSuceso==null?"":item.DesRolSuceso,
                                item.NomPersonReemplazo==null?"":item.NomPersonReemplazo,
                                
                                item.FlagPublicado==null?"":item.FlagPublicado,
                                item.TipEtapa==null?"":item.TipEtapa,
                                item.idUsuarioResp ==null?"":item.idUsuarioResp.ToString(),
                                item.Tipsol==null?"":item.Tipsol,
                                item.Des_etapa == null?"":item.Des_etapa

                               
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
        /// pagina para crear un nuevo reemplazo
        /// </summary>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult CrearReemplazo()
        {
            SolicitudRempCargoViewModel model;
            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            SedeNivel UsuarioSede;
            string Nombusuario=null;
            string codCodificado = null;
            string CodGenerado=null;

            DateTime hoy = DateTime.Today;
            
            model = new SolicitudRempCargoViewModel();
            
            var objUsuarioSede = Session[ConstanteSesion.UsuarioSede];
            var idSede = Session[ConstanteSesion.Sede];

            if (objUsuarioSede!=null)
            {
                UsuarioSede = new SedeNivel();
                UsuarioSede = (SedeNivel)objUsuarioSede;

                //incializa
                model = Inicializar(UsuarioSede);

                model.Pagina = TipoSolicitud.Remplazo;
                model.SolReqPersonal = new SolReqPersonal();
                model.SolReqPersonal.NumVacantes = 1;

                if (idRol == Roles.Gerente || idRol == Roles.Jefe)
                {
                    model.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                        && x.IdeSede == UsuarioSede.IDESEDE
                                                                        && x.IdeDependencia == UsuarioSede.IDEDEPENDENCIA
                                                                        ));
            

                    model.listaDepartamento = new List<Departamento>(_departamentoRepository.GetBy(x => x.IdeDepartamento == UsuarioSede.IDEDEPARTAMENTO
                                                                                              && x.Dependencia.IdeDependencia == UsuarioSede.IDEDEPENDENCIA
                                                                                              && x.EstadoActivo == IndicadorActivo.Activo));


                    model.listaArea = new List<Area>(_areaRepository.GetBy(x => x.IdeArea == UsuarioSede.IDEAREA
                                                                              && x.Departamento.IdeDepartamento == UsuarioSede.IDEDEPARTAMENTO
                                                                              && x.EstadoActivo == IndicadorActivo.Activo));

                    Cargo objCargo = new Cargo();

                    objCargo.IdeSede = Convert.ToInt32(idSede);
                    objCargo.IdeDependencia = UsuarioSede.IDEDEPENDENCIA;
                    objCargo.IdeDepartamento = UsuarioSede.IDEDEPARTAMENTO;
                    objCargo.IdeArea = UsuarioSede.IDEAREA;

                    model.listaTipCargo = new List<Cargo>(_cargoRepository.GetCargoxSede(objCargo));
                    model.listaTipCargo.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });
                
                }


                if (Roles.Gerente_General_Adjunto.Equals(idRol))
                {

                    model.listaDepartamento = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == UsuarioSede.IDEDEPENDENCIA
                                                                                             && x.EstadoActivo == IndicadorActivo.Activo));


                    model.listaArea = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == UsuarioSede.IDEDEPARTAMENTO
                                                                              && x.EstadoActivo == IndicadorActivo.Activo));

                    model.SolReqPersonal.IdeDependencia = UsuarioSede.IDEDEPENDENCIA;
                    model.SolReqPersonal.IdeDepartamento= UsuarioSede.IDEDEPARTAMENTO;
                    model.SolReqPersonal.IdeArea= UsuarioSede.IDEAREA;


                    Cargo objCargo = new Cargo();

                    objCargo.IdeSede = Convert.ToInt32(idSede);
                    objCargo.IdeDependencia = UsuarioSede.IDEDEPENDENCIA;
                    objCargo.IdeDepartamento = UsuarioSede.IDEDEPARTAMENTO;
                    objCargo.IdeArea = UsuarioSede.IDEAREA;

                    model.listaTipCargo = new List<Cargo>(_cargoRepository.listaCargosCompletos(objCargo.IdeSede));
                    model.listaTipCargo.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });
                }
                
            }
            
           

            model.Accion = Accion.Nuevo;
            var codUsuario = Convert.ToString(Session[ConstanteSesion.Usuario]);
            var codSede = Convert.ToString(Session[ConstanteSesion.Sede]);
            var codRol = Convert.ToString(Session[ConstanteSesion.Rol]);

            CodGenerado = codUsuario+codSede+codRol+String.Format("{0:MM/dd/yyyy}", hoy) + System.Guid.NewGuid().ToString();
            
            codCodificado = Base64Encode(CodGenerado);

            Session.Remove(ConstanteSesion.ListaReemplazo);
            Session.Remove(ConstanteSesion.codReqSolTemp);
            
            Session[ConstanteSesion.codReqSolTemp] = codCodificado;


            model.Accion = Accion.Enviar;
            model.idRol = Convert.ToInt32(codRol);



            if (idRol > 0)
            {
                if (Roles.Analista_Seleccion.Equals(idRol))
                {


                    model.CampoPopEliminar = "N";
                    model.CampoPopEditar = "N";
                    model.CampoTipPuesto = "N";
                    model.CampoNumVacantes = "N";


                }

                if (Roles.Encargado_Seleccion.Equals(idRol))
                {


                    model.CampoPopEliminar = "S";
                    model.CampoPopEditar = "S";
                    model.CampoTipPuesto = "S";
                    model.CampoNumVacantes = "S";

                }

                if (Roles.Gerente_General_Adjunto.Equals(idRol))
                {

                    model.CampoPopEliminar = "S";
                    model.CampoPopEditar = "S";
                    model.CampoTipPuesto = "S";
                    model.CampoNumVacantes = "S";

                }

                if (Roles.Jefe_Corporativo_Seleccion.Equals(idRol))
                {

                    model.CampoPopEliminar = "N";
                    model.CampoPopEditar = "N";
                    model.CampoTipPuesto = "N";
                    model.CampoNumVacantes = "N";

                }

                if (Roles.Gerente.Equals(idRol))
                {



                    model.CampoPopEliminar = "S";
                    model.CampoPopEditar = "S";
                    model.CampoTipPuesto = "S";
                    model.CampoNumVacantes = "S";

                }

                if (Roles.Jefe.Equals(idRol))
                {


                    model.CampoPopEliminar = "S";
                    model.CampoPopEditar = "S";
                    model.CampoTipPuesto = "S";
                    model.CampoNumVacantes = "S";

                }

                if (Roles.Administrador_Sistema.Equals(idRol))
                {

                    model.CampoPopEliminar = "S";
                    model.CampoPopEditar = "S";
                    model.CampoTipPuesto = "S";
                    model.CampoNumVacantes = "S";

                }

            }


            return View("InformacionReemplazo", model);
        }

        /// <summary>
        /// Inicializa la pagina inicial de reemplazo
        /// </summary>
        /// <returns></returns>
       
        private SolicitudRempCargoViewModel Inicializar(SedeNivel UsuarioSede)
        {
            var objModel = new SolicitudRempCargoViewModel();
            objModel.SolReqPersonal = new SolReqPersonal();

            objModel.SolReqPersonal.NumVacantes = 1; //iniciar en 1

            objModel.listaTipVacante=
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoVacante));
            objModel.listaTipVacante.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            objModel.listaTipPuesto =
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            objModel.listaTipPuesto.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            //inicio cargo x sede
            objModel.listaTipCargo = new List<Cargo>(_solReqPersonalRepository.GetTipCargo(0));
            objModel.listaTipCargo.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });
            //fin cargo x sede

            objModel.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                         && x.IdeSede == UsuarioSede.IDESEDE));

            objModel.listaDependencia.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

            objModel.listaDepartamento = new List<Departamento>();
            objModel.listaDepartamento.Add(new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

            objModel.listaArea = new List<Area>();
            objModel.listaArea.Add(new Area { IdeArea = 0, NombreArea = "Seleccionar" });

            objModel.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            objModel.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            objModel.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            objModel.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            objModel.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            objModel.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return objModel;
        }

       /// <summary>
       /// Incializa el popup de reemplazo
       /// </summary>
       /// <returns></returns>
        public ActionResult InicioPopupReemplazo(string id, string tipReemplazo, int idReemplazo) 
        {

            SolicitudRempCargoViewModel model;

            model = new SolicitudRempCargoViewModel();
            model.SolReqPersonal = new SolReqPersonal();

            model.Reemplazo = new Reemplazo();

            DateTime Hoy = DateTime.Today;
            
            //Edicion de reemplazo
            if (idReemplazo>0)
            {
                var objReemplazo = _reemplazoRepository.GetSingle(x => x.IdReemplazo == idReemplazo);
                if (objReemplazo != null)
                {
                    model.Reemplazo = (Reemplazo)objReemplazo;
                    model.Reemplazo.IdReemplazo = idReemplazo;
                }    
            }
            

           
            if (id != null && id.Length > 0)
            {
                model.SolReqPersonal.CodSolReqPersonal = id;
            }

            int codReemplazo = Convert.ToInt32(tipReemplazo);

            if (codReemplazo!=null)
            {
                if (codReemplazo == 5||codReemplazo == 6 || codReemplazo == 7 || codReemplazo == 8 || codReemplazo == 9 || codReemplazo == 10)
                {
                    // es un tipo de reemplazo
                    model.TipoReemplazo = "N";
                }
                else
                {
                    // es un tipo de suplencia
                    model.TipoReemplazo = "S";
                }
            }

            
            
            return View("PopupListaReemplazo",model);
        }
        
        /// <summary>
        /// obtiene los valores del popup
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetPopupReemplazo(SolicitudRempCargoViewModel model) 
        {

            JsonMessage objJson = new JsonMessage();
            Reemplazo objReemplazo;
            string mensaje ="";
            objReemplazo = new Reemplazo();
            objReemplazo = model.Reemplazo;

            if (model.Reemplazo.IdReemplazo != null && model.Reemplazo.IdReemplazo > 0)
            {
                var obj = _reemplazoRepository.GetSingle(x => x.IdReemplazo == model.Reemplazo.IdReemplazo);

                if (model.Reemplazo.FecFinalReemplazo!=null)
                {
                    obj.FecFinalReemplazo = model.Reemplazo.FecFinalReemplazo;    
                }
                if (model.Reemplazo.FecInicioReemplazo!=null)
                {
                    obj.FecInicioReemplazo = model.Reemplazo.FecInicioReemplazo;    
                }
                
                obj.ApePaterno = model.Reemplazo.ApePaterno;
                
                obj.Nombres = model.Reemplazo.Nombres;

                _reemplazoRepository.Update(obj);
               mensaje = "Se actualizo el registro";
                
            }
            else
            {
                var objSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == model.SolReqPersonal.CodSolReqPersonal && x.TipoSolicitud == TipoSolicitud.Remplazo);
                objReemplazo.IdeSolReqPersonal = Convert.ToInt32(objSol.IdeSolReqPersonal);
                objReemplazo.FechaCreacion = FechaSistema;
                objReemplazo.UsuarioCreacion = UsuarioActual.NombreUsuario;

                int dato = 0;
                dato = _solReqPersonalRepository.InsertTempReemplazo(objReemplazo);
            }
            mensaje = "Se grabo correctamente";
            
            objJson.Mensaje = mensaje;
            objJson.Resultado = true;

            return Json(objJson);
        }

        /// <summary>
        /// muestra la pantalla del requerimiento solo con la opcion de aprobar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Edit(string id, string pagina ) {
            SolicitudRempCargoViewModel model;
            model = new SolicitudRempCargoViewModel();
            string EtapaSol = null;

            model.CampoVacante = Visualicion.SI;
            model.CampoPuesto = Visualicion.SI;
            model.CampoObservacion = Visualicion.SI;

            var ObjSol = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(id));
            EtapaSol = ObjSol.TipEtapa;
            
            var idRolUsuario = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            var solicitud = ObjSol;

            if (ObjSol!=null)
            {
                SedeNivel sedeNivel = new SedeNivel();
                model.SolReqPersonal = new SolReqPersonal();
                sedeNivel.IDESEDE = ObjSol.IdeSede;

                model = Inicializar(sedeNivel);
               
                model.SolReqPersonal = ObjSol;
                model.Pagina = pagina;
                
                model.listaDependencia = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo
                                                                    && x.IdeSede == model.SolReqPersonal.IdeSede
                                                                    && x.IdeDependencia == model.SolReqPersonal.IdeDependencia
                                                                    ));


                model.listaDepartamento = new List<Departamento>(_departamentoRepository.GetBy(x => x.IdeDepartamento == model.SolReqPersonal.IdeDepartamento
                                                                                            && x.Dependencia.IdeDependencia == model.SolReqPersonal.IdeDependencia
                                                                                            && x.EstadoActivo == IndicadorActivo.Activo));


                model.listaArea = new List<Area>(_areaRepository.GetBy(x => x.IdeArea == model.SolReqPersonal.IdeArea
                                                                            && x.Departamento.IdeDepartamento == model.SolReqPersonal.IdeDepartamento
                                                                            && x.EstadoActivo == IndicadorActivo.Activo));


                int puntajeTotal = Convert.ToInt32(solicitud.PuntTotCentroEst) + Convert.ToInt32(solicitud.PuntTotConoGen) + Convert.ToInt32(solicitud.PuntTotDisCapa) +
                               Convert.ToInt32(solicitud.PuntEdad) + Convert.ToInt32(solicitud.PuntTotExpLaboral) + Convert.ToInt32(solicitud.PuntTotHorario) +
                               Convert.ToInt32(solicitud.PuntTotConoIdioma) + Convert.ToInt32(solicitud.PuntTotNivelEst) + Convert.ToInt32(solicitud.PuntajeTotalOfimatica) +
                               Convert.ToInt32(solicitud.PuntTotPostuinte) + Convert.ToInt32(solicitud.PuntSalario) + Convert.ToInt32(solicitud.PuntSexo) +
                               Convert.ToInt32(solicitud.PuntTotUbigeo);


                model.TotalMaxino = puntajeTotal;


                IdeSolicitudReemplazo = (int)ObjSol.IdeSolReqPersonal;
            }


            if (Etapa.Pendiente.Equals(EtapaSol))
            {
                if (Roles.Encargado_Seleccion.Equals(idRolUsuario))
                {
                    model.Accion = Accion.Aprobar;
                }    
            }

            if (Etapa.Aprobado.Equals(EtapaSol))
            {
                if (Roles.Encargado_Seleccion.Equals(idRolUsuario) || Roles.Analista_Seleccion.Equals(idRolUsuario))
                {
                    model.Accion = Accion.Publicar;


                    model.CampoVacante = Visualicion.NO;
                    model.CampoPuesto = Visualicion.NO;
                    model.CampoObservacion = Visualicion.NO;
                   
                }
            }

            if (Etapa.Publicado.Equals(EtapaSol))
            {
                if ((Roles.Analista_Seleccion.Equals(idRolUsuario)) || (Roles.Encargado_Seleccion.Equals(idRolUsuario)))
                {
                    model.Accion = Accion.Publicar;

                    model.CampoVacante = Visualicion.NO;
                    model.CampoPuesto = Visualicion.NO;
                    model.CampoObservacion = Visualicion.NO;
                }
            }


            if (idRolUsuario > 0)
            {
                if (Roles.Analista_Seleccion.Equals(idRolUsuario))
                {


                    model.CampoPopEliminar = "N";
                    model.CampoPopEditar = "N";
                    model.CampoTipPuesto = "N";
                    model.CampoNumVacantes = "N";


                }

                if (Roles.Encargado_Seleccion.Equals(idRolUsuario))
                {

                    model.CampoPopEliminar = "N";
                    model.CampoPopEditar = "N";
                    model.CampoTipPuesto = "N";
                    model.CampoNumVacantes = "N";

                }

                if (Roles.Gerente_General_Adjunto.Equals(idRolUsuario))
                {

                    model.CampoPopEliminar = "S";
                    model.CampoPopEditar = "S";
                    model.CampoTipPuesto = "S";
                    model.CampoNumVacantes = "S";

                }

                if (Roles.Jefe_Corporativo_Seleccion.Equals(idRolUsuario))
                {

                    model.CampoPopEliminar = "N";
                    model.CampoPopEditar = "N";
                    model.CampoTipPuesto = "N";
                    model.CampoNumVacantes = "N";

                }

                if (Roles.Gerente.Equals(idRolUsuario))
                {
                    model.CampoPopEliminar = "S";
                    model.CampoPopEditar = "S";
                    model.CampoTipPuesto = "S";
                    model.CampoNumVacantes = "S";

                }

                if (Roles.Jefe.Equals(idRolUsuario))
                {


                    model.CampoPopEliminar = "S";
                    model.CampoPopEditar = "S";
                    model.CampoTipPuesto = "S";
                    model.CampoNumVacantes = "S";

                }

                if (Roles.Administrador_Sistema.Equals(idRolUsuario))
                {

                    model.CampoPopEliminar = "S";
                    model.CampoPopEditar = "S";
                    model.CampoTipPuesto = "S";
                    model.CampoNumVacantes = "S";

                }

            }


            return View("InformacionReemplazo", model);
        }


        /// <summary>
        /// Elimina el detalle del reemplazo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        public ActionResult EliminarReemplazo(int id, int idReq, int idPersona)
        {
            JsonMessage objJson = new JsonMessage();
            SolicitudRempCargoViewModel model;

            model = new SolicitudRempCargoViewModel();
           
            if (idReq>0)
            {
                Reemplazo objReemplazo = new Reemplazo();
                objReemplazo.IdeSolReqPersonal = idReq;
                objReemplazo.IdReemplazo = id;
                objReemplazo.IdPersona = idPersona;
                int dato = _solReqPersonalRepository.EliminaListaReemplazo(objReemplazo);
                
                if (dato==1)
	            {
	                objJson.Resultado=true;	 
                    objJson.Mensaje = "Se elimino el registro correctamente";
	            }
            
            }

            return Json(objJson);
        }

        /// <summary>
        /// lista de reemplazos
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListReemplazoReq(GridTable grid)
        {
            try
            {
                List<Reemplazo> ListaReemplazo = new List<Reemplazo>();
                Reemplazo ObjReemplazo;
              
                ObjReemplazo = new Reemplazo();


                if (grid.rules[0].data != null)
                {
                    var objSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == grid.rules[0].data && x.TipoSolicitud == TipoSolicitud.Remplazo);
                    ObjReemplazo.IdeSolReqPersonal = (int)objSol.IdeSolReqPersonal;
                }
                else
                {
                    ObjReemplazo.IdeSolReqPersonal = 0;
                }
               

                ListaReemplazo = _solReqPersonalRepository.GetListaReemplazo(ObjReemplazo);

                var generic = GetListar(ListaReemplazo,
                                     grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);
               

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdReemplazo.ToString(),
                    cell = new string[]
                            {
                               
                                item.IdReemplazo==null?"":item.IdReemplazo.ToString(),
                                item.IdPersona==null?"":item.IdPersona.ToString(),
                                item.IdeSolReqPersonal==null?"":item.IdeSolReqPersonal.ToString(),
                                item.FecInicioReemplazo==null?"":String.Format("{0:dd/MM/yyyy}", item.FecInicioReemplazo), 
                                item.FecFinalReemplazo==null?"":String.Format("{0:dd/MM/yyyy}", item.FecFinalReemplazo),
                                item.Nombres==null?"":item.Nombres,
                                item.ApePaterno==null?"":item.ApePaterno,
                               
                               
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
        /// Crea o Actuliza la Solicitud de reemplazo Requerimiento de personal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult SetSolReqPersonal(SolicitudRempCargoViewModel model) 
        {

            JsonMessage objJson = new JsonMessage();
            Int32 retorno=0;
            model.Reemplazo = new Reemplazo();
            SolReqPersonal objSolReq;

            try
            {
                model.SolReqPersonal.Feccreacion = FechaSistema;
                model.SolReqPersonal.UsuarioCreacion = UsuarioActual.NombreUsuario;
               
                var Sede = Session[ConstanteSesion.Sede];
                model.SolReqPersonal.IdeSede = Convert.ToInt32(Sede);

                var UsuarioSede = Session[ConstanteSesion.UsuarioSede];


                if (model.SolReqPersonal.CodSolReqPersonal!=null)
                {
                    var objSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == model.SolReqPersonal.CodSolReqPersonal && x.TipoSolicitud==TipoSolicitud.Remplazo);

                    if (model.SolReqPersonal.IdeArea!=null)
                    {
                        objSol.IdeArea = model.SolReqPersonal.IdeArea;
                    }
                    if (model.SolReqPersonal.IdeDependencia!=null)
                    {
                        objSol.IdeDependencia = model.SolReqPersonal.IdeDependencia;
                    }
                    if (model.SolReqPersonal.IdeDepartamento!=null)
                    {
                        objSol.IdeDepartamento = model.SolReqPersonal.IdeDepartamento;
                    }
                    if (model.SolReqPersonal.Observacion!=null)
                    {
                        objSol.Observacion = model.SolReqPersonal.Observacion;
                    }

                    if (model.SolReqPersonal.TipPuesto!=null)
                    {
                         objSol.TipPuesto = model.SolReqPersonal.TipPuesto;
                    }


                    if (model.SolReqPersonal.TipVacante!=null)
                    {
                         objSol.TipVacante = model.SolReqPersonal.TipVacante;
                    }

                    if (model.SolReqPersonal.NumVacantes!=null)
                    {
                        objSol.NumVacantes = model.SolReqPersonal.NumVacantes;
                    }

                    
                    objSol.FechaModificacion = FechaSistema;
                    objSol.UsuarioModificacion = UsuarioActual.NombreUsuario;
                    _solReqPersonalRepository.Update(objSol);

                    retorno = Convert.ToInt32(objSol.CodSolReqPersonal);

                    objSolReq = new SolReqPersonal();
                    objSolReq = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == retorno.ToString() && x.TipoSolicitud == TipoSolicitud.Remplazo);


                    if (retorno > 0)
                    {
                        objJson.Resultado = true;
                        objJson.Mensaje = "Se actualizo la Solicitud";
                        objJson.IdDato = retorno;

                        objJson.IdSol = Convert.ToInt32(objSolReq.IdeSolReqPersonal);
                    }

                }
                else
                {
                    //crea
                    if (UsuarioSede != null)
                    {
                        SedeNivel objSedeNivel = (SedeNivel)UsuarioSede;


                        model.SolReqPersonal.IdeArea = model.SolReqPersonal.IdeArea;
                        model.SolReqPersonal.IdeDependencia = model.SolReqPersonal.IdeDependencia;
                        model.SolReqPersonal.IdeDepartamento = model.SolReqPersonal.IdeDepartamento;
                        model.SolReqPersonal.idRolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                        model.SolReqPersonal.IdRolResp = 0;
                        model.SolReqPersonal.idUsuarioSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                        model.SolReqPersonal.idUsuarioResp = 0;
                        model.SolReqPersonal.Tipsol = TipoSolicitud.Remplazo;

                        model.Reemplazo.CodGenerado = Convert.ToString(Session[ConstanteSesion.codReqSolTemp]);
                    }


                    model.SolReqPersonal.TipEtapa = Etapa.Pendiente;

                    retorno = _solReqPersonalRepository.CreaSolicitudReemplazo(model.SolReqPersonal, model.Reemplazo);

                    objSolReq = new SolReqPersonal();
                    objSolReq = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == retorno.ToString() && x.TipoSolicitud == TipoSolicitud.Remplazo);

                    if (retorno > 0)
                    {
                        objJson.Resultado = true;
                        objJson.Mensaje = "Se generó la solicitud";
                        objJson.IdDato = retorno;
                        objJson.IdSol = Convert.ToInt32(objSolReq.IdeSolReqPersonal);
                    }
                }
                

            }
            catch (Exception)
            {

                retorno = 0;
            }


            return Json(objJson);
        }


        /// <summary>
        /// valida si la solitud se ecuentra activa
        /// </summary>
        /// <param name="idSol">id de la solicitud reemplazo</param>
        /// <returns></returns>
        public ActionResult ValidaEstadoSol(int idSol)
        {
            JsonMessage jSonMensaje = new JsonMessage();
            Boolean respuesta = false;
            string mensaje="";

            var ObjSol = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == idSol);

            if (ObjSol!=null)
            {
                if (IndicadorActivo.Activo.Equals(ObjSol.EstadoActivo))
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }
            }
            else
            {
                respuesta = false;
            }

            jSonMensaje.Resultado = respuesta;
            jSonMensaje.Mensaje = mensaje;

            return Json(jSonMensaje);

        }


        
        /// <summary>
        /// valida si la solcitud esta activa, publicada o finalizada
        /// </summary>
        /// <param name="idSol"></param>
        /// <returns></returns>
        public ActionResult ValidaSol(int idSol)
        {
            JsonMessage jSonMensaje = new JsonMessage();
            Boolean respuesta = false;
            string mensaje = "";

            var ObjSol = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == idSol);

            if (ObjSol != null)
            {
                if ((Etapa.Publicado.Equals(ObjSol.TipEtapa) || Etapa.Finalizado.Equals(ObjSol.TipEtapa)) && IndicadorActivo.Activo.Equals(ObjSol.EstadoActivo))
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }

            }
            else
            {
                respuesta = false;
            }

            jSonMensaje.Resultado = respuesta;
            jSonMensaje.Mensaje = mensaje;

            return Json(jSonMensaje);

        }


        /// <summary>
        /// Envia la solicitud de Reemplazo de requeremiento personal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EnviaSolReqPersonal(SolicitudRempCargoViewModel model) {

            JsonMessage objJson = new JsonMessage();
            int retorno=0;
            Cargo objCargo = new Cargo();

            int idUsuario = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            string MensajeInformativo = "";
            var Sede = Convert.ToInt32(Session[ConstanteSesion.Sede]);

            List<String> listaSends=null;
            List<String> listaCopys=null;

            if (model!=null)
            {

                if (model.SolReqPersonal.CodSolReqPersonal != null)
                {

                    var objSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == model.SolReqPersonal.CodSolReqPersonal && x.TipoSolicitud == TipoSolicitud.Remplazo);

                    if (objSol != null)
                    {
                        model.SolReqPersonal.IdeSolReqPersonal = objSol.IdeSolReqPersonal;
                        model.SolReqPersonal.idUsuarioSuceso = idUsuario;
                        model.SolReqPersonal.UsuarioCreacion = UsuarioActual.NombreUsuario;
                        model.SolReqPersonal.Feccreacion = FechaSistema;
                        model.SolReqPersonal.idRolSuceso = idRol;
                        model.SolReqPersonal.TipEtapa = Etapa.Pendiente;
                        model.SolReqPersonal.IdeCargo = objSol.IdeCargo;
                        
                        // Se obtiene el usaurio reponsable
                        var ObjUsuarioResp = _usuarioRolSedeRepository.GetBy(x => x.IdRol == Roles.Encargado_Seleccion
                                                            && x.IdSede == Sede);


                        // obtiene el los roles de los emails a los que se deben enviar correo
                        System.Collections.ArrayList lista = listaEmail(Convert.ToInt32(objSol.IdeSolReqPersonal), idRol, AccionEnvioEmail.EnviarSolicitud, Sede, TipoSolicitud.Remplazo);
                        listaSends = new List<String>();

                        listaSends = (List<String>)lista[0];

                        listaCopys= new List<String>();

                        listaCopys = (List<String>)lista[1];

                        
                        // se valida que exista y se toma al primer responsable
                        if (ObjUsuarioResp != null)
                        {
                            List<UsuarioRolSede> listaUsuarios = (List<UsuarioRolSede>)ObjUsuarioResp;
                            UsuarioRolSede usuarioRolSede = (UsuarioRolSede)listaUsuarios[0];
                            
                            model.SolReqPersonal.idUsuarioResp = usuarioRolSede.IdUsuario;
                            
                            model.SolReqPersonal.IdRolResp = usuarioRolSede.IdRol;

                            var objRol = _rolRepository.GetSingle(x => x.IdRol == usuarioRolSede.IdRol && x.FlgEstado==IndicadorActivo.Activo);

                            var ObjUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == usuarioRolSede.IdUsuario);

                            if (ObjUsuario!=null)
                            {
                                retorno = _solReqPersonalRepository.EnviaSolicitud(model.SolReqPersonal);
                                var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == model.SolReqPersonal.idUsuarioResp &&
                                x.TipUsuario == TipUsuario.Instranet && x.FlgEstado == IndicadorActivo.Activo);
                                MensajeInformativo = "El proceso de envío se realizo exitosamente. La solicitud ha sido derivada al " + objRol.DscRol+ ", "+objUsuario.DscNombres+" "+objUsuario.DscApePaterno;

                            }
                            else
                            {
                                retorno = 0;
                                objJson.Resultado = false;
                                objJson.Mensaje = "No se encuentra un responsable(Encargado de selección) asignado";
                            }
                            
                        }
                        else
                        {
                            objJson.Resultado = false;
                            objJson.Mensaje = "No se encuentra un responsable(Encargado de selección) asignado";
                        }
                      
                    }
                    else
                    {
                        objJson.Resultado = false;
                        objJson.Mensaje = "no se puede enviar la solicitud, revise que los campos esten correctos";
                    }

                }
                else {
                    objJson.Resultado = false;
                    objJson.Mensaje = "Debe generar una solicitud";
                
                }
            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se puede enviar la solicitud, revise que los campos esten correctos";
            }

            if (retorno > 0)
            {

               //SolReqPersonal objSolicitud = new SolReqPersonal();
                SolReqPersonal objSolicitud = null;
               
                objSolicitud = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == model.SolReqPersonal.IdeSolReqPersonal && x.EstadoActivo == IndicadorActivo.Activo);

               var objUsuario =  _usuarioRepository.GetSingle(x => x.IdUsuario == model.SolReqPersonal.idUsuarioResp);
               string desRol = Convert.ToString(Session[ConstanteSesion.RolDes]);
               //objUsuario.listaSend = new List<String>();




               bool flag = EnviarCorreo(desRol, Etapa.Pendiente, "Reemplazo", objSolicitud.nombreCargo, ""+model.SolReqPersonal.IdeSolReqPersonal, listaSends, listaCopys, "");

                
                objJson.Resultado = true;
                objJson.Mensaje = MensajeInformativo;
            }


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
        public System.Collections.ArrayList listaEmail(int idSol,int idRolSuceso,string btnAccion,int idSede,string TipoSol)
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

                        if (item.RolSend!=null)
                        {
                        
                            if (item.RolSend.Equals("**"))
                            {
                                if (TipoSolicitud.Nuevo.Equals(TipoSol))
	                            {
		                            var objSolNuevo = _solicitudNuevoCargoRepository.GetSingle(x => x.IdeSolicitudNuevoCargo == idSol && x.EstadoActivo==IndicadorActivo.Activo);
	                                var idCargo = objSolNuevo.IdeCargo;

                                    var objCargo = _cargoRepository.GetSingle(x => x.IdeCargo == idCargo && x.EstadoActivo == IndicadorActivo.Activo);

                                    tipoReq = objCargo.TipoRequerimiento;
                                }
                                else
	                            {
                                    var objSolReq = _solReqPersonalRepository.GetSingle(x=> x.IdeSolReqPersonal == idSol && x.EstadoActivo == IndicadorActivo.Activo);
                                    if (objSolReq!=null)
                                    {
                                        tipoReq = objSolReq.TipoRequerimiento;
                                    }
                                   
                                
                                }

                                if (tipoReq!=null)
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
        /// inicializa el popup para las aprobaciones o rechazos.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult InicioPopupAprobReem(string id)
        {
            SolicitudRempCargoViewModel model;
            model = new SolicitudRempCargoViewModel();
         
            model.SolReqPersonal = new SolReqPersonal();
            model.LogSolReqPersonal = new LogSolReqPersonal();

            model.SolReqPersonal.CodSolReqPersonal = id;
            return View("PopupAprobacionRechazo", model);
        }

        /// <summary>
        /// obtiene los datos del popup de aprobacion y rechazo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidarSesion]
        [HttpPost]
        public ActionResult SetPopupAprobReem(SolicitudRempCargoViewModel model)
        {
            JsonMessage objJson = new JsonMessage();
            Boolean aprobacion = false;
            int retorno = 0;
            int sede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            string tipoReq=null;
            SolReqPersonal objSolReqPersonal = null;
            string mensaje = "";
            List<String> listSends = null;
            List<String> listCopys = null;

            if (model.SolReqPersonal.CodSolReqPersonal!=null)
            {
                aprobacion = model.LogSolReqPersonal.Aprobado;

                //se obtiene los datos de la solicitud
                var objSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == model.SolReqPersonal.CodSolReqPersonal && x.TipoSolicitud == TipoSolicitud.Remplazo);
                if (objSol!=null)
                {
                    model.LogSolReqPersonal.IdeSolReqPersonal = (int)objSol.IdeSolReqPersonal;
                    model.LogSolReqPersonal.UsrSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                    model.LogSolReqPersonal.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                    string desRol = Convert.ToString(Session[ConstanteSesion.RolDes]);

                    

                    model.LogSolReqPersonal.FecSuceso = FechaSistema;
                    if (aprobacion)
                    {

                        System.Collections.ArrayList lista = listaEmail(Convert.ToInt32(objSol.IdeSolReqPersonal), idRol, AccionEnvioEmail.AprobarSolicitud, sede, TipoSolicitud.Remplazo);
                        listSends = new List<String>();
                        listSends = (List<String>)lista[0];

                        listCopys = new List<String>();
                        listCopys = (List<String>)lista[1];
                        
                        
                        model.LogSolReqPersonal.TipEtapa = Etapa.Aprobado;
                        model.LogSolReqPersonal.Observacion = "";

                        
                            tipoReq = objSol.TipoRequerimiento;

                            objSolReqPersonal = new SolReqPersonal();
                            objSolReqPersonal = _solReqPersonalRepository.GetResponsable(TipoDerivacion.Publicado, sede, tipoReq);

                            model.LogSolReqPersonal.UsResponsable = objSolReqPersonal.idUsuarioResp;
                            model.LogSolReqPersonal.RolResponsable = objSolReqPersonal.IdRolResp;
                           
                            var objRol = _rolRepository.GetSingle(x => x.IdRol == objSolReqPersonal.IdRolResp && x.FlgEstado == IndicadorActivo.Activo);

                            _solReqPersonalRepository.ActualizaLogSolReq(model.LogSolReqPersonal);

                            objSol.TipEtapa = Etapa.Aprobado;
                            objSol.FechaModificacion = FechaSistema;
                            objSol.UsuarioModificacion = UsuarioActual.NombreUsuario;
                            _solReqPersonalRepository.Update(objSol);


                            var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == model.LogSolReqPersonal.UsResponsable);

                            bool flag = EnviarCorreo(desRol, Etapa.Aprobado, "Reemplazo", objSol.nombreCargo, ""+model.LogSolReqPersonal.IdeSolReqPersonal, listSends, listCopys, "");


                            retorno = 1;

                       
                        
                        if (retorno>0)
                        {
                            objJson.Resultado = true;
                            mensaje = "El proceso de envío se realizo exitosamente. La solicitud ha sido derivada al" + objRol.DscRol + ", " + objUsuario.DscNombres + " " + objUsuario.DscApePaterno;
                        }
                    }
                    else
                    {


                        System.Collections.ArrayList lista = listaEmail(Convert.ToInt32(objSol.IdeSolReqPersonal), idRol, AccionEnvioEmail.RechazarSolicitud, sede, TipoSolicitud.Remplazo);
                        listSends = new List<String>();
                        listSends = (List<String>)lista[0];

                        listCopys = new List<String>();
                        listCopys = (List<String>)lista[1];
                        
                        
                        model.LogSolReqPersonal.TipEtapa = Etapa.Rechazado;

                        objSol.TipEtapa = Etapa.Rechazado;
                        objSol.FechaModificacion = FechaSistema;
                        objSol.UsuarioModificacion = UsuarioActual.NombreUsuario;

                        bool flag = EnviarCorreo(desRol, Etapa.Rechazado, "Reemplazo", objSol.nombreCargo, "" + model.LogSolReqPersonal.IdeSolReqPersonal, listSends, listCopys, model.LogSolReqPersonal.Observacion);


                        _solReqPersonalRepository.Update(objSol);

                        
                        _solReqPersonalRepository.ActualizaLogSolReq(model.LogSolReqPersonal);
                        retorno = 1;
                        
                        if (retorno > 0)
                        {
                            objJson.Resultado = true;
                            mensaje = "Se rechazo la solicitud";
                        }
                    }
                    
                }

            }

            if (retorno > 0)
            {
                objJson.Resultado = true;
                if (aprobacion)
                {
                    objJson.Mensaje = mensaje;
                }
                else
                {
                    objJson.Mensaje = mensaje;
                }
            }
          

            return Json(objJson);
            
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
        public bool EnviarCorreo(string rolResponsable, string etapa, string tipoRq, string cargoDescripcion, string codCargo,List<String> Sends,List<String> Copys,string motivo)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            var usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            try
            {
                SendMail enviarMail = new SendMail();
                enviarMail.Area = usuarioSession.AREADES;
                enviarMail.Sede = usuarioSession.SEDEDES;
                enviarMail.Rol = Session[ConstanteSesion.RolDes].ToString();
                //enviarMail.Usuario = Session[ConstanteSesion.UsuarioDes].ToString();

                var objUsuario = (Usuario)Session[ConstanteSesion.ObjUsuario];

                if (objUsuario != null)
                {
                    enviarMail.Usuario = objUsuario.DscNombres + " " + objUsuario.DscApePaterno + " " + objUsuario.DscApeMaterno;
                }

                enviarMail.EnviarCorreoVarios(dir, etapa, rolResponsable, tipoRq, motivo, cargoDescripcion, codCargo, Sends, "suceso", Copys);
                return true;
            }
            catch (Exception Ex)
            {
                return false;

            }

        }

        /// <summary>
        /// Inicializa la ventana de publicacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Publica(string id, string pagina) 
        {
            SolicitudRempCargoViewModel model;
            model = new SolicitudRempCargoViewModel();
            model.SolReqPersonal = new SolReqPersonal();
            model.visualizarCompetencias = Indicador.No;
            model.visualizarOfrecemos = Indicador.No;

            model.editarFechaFinPublica = Indicador.Si;
            model.editarFechaInicoPublica = Indicador.Si;
            model.editarObservaciones = Indicador.Si;

            model.Sede = Session[ConstanteSesion.SedeDes].ToString();

            var ObjSol = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(id));

            if (ObjSol != null)
            {
                model.SolReqPersonal.nombreCargo = ObjSol.nombreCargo;
                model.SolReqPersonal.DesCargo = ObjSol.DesCargo;
                model.SolReqPersonal.IdeSolReqPersonal = ObjSol.IdeSolReqPersonal;

                model.SolReqPersonal = ObjSol;

                var objArea = _areaRepository.GetSingle(x => x.Departamento.IdeDepartamento == ObjSol.IdeDepartamento
                                                        && x.IdeArea == ObjSol.IdeArea);

                model.SolReqPersonal.Area_des = objArea.NombreArea;

                int TipoPuesto = Convert.ToInt32(TipoCampo.TipoSalario);

                var ObjDetalleGeneral = _detalleGeneralRepository.GetSingle(x => x.IdeGeneral == TipoPuesto
                                                                            && x.Valor == ObjSol.TipPuesto);

                model.SolReqPersonal.TipPuestoDes = ObjDetalleGeneral.Descripcion == null ? "" : ObjDetalleGeneral.Descripcion;

                model.SolReqPersonal.NumVacantes = ObjSol.NumVacantes;
                model.SolReqPersonal.FuncionesCargo = ObjSol.FuncionesCargo;

                model.listaRangoSalarial =
                    new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
                model.listaRangoSalarial.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

                model.SolReqPersonal.TipoRangoSalario = ObjSol.TipoRangoSalario == null ? "" : ObjSol.TipoRangoSalario;

                model.Pagina = pagina;

            }

            //visualizar competencias
            var contadorCompetencias = _competenciaRequerimientoRepository.CountByExpress(x => x.SolicitudRequerimiento.IdeSolReqPersonal == ObjSol.IdeSolReqPersonal);

            if (contadorCompetencias > 0)
            {
                model.visualizarCompetencias = Indicador.Si;
            }
            //visualizar ofrecemos

            var contadorOfrecemos = _ofrecemosRequerimientoRepository.CountByExpress(x => x.SolicitudRequerimiento.IdeSolReqPersonal == ObjSol.IdeSolReqPersonal);
            if (contadorOfrecemos > 0)
            {
                model.visualizarOfrecemos = Indicador.Si;
            }

            if (pagina == TipoSolicitud.ConsultaRequerimientos)
            {
                model.btnActualizar = Visualicion.SI;
                model.btnPublicar = Visualicion.NO;
            }
            else
            {
                model.btnPublicar = Visualicion.SI;
                model.btnActualizar = Visualicion.NO;
                if (ObjSol.FecPublicacion != null)
                {
                    model.editarFechaFinPublica = Indicador.Si;
                    model.editarFechaInicoPublica = Indicador.No;
                    model.editarObservaciones = Indicador.No;

                }
            }

            model.Pagina = pagina;

            return View("Publicacion", model);

            //SolicitudRempCargoViewModel model;
            //model = new SolicitudRempCargoViewModel();
            //model.SolReqPersonal = new SolReqPersonal();
            //model.editarObservaciones = Indicador.Si;
            //model.editarFechaFinPublica = Indicador.Si;
            //model.editarFechaInicoPublica = Indicador.Si;

            //var rolSession = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            
            //var ObjSol = _solReqPersonalRepository.GetSingle(x => x.CodSolReqPersonal == id && x.TipoSolicitud == TipoSolicitud.Remplazo);

            //if (ObjSol!=null)
            //{
            //    model.SolReqPersonal.nombreCargo = ObjSol.nombreCargo;
            //    model.SolReqPersonal.DesCargo = ObjSol.DesCargo;
            //    model.SolReqPersonal.IdeSolReqPersonal = ObjSol.IdeSolReqPersonal;
            //    model.SolReqPersonal = ObjSol;

            //    var objArea = _areaRepository.GetSingle(x => x.Departamento.IdeDepartamento == ObjSol.IdeDepartamento
            //                                            && x.IdeArea == ObjSol.IdeArea);
                
            //    model.SolReqPersonal.Area_des = objArea.NombreArea;

            //    int TipoPuesto = Convert.ToInt32(TipoCampo.TipoSalario);

            //    var ObjDetalleGeneral = _detalleGeneralRepository.GetSingle(x => x.IdeGeneral == TipoPuesto 
            //                                                                && x.Valor == ObjSol.TipPuesto);

            //    model.SolReqPersonal.TipPuestoDes = ObjDetalleGeneral.Descripcion==null?"":ObjDetalleGeneral.Descripcion;

            //    model.SolReqPersonal.NumVacantes = ObjSol.NumVacantes;
            //    model.SolReqPersonal.FuncionesCargo = ObjSol.FuncionesCargo;

            //    model.listaRangoSalarial =
            //        new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            //    model.listaRangoSalarial.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            //    model.SolReqPersonal.TipoRangoSalario = ObjSol.TipoRangoSalario==null?"":ObjSol.TipoRangoSalario;
                
            //}

            //if (pagina == TipoSolicitud.ConsultaRequerimientos)
            //{
            //    model.btnActualizar = Visualicion.SI;
            //    model.btnPublicar = Visualicion.NO;
            //    // si el usuario es el encargado de selección puede editar los datos

            //}
            //else
            //{

            //    model.btnPublicar = Visualicion.SI;
            //    model.btnActualizar = Visualicion.NO;

            //    if (ObjSol.FecPublicacion != null)
            //    {
            //        model.editarFechaFinPublica = Indicador.Si;
            //        model.editarFechaInicoPublica = Indicador.No;
            //        model.editarObservaciones = Indicador.No;

            //    }
            //}
            //model.Pagina = pagina;

            //return View("Publicacion",model);
        }

        public ActionResult actualizarFechaExpiracion(string idSolicitud,string fechaExpiracion, string fechaPublicacion, string Observacion)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            fechaExpiracion = String.Format("{0:dd/MM/yyyy}", fechaExpiracion);
            DateTime fecha = Convert.ToDateTime(fechaExpiracion);
            fechaPublicacion = String.Format("{0:dd/MM/yyyy}", fechaPublicacion);
            DateTime fechaPublica = Convert.ToDateTime(fechaPublicacion);
            try
            {
                if ((idSolicitud != null)&&(idSolicitud != "0"))
                {
                    var solicitud = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(idSolicitud));
                    solicitud.FecExpiracacion = fecha;
                    solicitud.FecPublicacion = fechaPublica;
                    solicitud.ObservacionPublica = Observacion;
                    _solReqPersonalRepository.Update(solicitud);

                    objJsonMessage.Mensaje = "Fecha de expiración , actualizada correctamente";
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
                    objJsonMessage.Mensaje = "ERROR: no se pudo actualizar la fecha de expiración";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);

                }

            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR: " + ex;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }
        /// <summary>
        /// lista de conocimientos de la solicitud del requerimiento
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Conocimientos(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));

            List<ConocimientoGeneralRequerimiento> lista = new List<ConocimientoGeneralRequerimiento>();

            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;


                lista = _ConocimientoGeneralRequerimientoRepository.listarConocimientosPublicacion(IdeSolReqPersonal);

                var generic = GetListar(lista,
                                         grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List.Select(item => new Row
                {
                    id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                    cell = new string[]
                            {
                                item.DescripcionConocimientoGeneral==null?"":item.DescripcionConocimientoGeneral.ToString(),
                                item.NombreConocimientoGeneral==null?"":item.NombreConocimientoGeneral.ToString(),
                            }
                }).ToArray();

                return Json(generic.Value);

            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
            //int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));

            //try
            //{

            //    grid.page = (grid.page == 0) ? 1 : grid.page;

            //    grid.rows = (grid.rows == 0) ? 100 : grid.rows;

            //    DetachedCriteria where = DetachedCriteria.For<ConocimientoGeneralRequerimiento>();
            //    where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));
               

            //    var generic = Listar(_ConocimientoGeneralRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

            //    generic.Value.rows = generic.List
            //        .Select(item => new Row
            //        {
            //            id = item.IdeConocimientoGeneralRequerimiento.ToString(),
            //            cell = new string[]
            //                {
            //                    item.DescripcionConocimientoGeneral,
            //                }
            //        }).ToArray();

            //    return Json(generic.Value);
            //}
            //catch (Exception ex)
            //{
            //    return MensajeError("ERROR: " + ex.Message);
            //}
        }


        /// <summary>
        /// Estudios de la solicitud del requerimiento
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Estudios(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<NivelAcademicoRequerimiento>();
                where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                var generic = Listar(_nivelAcademicoRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionAreaEstudio,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// Competencias de la solictud del requerimiento
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Competencias(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<CompetenciaRequerimiento>();
                where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                var generic = Listar(_competenciaRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCompetenciaRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionCompetencia,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }


        /// <summary>
        /// Lista de experiencias 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Experiencia(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            try
            {
                DetachedCriteria where = null;
                where = DetachedCriteria.For<ExperienciaRequerimiento>();

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;
  
                where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                var generic = Listar(_experienciaRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExperiencia,
                                item.CantidadAnhosExperiencia.ToString() + " AÑO(S) y " +item.CantidadMesesExperiencia.ToString() + " MES(ES)" 
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// Lista de ofrecimientos
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult Ofrecemos(GridTable grid)
        {
            int IdeSolReqPersonal = (grid.rules[0].data == null ? 0 : Convert.ToInt32(grid.rules[0].data));
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<OfrecemosRequerimiento>();
                where.Add(Expression.Eq("SolicitudRequerimiento.IdeSolReqPersonal", IdeSolReqPersonal));

                var generic = Listar(_ofrecemosRequerimientoRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeOfrecemosRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionOfrecimiento,
                                
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }


        /// <summary>
        /// Realiza la publicacion de la solicitud de reemplazo de personal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult PublicaSolReqPersonal(SolicitudRempCargoViewModel model) 
        {
            JsonMessage objJson = new JsonMessage();
            var verSalario = model.verSalario;
            string IndVerSalario;
            
            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            int idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
            List<String> listSends = null;
            List<String> listCopys = null;


            if (verSalario)
            {
                IndVerSalario = "S";
            }else
	        {
                IndVerSalario = "N";
	        }
            System.Collections.ArrayList lista = new System.Collections.ArrayList();

            if (model!=null)
            {

                var objSol = _solReqPersonalRepository.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(model.SolReqPersonal.IdeSolReqPersonal));

                if (objSol != null)
                {
                    if (objSol.FecPublicacion != null)
                    {
                        objSol.FecExpiracacion = model.SolReqPersonal.FecExpiracacion;
                        _solReqPersonalRepository.Update(objSol);
                        objJson.Resultado = true;
                        objJson.Mensaje = "Se actualizó la solictud correctamente";
                    }
                    else
                    {
                        if (TipoSolicitud.Ampliacion.Equals(objSol.TipoSolicitud))
                        {
                            lista = listaEmail(Convert.ToInt32(objSol.IdeSolReqPersonal), idRol, AccionEnvioEmail.Publicar, idSede, TipoSolicitud.Ampliacion);

                        }
                        else
                        {
                            lista = listaEmail(Convert.ToInt32(objSol.IdeSolReqPersonal), idRol, AccionEnvioEmail.Publicar, idSede, TipoSolicitud.Remplazo);

                        }




                        listSends = new List<String>();
                        listSends = (List<String>)lista[0];

                        listCopys = new List<String>();
                        listCopys = (List<String>)lista[1];


                        objSol.FecPublicacion = model.SolReqPersonal.FecPublicacion;
                        objSol.FechaModificacion = FechaSistema;
                        objSol.UsuarioModificacion = UsuarioActual.NombreUsuario;
                        objSol.FecExpiracacion = model.SolReqPersonal.FecExpiracacion;
                        objSol.TipEtapa = Etapa.Publicado;
                        objSol.IndicadorSalario = IndVerSalario;
                        objSol.ObservacionPublica = model.SolReqPersonal.ObservacionPublica;


                        ReclutamientoPersona objRecluta = new ReclutamientoPersona();

                        objRecluta.IdeSol = Convert.ToInt32(objSol.IdeSolReqPersonal);

                        //se obtiene el tipo de solicitud para saber si es una ampliacion o un reemplazo
                        objRecluta.TipSol = objSol.TipoSolicitud;

                        objRecluta.TipPuesto = objSol.TipPuesto;
                        objRecluta.IdSede = objSol.IdeSede;
                        objRecluta.IdeCargo = objSol.IdeCargo;


                        var objCargo = _cargoRepository.GetSingle(x => x.IdeCargo == objSol.IdeCargo);

                        //Se asigna postulantes potenciales si hay antes de publicar una nueva solicitud
                        _solReqPersonalRepository.verificaPotenciales(objRecluta);

                        //se atualiza la solicitud;
                        _solReqPersonalRepository.Update(objSol);

                        model.LogSolReqPersonal = new LogSolReqPersonal();
                        model.LogSolReqPersonal.IdeSolReqPersonal = (int)objSol.IdeSolReqPersonal;
                        model.LogSolReqPersonal.UsrSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                        model.LogSolReqPersonal.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                        string desRol = Convert.ToString(Session[ConstanteSesion.RolDes]);
                        model.LogSolReqPersonal.FecSuceso = FechaSistema;
                        model.LogSolReqPersonal.TipEtapa = Etapa.Publicado;
                        //rol del responsable el que publica es el reponsable de trabajar la solicitud
                        model.LogSolReqPersonal.UsResponsable = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                        model.LogSolReqPersonal.RolResponsable = Convert.ToInt32(Session[ConstanteSesion.Rol]);


                        _solReqPersonalRepository.ActualizaLogSolReq(model.LogSolReqPersonal);

                        var objUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == model.LogSolReqPersonal.UsrSuceso);

                        if (TipoSolicitud.Ampliacion.Equals(objSol.TipoSolicitud))
                        {
                            bool flag = EnviarCorreo(desRol, Etapa.Publicado, "Ampliación", objCargo.NombreCargo, "" + model.LogSolReqPersonal.IdeSolReqPersonal, listSends, listCopys, "");

                        }
                        else
                        {
                            bool flag = EnviarCorreo(desRol, Etapa.Publicado, "Reemplazo", objCargo.NombreCargo, "" + model.LogSolReqPersonal.IdeSolReqPersonal, listSends, listCopys, "");

                        }

                        objJson.Resultado = true;
                        objJson.Mensaje = "Requerimiento publicado exitosamente";

                    }
                }
            }
            else
            {
                objJson.Resultado = false;
                objJson.Mensaje = "No se puede realizar la publicación de la solicitud";
            }



            return Json(objJson);
        }


        #endregion


        #region GRILLAS PERFIL 
        ///
        ///COMPETENCIAS
        ///

        [HttpPost]
        public JsonResult ListarCompetencias(GridTable grid)
        {
            List<CompetenciaRequerimiento> lista = new List<CompetenciaRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;
                grid.rows = (grid.rows == 0) ? 10 : grid.rows;

                lista = _solReqPersonalRepository.ListaCompetencias(IdeSolicitudReemplazo);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCompetenciaRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionCompetencia,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }
        //    
        //OFRECEMOS
        //
        [HttpPost]
        public virtual JsonResult ListarOfrecemos(GridTable grid)
        {
            List<OfrecemosRequerimiento> lista = new List<OfrecemosRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solReqPersonalRepository.ListaOfrecemos(IdeSolicitudReemplazo);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeOfrecemosRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.IdeOfrecemosRequerimiento.ToString(),
                                item.DescripcionOfrecimiento,
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }
        ///
        ///HORARIO
        ///

        [HttpPost]
        public virtual JsonResult ListaHorario(GridTable grid)
        {
            List<HorarioRequerimiento> lista = new List<HorarioRequerimiento>();

            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solReqPersonalRepository.ListaHorarios(IdeSolicitudReemplazo);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeHorarioRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionHorario,
                                item.PuntajeHorario.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }
        /// <summary>
        ///UBIGEO
        /// </summary>

        [HttpPost]
        public virtual JsonResult ListaUbigeo(GridTable grid)
        {

            List<UbigeoReemplazo> lista = new List<UbigeoReemplazo>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;


                lista = _solReqPersonalRepository.ListaUbigeos(IdeSolicitudReemplazo);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeUbigeoReemplazo.ToString(),
                        cell = new string[]
                            {
                                item.Departamento,
                                item.Provincia,
                                item.Distrito,
                                item.PuntajeUbigeo.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult ListaCentroEstudio(GridTable grid)
        {
            List<CentroEstudioRequerimiento> lista = new List<CentroEstudioRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solReqPersonalRepository.ListaCentroEstudio(IdeSolicitudReemplazo);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeCentroEstudioRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoCentroEstudio,
                                item.DescripcionNombreCentroEstudio,
                                item.PuntajeCentroEstudios.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult ListaNivelAcademico(GridTable grid)
        {
            List<NivelAcademicoRequerimiento> lista = new List<NivelAcademicoRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solReqPersonalRepository.ListaNivelAcademico(IdeSolicitudReemplazo);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoEducacion,
                                item.DescripcionAreaEstudio,
                                item.DescripcionNivelAlcanzado,
                                item.CicloSemestre.ToString(),
                                item.PuntajeNivelEstudio.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult ListaOfimatica(GridTable grid)
        {
            List<ConocimientoGeneralRequerimiento> lista = new List<ConocimientoGeneralRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solReqPersonalRepository.ListaConocimientos(IdeSolicitudReemplazo, "OFIMATICA");

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionConocimientoOfimatica,
                                item.DescripcionNombreOfimatica,
                                item.DescripcionNivelConocimiento,
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult ListaIdioma(GridTable grid)
        {
            List<ConocimientoGeneralRequerimiento> lista = new List<ConocimientoGeneralRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solReqPersonalRepository.ListaConocimientos(IdeSolicitudReemplazo, "IDIOMA");

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionIdioma,
                                item.DescripcionConocimientoIdioma,
                                item.DescripcionNivelConocimiento,
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult ListaOtrosConocimientos(GridTable grid)
        {
            List<ConocimientoGeneralRequerimiento> lista = new List<ConocimientoGeneralRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solReqPersonalRepository.ListaConocimientos(IdeSolicitudReemplazo, "GENERAL");


                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionConocimientoGeneral,
                                item.DescripcionNombreConocimientoGeneral,
                                item.DescripcionNivelConocimiento,
                                item.PuntajeConocimiento.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult ListaExperiencia(GridTable grid)
        {
            List<ExperienciaRequerimiento> lista = new List<ExperienciaRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solReqPersonalRepository.ListaExperiencia(IdeSolicitudReemplazo);


                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExperiencia,
                                item.CantidadAnhosExperiencia.ToString(),
                                item.CantidadMesesExperiencia.ToString(),
                                item.PuntajeExperiencia.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult ListaDiscapacidad(GridTable grid)
        {
            List<DiscapacidadRequerimiento> lista = new List<DiscapacidadRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solReqPersonalRepository.ListaDiscapacidad(IdeSolicitudReemplazo);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeDiscapacidadRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoDiscapacidad,
                                item.PuntajeDiscapacidad.ToString(),
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult ListaEvaluaciones(GridTable grid)
        {
            List<EvaluacionRequerimiento> lista = new List<EvaluacionRequerimiento>();
            try
            {

                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                lista = _solReqPersonalRepository.ListaEvaluacion(IdeSolicitudReemplazo);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeEvaluacionRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExamen==null?"":item.DescripcionExamen,
                                item.DescripcionTipoExamen== null?"":item.DescripcionTipoExamen,
                                item.NotaMinimaExamen==null?"":item.NotaMinimaExamen.ToString()
                            }
                    }).ToArray();

                return Json(generic.Value);
            }
            catch (Exception ex)
            {
                return MensajeError("ERROR: " + ex.Message);
            }
        }

        #endregion



        /**************************************************************************************************************/
        #region ranking
        public ActionResult PostulantesPorRequerimiento()
        {
            return View();
        }
        public ActionResult PostulantesPorRequerimientoCV()
        {
            return View();
        }
        public ActionResult PostulantesPreSeleccionados()
        {
            return View();
        }
        public ActionResult EvaluacionesPreSeleccionados()
        {
            return View();
        }

        public ActionResult ProgramarEvaluacion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListaPostulantesPorRequerimiento(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Linares Roca",
                    "Miguel Fransisco",
                    "",
                    "Pre-Selecc",
                    "",
                    "80"
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Marsano Ramirez",
                    "Raul Ernesto",
                    "",
                    "Pre-Selecc",
                    "",
                    "70"
                }
            };
            lstFilas.Add(fila2);

            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Carmona Lino",
                    "Julio César",
                    "",
                    "No Pre-Selecc",
                    "",
                    "70"
                }
            };
            lstFilas.Add(fila3);

            var fila4 = new
            {
                id = 4,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Cevallos Tuesta",
                    "Yvette Isabel",
                    "",
                    "Excluido",
                    "No califica",
                    "70"
                }
            };
            lstFilas.Add(fila4);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
        }
        
        [HttpPost]

        public ActionResult ListaPostulantesPorRequerimientoCV(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Linares Roca",
                    "Miguel Fransisco",
                    "42158963",
                    "01-458796",
                    "28/11/13",
                    "10:25","",""
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Ochoa Ramirez",
                    "Maria ",
                    "42158963",
                    "01-458796",
                    "28/11/13",
                    "11:15","",""
                }
            };
            lstFilas.Add(fila2);

            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                     "Carbajal Saravia",
                    "Gianfranco ",
                    "42158963",
                    "01-458796",
                    "29/11/13",
                    "16:00","",""
                }
            };
            lstFilas.Add(fila3);

           //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
        }

        [HttpPost]
        public ActionResult ListaPostulantesPreSeleccionados(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Linares Roca",
                    "Miguel Fransisco",
                    "963452",
                    "94335624",
                    "",
                    "",
                    "Pre-Selecc",
                    "1/4",
                    "80"
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Marsano Ramirez",
                    "Raul Ernesto",
                    "82452",
                    "94335624",
                    "",
                    "",
                    "Pre-Selecc",
                    "4/4",
                    "70"
                }
            };
            lstFilas.Add(fila2);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
        }
        
        [HttpPost]
        public ActionResult ListaEvaluacionesPreSeleccionados(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Examen 1",
                    "Examen",
                    "",
                    "20/05/2013",
                    "09:00 am.",
                    "RRHH",
                    "Evaluado",
                    "30"
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Examen 2",
                    "Examen",
                    "",
                    "22/05/2013",
                    "03:00 pm.",
                    "RRHH",
                    "Programado",
                    "0"
                }
            };
            lstFilas.Add(fila2);

            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Entrevista",
                    "Entrevista",
                    "",
                    "",
                    "",
                    "",
                    "Pendiente",
                    "0"
                }
            };
            lstFilas.Add(fila3);

            var fila4 = new
            {
                id = 4,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                    "Entrevista",
                    "Entrevista",
                    "",
                    "",
                    "",
                    "",
                    "Pendiente",
                    "0"
                }
            };
            lstFilas.Add(fila4);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
        }

        [HttpPost]
        public ActionResult ListaEvaluacionesDetalle(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila                    
                    "Lima",
                    "0",                    
                    "0"
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila                    
                    "Arequipa",                    
                    "0",
                    "0"
                }
            };
            lstFilas.Add(fila2);

            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila                    
                    "Cusco",                    
                    "10",
                    "1"
                }
            };
            lstFilas.Add(fila3);

            var fila4 = new
            {
                id = 4,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila                    
                    "Lima",                    
                    "0",
                    "0"
                }
            };
            lstFilas.Add(fila4);
          

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;
        }
        
        #endregion

        public ActionResult Publicacion()
        {
            return View();
        }

        #region ListaFunciones2
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaFunciones2(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila     
                  
                    "Coordinar con los Médicos",                    
                }
            };
            lstFilas.Add(fila1);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        #region ListaCompetencias2
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaCompetencias2(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila                    
                   
                    "Responsabilidad",                    
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila   
                   
                    "Capacidad para resolver problemas",                    
                }
            };
            lstFilas.Add(fila2);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        #region ListaOfrecemos2
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaOfrecemos2(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila       
                    
                    "Remuneración promedio al mercado",                    
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila    
                   
                    "Ingreso a planilla con todos los beneficios de ley",                    
                }
            };
            lstFilas.Add(fila2);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        

        #region ListaEstudiosPublicacion
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaEstudiosPublicacion(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila       
                    
                    "Técnico en Enfermería",                    
                }
            };
            lstFilas.Add(fila1);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        #region ListaConocimientosPublicacion
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaConocimientosPublicacion(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila       
                    
                    "Microsoft Excel Avanzado",                    
                }
            };
            lstFilas.Add(fila1);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }
        #endregion

        #region ListaExperienciaPublicacion
        /// <summary>
        /// Lista de Evaluaciones Detalle
        /// </summary>       
        [HttpPost]
        public ActionResult ListaExperienciaPublicacion(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila       
                    
                    "Técnico en Enfermería",                    
                }
            };
            lstFilas.Add(fila1);

            //int totalPag = (int)Math.Ceiling((decimal)totalReg / (decimal)rows);
            var data = new
            {
                //total = totalPag,       // Total de páginas
                //page = page,            // Página actual
                //records = totalReg,     // Total de registros (obtenido del modelo)
                rows = lstFilas
            };
            result = Json(data);

            return result;

        }


        #endregion

        public ActionResult ResultadoExamen1()
        {
            return View();
        }

        public ActionResult ResultadoExamen2()
        {
            return View();
        }

        public ActionResult ResultadoExamen3()
        {
            return View();
        }

        public ActionResult ResultadoExamen4()
        {
            return View();
        }

        public ActionResult ResultadoExamen5()
        {
            return View();
        }
    }
}
