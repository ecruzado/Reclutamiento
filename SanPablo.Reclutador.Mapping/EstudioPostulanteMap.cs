namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class EstudioPostulanteMap: ClassMap<EstudioPostulante>
    {
        public EstudioPostulanteMap()
        {
            Id(m => m.IdeEstudiosPostulante, "IDEESTUDIOSPOSTULANTE")
                .GeneratedBy
                .Sequence("IDEESTUDIOSPOSTULANTE_SQ");
            References(x => x.Postulante);
            Map(x => x.IdePostulante, "IDEPOSTULANTE");
            Map(x => x.TipTipoInstitucion, "TIPTIPOINSTITUCION");
            Map(x => x.TipoNombreInstitucion, "TIPNOMINSTITUCION");
            Map(x => x.NombreInstitucion, "NOMINSTITUCION");
            Map(x => x.TipoArea, "TIPAREA");
            Map(x => x.TipoEducacion, "TIPEDUCACION");
            Map(x => x.TipoNivelAlcanzado, "TIPNIVELALCANZADO");
            Map(x => x.FechaEstudioInicio, "FECESTUDIOINICIO");
            Map(x => x.FechaEstudioFin, "FECESTUDIOFIN");
            Map(x => x.IndicadorActualmenteEstudiando, "INDESTACTUALMENTE");
            Table("ESTUDIOS_POSTULANTE");
        }
    }
}
