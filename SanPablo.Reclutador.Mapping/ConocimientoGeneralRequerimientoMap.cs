using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPablo.Reclutador.Mapping
{
    //class ConocimientoGeneralRequerimientoMap
    //{
    //}

    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ConocimientoGeneralRequerimientoMap : ClassMap<ConocimientoGeneralRequerimiento>
    {
        public ConocimientoGeneralRequerimientoMap()
        {
            Id(m => m.IdeConocimientoGeneralRequerimiento, "IDECONOGENSOLREQ")
                .GeneratedBy
                .Sequence("IDECONOGENSOLREQ_SQ");
            References(x => x.SolicitudRequerimiento, "IDESOLREQPERSONAL");
            Map(x => x.TipoConocimientoOfimatica, "TIPCONOFIMATICA ");
            Map(x => x.TipoNombreOfimatica, "TIPNOMOFIMATICA");
            Map(x => x.TipoIdioma, "TIPIDIOMA");
            Map(x => x.TipoConocimientoIdioma, "TIPCONOCIDIOMA");
            Map(x => x.TipoConocimientoGeneral, "TIPCONOGENERAL");
            Map(x => x.TipoNombreConocimientoGeneral, "TIPNOMCONOCGRALES");
            Map(x => x.NombreConocimientoGeneral, "NOMCONOCGRALES");
            Map(x => x.TipoNivelConocimiento, "TIPNIVELCONOCIMIENTO");
            Map(x => x.IndicadorCertificacion, "INDCERTIFICACION");
            Map(x => x.PuntajeConocimiento, "PUNTCONOCIMIENTO");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");

            Map(x => x.DescripcionConocimientoIdioma).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoConocimientoIdioma + " AND DG.VALOR = TIPCONOCIDIOMA)");
            Map(x => x.DescripcionConocimientoOfimatica).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoConocimientoOfimatica + " AND DG.VALOR = TIPCONOFIMATICA)");
            Map(x => x.DescripcionIdioma).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoIdioma + " AND DG.VALOR = TIPIDIOMA)");
            Map(x => x.DescripcionNivelConocimiento).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoNivelConocimiento + " AND DG.VALOR = TIPNIVELCONOCIMIENTO)");
            Map(x => x.DescripcionNombreConocimientoGeneral).Formula("(select DG.DESCRIPCION END FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoConocimientoGral + " AND DG.VALOR = TIPNOMCONOCGRALES)");
            Map(x => x.DescripcionNombreOfimatica).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TiponombreOfimatica + " AND DG.VALOR = TIPNOMOFIMATICA)");
            Map(x => x.DescripcionConocimientoGeneral).Formula("(select DG.DESCRIPCION FROM DETALLE_GENERAL DG where DG.IDEGENERAL = " + (int)TipoTabla.TipoConocimientoGral + " AND DG.VALOR = TIPCONOGENERAL)");


            Table("CONOGENERAL_SOLREQ");
        }

    }

}
