namespace SanPablo.Reclutador.Web.Controllers
{
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using SanPablo.Reclutador.Web.Models;
    using System.Collections.Generic;
    using SanPablo.Reclutador.Web.Core;
    using System.Web.Mvc;
    using System.Web;
    using System.Linq;
    using System.Web.Services;
    using SanPablo.Reclutador.Entity.Validation;
    using FluentValidation.Results;
    using FluentValidation;
    using System.IO;
    using System;
    using System.Drawing;
    using System.Web.Routing;

   
    public class PostulanteController :  BaseController
    {
        private IPostulanteRepository _postulanteRepository;
        private IEstudioPostulanteRepository _estudioPostulanteRepository;
        private IDetalleGeneralRepository _detalleGeneralRepository;
        private IUbigeoRepository _ubigeoRepository;
        private IExperienciaPostulanteRepository _experienciaGeneralRepository;
        private IConocimientoGeneralPostulanteRepository _conocimientoGeneralRepository;
        private IParientePostulanteRepository _parienteGeneralRepository;
        private IDiscapacidadPostulanteRepository _discapacidadGeneralRepository;
        private IUsuarioRepository _usuarioRepository;

        private PostulanteGeneralViewModel postulanteModel = new PostulanteGeneralViewModel();
                
        public PostulanteController(IPostulanteRepository postulanteRepository,
                                    IEstudioPostulanteRepository estudioPostulanteRepository,
                                    IUbigeoRepository ubigeoRepository, 
                                    IDetalleGeneralRepository detalleGeneralRepository,
                                    IExperienciaPostulanteRepository experienciaGeneralRepository,
                                    IConocimientoGeneralPostulanteRepository conocimientoGeneralRepository,
                                    IParientePostulanteRepository parienteGeneralRepository,
                                    IDiscapacidadPostulanteRepository discapacidadGeneralRepository,
                                    IUsuarioRepository usuarioRepository)
        {
            _postulanteRepository = postulanteRepository;
            _estudioPostulanteRepository = estudioPostulanteRepository;
            _ubigeoRepository = ubigeoRepository;
            _detalleGeneralRepository = detalleGeneralRepository;
            _experienciaGeneralRepository = experienciaGeneralRepository;
            _conocimientoGeneralRepository = conocimientoGeneralRepository;
            _parienteGeneralRepository = parienteGeneralRepository;
            _discapacidadGeneralRepository = discapacidadGeneralRepository;
            _usuarioRepository = usuarioRepository;            
        }
        #region General
        public PostulanteGeneralViewModel inicializarPostulante()
        {
            var postulanteGeneralViewModel = new PostulanteGeneralViewModel();
            postulanteGeneralViewModel.Postulante = new Postulante();

            postulanteGeneralViewModel.directorioImagen = "user4.png";
            //postulanteGeneralViewModel.Postulante.FechaNacimiento = DateTime.Now;

            postulanteGeneralViewModel.porcentaje = Convert.ToInt32(Session["Progreso"]);

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
                     
            //identificar si el usuario tiene CV ingresado
            var idUsuario = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
            var usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == idUsuario);
            IdePostulante = usuario.IdePostulante;
            return postulanteGeneralViewModel;
        }

        /// <summary>
        /// Realiza la validacion si tiene permisos para acceder a la pagina
        /// </summary>
        /// <returns></returns>
        public RouteValueDictionary verificaLogeo() 
        {

            var myListOp = (List<SanPablo.Reclutador.Entity.MenuItem>)Session["ListaMenu"];

            string rutaAbsoluta = (Request.Path).ToUpper();

            int indWeb = rutaAbsoluta.IndexOf("/INTRANET/");
            var tieneAcceso = myListOp.Where(x => x.DSCURL == Request.Path).ToList();

            
            if (tieneAcceso != null )
            {
                return null;
            }
            else
            {
                if (indWeb != -1)
                {
                    //intranet
                    var routeValues = new RouteValueDictionary();
                    routeValues["controller"] = "Seguridad";
                    routeValues["action"] = "Login";
                    routeValues["area"] = "Intranet";
                    return routeValues;

                }
                else
                {
                    //extranet
                    var routeValues = new RouteValueDictionary();
                    routeValues["controller"] = "Seguridad";
                    routeValues["action"] = "Login";
                    return routeValues;
                   
                }
            }
        }

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult General()
        {

            RouteValueDictionary retorno = new RouteValueDictionary();
            retorno = verificaLogeo();

            if (retorno != null)
            {
                return RedirectToRoute(retorno);
            }
           
            
            var postulanteGeneralViewModel = inicializarPostulante();
            if (IdePostulante != 0)
            {
                postulanteGeneralViewModel.Postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                mostrarUbigeo(postulanteGeneralViewModel);
            }
            return View(postulanteGeneralViewModel);
        }

