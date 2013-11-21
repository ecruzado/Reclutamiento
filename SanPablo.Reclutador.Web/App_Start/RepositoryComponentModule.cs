namespace SanPablo.Reclutador.Web.App_Start
{
    using Autofac;
    using SanPablo.Reclutador.Web.Repository;
    using SanPablo.Reclutador.Web.Repository.Interface;

    public class RepositoryComponentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SedeRepository>()
                .As<ISedeRepository>();
        }
    }
}