namespace SanPablo.Reclutador.Entity
{
    public enum TipoTabla:int
    {
        TipoVia = 1,
        TipoZona = 2,
        EstadoCivil = 3,
        Sexo = 4,
        TipoDocumento = 5,
        Nacionalidad = 6,
        DisponibilidadTrabajo = 8,
        DisponibilidadHorario = 9,
        TipoHorario = 10,
        TipoParienteSede = 100,
        TipoSalario = 11,
        TipoInstitucion = 12,
        TipoNombreInstitucion = 13,
        TipoArea = 14,
        TipoEducacion = 15,
        NivelAlcanzado = 16,
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
