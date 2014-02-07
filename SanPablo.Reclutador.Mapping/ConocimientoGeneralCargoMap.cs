namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ConocimientoGeneralCargoMap : ClassMap<ConocimientoGeneralCargo>
    {
        public ConocimientoGeneralCargoMap()
        {
            Id(m => m.IdeConocimientoGeneralCargo, "IDECONOGENCARGO")
                .GeneratedBy
                .Sequence("IDECONOGENCARGO_SQ");
            References(x => x.Cargo, "IDECARGO");
            //Map(x => x.IdePostulante, "IDEPOSTULANTE");
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

            Table("CONOGENERAL_CARGO");
        }

    }
}

   