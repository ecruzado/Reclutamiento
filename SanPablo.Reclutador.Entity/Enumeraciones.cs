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
        TipoCriterio = 20,
        Medicion = 21,
        EstadoCriterio = 22,
        Modo=23,
        TipoCalificacion = 24
    }

    public sealed class IndicadorActivo
    {
        public const string Activo = "A";
        public const string Inactivo = "I";
    }

}
