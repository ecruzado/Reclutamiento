namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class SedeMap : ClassMap<Sede>
    {
        public SedeMap()
        {
            /*Id(m => m.SedeId);*/
            Id(m => m.CodigoSede, "COD_SEDE");
            Map(x => x.DescripcionSede, "DSC_SEDE");
            Table("T_HMSEDE");
        }
    }
}