namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ParientesPostulanteMap : ClassMap<ParientePostulante>
    {
        public ParientesPostulanteMap()
        {
            Id(m => m.IdeParientePostulante, "IDEPARIENTESPOSTULANTE")
                .GeneratedBy
                .Sequence("IDEPARIENTESPOSTULANTE_SQ");
            References(x => x.Postulante, "IDEPOSTULANTE");
            //Map(x => x.IdePostulante, "IDEPOSTULANTE");
            Map(x => x.ApellidoPaterno, "APEPATERNO");
            Map(x => x.ApellidoMaterno, "APEMATERNO");
            Map(x => x.Nombres, "NOMBRES");
            Map(x => x.TipoDeVinculo, "TIPVINCULO");
            Map(x => x.FechaNacimiento, "FECNACIMIENTO");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Map(x => x.DescripcionVinculo).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoVinculo + " AND DG.VALOR = TIPVINCULO)");

            Table("PARIENTES_POSTULANTE");
        }
    }
}