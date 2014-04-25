namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class DiscapacidadPostulanteMap: ClassMap<DiscapacidadPostulante>
    {
        public DiscapacidadPostulanteMap()
        {
            Id(m => m.IdeDiscapacidadPostulante, "IDEDISCAPACIDADPOSTULANTE")
                .GeneratedBy
                .Sequence("IDEDISCAPACIDADPOSTULANTE_SQ");
            References(x => x.Postulante, "IDEPOSTULANTE");
            //Map(x => x.IdePostulante, "IDEPOSTULANTE");
            Map(x => x.TipoDiscapacidad, "TIPODISCAPACIDAD");
            Map(x => x.DescripcionDiscapacidad, "DESDISCAPACIDAD");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICACION");

            Map(x => x.DescripcionTipoDiscapacidad).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoDiscapacidad + " AND DG.VALOR = TIPODISCAPACIDAD)");

            Table("DISCAPACIDAD_POSTULANTE");
        }
    }
}



      