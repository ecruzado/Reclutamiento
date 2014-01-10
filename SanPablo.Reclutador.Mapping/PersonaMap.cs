﻿namespace SanPablo.Reclutador.Repository.Mapping
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
            Map(x => x.TipoVia, "TIPVIA");
            Map(x => x.NombreVia, "NOMVIA");
            Map(x => x.NumeroDireccion, "NUMDIRECCION");
            Map(x => x.InteriorDireccion, "INTERIOR");
            Map(x => x.Manzana, "MANZANA");
            Map(x => x.Lote, "LOTE");
            Map(x => x.Bloque, "BLOQUE");
            Map(x => x.Etapa, "ETAPA");
            Map(x => x.Correo, "CORREO");
            Map(x => x.Observacion, "OBSERVACION");

            Map(x => x.TelefonoMovil, "TELMOVIL");
            Map(x => x.TelefonoFijo, "TELFIJO");
            Map(x => x.TipoZona, "TIPZONA");
            Map(x => x.NombreZona, "NOMZONA");
                    
            Map(x => x.IdeUbigeo, "IDEUBIGEO");
            HasMany(x => x.Estudios)
                    .Inverse()
                    .Cascade.All();
            Table("POSTULANTE");
        }
    }
}