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

            builder.RegisterType<PostulanteValidator>()
                   .Keyed<IValidator>(typeof(IValidator<Postulante>))
                   .As<IValidator>();

            builder.RegisterType<EstudioPostulanteValidator>()
                .Keyed<IValidator>(typeof(IValidator<EstudioPostulante>))
                .As<IValidator>();

            builder.RegisterType<ExperienciaPostulanteValidator>()
                .Keyed<IValidator>(typeof(IValidator<ExperienciaPostulante>))
                .As<IValidator>();

            builder.RegisterType<CriterioValidator>()
                .Keyed<IValidator>(typeof(IValidator<Criterio>))
                .As<IValidator>();

            builder.RegisterType<AlternativaValidator>()
                .Keyed<IValidator>(typeof(IValidator<Alternativa>))
                .As<IValidator>();

            builder.RegisterType<CategoriaValidator>()
                .Keyed<IValidator>(typeof(IValidator<Categoria>))
                .As<IValidator>();


         

        }
    }
}