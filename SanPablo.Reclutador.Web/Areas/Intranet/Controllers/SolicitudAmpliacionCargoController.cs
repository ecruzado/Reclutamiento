﻿using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Entity.Validation;
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

namespace SanPablo.Reclutador.Web.Areas.Intranet.Controllers
{
    [Authorize]
    public class SolicitudAmpliacionCargoController : BaseController
    {
        
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private ISolReqPersonalRepository _solicitudAmpliacionPersonal;
        private ICargoRepository _cargoRepository;
        private IAreaRepository _areaRepository;
        private IDependenciaRepository _dependenciaRepository;
        private IDepartamentoRepository _departamentoRepository;
        private IUsuarioRolSedeRepository _usuarioRolSedeRepository;
        private IUsuarioRepository _usuarioRepository;
        private ILogSolicitudRequerimientoRepository _logAmpliacionRepository;
        private IConocimientoGeneralRequerimientoRepository _conocimientoGeneralReqRepository;
        private INivelAcademicoRequerimientoRepository _nivelAcademicoReqRepository;
        private IExperienciaRequerimientoRepository _experienciaReqRepository;
        private ICompetenciaRequerimientoRepository _competenciaReqRepository;
        private IOfrecemosRequerimientoRepository _ofrecemosReqRepository;
        private ISolicitudNuevoCargoRepository _solicitudNuevoCargoRepository;
        private ISolReqPersonalRepository _solReqPersonalRepository;
        private IRolRepository _rolRepository;
        

        public SolicitudAmpliacionCargoController(IDetalleGeneralRepository detalleGeneralRepository,
                                                  ISolReqPersonalRepository solicitudAmpliacionPersonal,
                                                  ICargoRepository cargoRepository,
                                                  IAreaRepository areaRepository,
                                                  IDependenciaRepository dependenciaRepository,
                                                  IDepartamentoRepository departamentoRepository,
                                                  IUsuarioRolSedeRepository usuarioRolSedeRepository,
                                                  IUsuarioRepository usuarioRepository,
                                                  ILogSolicitudRequerimientoRepository logAmpliacionRepository,
                                                  IConocimientoGeneralRequerimientoRepository conocimientoGeneralReqRepository,
                                                  INivelAcademicoRequerimientoRepository nivelAcademicoReqRepository,
                                                  IExperienciaRequerimientoRepository experienciaReqRepository,
                                                  ICompetenciaRequerimientoRepository competenciaReqRepository,
                                                  IOfrecemosRequerimientoRepository ofrecemosReqRepository,
                                                  ISolReqPersonalRepository solReqPersonalRepository,
                                                  ISolicitudNuevoCargoRepository solicitudNuevoCargoRepository,
                                                  IRolRepository rolRepository
            )
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solicitudAmpliacionPersonal = solicitudAmpliacionPersonal;
            _cargoRepository = cargoRepository;
            _areaRepository = areaRepository;
            _dependenciaRepository = dependenciaRepository;
            _departamentoRepository = departamentoRepository;
            _usuarioRolSedeRepository = usuarioRolSedeRepository;
            _usuarioRepository = usuarioRepository;
            _logAmpliacionRepository = logAmpliacionRepository;
            _conocimientoGeneralReqRepository = conocimientoGeneralReqRepository;
            _nivelAcademicoReqRepository = nivelAcademicoReqRepository;
            _experienciaReqRepository = experienciaReqRepository;
            _competenciaReqRepository = competenciaReqRepository;
            _ofrecemosReqRepository = ofrecemosReqRepository;
            _solicitudNuevoCargoRepository = solicitudNuevoCargoRepository;
            _solReqPersonalRepository = solReqPersonalRepository;
            _rolRepository = rolRepository;
        }
        
        
        [ValidarSesion]
        public ActionResult Edit(string id, string pagina)
        {
            SolReqPersonal solicitud = null;
            IdeSolicitudAmpliacion = Convert.ToInt32(id);

            if (IdeSolicitudAmpliacion != 0)
            {
                solicitud = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion && x.TipoSolicitud == TipoSolicitud.Ampliacion);
            }

            SolicitudAmpliacionCargoViewModel solicitudModel = inicializarAmpliacionCargo( pagina, solicitud);
            
            var usuario = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            
            SolReqPersonal solicitudAmpliacion = new SolReqPersonal();

            if (solicitud != null)
            {

                
                solicitudModel.SolicitudRequerimiento = solicitud;


                actualizarDatosAreas(solicitudModel, solicitud.IdeArea, solicitud.IdeDepartamento, solicitud.IdeDependencia, solicitud.IdeSede);


                int puntajeTotal = Convert.ToInt32(solicitud.PuntTotCentroEst) + Convert.ToInt32(solicitud.PuntTotConoGen) + Convert.ToInt32(solicitud.PuntTotDisCapa) +
                              Convert.ToInt32(solicitud.PuntEdad) + Convert.ToInt32(solicitud.PuntTotExpLaboral) + Convert.ToInt32(solicitud.PuntTotHorario) +
                              Convert.ToInt32(solicitud.PuntTotConoIdioma) + Convert.ToInt32(solicitud.PuntTotNivelEst) + Convert.ToInt32(solicitud.PuntajeTotalOfimatica) +
                              Convert.ToInt32(solicitud.PuntTotPostuinte) + Convert.ToInt32(solicitud.PuntSalario) + Convert.ToInt32(solicitud.PuntSexo) +
                              Convert.ToInt32(solicitud.PuntTotUbigeo);
                
                solicitudModel.TotalMaxino = puntajeTotal;
                solicitudModel.nuevaSolicitud = Indicador.No;

               
                //var departamento = _departamentoRepository.GetSingle(x => x.IdeDepartamento == solicitud.IdeDepartamento);
                //var area = _areaRepository.GetSingle(x => x.IdeArea == solicitud.IdeArea);

                //solicitudModel.Areas.Add(area);
                //solicitudModel.Departamentos.Add(departamento);
                
            }
            else
            {
                var usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];

