namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class AlternativaMap : ClassMap<Alternativa>
    {
        public AlternativaMap()
        {
            Id(m => m.CodigoAlternativa, "IDEALTERNATIVA");
            References(x => x.Criterio).Column("IDECRITERIO");
            Map(x => x.NombreAlternativa, "NOMBRE");
            Map(x => x.Peso, "PESO");
            Map(x => x.RutaDeImagen, "RUTAIMAGEN");
            Map(x => x.EstadoDeRegistro, "ESTREGISTRO");
            Table("ALTERNATIVA");
        }
    }
}