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
        }
    }
}