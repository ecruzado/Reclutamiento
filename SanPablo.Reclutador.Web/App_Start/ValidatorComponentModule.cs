namespace SanPablo.Reclutador.Web.App_Start
{
    using Autofac;
    using FluentValidation;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;

    public class ValidatorComponentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SedeValidator>()
                   .Keyed<IValidator>(typeof(IValidator<Sede>))
                   .As<IValidator>();

            builder.RegisterType<PersonaValidator>()
                   .Keyed<IValidator>(typeof(IValidator<Persona>))
                   .As<IValidator>();
        }
    }
}