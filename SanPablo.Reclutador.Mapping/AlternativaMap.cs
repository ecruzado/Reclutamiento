namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class AlternativaMap : ClassMap<Alternativa>
    {
        public AlternativaMap()
        {
            Id(m => m.IdeAlternativa, "IDEALTERNATIVA")
                .GeneratedBy
                .Sequence("IDEALTERNATIVA_SQ");
           References(x => x.Criterio, "IDECRITERIO");
           Map(x => x.NombreAlternativa, "ALTERNATIVA");
           Map(x => x.Peso, "PESO");
           Map(x => x.RutaDeImagen, "RUTAIMAGEN");
           Map(x => x.UsuarioCreacion, "USRCREACION");
           Map(x => x.FechaCreacion, "FECCREACION");
           Map(x => x.UsuarioModificacion, "USRMODIFICACION");
           Map(x => x.FechaModificacion, "FECMODIFICACION");
           Map(x => x.Image, "IMAGE");
           Table("ALTERNATIVA");
        }
    }
}