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

        public SolicitudAmpliacionCargoController(IDetalleGeneralRepository detalleGeneralRepository,
                                                  ISolReqPersonalRepository solicitudAmpliacionPersonal,
                                                  ICargoRepository cargoRepository)
        {
            _detalleGeneralRepository = detalleGeneralRepository;
            _solicitudAmpliacionPersonal = solicitudAmpliacionPersonal;
            _cargoRepository = cargoRepository;
            
        }
        
        [AuthorizeUser]        
        public ActionResult Index()
        {
            return View();
        }
        [ValidarSesion]
        [AuthorizeUser]  
        public ActionResult Edit(string idSolictud)
        {
            SolicitudAmpliacionCargoViewModel solicitudModel = inicializarAmpliacionCargo();
            int idSolicitudAmpliacion = Convert.ToInt32(idSolictud);
            if (idSolicitudAmpliacion != 0)
            {
                solicitudModel.SolReqPersonal = _solicitudAmpliacionPersonal.GetSingle(x => x.IdeSolReqPersonal == idSolicitudAmpliacion);
            }
            else
            {
                var usuario = (SedeNivel)Session[ConstanteSesion.UsuarioSede];
                solicitudModel.SolReqPersonal.IdeArea = usuario.IDEAREA;
                solicitudModel.SolReqPersonal.IdeDepartamento = usuario.IDEDEPARTAMENTO;
                solicitudModel.SolReqPersonal.IdeDependencia = usuario.IDEDEPENDENCIA;
            }
            return View(solicitudModel);
        }

        [ValidarSesion(TipoDevolucionError = Core.TipoDevolucionError.Json)]
        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "SolReqPersonal")]SolReqPersonal solicitudAmpliacion)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                SolReqPersonalValidator validation = new SolReqPersonalValidator();
                ValidationResult result = validation.Validate(solicitudAmpliacion, "IdeCargo", "NumVacantes", "Observacion", "Motivo");
                
                if (!result.IsValid)
                {
                    var solicitudAmpliacionModel = inicializarAmpliacionCargo();
                    solicitudAmpliacionModel.SolReqPersonal = solicitudAmpliacion;
                    return View(solicitudAmpliacionModel);
                }
                solicitudAmpliacion.EstadoActivo = "A";
                solicitudAmpliacion.FechaCreacion = FechaCreacion;
                solicitudAmpliacion.UsuarioCreacion = "YO";
                solicitudAmpliacion.FechaModificacion = FechaCreacion;

                _solicitudAmpliacionPersonal.Add(solicitudAmpliacion);

                objJsonMessage.Mensaje = "Solicitud enviada correctamente";
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
            }
            catch (Exception ex)
            {
                objJsonMessage.Mensaje = "ERROR:" + ex.Message;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            }

        }

        public SolicitudAmpliacionCargoViewModel inicializarAmpliacionCargo()
        {
            SolicitudAmpliacionCargoViewModel model = new SolicitudAmpliacionCargoViewModel();
            model.SolReqPersonal = new SolReqPersonal();
            model.Cargos = new List<Cargo>(_cargoRepository.GetBy(x => x.EstadoActivo == IndicadorActivo.Activo));
            model.Cargos.Insert(0, new Cargo { IdeCargo = 0, NombreCargo = "Seleccionar" });

            return model;
        }





    }
}
