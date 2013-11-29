namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class CriterioMap : ClassMap<Criterio>
    {
        public CriterioMap()
        {
            Id(m => m.CodigoCriterio, "IDECRITERIO");
            Map(x => x.NombreCriterio, "NOMBRE");
            Map(x => x.DescripcionCriterio, "DESCRIPCION");
            Map(x => x.Calificacion, "CALIFICACION");
            Map(x => x.TipoMedicion, "TIPMEDICION");
            Map(x => x.TipoCriterio, "TIPCRITE");
            Map(x => x.TipoModoRegistro, "TIPMODOREG");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            Table("CRITERIO");
        }
    }
}