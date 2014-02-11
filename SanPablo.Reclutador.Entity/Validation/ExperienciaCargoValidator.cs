namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class ExperienciaCargoValidator : AbstractValidator<ExperienciaCargo>
    {
        public ExperienciaCargoValidator()
        {
            RuleFor(x => x.TipoExperiencia)
                .NotEqual("00")
                .WithMessage("Seleccionar una Experiencia");

            RuleFor(x => x.CantidadAnhosExperiencia)
                .NotEmpty()
                .WithMessage("Ingresar Años de Experiencia");
            RuleFor(x => x.CantidadAnhosExperiencia)
                .InclusiveBetween(0, 70)
                .WithMessage("Ingresar una cantidad válida");

            RuleFor(x => x.CantidadMesesExperiencia)
                .NotEmpty()
                .WithMessage("Ingresar Meses de Experiencia");
            RuleFor(x => x.CantidadMesesExperiencia)
                .InclusiveBetween(0, 12)
                .WithMessage("Ingresar una cantidad válida");

            RuleFor(x => x.PuntajeExperiencia)
                .NotEmpty()
                .WithMessage("Ingresar Puntaje");
            RuleFor(x => x.PuntajeExperiencia)
                .InclusiveBetween(0, 20)
                .WithMessage("Ingresar un puntaje válido");

         }

 
    }
}
