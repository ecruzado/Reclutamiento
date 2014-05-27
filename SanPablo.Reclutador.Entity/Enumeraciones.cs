namespace SanPablo.Reclutador.Entity
{
    public enum TipoTabla:int
    {
        //GENERAL
        EstadosSolicitud = 56,
        TipoVia = 9,
        TipoZona = 10,
        EstadoCivil = 8,
        Sexo = 15,
        TipoDocumento = 6,
        Nacionalidad = 7,
        DisponibilidadTrabajo = 12,
        DisponibilidadHorario = 13,
        TipoHorario = 14,
        TipoParienteSede = 38,
        TipoSalario = 11,
        TipoInstitucion = 17,

        //ESTUDIOS
        TipoNombreUnivesidad = 18,
        TipoNombreInstituto = 22,
        TipoNombreColegio = 23,
        TipoArea = 19,
        TipoEducacion = 20,
        //NivelAlcanzado = 21,

        //EXPERIENCIA
        TipoCargo = 24,
        TipoMotivoCese = 25,
        TipoCargoReferente = 24,

        //CONOCIMIENTOS GENERALES
        TipoConocimientoOfimatica = 27,
        TiponombreOfimatica = 28,
        TipoNivelConocimiento = 29,
        TipoIdioma = 30,
        TipoConocimientoIdioma=31,
        TipoConocimientoGral =32,
        TipoCGSoftware = 33,
        TipoCGPrimerosAux =34,
        TipoCGContabilidad = 35,

        //PARIENTE
        TipoVinculo = 36,

        //DISCAPACIDAD
        TipoDiscapacidad = 37,

        TipoCriterio = 1,
        Medicion = 2,
        EstadoMant = 3,
        Modo=4,
        TipoCalificacion = 5,
        TipoCategoria = 1,


        //MANTENIMIENTO CARGO

        TipoCompetencia = 39,
        TipoOfrecimiento = 40,
        TipoRequerimiento = 41,
        TipoSexos = 42,

        //MANTENIMIENTO DE ROL

        TipoSede = 43,

        //Solicitud Nuevo Cargo
        TipoEtapaSolicitud = 45,
        
        TipoEtapa = 50,

        //Tipo de Menu
        TipoMenu = 47,
        EstadoRegistro = 3,
        //Tipo de Vacante
        TipoVacante = 48,
        TipoSolicitud = 49,
        
        /// <summary>
        /// Estados del postulante
        /// </summary>
        EstadoPostulante = 51,

        EstadoEvaluacion = 52


    }

    public sealed class TipoModo
    {
        public const string Texto = "01";
        public const string Imagen = "02";
    }

    public sealed class TipoCampo
    {
        public const string TipoSalario = "14";
        
    }

    public sealed class IndicadorActivo
    {
        public const string Activo = "A";
        public const string Inactivo = "I";
    }

    public sealed class Indicador
    {
        public const string Si = "S";
        public const string No = "N";
    }


    public sealed class Contactado
    {
        public const string Si = "Si";
        public const string No = "No";
    }



    public sealed class Accion
    {
        public const string Nuevo = "N";
        public const string Editar = "E";
        public const string Eliminar = "D";
        public const string Actualizar = "A";
        public const string Consultar = "C";
        public const string Aprobar = "Aprobar";
        public const string Enviar = "Enviar";
        public const string Publicar = "Publicar";
        public const string Aceptar = "Aceptar";
        public const string Siguiente = "S";
        public const string Terminar = "T";
        public const string Close = "Close";


    }

    /// <summary>
    /// botones que envia email
    /// </summary>
    public sealed class AccionEnvioEmail
    {
        public const string EnviarSolicitud = "01";
        public const string AprobarSolicitud = "02";
        public const string RechazarSolicitud = "03";
        public const string EnviarPerfil = "04";
        public const string AprobarPerfil = "05";
        public const string ObservarPerfil = "06";
        public const string AceptarPerfil = "07";
        public const string Publicar = "08";
        public const string GrabarProg = "09";
        public const string Finalizar = "10";
    }


    public sealed class TipoRequerimientos
    {
        public const string jefe_corporativo_administrativo = "01";
        public const string personal_administrativo = "02";
        public const string personal_asistencial = "03";
        public const string personal_contacto = "04";
        public const string personal_operativo = "05";
    }

    public sealed class Etapa
    {
        public const string Pendiente = "01";
        public const string Validado = "02";
        public const string Aprobado = "03";
        public const string Publicado = "04";
        public const string Generacion_Perfil = "05";
        public const string Aprobacion_Perfil = "06";
        public const string Observado = "07";
        public const string Finalizado = "08";
        public const string Rechazado = "09";
        public const string Aceptado = "10";
    }

    /// <summary>
    /// Estados de la Evaluacion
    /// </summary>
    public sealed class EstadoEvaluacion
    {
        public const string Pendiente = "01";
        public const string Programado = "02";
        public const string Evaluado = "03";
        public const string Aprobado = "04";
        public const string Desaprobado = "05";
    }

    /// <summary>
    /// Estados de la Categoria
    /// </summary>
    public sealed class EstadoCategoria
    {
        public const string Pendiente = "01";
        public const string Evaluado = "02";
        public const string Finalizado = "03";
    }
    /// <summary>
    /// Notas referenciales
    /// </summary>
    public sealed class NotaEvaluacion
    {
        public const int Aprobado = 16;
        public const int Desaprobado = 7;
    }

    /// <summary>
    /// tipo de derivacion
    /// </summary>
    public sealed class TipoDerivacion
    {
        public const string Pendiente = "P";
        public const string Aprobado = "A";
        public const string Publicado = "U";
    }



    public sealed class Visualicion
    {
        public const string SI = "S";
        public const string NO = "N";
    }

    public sealed class Grilla
    {
        public const string Tabla1 = "Tabla1";
        public const string Tabla2 = "Tabla2";
    }

    public sealed class TipoInstitucion
    {
        public const string TipoUniversidad = "01";
        public const string TipoInstituto = "02";
        public const string TipoColegio = "03";
    }

    public sealed class TipoVinculo
    {
        public const string Hijo = "03";
    }

    public sealed class TipoSolicitud
    {
        public const string Nuevo = "01";
        public const string Ampliacion = "02";
        public const string Remplazo = "03";
        
        //caso de consulta de requerimientos
        public const string ConsultaRequerimientos = "04";
    }

    public sealed class Decision
    {
        public const string Aprobado = "A";
        public const string Rechazado = "R";
    }

    /// <summary>
    /// Roles del Sistema 
    /// </summary>
    public sealed class Roles
    {
        public const int Administrador_Sistema = 1;
        public const int Jefe = 2;
        public const int Gerente_General_Adjunto = 5;
        public const int Jefe_Corporativo_Seleccion = 6;
        public const int Consultor = 7;
        public const int Analista_Seleccion = 8;
        public const int Encargado_Seleccion = 9;
        public const int Postulante = 10;
        public const int Gerente =3;


    }

    public sealed class TipMenu
    {
        public const string Instranet = "I";
        public const string Extranet = "E";
    }

    public sealed class TipUsuario
    {
        public const string Instranet = "I";
        public const string Extranet = "E";
    }

    public sealed class UsuarioInicio
    {
        public const string Web = "Web";
        public const string POSTULANTE = "POSTULANTE";
    }

    public sealed class TipoExamen
    {
        public const string Examen = "01";
        public const string Evaluacion = "04";
    }

    public sealed class PostulanteEstado
    {
        public const string REGISTRADO = "01";
        public const string PRESELECCIONADO_AUTOMATICO = "02";
        public const string PRESELECCIONADO_MANUAL = "03";
        public const string NO_PRESELECCIONADO = "04";
        public const string EXCLUIDO = "05";
        public const string NO_APTO = "06";
        public const string EN_EVALUACION = "07";
        public const string SELECCIONADO = "08";
        public const string CONTRATADO = "09";
        public const string POSTULANTE_POTENCIAL = "10";
        public const string FINALIZADO = "11";
    
    }

    public sealed class TipoRequerimientoEmail
    {
        public const string Ampliacion = "Ampliación de Cargo";
        public const string Nuevo = "Nuevo Cargo";
    }

}
