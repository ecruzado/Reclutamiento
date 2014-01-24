namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class DiscapacidadPostulanteMap: ClassMap<DiscapacidadPostulante>
    {
        public DiscapacidadPostulanteMap()
        {
            Id(m => m.IdeDiscapacidadPostulante, "IDEDISCAPACIDADPOSTULANTE")
                .GeneratedBy
                .Sequence("IDEDISCAPACIDADPOSTULANTE_SQ");
            References(x => x.Postulante, "IDEPOSTULANTE");
            //Map(x => x.IdePostulante, "IDEPOSTULANTE");
            Map(x => x.TipoDiscapacidad, "TIPODISCAPACIDAD");
            Map(x => x.DescripcionDiscapacidad, "DESDISCAPACIDAD");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Table("DISCAPACIDAD_POSTULANTE");
        }
    }
}



      