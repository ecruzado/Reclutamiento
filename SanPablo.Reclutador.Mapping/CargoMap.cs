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
            Map(x => x.NombreCargo, "NOMCARGO");
            Map(x => x.DescripcionCargo, "DESCARGO");
            Map(x => x.IdeArea, "IDEAREA");
            Map(x => x.NumeroPosiciones, "NUMPOSICION");
            Map(x => x.PuntajePostulanteInterno, "PUNTPOSTUINTE");
            Map(x => x.Sexo, "SEXO");
            Map(x => x.IndicadorSexo, "INDSEXO");
            Map(x => x.EdadInicio, "EDADINICIO");
            Map(x => x.EdadFin, "EDADFIN");
            Map(x => x.PuntajeEdad, "PUNTEDAD");
            Map(x => x.TipoRangoSalarial, "TIPRANGOSALARIO");
            Map(x => x.TipoMoneda, "TIPMONEDA");
            Map(x => x.PuntajeSalario, "PUNTSALARIO");
            Map(x => x.PuntajeSexo, "PUNTSEXO");
            Map(x => x.IndicadorSalario, "INDVERSALARIO");
            Map(x => x.ObjetivoCargo, "OBJETIVOSCARGO");
            Map(x => x.FuncionCargo, "FUNCIONESCARGO");
            Map(x => x.ObservacionCargo, "OBSERVACIONCARGO");

            Map(x => x.PuntajeTotalPostulanteInterno, "PUNTTOTPOSTUINTE");
            Map(x => x.PuntajeMinimoPostulanteInterno, "PUNTMINPOSTUINTE");
            Map(x => x.PuntajeTotalEdad, "PUNTTOTEDAD");
            Map(x => x.PuntajeMinimoEdad, "PUNTMINEDAD");
            Map(x => x.PuntajeTotalSexo, "PUNTTOTSEXO");

            Map(x => x.PuntajeTotalSalario, "PUNTTOTSEXO");
            Map(x => x.PuntajeMinimoSalario, "PUNTMINSALARIO");

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

            Map(x => x.PuntajeTotalNivelEstudio, "PUNTTOTNIVELEST");
            Map(x => x.PuntajeMinimoNivelEstudio, "PUNTMINNIVELEST");
            Map(x => x.PuntajeTotalCentroEstudio, "PUNTTOTCENTROEST");
            Map(x => x.PuntajeMinimoCentroEstudio, "PUNTMINCENTROEST");
            Map(x => x.PuntajeTotalExperiencia, "PUNTTOTEXPLABORAL");
            Map(x => x.PuntajeMinimoExperiencia, "PUNTMINEXPLABORAL");
            Map(x => x.PuntajeTotalFuncionesDesempeñandas, "PUNTTOTFUNDESE");
            Map(x => x.PuntajeMinimoFuncionesDesempeñandas, "PUNTMINFUNDESE");
            Map(x => x.PuntajeTotalConocimientoGeneral, "PUNTTOTCONOGEN");
            Map(x => x.PuntajeMinimoConocimientoGeneral, "PUNTMINCONOGEN");
            Map(x => x.PuntajeTotalDiscapacidad, "PUNTTOTDISCAPA");
            Map(x => x.PuntajeMinimoDiscapacidad, "PUNTMINDISCAPA");

            Map(x => x.PuntajeTotalHorario, "PUNTTOTHORARIO");
            Map(x => x.PuntajeMinimoHorario, "PUNTMINHORARIO");
            Map(x => x.PuntajeTotalUbigeo, "PUNTTOTUBIGEO");
            Map(x => x.PuntajeMinimoUbigeo, "PUNTMINUBIGEO");
            Map(x => x.PuntajeTotalExamen, "PUNTTOTEXAMEN");
            Map(x => x.PuntajeMinimoExamen, "PUNTMINEXAMEN");
            Map(x => x.CantidadPreseleccionados, "CANTPRESELEC");
            Map(x => x.EstadoActivo, "ESTACTIVO");

            Table("CARGO");
        }
    }
}