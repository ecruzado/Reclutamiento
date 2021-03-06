﻿namespace SanPablo.Reclutador.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class PostulanteMap : ClassMap<Postulante>
    {
        public PostulanteMap()
        {
            Id(m => m.IdePostulante, "IDEPOSTULANTE")
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
            Map(x => x.ReferenciaDireccion, "REFERENCIA");
                    
            Map(x => x.IdeUbigeo, "IDEUBIGEO");
            Map(x => x.TipoNacionalidad, "TIPNACIONALIDAD");

            HasMany(x => x.Estudios)
                    .Inverse()
                    .Cascade.All();
            HasMany(x => x.Experiencias)
                    .Inverse()
                    .Cascade.All();
            HasMany(x => x.Parientes)
                    .Inverse()
                    .Cascade.All();
            HasMany(x => x.Discapacidades)
                    .Inverse()
                    .Cascade.All();

            Map(x => x.TipoSalario, "TIPSALARIO");
            Map(x => x.TipoDisponibilidadTrabajo, "TIPDISPTRABAJO");
            Map(x => x.TipoDisponibilidadHorario, "TIPDISPHORARIO");
            Map(x => x.TipoHorario, "TIPHORARIO");
            Map(x => x.IndicadorReubicarseInterior, "INDREUBIINTEPAIS");
            Map(x => x.IndicadorParientesCHSP, "INDPARIENTECHSP");
            Map(x => x.TipoParienteSede, "TIPPARIENTESEDE");
            Map(x => x.ParienteNombre, "PARIENTENOMBRE");
            Map(x => x.ParienteCargo, "PARIENTECARGO");
            Map(x => x.TipoComoSeEntero, "TIPCOMOSEENTERO");
            Map(x => x.DescripcionOtroMedio, "DESOTROMEDIO");

            Map(x => x.FotoPostulante, "FOTOPOSTULANTE");

            Map(x => x.IndicadorRegistroCompleto, "INDTREGISTRO");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICACION");
            Map(x => x.FechaModificacion, "FECMODIFICACION");

            Table("POSTULANTE");
        }
    }
}