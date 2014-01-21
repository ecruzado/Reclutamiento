namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class ExperienciaPostulanteMap : ClassMap<ExperienciaPostulante>
    {
        public ExperienciaPostulanteMap()
        {
            Id(m => m.IdeExperienciaPostulante, "IDEEXPPOSTULANTE")
                .GeneratedBy
                .Sequence("IDEEXPERIENCIAPOSTULANTE_SQ");
            References(x => x.Postulante, "IDEPOSTULANTE");
            //Map(x => x.IdePostulante, "IDEPOSTULANTE");
            Map(x => x.NombreEmpresa, "NOMEMPRESA ");
            Map(x => x.TipoCargoTrabajo, "TIPCARGOTRABAJO");
            Map(x => x.NombreCargoTrabajo, "NOMCARGOTRABAJO");
            Map(x => x.FechaTrabajoInicio, "FECTRABINICIO");
            Map(x => x.FechaTrabajoFin, "FECTRABFIN");
            Map(x => x.IndicadorActualmenteTrabajo, "INDTRABACTUALMENTE");
            Map(x => x.TiempoDeServicio, "TIEMPOSERVICIO");
            Map(x => x.TipoMotivoCese, "TIPMOTIVOCESE");
            Map(x => x.NombreReferente, "NOMREFERENTE");
            Map(x => x.NumeroMovilReferencia, "NUMTELEFMOVILREF");
            Map(x => x.CorreoReferente, "CORREOREFERENTE");
            Map(x => x.TipoCargoTrabajoReferente, "TIPCARGOTRABAJOREF");
            Map(x => x.NumeroFijoInstitucionReferente, "NUMTELEFONOFIJOINST");
            Map(x => x.NumeroAnexoInstitucionReferente, "NUMANEXOINST");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Table("EXP_POSTULANTE");
        }

    }
}