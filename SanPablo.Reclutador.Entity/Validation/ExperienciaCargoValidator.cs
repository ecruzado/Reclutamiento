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
                .GreaterThan(-1)
                .WithMessage("Ingresar una cantidad mayor igual a cero");
            RuleFor(x => x.CantidadMesesExperiencia)
                .LessThan(13)
                .WithMessage("Cantidad máximo de experiencia en meses es 12");

            RuleFor(x => x.PuntajeExperiencia)
                .NotEmpty()
                .WithMessage("Ingresar Puntaje");

            RuleFor(x => x.PuntajeExperiencia)
                .GreaterThan(-1)
                .WithMessage("Ingresar un puntaje mayor igual a cero");
            RuleFor(x => x.PuntajeExperiencia)
                .LessThan(21)
                .WithMessage("Cantidad máximo de puntaje es 20");

         }

 
    }
}
