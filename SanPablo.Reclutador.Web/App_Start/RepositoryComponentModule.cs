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

            builder.RegisterType<PersonaRepository>()
                .As<IPersonaRepository>();

            builder.RegisterType<DetalleGeneralRepository>()
                .As<IDetalleGeneralRepository>();

            builder.RegisterType<EstudioPostulanteRepository>()
                .As<IEstudioPostulanteRepository>();

            builder.RegisterType<UbigeoRepository>()
                .As<IUbigeoRepository>();

            builder.RegisterType<CriterioRepository>()
                .As<ICriterioRepository>();

            builder.RegisterType<ExperienciaPostulanteRepository>()
                .As<IExperienciaPostulanteRepository>();

        }
    }
}