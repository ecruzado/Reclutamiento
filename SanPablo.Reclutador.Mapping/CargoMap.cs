namespace SanPablo.Reclutador.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;

    public class CargoMap : ClassMap<Cargo>
    {
        public CargoMap()
        {
            Id(m => m.IdeCargo, "IDECARGO")
                .GeneratedBy
                .Sequence("IDECARGO_SQ");
            Map(x => x.IdeSede, "IDESEDE");
            Map(x => x.NombreCargo, "NOMCARGO");
            Map(x => x.DescripcionCargo, "DESCARGO");
            Map(x => x.CodigoCargo, "CODCARGO");
            Map(x => x.IdeDependencia, "IDEDEPENDENCIA");
            Map(x => x.IdeDepartamento, "IDEDEPARTAMENTO");
            Map(x => x.IdeArea, "IDEAREA");
            Map(x => x.NumeroPosiciones, "NUMPOSICION");
            Map(x => x.Sexo, "SEXO");
            Map(x => x.IndicadorSexo, "INDSEXO");
            Map(x => x.EdadInicio, "EDADINICIO");
            Map(x => x.EdadFin, "EDADFIN");
            Map(x => x.IndicadorEdad, "INDEDAD");
            Map(x => x.PuntajeEdad, "PUNTEDAD");
            Map(x => x.PuntajeSexo, "PUNTSEXO");
            Map(x => x.TipoRangoSalarial, "TIPRANGOSALARIO");
            Map(x => x.IndicadorSalario, "INDSALARIO");
            Map(x => x.PuntajeSalario, "PUNTSALARIO");
            Map(x => x.TipoRequerimiento, "TIPREQUERIMIENTO");

            HasMany(x => x.Competencias)
                   .Inverse()
                   .Cascade.All();
            HasMany(x => x.Ofrecimientos)
                    .Inverse()
                    .Cascade.All();
            HasMany(x => x.Horarios)
                    .Inverse()
                    .Cascade.All();
            HasMany(x => x.Ubigeos)
                    .Inverse()
                    .Cascade.All();
            HasMany(x => x.CentrosEstudios)
                    .Inverse()
                    .Cascade.All();
            HasMany(x => x.NivelesAcademicos)
                    .Inverse()
                    .Cascade.All();
            HasMany(x => x.Experiencias)
                    .Inverse()
                    .Cascade.All();
            HasMany(x => x.Evaluaciones)
                    .Inverse()
                    .Cascade.All();
            HasMany(x => x.Conocimientos)
                    .Inverse()
                    .Cascade.All();

            Map(x => x.ObjetivoCargo, "OBJETIVOSCARGO");
            Map(x => x.FuncionCargo, "FUNCIONESCARGO");
            Map(x => x.ObservacionCargo, "OBSERVACIONCARGO");

            Map(x => x.PuntajeTotalPostulanteInterno, "PUNTTOTPOSTUINTE");
            Map(x => x.PuntajeTotalEdad, "PUNTTOTEDAD");
            Map(x => x.PuntajeTotalSexo, "PUNTTOTSEXO");
            Map(x => x.PuntajeTotalSalario, "PUNTTOTSALARIO");

           
            
            Map(x => x.PuntajeTotalNivelEstudio, "PUNTTOTNIVELEST");
            Map(x => x.PuntajeTotalCentroEstudio, "PUNTTOTCENTROEST");
            Map(x => x.PuntajeTotalExperiencia, "PUNTTOTEXPLABORAL");

            Map(x => x.PuntajeTotalOfimatica, "PUNTTOTOFIMATI");
            Map(x => x.PuntajeTotalIdioma, "PUNTTOTIDIOMA");
            Map(x => x.PuntajeTotalConocimientoGeneral, "PUNTTOTCONOGEN");
            Map(x => x.PuntajeTotalDiscapacidad, "PUNTTOTDISCAPA");

            Map(x => x.PuntajeTotalHorario, "PUNTTOTHORARIO");
            Map(x => x.PuntajeTotalUbigeo, "PUNTTOTUBIGEO");
            Map(x => x.PuntajeTotalExamen, "PUNTTOTEXAMEN");
            Map(x => x.PuntajeMinimoExamen, "PUNTMINEXAMEN");
            Map(x => x.CantidadPreseleccionados, "CANTPRESELEC");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Map(x => x.UsuarioCreacion, "USRCREACION");
            Map(x => x.FechaCreacion, "FECCREACION");
            Map(x => x.UsuarioModificacion, "USRMODIFICA");
            Map(x => x.FechaModificacion, "FECMODIFICA");
            Map(x => x.PuntajeMinimoGeneral, "PUNTMINGRAL");

            Table("CARGO");
        }
    }
}