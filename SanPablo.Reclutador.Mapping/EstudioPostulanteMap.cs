namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class EstudioPostulanteMap: ClassMap<EstudioPostulante>
    {
        public EstudioPostulanteMap()
        {
            Id(m => m.ideEstudiosPostulante, "IDEESTUDIOSPOSTULANTE")
                .GeneratedBy
                .Sequence("IDEESTUDIOSPOSTULANTE_SQ");
            References(x => x.postulante);
            Map(x => x.tipTipoInstitucion, "TIPTIPOINSTITUCION");
            Map(x => x.tipNombreInstitucion, "TIPNOMINSTITUCION");
            Map(x => x.nombreInstitucion, "NOMINSTITUCION");
            Map(x => x.tipoArea, "TIPAREA");
            Map(x => x.tipoEducacion, "TIPEDUCACION");
            Map(x => x.tipoNivelAlcanzado, "TIPNIVELALCANZADO");
            Map(x => x.fechaEstudioInicio, "FECESTUDIOINICIO");
            Map(x => x.fechaEstudioFin, "FECESTUDIOFIN");
            Map(x => x.indicadorActualmenteEstudiando, "INDESTACTUALMENTE");
            Table("ESTUDIOS_POSTULANTE");
        }
    }
}
