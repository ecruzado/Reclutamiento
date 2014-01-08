namespace SanPablo.Reclutador.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class GeneralMap : ClassMap<General>
    {
        public GeneralMap()
        {
            Id(m => m.CodigoGeneral, "CODGENERAL");
            Map(x => x.NombreGeneral, "NOMBRE");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            Table("GENERAL");
        }
    }
}
