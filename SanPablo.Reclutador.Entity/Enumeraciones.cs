namespace SanPablo.Reclutador.Entity
{
    public enum TipoTabla:int
    {
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
        TipoNombreUnivesidad = 18,
        TipoNombreInstituto = 22,
        TipoNombreColegio = 23,
        TipoArea = 19,
        TipoEducacion = 20,
        NivelAlcanzado = 21,
        TipoCargo = 17,
        TipoMotivoCese = 18,
        TipoCargoReferente = 19,
        TipoCriterio = 1,
        Medicion = 2,
        EstadoMant = 3,
        Modo=4,
        TipoCalificacion = 5,
        TipoCategoria = 26
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


}
