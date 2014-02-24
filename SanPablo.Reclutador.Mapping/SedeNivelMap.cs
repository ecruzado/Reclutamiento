

namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;
    
    
    public class SedeNivelMap : ClassMap<SedeNivel>
    {
        public SedeNivelMap()
        {
            Id(x => x.IDUSUARIONIVEL, "IDUSUARIONIVEL")
                .GeneratedBy
                .Sequence("USUARIO_NIVEL_SQ");
            Map(x => x.IDUSUARIO, "IDUSUARIO");
            Map(x => x.IDESEDE, "IDESEDE");
            Map(x => x.IDEDEPARTAMENTO, "IDEDEPARTAMENTO");
            Map(x => x.IDEDEPENDENCIA, "IDEDEPENDENCIA");
            Map(x => x.IDEAREA, "IDEAREA");
            Map(x => x.FLGESTADO, "FLGESTADO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");
            Map(x => x.SEDEDES).Formula("(select S.DESCRIPCION from SEDE S WHERE S.IDESEDE=IDESEDE AND S.ESTREGISTRO='A')");
            Map(x => x.AREADES).Formula("(SELECT A.NOMAREA FROM AREA A WHERE A.IDEDEPARTAMENTO=IDEDEPARTAMENTO AND A.IDEAREA=IDEAREA)");
            Map(x => x.DEPENDENCIADES).Formula("( SELECT D.NOMDEPENDENCIA FROM DEPENDENCIA D WHERE D.IDEDEPENDENCIA = IDEDEPENDENCIA " +
                                                " AND D.IDESEDE= IDESEDE " +
                                                " AND D.ESTACTIVO = 'A')");
            Map(x => x.DEPARTAMENTODES).Formula("(SELECT T.NOMDEPARTAMENTO FROM DEPARTAMENTO T WHERE T.ESTACTIVO='A' AND T.IDEDEPENDENCIA =IDEDEPENDENCIA " +
                                                " AND T.IDEDEPARTAMENTO= IDEDEPARTAMENTO )");
            Table("USUARIO_NIVEL");


        }
    }

}
