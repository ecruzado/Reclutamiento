namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class NivelAcademicoMap : ClassMap<NivelAcademicoCargo>
    {
        public NivelAcademicoMap()
        {
            Id(m => m.IdeNivelAcademicoCargo, "NIVELACADEMICO_CARGO")
                .GeneratedBy
                .Sequence("IDEPARIENTESPOSTULANTE_SQ");
            References(x => x.Cargo, "IDECARGO");
            
            Map(x => x.TipoNivelEstudios, "TIPNIVESTUDIO");
            Map(x => x.TipoCarrera, "TIPCARRERA");
            Map(x => x.TipoNivelAlcanzado, "TIPNIVELCANZADO");
            Map(x => x.PuntajeNivelEstudio, "PUNTNIVESTUDIO");

            Table("NIVELACADEMICO_CARGO");
        }
    }
}