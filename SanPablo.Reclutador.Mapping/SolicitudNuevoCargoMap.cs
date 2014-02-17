namespace SanPablo.Reclutador.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class SolicitudNuevoCargoMap : ClassMap<SolicitudNuevoCargo>
    {
        public SolicitudNuevoCargoMap()
        {
            Id(m => m.IdeSolicitudNuevoCargo, "IDESOLNUEVOCARGO")
                .GeneratedBy
                .Sequence("IDESOLNUEVOCARGO_SQ");
            Map(x => x.IdeSede, "IDESEDE");
            Map(x => x.CodigoCargo, "CODCARGO");
            Map(x => x.NombreCargo, "NOMBRE");
            Map(x => x.DescripcionCargo, "DESCRIPCION");
            Map(x => x.NumeroPosiciones, "NUMPOSICIONES");
            Map(x => x.TipoRangoSalarial, "TIPRANSALARIO");
            Map(x => x.IdeArea, "IDEAREA");
            
            Map(x => x.IndicadorVerSexo, "INDVERSEXO");
            Map(x => x.IndicadorVerSalario, "INDVERSALARIO");
            Map(x => x.DescripcionEstudios, "ESTUDIOS");
            Map(x => x.DescripcionFunciones, "FUNCIONES");
            Map(x => x.DescripcionCompetencias, "COMPETENCIAS");
            Map(x => x.DescripcionObservaciones, "OBSERVACIONES");
            Map(x => x.FechaPublicacion, "FECPUBLICACION");
            Map(x => x.FechaExpiracion, "FECEXPIRACION");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");


            Table("SOLNUEVO_CARGO");
        }
    }
}