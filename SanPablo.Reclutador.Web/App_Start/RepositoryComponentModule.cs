namespace SanPablo.Reclutador.Web.App_Start
{
    using Autofac;
    using SanPablo.Reclutador.Repository;
    using SanPablo.Reclutador.Repository.Interface;

    public class RepositoryComponentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SedeRepository>()
                .As<ISedeRepository>();

            builder.RegisterType<PostulanteRepository>()
                .As<IPostulanteRepository>();

            builder.RegisterType<DetalleGeneralRepository>()
                .As<IDetalleGeneralRepository>();

            builder.RegisterType<EstudioPostulanteRepository>()
                .As<IEstudioPostulanteRepository>();

            builder.RegisterType<UbigeoRepository>()
                .As<IUbigeoRepository>();

            builder.RegisterType<CriterioRepository>()
                .As<ICriterioRepository>();

            builder.RegisterType<AlternativaRepository>()
                .As<IAlternativaRepository>();

            builder.RegisterType<CategoriaRepository>()
                .As<ICategoriaRepository>();

            builder.RegisterType<SubcategoriaRepository>()
                .As<ISubcategoriaRepository>();

            builder.RegisterType<CriterioPorSubcategoriaRepository>()
                .As<ICriterioPorSubcategoriaRepository>();

            builder.RegisterType<ExperienciaPostulanteRepository>()
                .As<IExperienciaPostulanteRepository>();

            builder.RegisterType<ParientePostulanteRepository>()
                .As<IParientePostulanteRepository>();

            builder.RegisterType<ConocimientoGeneralPostulanteRepository>()
                .As<IConocimientoGeneralPostulanteRepository>();

            builder.RegisterType<ParientePostulanteRepository>()
                .As<IParientePostulanteRepository>();

            builder.RegisterType<DiscapacidadPostulanteRepository>()
                .As<IDiscapacidadPostulanteRepository>();

            builder.RegisterType<ExamenRepository>()
               .As<IExamenRepository>();

            builder.RegisterType<ExamenPorCategoriaRepository>()
               .As<IExamenPorCategoriaRepository>();

            builder.RegisterType<CargoRepository>()
                .As<ICargoRepository>();

            builder.RegisterType<CompetenciasCargoRepository>()
                .As<ICompetenciaCargoRepository>();

            builder.RegisterType<NivelAcademicoCargoRepository>()
                .As<INivelAcademicoCargoRepository>();

            builder.RegisterType<CentroEstudioCargoRepository>()
                .As<ICentroEstudioCargoRepository>();

            builder.RegisterType<OfrecemosCargoRepository>()
                .As<IOfrecemosCargoRepository>();

            builder.RegisterType<HorarioCargoRepository>()
                .As<IHorarioCargoRepository>();

            builder.RegisterType<UbigeoCargoRepository>()
                .As<IUbigeoCargoRepository>();

            builder.RegisterType<ConocimientoGeneralCargoRepository>()
                .As<IConocimientoGeneralCargoRepository>();

            builder.RegisterType<ExperienciaCargoRepository>()
                .As<IExperienciaCargoRepository>();

            builder.RegisterType<DiscapacidadCargoRepository>()
                .As<IDiscapacidadCargoRepository>();

            builder.RegisterType<EvaluacionCargoRepository>()
                .As<IEvaluacionCargoRepository>();

            builder.RegisterType<RolRepository>()
               .As<IRolRepository>();

            builder.RegisterType<RolOpcionRepository>()
              .As<IRolOpcionRepository>();

            builder.RegisterType<DependenciaRepository>()
              .As<IDependenciaRepository>();

            builder.RegisterType<DepartamentoRepository>()
              .As<IDepartamentoRepository>();

            builder.RegisterType<AreaRepository>()
              .As<IAreaRepository>();

            builder.RegisterType<SolicitudNuevoCargoRepository>()
              .As<ISolicitudNuevoCargoRepository>();

            builder.RegisterType<LogSolicitudNuevoCargoRepository>()
             .As<ILogSolicitudNuevoCargoRepository>();


            builder.RegisterType<OpcionRepository>()
             .As<IOpcionRepository>();

            builder.RegisterType<UsuarioRepository>()
            .As<IUsuarioRepository>();

            builder.RegisterType<UsuarioRolSedeRepository>()
            .As<IUsuarioRolSedeRepository>();

            builder.RegisterType<UsuarioVistaRepository>()
            .As<IUsuarioVistaRepository>();

            builder.RegisterType<TipoRequerimientoRepository>()
           .As<ITipoRequerimiento>();



        }
    }
}