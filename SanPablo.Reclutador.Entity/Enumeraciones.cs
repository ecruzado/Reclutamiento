namespace SanPablo.Reclutador.Entity
{
    public enum TipoTabla:int
    {
        //GENERAL
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
        NivelAlcanzado = 21,

        //EXPERIENCIA
        TipoCargo = 24,
        TipoMotivoCese = 25,
        TipoCargoReferente = 19,

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
        TipoSucesoSolicitud = 46,

        //Tipo de Menu
        TipoMenu = 47

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
    public sealed class Accion
    {
        public const string Nuevo = "N";
        public const string Editar = "E";
        public const string Eliminar = "D";
        public const string Actualizar = "A";
        public const string Consultar = "C";

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

    public sealed class EtapasSolicitud
    {
        public const string PendienteAprobacion = "PA";
        public const string PendienteAprobacionPerfil = "PAP";
        public const string ElaboracionPerfil = "EP";
        public const string PendientePublicacion = "PP";
        public const string Finalizado = "F";

    }
   
    public sealed class EstadoSolicitud
    {
        public const string Aprobado = "A";
        public const string Rechazado = "R";
        public const string Pendiente = "P";
    }

    public sealed class Solicitud
    {
        public const string Nuevo = " Nuevo ";
        public const string Remplazo = " Remplazo de ";
        public const string Ampliacion = " Ampliacion de ";
    }

    public sealed class Decision
    {
        public const string Aprobado = "A";
        public const string Rechazado = "R";
    }

    public sealed class Asunto 
    {
        public const string Solicitado = "solicitado";
        public const string Aprobacion = "Aprobación";
        public const string AprobacionFinal = "Aprobación final";
        public const string Rechazo = "Rechazo";
        public const string AprobacionPerfil = "Aprobación de perfil";
        public const string RechazoPerfil = "Rechazo de perfil";
    }

    public sealed class AccionMail
    {
        public const string  Solicitado = " solicitado un "; //1.
        public const string  Aprobacion = " Aprobado el ";//2.4
        public const string  Rechazo = " Rechazado el ";//3
        public const string  ElaboracionPerfil = " Elaborado el perfil para el ";//5
        public const string  AprobacionPerfil = " Aprobado el perfil del ";//6
        public const string  RechazoPerfil = " Rechazado el perfil del ";//6
    }

    public sealed class Responsable
    {
        public const string GerenteArea = "GA";
        public const string GerenteAdministrativoSede = "GAS";
        public const string GerenteGeneralAdjunto = "GGA";
        public const string JefeProcesos = "JP";
        public const string JefeArea = "JA";
        public const string EncargadoSeleccion = "ES";
        public const string AsistenteSeleccion = "AS";

    }

    public sealed class TipMenu
    {
        public const string Instranet = "I";
        public const string Extranet = "E";
    }
    



}
