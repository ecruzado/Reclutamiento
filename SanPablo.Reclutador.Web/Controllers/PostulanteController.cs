namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Models;
    using System.Collections.Generic;
    using SanPablo.Reclutador.Web.Core;
    using System.Web.Mvc;
    using System.Web;
    using System.Web.Services;
    using SanPablo.Reclutador.Entity.Validation;
    using FluentValidation.Results;
    using FluentValidation;
    using System.IO;
    using System;
    using System.Drawing;


    public class PostulanteController :  BaseController
    {
        private IPostulanteRepository _postulanteRepository;
        private IEstudioPostulanteRepository _estudioPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IUbigeoRepository _ubigeoRepository;
        private PostulanteGeneralViewModel postulanteModel = new PostulanteGeneralViewModel();
                
        public PostulanteController(IPostulanteRepository postulanteRepository,IEstudioPostulanteRepository estudioPostulanteRepository,IUbigeoRepository ubigeoRepository, IDetalleGeneralRepository detalleGeneralRepository)
        {
            _postulanteRepository = postulanteRepository;
            _estudioPostulanteRepository = estudioPostulanteRepository;
            _ubigeoRepository = ubigeoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
                        
        }
        #region General
        public PostulanteGeneralViewModel inicializarPostulante()
        {
            var postulanteGeneralViewModel = new PostulanteGeneralViewModel();
            postulanteGeneralViewModel.Postulante = new Postulante();

            postulanteGeneralViewModel.directorioImagen = "user4.png";

            postulanteGeneralViewModel.TipoDocumentos = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoDocumento));
            postulanteGeneralViewModel.TipoDocumentos.Insert(0,new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            
            postulanteGeneralViewModel.Nacionalidad = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.Nacionalidad));
            postulanteGeneralViewModel.Nacionalidad.Insert(0,new DetalleGeneral { Valor = "00", Descripcion = "Seleccionar" });
            
            postulanteGeneralViewModel.Sexo = new List<DetalleGeneral>( _detalleGeneralRepository.GetByTipoTabla(TipoTabla.Sexo));
            postulanteGeneralViewModel.Sexo.Insert(0,new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            
            postulanteGeneralViewModel.EstadosCiviles = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoCivil));
            postulanteGeneralViewModel.EstadosCiviles.Insert(0,new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });
            
            postulanteGeneralViewModel.TipoVias = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoVia));
            postulanteGeneralViewModel.TipoVias.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.TipoZonas = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoZona));
            postulanteGeneralViewModel.TipoZonas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.Departamentos = new List<Ubigeo>();
            postulanteGeneralViewModel.Departamentos = cargarDepartamentos();
            postulanteGeneralViewModel.Departamentos.Insert(0, new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });

            postulanteGeneralViewModel.Provincias = new List<Ubigeo>();
            postulanteGeneralViewModel.Provincias.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });

            postulanteGeneralViewModel.Distritos = new List<Ubigeo>();
            postulanteGeneralViewModel.Distritos.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "Seleccionar" });
                      
            return postulanteGeneralViewModel;
        }
        public ViewResult General()
        {
            var postulanteGeneralViewModel = inicializarPostulante();
            if (IdePostulante != 0)
            {
                postulanteGeneralViewModel.Postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
               // postulanteGeneralViewModel.directorioImagen = Bytes_A_Imagen(postulanteGeneralViewModel.Postulante.FotoPostulante);
                mostrarUbigeo(postulanteGeneralViewModel);
            }
            return View(postulanteGeneralViewModel);
        }

        [HttpPost]
        //public ActionResult General([Bind(Prefix = "Postulante")]Postulante postulante, HttpPostedFileBase fotoPostulante)
        public ActionResult General(PostulanteGeneralViewModel model, HttpPostedFileBase fotoPostulante)
        {
            try
            {
                PostulanteValidator validator = new PostulanteValidator();
                ValidationResult result = validator.Validate(model.Postulante, "TipoDocumento", "NumeroDocumento", "ApellidoPaterno", "ApellidoMaterno", "PrimerNombre",
                                          "SegundoNombre", "FechaNacimiento", "IndicadorSexo", "TipoEstadoCivil", "IdeUbigeo", "Correo", "TipoVia", "NumeroDireccion");

                byte[] data;
                if (!result.IsValid)
                {
                    postulanteModel = inicializarPostulante();
                    postulanteModel.Postulante = model.Postulante;
                    return View("General", postulanteModel);
                }

                //Guardar la foto del postulante
                if (model.FotoPostulante != null)
                {
                    using (Stream inputStream = model.FotoPostulante.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }
                    model.Postulante.FotoPostulante = data;

                }
                //Guardar postulante si es nuevo
                if (IdePostulante == 0)
                {
                    model.Postulante.IndicadorRegistroCompleto = "A";
                    _postulanteRepository.Add(model.Postulante);

                }
                else
                {
                    var postulante = model.Postulante;
                    var postulanteEdit = _postulanteRepository.GetSingle(x=>x.IdePostulante == IdePostulante);
                    #region copiar datos
                    postulanteEdit.TipoZona = postulante.TipoZona;
                    postulanteEdit.TipoVia = postulante.TipoVia;
                    postulanteEdit.TipoNacionalidad = postulante.TipoNacionalidad;
                    postulanteEdit.TipoEstadoCivil = postulante.TipoHorario;
                    postulanteEdit.TipoEstadoCivil = postulante.TipoEstadoCivil;
                    postulanteEdit.TipoDocumento = postulante.TipoDocumento;
                    postulanteEdit.TelefonoMovil = postulante.TelefonoMovil;
                    postulanteEdit.TelefonoFijo = postulante.TelefonoFijo;
                    postulanteEdit.SegundoNombre = postulante.SegundoNombre;
                    postulanteEdit.ReferenciaDireccion = postulante.ReferenciaDireccion;
                    postulanteEdit.PrimerNombre = postulante.PrimerNombre;
                    postulanteEdit.Observacion = postulante.Observacion;
                    postulanteEdit.NumeroLicencia = postulante.NumeroLicencia;
                    postulanteEdit.NumeroDocumento = postulante.NumeroDocumento;
                    postulanteEdit.NumeroDireccion = postulante.NumeroDireccion;
                    postulanteEdit.NombreZona = postulante.NombreZona;
                    postulanteEdit.NombreVia = postulante.NombreVia;
                    postulanteEdit.Manzana = postulante.Manzana;
                    postulanteEdit.Lote = postulante.Lote;
                    postulanteEdit.InteriorDireccion = postulante.InteriorDireccion;
                    postulanteEdit.IndicadorSexo = postulante.IndicadorSexo;
                    postulanteEdit.IdeUbigeo = postulante.IdeUbigeo;
                    postulanteEdit.FotoPostulante = postulante.FotoPostulante;
                    postulanteEdit.FechaNacimiento = postulante.FechaNacimiento;
                    postulanteEdit.Etapa = postulante.Etapa;
                    postulanteEdit.Correo = postulante.Correo;
                    postulanteEdit.Bloque = postulante.Bloque;
                    postulanteEdit.ApellidoPaterno = postulante.ApellidoPaterno;
                    postulanteEdit.ApellidoMaterno = postulante.ApellidoMaterno;
                    #endregion
                    _postulanteRepository.Update(postulanteEdit);
                }
                return RedirectToAction("../EstudioPostulante/Index");
            }
            catch (InvalidCastException e)
            {
               // throw new Exception("ERROR.", e);
                return View("General", postulanteModel);
            } 
        }
        
        public String Bytes_A_Imagen(Byte[] ImgBytes)
        {
            var rutaImagen = Server.MapPath(@"~/Content/images");
            Guid clave = Guid.NewGuid();
            String DirTemp = rutaImagen + "\\" + clave.ToString()+".jpg";

            if (ImgBytes != null)
            {
                //String DirTemp = Path.
                Bitmap imagen = null;
                Byte[] bytes = (Byte[])(ImgBytes);
                MemoryStream ms = new MemoryStream(bytes);
                imagen = new Bitmap(ms);
                imagen.Save(DirTemp, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return clave.ToString() + ".jpg";
        }

        #endregion


        #region DatosComplementarios

        public PostulanteGeneralViewModel inicializarDatosComplementarios()
        {
            var postulanteGeneralViewModel = new PostulanteGeneralViewModel();
            postulanteGeneralViewModel.Postulante = new Postulante();
            postulanteGeneralViewModel.TipoSueldosBrutos = _detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario);
            postulanteGeneralViewModel.TipoSueldosBrutos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.TipoDisponibilidadesTrabajos = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.DisponibilidadTrabajo));
            postulanteGeneralViewModel.TipoDisponibilidadesTrabajos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.TipoDisponibilidadesHorarios = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.DisponibilidadHorario));
            postulanteGeneralViewModel.TipoDisponibilidadesHorarios.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.TipoHorarios = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            postulanteGeneralViewModel.TipoHorarios.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            postulanteGeneralViewModel.TipoParientesSedes = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoParienteSede));
            postulanteGeneralViewModel.TipoParientesSedes.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "Seleccionar" });

            return postulanteGeneralViewModel;
        }
        public ViewResult DatosComplementarios()
        {
            var postulanteGeneralViewModel = inicializarDatosComplementarios();
            if (IdePostulante != 0)
            {
                postulanteGeneralViewModel.Postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            }
            return View(postulanteGeneralViewModel);
        }

        [HttpPost]
        public ActionResult DatosComplementarios([Bind(Prefix = "Postulante")]Postulante postulante)
        {
            PostulanteValidator validator = new PostulanteValidator();
            ValidationResult result = validator.Validate(postulante, "TipoSalario", "TipoDisponibilidadTrabajo", "TipoDisponibilidadHorario", "TipoComoSeEntero");
            
            if (!result.IsValid)
            {
                postulanteModel = inicializarDatosComplementarios();
                postulanteModel.Postulante = postulante;
                return View("DatosComplementarios", postulanteModel);
            }
            var postulanteEdit = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            postulanteEdit.TipoSalario = postulante.TipoSalario;
            postulanteEdit.TipoDisponibilidadTrabajo = postulante.TipoDisponibilidadTrabajo;
            postulanteEdit.TipoDisponibilidadHorario = postulante.TipoDisponibilidadHorario;
            postulanteEdit.TipoComoSeEntero = postulante.TipoComoSeEntero;
            postulanteEdit.TipoParienteSede = postulante.TipoParienteSede;
            postulanteEdit.TipoHorario = postulante.TipoHorario;
            postulanteEdit.ParienteCargo = postulante.ParienteCargo;
            postulanteEdit.ParienteNombre = postulante.ParienteNombre;
            postulanteEdit.IndicadorReubicarseInterior = postulante.IndicadorReubicarseInterior;
            postulanteEdit.IndicadorParientesCHSP = postulante.IndicadorParientesCHSP;
            postulanteEdit.DescripcionOtroMedio = postulante.DescripcionOtroMedio;
                       
            _postulanteRepository.Update(postulanteEdit);
            return RedirectToAction("../ParientePostulante/Index");
        }

        #endregion

        #region METODOS

        public List<Ubigeo> cargarDepartamentos()
        {
            var departamentos = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == null));
            return departamentos;
            
        }
        [HttpPost]
        public ActionResult  listarUbigeos (int ideUbigeoPadre)
        {
             ActionResult result = null;

             var listaResultado = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == ideUbigeoPadre));
            result = Json(listaResultado);
            return result;
        }

        public void mostrarUbigeo(PostulanteGeneralViewModel model)
        {
            int ubigeo = model.Postulante.IdeUbigeo;
            //recuperar distrito
            var distrito = new Ubigeo();
            var provincia = new Ubigeo();
            var departamento = new Ubigeo();

            distrito = _ubigeoRepository.GetSingle(x => x.IdeUbigeo == ubigeo);
            provincia = _ubigeoRepository.GetSingle(x => x.IdeUbigeo == distrito.IdeUbigeoPadre);
            departamento = _ubigeoRepository.GetSingle(x => x.IdeUbigeo == provincia.IdeUbigeoPadre);

            model.Distritos = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == provincia.IdeUbigeo));
            model.Provincias = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == departamento.IdeUbigeo));
            model.Departamentos = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == null));
            model.Departamentos.Insert(departamento.IdeUbigeo, departamento);
            

        }
        #endregion

 

        public ActionResult Referencias()
        {
            return View("Referencias");
        }

        public ActionResult Reclutamientos()
        {
            return View("Reclutamientos");
        }

        public ActionResult DetalleCargo()
        {
            return View("DetalleCargo");
        }

        public ActionResult InstruccionesExamen()
        {
            return View("InstruccionesExamen");
        }

        public ActionResult Lista()
        {
            return View();
        }

        public ActionResult Postulaciones()
        {
            return View();
        }
        public ActionResult EvaluacionPostulante()
        {
            return View("EvaluacionPostulante");
        }

        public ActionResult Examen()
        {
            return View("Examen");
        }


    }
}
