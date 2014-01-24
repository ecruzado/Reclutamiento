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
        TipoParienteSede = 100,
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
        TipoCategoria = 1


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
}
