namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class DepartamentoMap : ClassMap<Departamento>
    {
        public DepartamentoMap()
        {
            Id(m => m.IdeDepartamento, "IDEDEPARTAMENTO");
            References(x => x.Dependencia).Column("IDEDEPENDENCIA");
            Map(x => x.NombreDepartamento, "NOMDEPARTAMENTO");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Table("DEPARTAMENTO");
        }
    }
}