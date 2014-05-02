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
    using SanPablo.Reclutador.Web.Models.JQGrid;
    using System.Configuration;
    using Newtonsoft.Json;

   
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
            
            postulanteGeneralViewModel.Nacionalidad = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.Nacionalidad));
            
            postulanteGeneralViewModel.Sexo = new List<DetalleGeneral>( _detalleGeneralRepository.GetByTipoTabla(TipoTabla.Sexo));
            postulanteGeneralViewModel.Sexo.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONE" });
            
            postulanteGeneralViewModel.EstadosCiviles = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.EstadoCivil));
            postulanteGeneralViewModel.EstadosCiviles.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONE" });
            
            postulanteGeneralViewModel.TipoVias = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoVia));
            postulanteGeneralViewModel.TipoVias.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONE" });

            postulanteGeneralViewModel.TipoZonas = new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoZona));
            postulanteGeneralViewModel.TipoZonas.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONE" });

            postulanteGeneralViewModel.Departamentos = new List<Ubigeo>();
            postulanteGeneralViewModel.Departamentos = cargarDepartamentos();
            postulanteGeneralViewModel.Departamentos.Insert(0, new Ubigeo { IdeUbigeo = 0, Nombre = "SELECCIONE" });

            postulanteGeneralViewModel.Provincias = new List<Ubigeo>();
            postulanteGeneralViewModel.Provincias.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "SELECCIONE" });

            postulanteGeneralViewModel.Distritos = new List<Ubigeo>();
            postulanteGeneralViewModel.Distritos.Add(new Ubigeo { IdeUbigeo = 0, Nombre = "SELECCIONE" });
                     
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
            if (IdePostulante==0)
            {
                Usuario user = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(Session[ConstanteSesion.Usuario]));
                postulanteGeneralViewModel.Postulante.Correo = user.CodUsuario;
            }
            
            if (IdePostulante != 0)
            {
                postulanteGeneralViewModel.Postulante = _postulanteRepository.GetSingle(x => x.IdePostulante == IdePostulante);
                mostrarUbigeo(postulanteGeneralViewModel);
            }
            return View(postulanteGeneralViewModel);
        }

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        [HttpPost]
        public ActionResult General(PostulanteGeneralViewModel model, HttpPostedFileBase FotoPostulante)
        {
            JsonMessage objJsonMessage = new JsonMessage();
            string fullPath = null;
            var usuarioExtranet = (Usuario)Session[ConstanteSesion.ObjUsuarioExtranet];
            var usuarioSession = usuarioExtranet.CodUsuario.Length <= 15 ? usuarioExtranet.CodUsuario : usuarioExtranet.CodUsuario.Substring(0, 15);
            try
            {
                PostulanteValidator validator = new PostulanteValidator();
                ValidationResult result = validator.Validate(model.Postulante, "TipoDocumento", "NumeroDocumento", "ApellidoPaterno", "ApellidoMaterno", "PrimerNombre",
                                          "SegundoNombre", "FechaNacimiento", "IndicadorSexo", "TipoEstadoCivil", "IdeUbigeo", "Correo", "TipoVia", "NumeroDireccion");

                byte[] data;
                if (!result.IsValid)
                {
                    //postulanteModel = inicializarPostulante();
                    //postulanteModel.Postulante = model.Postulante;
                    //return View("General", postulanteModel);
                    objJsonMessage.Mensaje = "Verificar los datos ingresados";
                    objJsonMessage.Resultado = false;
                    return Json(objJsonMessage);

                }

                //Guardar la foto del postulante
                //if (model.FotoPostulante != null)
                //{
                //    using (Stream inputStream = model.FotoPostulante.InputStream)
                //    {
                //        MemoryStream memoryStream = inputStream as MemoryStream;
                //        if (memoryStream == null)
                //        {
                //            memoryStream = new MemoryStream();
                //            inputStream.CopyTo(memoryStream);
                //        }
                //        data = memoryStream.ToArray();
                //        memoryStream.Dispose();
                //    }
                //    model.Postulante.FotoPostulante = data;
                    
                //}
                 if (!string.IsNullOrEmpty(model.nombreTemporalArchivo))
                    {

                        string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                        string directoryPath = "Archivos\\Imagenes\\";
                        fullPath = Path.Combine(applicationPath, string.Format("{0}{1}", directoryPath, model.nombreTemporalArchivo));

                        using (Stream s = System.IO.File.OpenRead(fullPath))
                        {
                            byte[] buffer = new byte[s.Length];
                            s.Read(buffer, 0, (int)s.Length);
                            int len = (int)s.Length;
                            s.Close();
                            model.Postulante.FotoPostulante = buffer;
                        }
                    }
                //Guardar postulante si es nuevo
                if (IdePostulante == 0)
                {
                    model.Postulante.EstadoActivo = IndicadorActivo.Activo;
                    model.Postulante.UsuarioCreacion = usuarioSession;
                    model.Postulante.FechaCreacion = FechaCreacion;
                    model.Postulante.Correo = usuarioExtranet.CodUsuario;
                    _postulanteRepository.Add(model.Postulante);
                    IdePostulante = model.Postulante.IdePostulante;
                    
                    var usuario = _usuarioRepository.GetSingle(x => x.IdUsuario == Convert.ToInt32(usuarioExtranet.IdUsuario) && x.IndicadorPostulante == TipUsuario.Instranet);
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
                    if (postulante.FotoPostulante != null)
                    {
                        postulanteEdit.FotoPostulante = postulante.FotoPostulante;
                    }
                    postulanteEdit.FechaNacimiento = postulante.FechaNacimiento;
                    postulanteEdit.Etapa = postulante.Etapa;
                    postulanteEdit.Bloque = postulante.Bloque;
                    postulanteEdit.ApellidoPaterno = postulante.ApellidoPaterno;
                    postulanteEdit.ApellidoMaterno = postulante.ApellidoMaterno;

                    var ObUsuarioExtranet = Session[ConstanteSesion.ObjUsuarioExtranet] == null ? "" : Session[ConstanteSesion.ObjUsuarioExtranet];
                    Usuario objUsuario;
                    string codUsuario = null;
                    if (ObUsuarioExtranet != "")
                    {
                        objUsuario = new Usuario();
                        objUsuario = (Usuario)ObUsuarioExtranet;

                        codUsuario = objUsuario.CodUsuario;
                    }


                    postulanteEdit.UsuarioModificacion = codUsuario.ToString().Length <= 15 ? codUsuario.ToString() : codUsuario.ToString().Substring(0, 15);

                    postulanteEdit.FechaModificacion = FechaModificacion;
                    #endregion
                    _postulanteRepository.Update(postulanteEdit);
                }
                objJsonMessage.Resultado = true;
                return Json(objJsonMessage);
                // return RedirectToAction("../EstudioPostulante/Index");

            }
            catch (InvalidCastException e)
            {
                objJsonMessage.Mensaje = "ERROR" + e;
                objJsonMessage.Resultado = false;
                return Json(objJsonMessage);
            } 
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
            JsonMessage objJson = new JsonMessage();
            objJson.Resultado = false;
            if (IdePostulante != 0)
            {
                objJson.Resultado = true;
            }
            else
            {
                objJson.Resultado = false;
            }
            return Json(objJson);
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
        [HttpPost]
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

        [HttpPost]
        public ActionResult validarFechaNacimiento(DateTime date)
        {
            ActionResult result = null;

            bool valido = false;
            DateTime fechaValida = DateTime.Now.AddYears(-18);

            if (date < fechaValida)
            {
                valido = true;
                
            }
            return result = Json(valido);
            
        }


        #region DatosComplementarios

        public PostulanteGeneralViewModel inicializarDatosComplementarios()
        {
            var postulanteGeneralViewModel = new PostulanteGeneralViewModel();
            postulanteGeneralViewModel.Postulante = new Postulante();
            postulanteGeneralViewModel.TipoSueldosBrutos = _detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoSalario);
            postulanteGeneralViewModel.TipoSueldosBrutos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONE" });

            postulanteGeneralViewModel.TipoDisponibilidadesTrabajos = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.DisponibilidadTrabajo));
            postulanteGeneralViewModel.TipoDisponibilidadesTrabajos.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONE" });

            postulanteGeneralViewModel.TipoDisponibilidadesHorarios = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.DisponibilidadHorario));
            postulanteGeneralViewModel.TipoDisponibilidadesHorarios.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONE" });

            postulanteGeneralViewModel.TipoHorarios = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoHorario));
            postulanteGeneralViewModel.TipoHorarios.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONE" });

            postulanteGeneralViewModel.TipoParientesSedes = 
            new List<DetalleGeneral>(_detalleGeneralRepository.GetByTipoTabla(TipoTabla.TipoParienteSede));
            postulanteGeneralViewModel.TipoParientesSedes.Insert(0, new DetalleGeneral { Valor = "0", Descripcion = "SELECCIONE" });

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

        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        [HttpPost]
        public ActionResult DatosComplementarios([Bind(Prefix = "Postulante")]Postulante postulante)
        {
            PostulanteValidator validator = new PostulanteValidator();
            ValidationResult result = validator.Validate(postulante, "TipoSalario", "TipoDisponibilidadTrabajo", "TipoDisponibilidadHorario", "TipoComoSeEntero",
                                                        "TipoParienteSede", "ParienteNombre", "ParienteCargo");
            
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
                var usuarioSession = (Usuario)Session[ConstanteSesion.ObjUsuarioExtranet];

                postulanteEdit.UsuarioModificacion = usuarioSession.CodUsuario.Length <= 15 ? usuarioSession.CodUsuario : usuarioSession.CodUsuario.Substring(0, 15);
                postulanteEdit.FechaModificacion = FechaModificacion;

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

            if (distrito!=null)
            {
                provincia = _ubigeoRepository.GetSingle(x => x.IdeUbigeo == distrito.IdeUbigeoPadre);
                departamento = _ubigeoRepository.GetSingle(x => x.IdeUbigeo == provincia.IdeUbigeoPadre);

                model.Distritos = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == provincia.IdeUbigeo));
                model.Provincias = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == departamento.IdeUbigeo));
                model.Departamentos = new List<Ubigeo>(_ubigeoRepository.GetBy(x => x.IdeUbigeoPadre == null));
                model.Departamentos.Insert(departamento.IdeUbigeo, departamento);

            }
            
            

        }
        #endregion


        [ValidarSesion(TipoServicio = TipMenu.Extranet)]
        public ActionResult Lista()
        {
            return View();
        }

        /// <summary>
        /// Subida de imagen a la carpeta temporal
        /// </summary>
        /// <param name="file"></param>
        /// <param name="forms"></param>
        /// <returns></returns>
        [HttpPost]
        public string Upload(HttpPostedFileBase file, FormCollection forms)
        {
            var jsonResponse = new JsonResponse { Success = false };

            try
            {
                string[] extensiones = forms.Get("ext").Split(';');

                string extensionArchivo = Path.GetExtension(file.FileName);

                if (extensiones.Contains(extensionArchivo.ToLower()))
                {
                    var content = new byte[file.ContentLength];
                    file.InputStream.Read(content, 0, file.ContentLength);

                    var indexOfLastDot = file.FileName.LastIndexOf('.');
                    var extension = file.FileName.Substring(indexOfLastDot + 1, file.FileName.Length - indexOfLastDot - 1);
                    var name = file.FileName.Substring(0, indexOfLastDot);

                    string applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                    string directoryPath = ConfigurationManager.AppSettings["ImageFilePath"];
                    string nombreTemporalArchivo = Guid.NewGuid().ToString();
                    string fullPath = Path.Combine(applicationPath, string.Format("{0}{1}{2}", directoryPath, nombreTemporalArchivo, extensionArchivo));

                    System.IO.File.WriteAllBytes(fullPath, content);


                    jsonResponse.Data = new
                    {
                        NombreArchivo = file.FileName,
                        NombreTemporalArchivo = string.Format("{0}{1}", nombreTemporalArchivo, extensionArchivo)
                    };
                    jsonResponse.Success = true;

                }
                else
                {
                    jsonResponse.Success = false;
                    jsonResponse.Message = "0";

                }
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                jsonResponse.Message = "Ocurrio un error, por favor intente de nuevo o más tarde.";
            }

            return JsonConvert.SerializeObject(jsonResponse);
        }


        /// <summary>
        /// valida que tenga registrado conocimientos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult validaConocimiento()
        {
            JsonMessage objMensaje = new JsonMessage();
            int idUsuario = 0;

            idUsuario = (Session[ConstanteSesion.Usuario] == null ? 0 : (Convert.ToInt32(Session[ConstanteSesion.Usuario])));

            var ObjUsuario = _usuarioRepository.GetSingle(x => x.IdUsuario == idUsuario
                                                          && x.TipUsuario == TipUsuario.Extranet
                                                          && x.FlgEstado == IndicadorActivo.Activo);

            if (ObjUsuario != null)
            {
                List<ConocimientoGeneralPostulante> listaConocimientos = (List<ConocimientoGeneralPostulante>)_conocimientoGeneralRepository.GetBy(x => x.Postulante.IdePostulante == ObjUsuario.IdePostulante);

                if (listaConocimientos != null && listaConocimientos.Count > 0)
                {
                    objMensaje.Resultado = true;
                }
                else
                {
                    objMensaje.Resultado = false;
                    objMensaje.Mensaje = "Registre sus conocimientos";
                }
            }

            return Json(objMensaje);
        }

    }
}
