﻿namespace SanPablo.Reclutador.Web.App_Start
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

            builder.RegisterType<ConocimientoGeneralPostulanteValidator>()
                   .Keyed<IValidator>(typeof(IValidator<ConocimientoGeneralPostulante>))
                   .As<IValidator>();

            builder.RegisterType<ParientePostulanteValidator>()
                   .Keyed<IValidator>(typeof(IValidator<ParientePostulante>))
                   .As<IValidator>();

            builder.RegisterType<CriterioValidator>()
                   .Keyed<IValidator>(typeof(IValidator<Criterio>))
                   .As<IValidator>();

            builder.RegisterType<AlternativaValidator>()
                   .Keyed<IValidator>(typeof(IValidator<Alternativa>))
                   .As<IValidator>();

            builder.RegisterType<SubCategoriaValidator>()
                   .Keyed<IValidator>(typeof(IValidator<SubCategoria>))
                   .As<IValidator>();

            builder.RegisterType<ExamenValidator>()
                   .Keyed<IValidator>(typeof(IValidator<Examen>))
                   .As<IValidator>();

            builder.RegisterType<CategoriaValidator>()
                .Keyed<IValidator>(typeof(IValidator<Categoria>))
                .As<IValidator>();

            builder.RegisterType<DiscapacidadPostulanteValidator>()
                  .Keyed<IValidator>(typeof(IValidator<DiscapacidadPostulante>))
                  .As<IValidator>();

            builder.RegisterType<CompetenciaCargoValidator>()
                  .Keyed<IValidator>(typeof(IValidator<CompetenciaCargo>))
                  .As<IValidator>();

            builder.RegisterType<OfrecemosCargoValidator>()
                  .Keyed<IValidator>(typeof(IValidator<OfrecemosCargo>))
                  .As<IValidator>();

            builder.RegisterType<HorarioCargoValidator>()
                .Keyed<IValidator>(typeof(IValidator<HorarioCargo>))
                .As<IValidator>();

            builder.RegisterType<UbigeoCargoValidator>()
                .Keyed<IValidator>(typeof(IValidator<UbigeoCargo>))
                .As<IValidator>();

            builder.RegisterType<NivelAcademicoCargoValidator>()
               .Keyed<IValidator>(typeof(IValidator<NivelAcademicoCargo>))
               .As<IValidator>();

            builder.RegisterType<CentroEstudioCargoValidator>()
                .Keyed<IValidator>(typeof(IValidator<CentroEstudioCargo>))
                .As<IValidator>();

            builder.RegisterType<ExperienciaCargoValidator>()
                .Keyed<IValidator>(typeof(IValidator<ExperienciaCargo>))
                .As<IValidator>();

            builder.RegisterType<ConocimientoGeneralCargoValidator>()
                .Keyed<IValidator>(typeof(IValidator<ConocimientoGeneralCargo>))
                .As<IValidator>();

            builder.RegisterType<DiscapacidadCargoValidator>()
                .Keyed<IValidator>(typeof(IValidator<DiscapacidadCargo>))
                .As<IValidator>();

            builder.RegisterType<EvaluacionCargoValidator>()
                .Keyed<IValidator>(typeof(IValidator<EvaluacionCargo>))
                .As<IValidator>();
        }
    }
}