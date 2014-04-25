namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class EstudioPostulanteMap: ClassMap<EstudioPostulante>
    {
        public EstudioPostulanteMap()
        {
            Id(m => m.IdeEstudiosPostulante, "IDEESTUDIOSPOSTULANTE")
                .GeneratedBy
                .Sequence("IDEESTUDIOSPOSTULANTE_SQ");
            References(x => x.Postulante, "IDEPOSTULANTE");
            //Map(x => x.IdePostulante, "IDEPOSTULANTE");
            Map(x => x.TipTipoInstitucion, "TIPTIPOINSTITUCION");
            Map(x => x.TipoNombreInstitucion, "TIPNOMINSTITUCION");
            Map(x => x.NombreInstitucion, "NOMINSTITUCION");
            Map(x => x.TipoArea, "TIPAREA");
            Map(x => x.TipoEducacion, "TIPEDUCACION");
            Map(x => x.TipoNivelAlcanzado, "TIPNIVELALCANZADO");
            Map(x => x.FechaEstudioInicio, "FECESTUDIOINICIO");
            Map(x => x.FechaEstudioFin, "FECESTUDIOFIN");
            Map(x => x.IndicadorActualmenteEstudiando, "INDESTACTUALMENTE");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICACION");

            Map(x => x.DescripcionTipoInstitucion).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoInstitucion + " AND DG.VALOR = TIPTIPOINSTITUCION)");
            Map(x => x.DescripcionNombreInstitucion).Formula("(select CASE WHEN TIPNOMINSTITUCION = 'XX' THEN NOMINSTITUCION  ELSE DG.DESCRIPCION END FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoInstitucion + " AND DG.VALOR = TIPNOMINSTITUCION)");
            Map(x => x.DescripcionNivelAlcanzado).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoEducacion + " AND DG.VALOR = TIPNIVELALCANZADO)");
            Map(x => x.DescripcionEducacion).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoEducacion + " AND DG.VALOR = TIPEDUCACION)");
            Map(x => x.DescripcionArea).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoArea + " AND DG.VALOR = TIPAREA)");

            Table("ESTUDIOS_POSTULANTE");
        }
    }
}
