namespace SanPablo.Reclutador.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class PersonaMap : ClassMap<Persona>
    {
        public PersonaMap()
        {
            Id(m => m.IdePersona, "IDEPOSTULANTE")
                .GeneratedBy
                .Sequence("IDEPOSTULANTE_SQ");
            Map(x => x.TipoDocumento, "TIPDOCUMENTO");
            Map(x => x.NumeroDocumento, "NUMDOCUMENTO");
            Map(x => x.ApellidoPaterno, "APEPATERNO");
            Map(x => x.ApellidoMaterno, "APEMATERNO");
            Map(x => x.PrimerNombre, "PRINOMBRE");
            Map(x => x.SegundoNombre, "SEGNOMBRE");
            Map(x => x.FechaNacimiento, "FECNACIMIENTO");
            Map(x => x.NumeroLicencia, "NUMLICENCIA");
            Map(x => x.IndicadorSexo, "INDSEXO");
            Map(x => x.TipoEstadoCivil, "TIPESTCIVIL");
            Map(x => x.Correo, "CORREO");
            Map(x => x.Observacion, "OBSERVACION");
            HasMany(x => x.Estudios)
                    .Inverse()
                    .Cascade.All();
            Table("POSTULANTE");
        }
    }
}