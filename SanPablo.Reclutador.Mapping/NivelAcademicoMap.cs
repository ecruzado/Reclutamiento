namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class NivelAcademicoMap : ClassMap<NivelAcademicoCargo>
    {
        public NivelAcademicoMap()
        {
            Id(m => m.IdeNivelAcademicoCargo, "IDENIVELACADECARGO")
                .GeneratedBy
                .Sequence("IDENIVELACADECARGO_SQ");
            References(x => x.Cargo, "IDECARGO");

            Map(x => x.TipoEducacion, "TIPEDUCACION");
            Map(x => x.TipoAreaEstudio, "TIPAREAESTUDIO");
            Map(x => x.TipoNivelAlcanzado, "TIPNIVELCANZADO");
            Map(x => x.PuntajeNivelEstudio, "PUNTNIVESTUDIO");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Table("NIVELACADEMICO_CARGO");
        }
    }
}