        [HttpPost]
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
                        memoryStream.Dispose();
                    }
                    model.Postulante.FotoPostulante = data;
                    
                }
                //Guardar postulante si es nuevo
                if (IdePostulante == 0)
                {
                    _postulanteRepository.Add(model.Postulante);
                    IdePostulante = model.Postulante.IdePostulante;
                    var ideUsuario = Convert.ToInt32(Session[ConstanteSesion.Usuario]);
                    var usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == ideUsuario);
                    usuario.IdePostulante = IdePostulante;
                    usuario.IndicadorPostulante = Indicador.Si;
                    _usuarioRepository.Update(usuario);
                    
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
                    if (fotoPostulante != null)
                    {
                        postulanteEdit.FotoPostulante = postulante.FotoPostulante;
                    }
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
                ms.Dispose();

            }
            return clave.ToString() + ".jpg";
        }
        
        public ActionResult GetImage(int id)
        {
            var firstOrDefault = _postulanteRepository.GetSingle(x => x.IdePostulante == id);
            if (firstOrDefault.FotoPostulante != null)
            {
                byte[] image = firstOrDefault.FotoPostulante;
                return File(image, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public ActionResult pestañasActivas()
        {
            ActionResult result = null;
            List<int> pestanas = new List<int>();
            if (IdePostulante != 0)
            {
                var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                if (_estudioPostulanteRepository.CountByExpress(x => x.Postulante == postulante) > 0)
                { pestanas.Add(1); }
                if (_experienciaGeneralRepository.CountByExpress(x => x.Postulante == postulante) > 0)
                { pestanas.Add(2); }
                if (_conocimientoGeneralRepository.CountByExpress(x => x.Postulante == postulante) > 0)
                { pestanas.Add(3); }
                if (postulante.TipoSalario != null)
                { pestanas.Add(4); }
                if (_parienteGeneralRepository.CountByExpress(x => x.Postulante == postulante) > 0)
                { pestanas.Add(5); }
                if (_discapacidadGeneralRepository.CountByExpress(x => x.Postulante == postulante) > 0)
                { pestanas.Add(6); }
                  pestanas.Add(9);
            }


            result = Json(pestanas);
            return result;
        }

        public ActionResult IndicadorSession()
        {
            ActionResult result = null;
            bool indicador = false;
            if (IdePostulante != 0)
            {
                indicador = true;
            }
            else
            {
                indicador = false;
            }
            result = Json(indicador);
            return result;
        }

        [HttpPost]
        public ActionResult mostrarAlerta()
        {
            ActionResult result = null;
            string indicador = "N";
            if (IdePostulante != 0) 
            {
                var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                indicador = postulante.IndicadorRegistroCompleto;
            }
            result = Json(indicador);
            return result;
        }
        
        [HttpPost]
        public void actualizarAlerta()
        {
            if (IdePostulante != 0)
            {
                var postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                postulante.IndicadorRegistroCompleto = "X";
                _postulanteRepository.Update(postulante);                
            }
            
        }
        #endregion

        public ActionResult validarDocumento(string dni)
        {
            ActionResult result = null;
            bool valido = false;
            int nroDocumentos = _postulanteRepository.CountByExpress(x => x.NumeroDocumento == dni);

            if (nroDocumentos == 0)
            {
                valido = true;
            }
            else
            {
                valido = false;
            }
            return result = Json(valido);
        }

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

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
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
            if (IdePostulante != 0)
            {
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
            }
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

        [HttpPost]
        public ViewResult subirFoto()
        {
            var postulanteGeneralViewModel = inicializarPostulante();
            
            if (IdePostulante != 0)
            {
                postulanteGeneralViewModel.Postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
            }
            return View(postulanteGeneralViewModel);
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


        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Lista()
        {
            return View();
        }




        #region ListaOfertasLaborales
        /// <summary>
        /// Lista de Ofertas Laborales
        /// </summary>       
        [HttpPost]
        public ActionResult ListaOfertasLaborales(string sidx, string sord, int page, int rows)
        {
            ActionResult result = null;
            List<object> lstFilas = new List<object>();

            var fila1 = new
            {
                id = 1,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "13/05/2013",
                          "Santiago de Surco, Lima",
                          "Secretaria de Hospitalización",                          
                          "Tiempo Parcial",    
                          "15/07/2013",
                     
                }
            };
            lstFilas.Add(fila1);

            var fila2 = new
            {
                id = 2,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "23/05/2013",
                          "San Juan de Miraflores, Lima",
                          "Técnico en Enfermería",                          
                          "Tiempo Completo",            
                          "15/07/2013",
                }
            };
            lstFilas.Add(fila2);

            var fila3 = new
            {
                id = 3,                 // ID único de la fila
                cell = new string[] {   // Array de celdas de la fila
                          "23/05/2013",
                          "Rimac, Lima",
                          "Ayudante de cocina",                          
                          "Tiempo Completo",         
                          "31/07/2013",
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
        #endregion
    }
}
