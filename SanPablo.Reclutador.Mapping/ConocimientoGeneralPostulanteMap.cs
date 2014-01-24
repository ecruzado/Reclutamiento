namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ConocimientoGeneralPostulanteMap : ClassMap<ConocimientoGeneralPostulante>
    {
        public ConocimientoGeneralPostulanteMap()
        {
            Id(m => m.IdeConocimientoGeneralPostulante, "IDECONOGENPOSTULANTE")
                .GeneratedBy
                .Sequence("IDECONOGENPOSTULANTE_SQ");
            References(x => x.Postulante, "IDEPOSTULANTE");
            //Map(x => x.IdePostulante, "IDEPOSTULANTE");
            Map(x => x.TipoConocimientoOfimatica, "TIPCONOFIMATICA ");
            Map(x => x.TipoNombreOfimatica, "TIPNOMOFIMATICA");
            Map(x => x.TipoIdioma, "TIPIDIOMA");
            Map(x => x.TipoConocimientoIdioma, "TIPCONOCIDIOMA");
            Map(x => x.TipoConocimientoGeneral, "TIPCONOCGENERALES");
            Map(x => x.TipoNombreConocimientoGeneral, "TIPNOMCONOCGRALES");
            Map(x => x.NombreConocimientoGeneral, "NOMCONOCGRALES");
            Map(x => x.TipoNivelConocimiento, "TIPNIVELCONOCIMIENTO");
            Map(x => x.IndicadorCertificacion, "INDCERTIFICACION");
            Map(x => x.EstadoActivo, "ESTACTIVO");
            Table("CONOGEN_POSTULANTE");
        }

    }
}

   