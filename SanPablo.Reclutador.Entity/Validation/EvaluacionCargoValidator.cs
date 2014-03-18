

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class EvaluacionCargoValidator : AbstractValidator<EvaluacionCargo>
    {
        public EvaluacionCargoValidator()
        {
            RuleFor(x => x.TipoExamen)
               .NotEmpty()
               .WithMessage("Seleccionar un Examen");

            RuleFor(x => x.TipoAreaResponsable)
                .NotEqual("00")
                .WithMessage("Seleccionar un area Responsable");

            RuleFor(x => x.PuntajeExamen)
                 .NotEmpty()
                .WithMessage("Ingresar un puntaje");

            RuleFor(x => x.PuntajeExamen)
                .InclusiveBetween(0, 10)
               .WithMessage("Ingresar un puntaje entre 0 y 10");

            RuleFor(x => x.NotaMinimaExamen)
                 .NotEmpty()
                .WithMessage("Ingresar una nota mínima");

            RuleFor(x => x.NotaMinimaExamen)
                .InclusiveBetween(0, 10)
               .WithMessage("Ingresar un puntaje entre 0 y 10");

        }

    }

}
