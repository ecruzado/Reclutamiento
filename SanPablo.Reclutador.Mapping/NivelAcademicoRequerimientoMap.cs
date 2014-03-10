
namespace SanPablo.Reclutador.Mapping
{
    
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class NivelAcademicoRequerimientoMap : ClassMap<NivelAcademicoRequerimiento>
    {
        public NivelAcademicoRequerimientoMap()
        {
            Id(m => m.IdeNivelAcademicoRequerimiento, "IDENIVELACADESOLREQ")
                .GeneratedBy
                .Sequence("IDENIVELACADESOLREQ_SQ");
            References(x => x.SolicitudRequerimiento, "IDESOLREQPERSONAL");

            Map(x => x.TipoEducacion, "TIPEDUCACION");
            Map(x => x.TipoAreaEstudio, "TIPAREAESTUDIO");
            Map(x => x.TipoNivelAlcanzado, "TIPNIVELCANZADO");
            Map(x => x.CicloSemestre, "CICLOSEMESTRE");
            Map(x => x.PuntajeNivelEstudio, "PUNTNIVESTUDIO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Map(x => x.DescripcionTipoEducacion).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoEducacion + " AND DG.VALOR = TIPEDUCACION AND DG.ESTACTIVO = 'A' )");
            Map(x => x.DescripcionAreaEstudio).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoArea + " AND DG.VALOR = TIPAREAESTUDIO AND DG.ESTACTIVO = 'A' )");
            Map(x => x.DescripcionNivelAlcanzado).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.NivelAlcanzado + " AND DG.VALOR = TIPNIVELCANZADO AND DG.ESTACTIVO = 'A' )");

            Table("NIVELACADEMICO_SOLREQ");
        }
    }


}