                var rolSession = Convert.ToInt32(Session[ConstanteSesion.Rol]);

                if (rolSession != Roles.Gerente_General_Adjunto)
                {
                    solicitudAmpliacion.Departamento_des = usuarioSession.DEPARTAMENTODES;
                    solicitudAmpliacion.Dependencia_des = usuario.DEPENDENCIADES;
                    solicitudAmpliacion.Area_des = usuario.AREADES;
                    solicitudAmpliacion.IdeDependencia = usuario.IDEDEPENDENCIA;
                    solicitudAmpliacion.IdeDepartamento = usuario.IDEDEPARTAMENTO;
                    solicitudAmpliacion.IdeArea = usuario.IDEAREA;
                }
                solicitudAmpliacion.IdeSede = usuario.IDESEDE;
                solicitudAmpliacion.NumVacantes = 1;

                solicitudModel.SolicitudRequerimiento = solicitudAmpliacion;
                solicitudModel.nuevaSolicitud = Indicador.Si;
            }


            return View(solicitudModel);
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit(SolicitudAmpliacionCargoViewModel model)
        {

            SolReqPersonal solicitudAmpliacion = model.SolicitudRequerimiento;
            JsonMessage objJsonMessage = new JsonMessage();

            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            int idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
            SolReqPersonal obj = new SolReqPersonal();

            try
            {
                SolReqPersonalValidator validation = new SolReqPersonalValidator();
                ValidationResult result = validation.Validate(solicitudAmpliacion, "IdeCargo", "IdeDependencia", "IdeDepartamento", "IdeArea", 
                                                                                    "NumVacantes", "Motivo");

                List<String> listSends = null;
                List<String> listCopys = null;

                if (!result.IsValid)
                {
                    objJsonMessage.Mensaje = "ERROR: Verificar los datos ingresados";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                    
                }
                if (model.SolicitudRequerimiento.IdeSolReqPersonal == null)
                {
                    solicitudAmpliacion.TipoSolicitud = TipoSolicitud.Ampliacion;
                    solicitudAmpliacion.FechaModificacion = FechaCreacion;
                    Cargo cargoSol = _cargoRepository.GetSingle(x => x.IdeCargo == solicitudAmpliacion.IdeCargo);
                    var rolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                    var rolResponsable = 0;
                    var etapaInicio = "";
                    string indicadorArea = "NO";
                    string rol = "";
                    /// 
                    ///enviar dependiendo el usuario que registra la solicitud
                    /// 
                    switch (rolSuceso)
                    {
                        case Roles.Jefe:
                            rolResponsable = Roles.Gerente;
                            etapaInicio = Etapa.Pendiente;
                            rol = "Gerente";
                            indicadorArea = "SI";
                            break;

                        case Roles.Gerente:
                            rolResponsable = Roles.Gerente_General_Adjunto;
                            etapaInicio = Etapa.Validado;
                            rol = "Gerente General Adjunto";
                            break;

                        case Roles.Gerente_General_Adjunto:
                            rolResponsable = Roles.Encargado_Seleccion;
                            etapaInicio = Etapa.Aprobado;
                            rol = "Encargado de Selección";
                            break;
                    }

                    System.Collections.ArrayList lista = listaEmail(Convert.ToInt32(0), idRol, AccionEnvioEmail.EnviarSolicitud, idSede, TipoSolicitud.Ampliacion);
                    listSends = new List<String>();
                    listSends = (List<String>)lista[0];

                    listCopys = new List<String>();
                    listCopys = (List<String>)lista[1];

                    

                    if ((rolResponsable != 0) && (etapaInicio != ""))
                    {
                        obj = _solicitudAmpliacionPersonal.insertarSolicitudAmpliacion(solicitudAmpliacion, Convert.ToInt32(Session[ConstanteSesion.Usuario]), rolSuceso, etapaInicio, rolResponsable, indicadorArea);




                        if (obj.idUsuarioResp != -1)
                        {
                            var usuarioResponsable = _usuarioRepository.GetSingle(x => x.IdUsuario == obj.idUsuarioResp);

                            bool flag = EnviarCorreoAll(usuarioResponsable, rolResponsable.ToString(), etapaInicio, "", cargoSol.NombreCargo, "" + obj.IdeSolReqPersonal, listSends, listCopys);
                            string msj = "";
                            string msjRspta = "";
                            if (!flag)
                            {
                                msj = "Falló envio de correo";
                            }
                            msjRspta = "Solicitud enviada exitosamente. ";
                            msjRspta += "Derivada a " + rol + " " + usuarioResponsable.DscNombres + " " + usuarioResponsable.DscApePaterno;
                            objJsonMessage.Mensaje = msjRspta +" "+ msj;
                            objJsonMessage.Resultado = true;
                            return Json(objJsonMessage);
                        }
                        else
                        {
                            objJsonMessage.Mensaje = "Solicitud no enviada, intente de nuevo ";
                            objJsonMessage.Resultado = false;
                            return Json(objJsonMessage);
                        }

                    }
                    else
                    {
                        objJsonMessage.Mensaje = "No puede realizar esta accion, revise sus permisos";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
                else 
                {
                    var solicitudAnterior = model.SolicitudRequerimiento;
                    var solicitudEditar = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == model.SolicitudRequerimiento.IdeSolReqPersonal);
                    solicitudEditar.TipPuesto = solicitudAnterior.TipPuesto;
                    solicitudEditar.IdeCargo = solicitudAnterior.IdeCargo;
                    solicitudEditar.NumVacantes = solicitudAnterior.NumVacantes;
                    solicitudEditar.Motivo = solicitudAnterior.Motivo;
                    solicitudEditar.Observacion = solicitudAnterior.Observacion;
                    if ((solicitudAnterior.IdeDependencia != 0) || (solicitudAnterior.IdeDepartamento != 0) || (solicitudAnterior.IdeArea != 0))
                    {
                        solicitudEditar.IdeDependencia = solicitudAnterior.IdeDependencia;
                        solicitudEditar.IdeDepartamento = solicitudAnterior.IdeDepartamento;
                        solicitudEditar.IdeArea = solicitudAnterior.IdeArea;
                    }
                    _solicitudAmpliacionPersonal.Update(solicitudEditar);
                    
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);

                }
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }

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

        public SolicitudAmpliacionCargoViewModel inicializarAmpliacionCargo(string pagina, SolReqPersonal solicitud)
        {
            SolicitudAmpliacionCargoViewModel model = new SolicitudAmpliacionCargoViewModel();

            int idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
            int rolSession = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            model.SolicitudRequerimiento = new SolReqPersonal();

            model.Pagina = pagina;
            model.rolSession = rolSession;

            if (pagina == TipoSolicitud.Ampliacion)
            {
                if ((rolSession == Roles.Jefe) || (rolSession == Roles.Gerente_General_Adjunto) || (rolSession == Roles.Gerente))
                {
                    model.Accion = Accion.Editar;
                }
                else
                { model.Accion = Accion.Consultar; }
            }
            else
            {
                model.Accion = Accion.Consultar;
            }
            
            

            model.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            model.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            model.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            model.TipoPuestos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            model.TipoPuestos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            model.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            model.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            model.Departamentos = new List<Departamento>();
            model.Areas = new List<Area>();
            if (rolSession == Roles.Gerente_General_Adjunto)
            {
                model.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo && x.IdeSede == idSede));
                model.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccione" });
                
                model.Departamentos.Insert(0, new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccione" });
                
                model.Areas.Insert(0, new Area { IdeArea = 0, NombreArea = "Seleccione" });

                model.Cargos = new List<Cargo>(_cargoRepository.listaCargosCompletos(idSede));
                model.Cargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });
            }
            else
            {
                var usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
                model.Dependencias = new List<Dependencia>();
                model.Dependencias.Add(new Dependencia { IdeDependencia = usuarioSession.IDEDEPENDENCIA, NombreDependencia = usuarioSession.DEPENDENCIADES });

                model.Departamentos = new List<Departamento>();
                model.Departamentos.Add(new Departamento { IdeDepartamento = usuarioSession.IDEDEPARTAMENTO, NombreDepartamento = usuarioSession.DEPARTAMENTODES });

                model.Areas = new List<Area>();
                model.Areas.Add(new Area { IdeArea = usuarioSession.IDEAREA, NombreArea = usuarioSession.AREADES });


                /// mostrar los cargos de acuerdo al area del usuario
                var cargoDatos = new Cargo();
                cargoDatos.IdeArea = usuarioSession.IDEAREA;
                cargoDatos.IdeDepartamento = usuarioSession.IDEDEPARTAMENTO;
                cargoDatos.IdeDependencia = usuarioSession.IDEDEPENDENCIA;
                cargoDatos.IdeSede = usuarioSession.IDESEDE;

                model.Cargos = new List<Cargo>(_cargoRepository.GetCargoxSede(cargoDatos));

                
                model.Cargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });
            }

            

            #region iniciar botonera
            switch (rolSession)
            {
                case Roles.Jefe:
                    model.btnVerEnviar = Visualicion.SI;
                    model.btnVerAceptar = Visualicion.NO;
                    model.btnVerAproRech = Visualicion.NO;
                    model.btnVerPublicar = Visualicion.NO;
                    model.btnVerPerfil = Visualicion.NO;
                    break;
                case Roles.Gerente:
                    if (IdeSolicitudAmpliacion == 0)
                    {
                        model.btnVerEnviar = Visualicion.SI;
                        model.btnVerAproRech = Visualicion.NO;
                    }
                    else
                    {
                        model.btnVerEnviar = Visualicion.NO;
                        model.btnVerAproRech = Visualicion.SI;
                    }
                    model.btnVerAceptar = Visualicion.NO;

                    model.btnVerPublicar = Visualicion.NO;
                    model.btnVerPerfil = Visualicion.NO;
                    break;
                case Roles.Gerente_General_Adjunto:
                    if (IdeSolicitudAmpliacion == 0)
                    {
                        model.btnVerEnviar = Visualicion.SI;
                        model.btnVerAproRech = Visualicion.NO;
                    }
                    else
                    {
                        model.btnVerEnviar = Visualicion.NO;
                        model.btnVerAproRech = Visualicion.SI;
                    }
                    model.btnVerAceptar = Visualicion.NO;

                    model.btnVerPublicar = Visualicion.NO;
                    model.btnVerPerfil = Visualicion.NO;
                    break;
                case Roles.Encargado_Seleccion:
                    model.btnVerEnviar = Visualicion.NO;
                    model.btnVerAproRech = Visualicion.NO;
                    model.btnVerPublicar = Visualicion.NO;
                    model.btnVerAceptar = Visualicion.NO;
                    if ((solicitud != null) && (solicitud.TipEtapa == Etapa.Aceptado))
                    {
                        model.btnVerPublicar = Visualicion.SI;
                        model.btnVerAceptar = Visualicion.NO;
                    }
                    else if ((solicitud != null) && (solicitud.TipEtapa == Etapa.Publicado)) 
                    {
                        model.btnVerPublicar = Visualicion.SI;
                        model.btnVerAceptar = Visualicion.NO;
                    }
                    else if ((solicitud != null) && (solicitud.TipEtapa == Etapa.Aprobado)) 
                    {
                        model.btnVerPublicar = Visualicion.NO;
                        model.btnVerAceptar = Visualicion.SI;
                    }
                    model.btnVerPerfil = Visualicion.SI;
                    break;
                case Roles.Analista_Seleccion:
                    model.btnVerEnviar = Visualicion.NO;
                    model.btnVerAceptar = Visualicion.NO;
                    model.btnVerAproRech = Visualicion.NO;
                    if ((solicitud != null) && (solicitud.TipEtapa == Etapa.Aceptado))
                    {
                        model.btnVerPublicar = Visualicion.SI;
                    }
                    else
                    {
                       // model.btnVerPublicar = Visualicion.NO;
                        if ((solicitud != null) && (solicitud.TipEtapa == Etapa.Publicado))
                        {
                            model.btnVerPublicar = Visualicion.SI;
                        }
                        else {
                            model.btnVerPublicar = Visualicion.NO;
                        }
                    }
                    model.btnVerPerfil = Visualicion.SI;
                    break;
                default:
                    model.btnVerEnviar = Visualicion.NO;
                    model.btnVerAceptar = Visualicion.NO;
                    model.btnVerAproRech = Visualicion.NO;
                    model.btnVerPublicar = Visualicion.NO;
                    model.btnVerPerfil = Visualicion.NO;
                    break;
            }
            #endregion

            return model;
        }

        public void actualizarDatosAreas(SolicitudAmpliacionCargoViewModel model, int ideArea, int ideDepartamento, int ideDependencia, int ideSede )
        {
           
                model.Areas.Add(_areaRepository.GetSingle(x => x.IdeArea == ideArea));
                model.Departamentos.Add(_departamentoRepository.GetSingle(x => x.IdeDepartamento == ideDepartamento));
                model.Dependencias.Add(_dependenciaRepository.GetSingle(x => x.IdeDependencia == ideDependencia && x.IdeSede == ideSede));
        }


        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult EstadoSolicitudPublicacion(string ideSolicitud)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                LogSolReqPersonal estado = _logAmpliacionRepository.estadoSolicitud(Convert.ToInt32(ideSolicitud));

                if ((Etapa.Publicado == estado.TipEtapa) || (Etapa.Finalizado == estado.TipEtapa))
                {
                    objJsonMessage.Resultado = true;
                    return Json(objJsonMessage);
                }
                else
                {
                    objJsonMessage.Mensaje = "El requerimiento no se encuentra publicado,para ver el estado revise en el menu 'Consulta de Requerimientos'";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }

        [ValidarSesion]
        public ActionResult Puesto(string ideSolicitud)
        {
            try
            {

                var perfilAmpliacionViewModel = inicializarPerfil();
                var usuario = Session[ConstanteSesion.UsuarioDes].ToString();
                if (ideSolicitud != null)
                {
                    IdeSolicitudAmpliacion = Convert.ToInt32(ideSolicitud);
                    var cargoAmpliacion = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == IdeSolicitudAmpliacion && x.TipoSolicitud == TipoSolicitud.Ampliacion);
                    perfilAmpliacionViewModel.SolicitudRequerimiento = cargoAmpliacion;
                }

                return PartialView(perfilAmpliacionViewModel);
            }
            catch (Exception)
            {
                
                return PartialView();
            }

        }


        public SolicitudAmpliacionCargoViewModel inicializarPerfil()
        {
            var ampliacionViewModel = new SolicitudAmpliacionCargoViewModel();
            ampliacionViewModel.SolicitudRequerimiento = new SolReqPersonal();


            return ampliacionViewModel;
        }

        public bool EnviarCorreo(Usuario usuarioDestinatario, string rolResponsable, string etapa, string observacion, string cargoDescripcion, string codCargo)
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

                enviarMail.EnviarCorreo(dir, etapa, rolResponsable, "Ampliación de Cargo", observacion, cargoDescripcion, codCargo, usuarioDestinatario.Email, "suceso");
                
               return true;
            }
            catch (Exception Ex)
            {
                return false;
                
            }

        }

        /// <summary>
        /// Envioa Correo a todos los usuarios configurados
        /// </summary>
        /// <param name="usuarioDestinatario"></param>
        /// <param name="rolResponsable"></param>
        /// <param name="etapa"></param>
        /// <param name="observacion"></param>
        /// <param name="cargoDescripcion"></param>
        /// <param name="codCargo"></param>
        /// <returns></returns>
        public bool EnviarCorreoAll(Usuario usuarioDestinatario, string rolResponsable, string etapa, string observacion, string cargoDescripcion, string codCargo,List<String> Sends,List<String> Copys)
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
               

                var objUsuario = (Usuario)Session[ConstanteSesion.ObjUsuario];

                if (objUsuario != null)
                {
                    enviarMail.Usuario = objUsuario.DscNombres + " " + objUsuario.DscApePaterno + " " + objUsuario.DscApeMaterno;
                }


                enviarMail.EnviarCorreoVarios(dir, etapa, rolResponsable, "Ampliación de Cargo", observacion, cargoDescripcion, codCargo, Sends, "suceso", Copys);

                return true;
            }
            catch (Exception Ex)
            {
                return false;

            }

        }

        /// <summary>
        /// Acepta la ampliacion de la solicitud
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult aceptarSolicitud(string id)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            var enviarMail = new SendMail();
            SedeNivel usuarioSession = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            

            var dir = Server.MapPath(@"~/TemplateEmail/EnviarSolicitud.htm");
            var SedeSession = Session[ConstanteSesion.Sede];
            string SedeDescripcion = "-";

            string msjFinal = "";

            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            int idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);

            List<String> listSends = null;
            List<String> listCopys = null;

            System.Collections.ArrayList lista = listaEmail(Convert.ToInt32(id), idRol, AccionEnvioEmail.AceptarPerfil, idSede, TipoSolicitud.Ampliacion);
            
            listSends = new List<String>();
            listSends = (List<String>)lista[0];

            listCopys = new List<String>();
            listCopys = (List<String>)lista[1];


            if (SedeSession != null)
            {
                SedeDescripcion = Session[ConstanteSesion.SedeDes].ToString();
            }

            var solicitud = new SolReqPersonal();
            if ((id != null) && (id != "0"))
            {
                solicitud = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(id));
            }
            
            try
            {

                if ((solicitud.TipEtapa == Etapa.Aprobado) && (Roles.Encargado_Seleccion == Convert.ToInt32(Session[ConstanteSesion.Rol])))
                {

                    string IndArea = "NO";
                    LogSolReqPersonal logSolicitud = new LogSolReqPersonal();

                    logSolicitud.IdeSolReqPersonal = Convert.ToInt32(solicitud.IdeSolReqPersonal);
                    
                    var logSolResponsable = _solicitudAmpliacionPersonal.responsablePublicacion(Convert.ToInt32(solicitud.IdeSolReqPersonal), solicitud.IdeSede);
                    logSolicitud.RolResponsable = logSolResponsable.RolResponsable;
                    logSolicitud.UsResponsable = logSolResponsable.UsResponsable;

                    logSolicitud.TipEtapa = Etapa.Aceptado;
                    logSolicitud.RolSuceso = Convert.ToInt32(Session[ConstanteSesion.Rol]);
                    logSolicitud.UsrSuceso = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                    logSolicitud.Observacion = "";

                    int ideUsuario = _logAmpliacionRepository.solicitarAprobacion(logSolicitud,Convert.ToInt32(solicitud.IdeSolReqPersonal), solicitud.IdeSede, solicitud.IdeArea, IndArea);

                    if (ideUsuario != -1)
                    {
                        var rolResponsable = _rolRepository.GetSingle(x=>x.IdRol == logSolResponsable.RolResponsable);
                        
                        var usuarioResp = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuario);
                        
                        var objUsuario = (Usuario)Session[ConstanteSesion.ObjUsuario];

                        if (objUsuario != null)
                        {
                            enviarMail.Usuario = objUsuario.DscNombres + " " + objUsuario.DscApePaterno + " " + objUsuario.DscApeMaterno;
                        }

                        
                        enviarMail.Rol = Session[ConstanteSesion.RolDes].ToString();
                        enviarMail.Sede = SedeDescripcion;
                        enviarMail.Area = usuarioSession.AREADES;

                        enviarMail.EnviarCorreoVarios(dir.ToString(), Etapa.Aceptado, usuarioResp.NombreUsuario, "Ampliación de Cargo", "", solicitud.nombreCargo, ""+solicitud.IdeSolReqPersonal, listSends, "Suceso", listCopys);

                        msjFinal = "El proceso de envío se realizó exitosamente. ";
                        msjFinal += "Derivada a " + rolResponsable.DscRol + " " + usuarioResp.DscNombres + " " + usuarioResp.DscApePaterno;
                        objJsonMessage.Mensaje = msjFinal;
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                    else
                    {
                        objJsonMessage.Mensaje = "ERROR: no se pudo aceptar la solicitud intente de nuevo";
                        objJsonMessage.Resultado = false;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    objJsonMessage.Mensaje = "ERROR no tiene permisos para la accion o el estado de la solicitud no requiere esta accion";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);
                }
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }
        }

        public SolicitudAmpliacionCargoViewModel inicializarGeneral()
        {
            
            var cargoViewModel = new SolicitudAmpliacionCargoViewModel();
            cargoViewModel.SolicitudRequerimiento = new SolReqPersonal();

            cargoViewModel.Accion = Accion.Nuevo;

            cargoViewModel.Sexos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSexos));
            cargoViewModel.Sexos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            cargoViewModel.TiposRequerimientos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoRequerimiento));
            cargoViewModel.TiposRequerimientos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });

            cargoViewModel.RangoSalariales = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario));
            cargoViewModel.RangoSalariales.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });


            return cargoViewModel;
        }


        #region GRILLAS PERFIL AMPLIACION
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

                lista = _solicitudAmpliacionPersonal.ListaCompetencias(IdeSolicitudAmpliacion);

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

                lista = _solicitudAmpliacionPersonal.ListaOfrecemos(IdeSolicitudAmpliacion);

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

                lista = _solicitudAmpliacionPersonal.ListaHorarios(IdeSolicitudAmpliacion);

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


                lista = _solicitudAmpliacionPersonal.ListaUbigeos(IdeSolicitudAmpliacion);

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

                lista = _solicitudAmpliacionPersonal.ListaCentroEstudio(IdeSolicitudAmpliacion);

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

                lista = _solicitudAmpliacionPersonal.ListaNivelAcademico(IdeSolicitudAmpliacion);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoEducacion == null?"":item.DescripcionTipoEducacion,
                                item.DescripcionAreaEstudio==null?"":item.DescripcionAreaEstudio,
                                item.DescripcionNivelAlcanzado==null?"":item.DescripcionNivelAlcanzado,
                                item.CicloSemestre==0?"":item.CicloSemestre.ToString(),
                                item.PuntajeNivelEstudio==0?"":item.PuntajeNivelEstudio.ToString(),
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

                lista = _solicitudAmpliacionPersonal.ListaConocimientos(IdeSolicitudAmpliacion, "OFIMATICA");

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeConocimientoGeneralRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionConocimientoOfimatica==null?"":item.DescripcionConocimientoOfimatica,
                                item.DescripcionNombreOfimatica==null?"":item.DescripcionNombreOfimatica,
                                item.DescripcionNivelConocimiento==null?"":item.DescripcionNivelConocimiento,
                                item.PuntajeConocimiento == 0?"":item.PuntajeConocimiento.ToString(),
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

                lista = _solicitudAmpliacionPersonal.ListaConocimientos(IdeSolicitudAmpliacion, "IDIOMA");

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

                lista = _solicitudAmpliacionPersonal.ListaConocimientos(IdeSolicitudAmpliacion, "GENERAL");


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

                lista = _solicitudAmpliacionPersonal.ListaExperiencia(IdeSolicitudAmpliacion);


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

                lista = _solicitudAmpliacionPersonal.ListaDiscapacidad(IdeSolicitudAmpliacion);

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

                lista = _solicitudAmpliacionPersonal.ListaEvaluacion(IdeSolicitudAmpliacion);

                var generic = GetListar(lista, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeEvaluacionRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExamen==null?"":item.DescripcionExamen,
                                item.DescripcionTipoExamen== null?"":item.DescripcionTipoExamen,
                                item.NotaMinimaExamen==null?"":item.NotaMinimaExamen.ToString(),
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

        #region lista ampliacion

        /// <summary>
        /// inicializa la busqueda de lista de reemplazo
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser]
        [ValidarSesion]
        public ActionResult Index()
        {
            SolicitudAmpliacionCargoViewModel model;
            try
            {
                model = new SolicitudAmpliacionCargoViewModel();

                var rolSession = Convert.ToInt32(Session[ConstanteSesion.Rol]);



                var sede = Session[ConstanteSesion.Sede];

                model = InicializarListasAmpliacion(Convert.ToInt32(sede), rolSession);

               

                #region botonera

                model.rolSession = rolSession;
                switch (rolSession)
                {
                    case Roles.Jefe:
                        {
                            model.btnVerRanking = Indicador.No;
                            model.btnVerPreSeleccion = Indicador.No;
                            model.btnVerNuevo = Indicador.Si;
                            model.btnVerRequerimiento = Indicador.No;
                            break;
                        }
                    case Roles.Gerente:
                        {
                            model.btnVerRanking = Indicador.No;
                            model.btnVerPreSeleccion = Indicador.No;
                            model.btnVerNuevo = Indicador.Si;
                            model.btnVerRequerimiento = Indicador.Si;
                            break;
                        }
                    case Roles.Gerente_General_Adjunto:
                        {
                            model.btnVerRanking = Indicador.No;
                            model.btnVerPreSeleccion = Indicador.No;
                            model.btnVerNuevo = Indicador.Si;
                            model.btnVerRequerimiento = Indicador.Si;
                            break;
                        }
                    case Roles.Encargado_Seleccion:
                        {
                            model.btnVerRanking = Indicador.Si;
                            model.btnVerPreSeleccion = Indicador.Si;
                            model.btnVerNuevo = Indicador.No;
                            model.btnVerRequerimiento = Indicador.Si;
                            break;
                        }
                    case Roles.Analista_Seleccion:
                        {
                            model.btnVerRanking = Indicador.Si;
                            model.btnVerPreSeleccion = Indicador.Si;
                            model.btnVerNuevo = Indicador.No;
                            model.btnVerRequerimiento = Indicador.Si;
                            break;
                        }

                    default:
                        {
                            model.btnVerRanking = Indicador.No;
                            model.btnVerPreSeleccion = Indicador.No;
                            model.btnVerNuevo = Indicador.No;
                            model.btnVerRequerimiento = Indicador.No;
                            break;
                        }
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }

            return View("Index", model);
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

            var listaResultado = new List<Departamento>(_departamentoRepository.GetBy(x => x.Dependencia.IdeDependencia == ideDependencia && x.EstadoActivo == IndicadorActivo.Activo));

            foreach (Departamento item in listaResultado)
            {
                item.Dependencia = null;
            }
            
            result = Json(listaResultado);
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

            var listaResultado = new List<Area>(_areaRepository.GetBy(x => x.Departamento.IdeDepartamento == ideDepartamento && x.EstadoActivo == IndicadorActivo.Activo));

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
        public SolicitudAmpliacionCargoViewModel InicializarListasAmpliacion(int idSel, int rolSession)
        {

            var model = new SolicitudAmpliacionCargoViewModel();
            model.SolicitudRequerimiento = new SolReqPersonal();

            var usuarioSede = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
            var idSede = Convert.ToInt32(Session[ConstanteSesion.Sede]);
            model.rolSession = rolSession;


            if ((idSel != null) || (idSel != 0))
            {

                if ((rolSession == Roles.Jefe) || (rolSession == Roles.Gerente))
                {

                    var dependencia = _dependenciaRepository.GetSingle(x => x.IdeDependencia == usuarioSede.IDEDEPENDENCIA
                                                                            && x.IdeSede == idSel && x.EstadoActivo == IndicadorActivo.Activo);
                    model.Dependencias = new List<Dependencia>();
                    model.Dependencias.Add(dependencia);

                    model.SolicitudRequerimiento.IdeDependencia = dependencia.IdeDependencia;

                    var departamento = _departamentoRepository.GetSingle(x => x.IdeDepartamento == usuarioSede.IDEDEPARTAMENTO);
                    model.Departamentos = new List<Departamento>();
                    model.Departamentos.Add(departamento);

                    model.SolicitudRequerimiento.IdeDepartamento = departamento.IdeDepartamento;

                    var area = _areaRepository.GetSingle(x => x.IdeArea == usuarioSede.IDEAREA);
                    model.Areas = new List<Area>();
                    model.Areas.Add(area);

                    model.SolicitudRequerimiento.IdeArea = area.IdeArea;

                   
                    if (rolSession == Roles.Jefe)
                    {
                        model.Etapas = new List<DetalleGeneral>(_detalleGeneralRepository.GetBy(x => x.General.IdeGeneral == 50 && x.Valor == "05"));
                    }
                    if (rolSession == Roles.Gerente)
                    {
                        model.Etapas = new List<DetalleGeneral>(_detalleGeneralRepository.GetBy(x => x.General.IdeGeneral == 50 && ((x.Valor == "05") || (x.Valor == "01"))));
                        model.Etapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
                    }



                }
                else
                {
                    if (rolSession == Roles.Gerente_General_Adjunto)
                    {
                        model.Etapas = new List<DetalleGeneral>(_detalleGeneralRepository.GetBy(x => x.General.IdeGeneral == 50 && ((x.Valor == "05") || (x.Valor == "02"))));
                        model.Etapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
                    }
                    else
                    {
                        model.Etapas = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoEtapa));
                        model.Etapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
                    }


                    model.Dependencias = new List<Dependencia>(_dependenciaRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo && x.IdeSede == idSel));
                    model.Dependencias.Insert(0, new Dependencia { IdeDependencia = 0, NombreDependencia = "Seleccionar" });

                    model.Departamentos = new List<Departamento>();
                    model.Departamentos.Insert(0, new Departamento { IdeDepartamento = 0, NombreDepartamento = "Seleccionar" });

                    model.Areas = new List<Area>();
                    model.Areas.Insert(0, new Area { IdeArea = 0, NombreArea = "Seleccionar" });

                }
            }
            int idRol = Convert.ToInt32(Session[ConstanteSesion.Rol]);
            if (Roles.Analista_Seleccion.Equals(idRol))
            {
                model.Etapas = new List<DetalleGeneral>(_detalleGeneralRepository.GetBy(x => x.General.IdeGeneral == 50 && ((x.Valor == "10") || (x.Valor == "04") || (x.Valor == "08"))));
                model.Etapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            }

            if (Roles.Encargado_Seleccion.Equals(idRol))
            {
                model.Etapas = new List<DetalleGeneral>(_detalleGeneralRepository.GetBy(x => x.General.IdeGeneral == 50 && ((x.Valor == "03") || (x.Valor == "10") || (x.Valor == "04") || (x.Valor == "08"))));
                model.Etapas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            }


            Cargo objCargo = new Cargo();

            objCargo.IdeSede = Convert.ToInt32(idSede);
           

            model.Cargos = new List<Cargo>(_cargoRepository.listarCargosSedeCodigo(objCargo.IdeSede));
            model.Cargos.Insert(0, new Cargo { CodigoCargo = "0", NombreCargo = "Seleccionar" });

            model.Estados = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadosSolicitud));
            model.Estados.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            model.Puestos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            model.Puestos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return model;
        }


        /// <summary>
        /// Lista de busqueda de reemplazo
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListBusquedaAmpliacion(GridTable grid)
        {

            SolReqPersonal solicitudRequerimiento;
            List<SolReqPersonal> lista = new List<SolReqPersonal>();
            try
            {

                    solicitudRequerimiento = new SolReqPersonal();

                    solicitudRequerimiento.CodCargo = (grid.rules[1].data == null ? "0" : grid.rules[1].data);
                    solicitudRequerimiento.IdeDependencia = (grid.rules[2].data == null ? 0 : Convert.ToInt32(grid.rules[2].data));
                    solicitudRequerimiento.IdeArea = (grid.rules[3].data == null ? 0 : Convert.ToInt32(grid.rules[3].data));
                    solicitudRequerimiento.TipResponsable = (grid.rules[4].data == null ? "" : grid.rules[4].data);

                    if (grid.rules[5].data != null && grid.rules[6].data != null)
                    {
                        solicitudRequerimiento.FechaInicioBus = Convert.ToDateTime(grid.rules[5].data);
                        solicitudRequerimiento.FechaFinBus = Convert.ToDateTime(grid.rules[6].data);
                    }

                    solicitudRequerimiento.IdeDepartamento = (grid.rules[7].data == null ? 0 : Convert.ToInt32(grid.rules[7].data));
                    solicitudRequerimiento.TipEtapa = (grid.rules[8].data == null ? "" : grid.rules[8].data);
                    solicitudRequerimiento.TipEstado = (grid.rules[9].data == null ? "" : grid.rules[9].data);

                    solicitudRequerimiento.Tipsol = TipoSolicitud.Ampliacion;

                    var IdRolResp = Session[ConstanteSesion.Rol];
                    solicitudRequerimiento.IdRolResp = Convert.ToInt32(IdRolResp);
                    
                    var idUsuarioResp = Session[ConstanteSesion.Usuario];
                    solicitudRequerimiento.idUsuarioResp = Convert.ToInt32(idUsuarioResp);

                    var idSede = Session[ConstanteSesion.Sede];
                    solicitudRequerimiento.IdeSede = Convert.ToInt32(idSede);

                    lista = _solicitudAmpliacionPersonal.GetListaAmpliacionPersonal(solicitudRequerimiento);
                //}


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
                                item.Tipsol==null?"":item.Tipsol,
                                item.Des_etapa==null?"":item.Des_etapa
                               
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
        /// view de publicacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidarSesion]
        public ActionResult Publica(string id , string pagina)
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

            var ObjSol = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(id));

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
            var contadorCompetencias = _competenciaReqRepository.CountByExpress(x => x.SolicitudRequerimiento.IdeSolReqPersonal == ObjSol.IdeSolReqPersonal);

            if (contadorCompetencias > 0)
            {
                model.visualizarCompetencias = Indicador.Si;
            }
            //visualizar ofrecemos

            var contadorOfrecemos = _ofrecemosReqRepository.CountByExpress(x => x.SolicitudRequerimiento.IdeSolReqPersonal == ObjSol.IdeSolReqPersonal);
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
        }


        public ActionResult actualizarFechaExpiracion(string idSolicitud, string fechaExpiracion)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            fechaExpiracion = String.Format("{0:dd/MM/yyyy}", fechaExpiracion);
            DateTime fecha = Convert.ToDateTime(fechaExpiracion);
            try
            {
                if ((idSolicitud != null) && (idSolicitud != "0"))
                {
                    var solicitud = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == Convert.ToInt32(idSolicitud));
                    solicitud.FecExpiracacion = fecha;
                    _solicitudAmpliacionPersonal.Update(solicitud);

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


                lista = _conocimientoGeneralReqRepository.listarConocimientosPublicacion(IdeSolReqPersonal);

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

                var generic = Listar(_nivelAcademicoReqRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeNivelAcademicoRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionTipoEducacion == null?"":item.DescripcionTipoEducacion.ToString(),
                                item.DescripcionAreaEstudio == null?"":item.DescripcionAreaEstudio.ToString(),
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

                var generic = Listar(_competenciaReqRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

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

                var generic = Listar(_experienciaReqRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeExperienciaRequerimiento.ToString(),
                        cell = new string[]
                            {
                                item.DescripcionExperiencia,
                                item.CantidadAnhosExperiencia.ToString() + " AÑO(S) y " +item.CantidadMesesExperiencia.ToString() +  " MES(ES)" 
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

                var generic = Listar(_ofrecemosReqRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

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


        public bool EnviarCorreo(Usuario usuarioDestinatario, string rolResponsable, string etapa, string tipoRq, string cargoDescripcion, string codCargo, List<String> Sends, List<String> Copys)
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
              
                var objUsuario = (Usuario)Session[ConstanteSesion.ObjUsuario];

                if (objUsuario != null)
                {
                    enviarMail.Usuario = objUsuario.DscNombres + " " + objUsuario.DscApePaterno + " " + objUsuario.DscApeMaterno;
                }

                enviarMail.EnviarCorreoVarios(dir, etapa, rolResponsable, tipoRq, "", cargoDescripcion, codCargo, Sends, "suceso", Copys);
                return true;
            }
            catch (Exception Ex)
            {
                return false;

            }

        }

        #endregion


    }
}
