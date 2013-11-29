namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class DepartamentoMap : ClassMap<Departamento>
    {
        public DepartamentoMap()
        {
            Id(m => m.CodigoDepartamento, "IDEDEPARTAMENTO");
            References(x => x.Dependencia).Column("IDEDEPENDENCIA");
            Map(m => m.NombreDepartamento, "NOMBRE");
            Map(m => m.EstadoRegistro, "ESTREGISTRO");
            Table("DEPARTAMENTO");
        }
    }
}