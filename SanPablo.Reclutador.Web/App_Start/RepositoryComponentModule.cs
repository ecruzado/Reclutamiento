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
        }
    }
}