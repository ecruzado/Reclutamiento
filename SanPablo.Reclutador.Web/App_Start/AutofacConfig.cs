namespace SanPablo.Reclutador.Web.App_Start
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using FluentValidation.Mvc;
    using NHibernate;
    using SanPablo.Reclutador.Web.Controllers;
    using System.Reflection;
    using System.Web.Mvc;

    public class AutofacConfig
    {
        public static void Config()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetAssembly(typeof(HomeController)));

            builder.Register(x => new NHibernateConfigurator().GetSessionFactory())
                .SingleInstance();
            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession())
                .As<ISession>().InstancePerHttpRequest();

            builder.RegisterModule(new RepositoryComponentModule());
            builder.RegisterModule(new ValidatorComponentModule());

            builder.RegisterModule(new AutofacWebTypesModule());
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            // Set up the FluentValidation provider factory and add it as a Model validator
            var fluentValidationModelValidatorProvider = new FluentValidationModelValidatorProvider(new AutofacValidatorFactory(container));
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            fluentValidationModelValidatorProvider.AddImplicitRequiredValidator = false;
            ModelValidatorProviders.Providers.Add(fluentValidationModelValidatorProvider);
        }
    }
}