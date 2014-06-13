namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Core;
    using SanPablo.Reclutador.Web.Models;
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Linq;
    using NHibernate.Criterion;

    public class ParientePostulanteController : BaseController
    {
        private IParientePostulanteRepository _parientePostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IPostulanteRepository _postulanteRepository;

        public ParientePostulanteController(IParientePostulanteRepository parientePostulanteRepository, 
                                            IDetalleGeneralRepository detalleGeneralRepository,
                                            IPostulanteRepository postulanteRepository)
        {
            _parientePostulanteRepository = parientePostulanteRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _postulanteRepository = postulanteRepository;
        }




        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Index()
        {
            var parienteViewModel = InicializarParientes();
            var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            parienteViewModel.Pariente.Postulante = postulante;
            return View(parienteViewModel);
        }

        [HttpPost]
        public virtual JsonResult Listar(GridTable grid)
        {

            try
            {
                grid.page = (grid.page == 0) ? 1 : grid.page;

                grid.rows = (grid.rows == 0) ? 100 : grid.rows;

                DetachedCriteria where = DetachedCriteria.For<ParientePostulante>();
                where.Add(Expression.Eq("Postulante.IdePostulante", IdePostulante));

                var generic = Listar(_parientePostulanteRepository, grid.sidx, grid.sord, grid.page, grid.rows, grid._search, grid.searchField, grid.searchOper, grid.searchString, where);

                generic.Value.rows = generic.List
                    .Select(item => new Row
                    {
                        id = item.IdeParientePostulante.ToString(),
                        cell = new string[]
                            {
                                //item.IdeParientePostulante.ToString(),
                                item.ApellidoPaterno.ToUpper(),
                                item.ApellidoMaterno.ToUpper(),
                                item.Nombres.ToUpper(),
                                item.DescripcionVinculo,
                                String.Format("{0:dd/MM/yyyy}", item.FechaNacimiento)
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

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ViewResult Edit(string id)
        {
            var parientePostulanteViewModel = InicializarParientes();
            if (id == "0")
            {
                return View(parientePostulanteViewModel);
            }
            else
            {
                var parienteResultado = new ParientePostulante();
                parienteResultado = _parientePostulanteRepository.GetSingle(x => x.IdeParientePostulante == Convert.ToInt32(id));
                parientePostulanteViewModel.Pariente = parienteResultado;
                return View(parientePostulanteViewModel);
            }
        }


        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "Pariente")]ParientePostulante parientePostulante)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            try
            {
                var objetoUsuario = (Usuario)Session[ConstanteSesion.ObjUsuarioExtranet];
                string usuarioActual = objetoUsuario.CodUsuario.Length <= 15 ? objetoUsuario.CodUsuario : objetoUsuario.CodUsuario.Substring(0, 15);


                if (!ModelState.IsValid)
                {
                    //return Json(new { msj = false }, JsonRequestBehavior.DenyGet);
                    var parientePostulanteViewModel = InicializarParientes();
                    parientePostulanteViewModel.Pariente = parientePostulante;
                    return View(parientePostulanteViewModel);
                }

                if (!existePariente(parientePostulante))
                {
                    if (parientePostulante.IdeParientePostulante == 0)
                    {
                       
                        parientePostulante.EstadoActivo = IndicadorActivo.Activo;
                        parientePostulante.FechaCreacion = FechaCreacion;
                        parientePostulante.UsuarioCreacion = usuarioActual;
                        var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                        postulante.agregarPariente(parientePostulante);
                        _parientePostulanteRepository.Add(parientePostulante);
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);

                    }
                    else
                    {
                        var parienteEdit = _parientePostulanteRepository.GetSingle(x => x.IdeParientePostulante == parientePostulante.IdeParientePostulante);
                        parienteEdit.TipoDeVinculo = parientePostulante.TipoDeVinculo;
                        parienteEdit.Nombres = parientePostulante.Nombres;
                        parienteEdit.FechaNacimiento = parientePostulante.FechaNacimiento;
                        parienteEdit.ApellidoPaterno = parientePostulante.ApellidoPaterno;
                        parienteEdit.ApellidoMaterno = parientePostulante.ApellidoMaterno;
                        parienteEdit.FechaModificacion = FechaModificacion;
                        parienteEdit.UsuarioModificacion = usuarioActual;
                        _parientePostulanteRepository.Update(parienteEdit);
                        objJsonMessage.Resultado = true;
                        return Json(objJsonMessage);
                    }
                }
                else
                {
                    objJsonMessage.Mensaje = "ERROR: Ud. ya ingreso a este tipo de vínculo, verifique la información a ingresar";
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

        public ParientePostulanteGeneralViewModel InicializarParientes()
        {
            var parientePostulanteGeneralViewModel = new ParientePostulanteGeneralViewModel();
            parientePostulanteGeneralViewModel.Pariente = new ParientePostulante();

            parientePostulanteGeneralViewModel.porcentaje = Convert.ToInt32(Session["Progreso"]);

            parientePostulanteGeneralViewModel.TipoVinculos = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoVinculo).OrderBy(x=>x.Descripcion));
            parientePostulanteGeneralViewModel.TipoVinculos.Insert(0, new DetalleGeneral { Valor = "00", Descripcion = "SELECCIONE" });


            return parientePostulanteGeneralViewModel;
        }


        [HttpPost]
        public ActionResult eliminarPariente(int idePariente)
        {
            ActionResult result = null;

            var parienteEliminar = new ParientePostulante();
            parienteEliminar = _parientePostulanteRepository.GetSingle(x => x.IdeParientePostulante == idePariente);
            _parientePostulanteRepository.Remove(parienteEliminar);
           

            return result;
        }

        public bool existePariente(ParientePostulante pariente)
        {
            bool respuesta = false;
            int nroPariente = 0;
            if (TipoVinculo.Hijo != pariente.TipoDeVinculo)
            {
                if (pariente.IdeParientePostulante == 0)
                {
                    nroPariente = _parientePostulanteRepository.CountByExpress(x => x.Postulante.IdePostulante == IdePostulante
                                                                               && x.TipoDeVinculo == pariente.TipoDeVinculo);

                }
                else
                {
                    nroPariente = _parientePostulanteRepository.CountByExpress(x => x.Postulante.IdePostulante == IdePostulante
                                                                               && x.TipoDeVinculo == pariente.TipoDeVinculo
                                                                               && x.IdeParientePostulante != pariente.IdeParientePostulante);
                }
                if (nroPariente > 0)
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }

            }
            return respuesta;

        }


       
    }
}
