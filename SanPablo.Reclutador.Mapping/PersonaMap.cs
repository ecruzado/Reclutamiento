namespace SanPablo.Reclutador.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class PersonaMap : ClassMap<Persona>
    {
        public PersonaMap()
        {
            Id(m => m.IdePersona, "IDEPERSONA")
                .GeneratedBy
                .Sequence("IDEPERSONA_SQ");
            Map(x => x.TipoDocumento, "TIPDOCUMEN");
            Map(x => x.NumeroDocumento, "NUMDOCUMENTO");
            Map(x => x.ApellidoPaterno, "APEPATERNO");
            Map(x => x.ApellidoMaterno, "APEMATERNO");
            Map(x => x.PrimerNombre, "PRINOMBRE");
            Map(x => x.SegundoNombre, "SEGNOMBRE");
            Map(x => x.FechaNacimiento, "FECNACIMIEN");
            Map(x => x.NumeroLicencia, "NUMLICENCIA");
            Map(x => x.IndicadorSexo, "INDSEXO");
            Map(x => x.TipoEstadoCivil, "TIPESTCIVIL");
            Map(x => x.Observacion, "OBSERVACION");
            Table("PERSONA");
        }
    }
}