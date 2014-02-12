

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
                .ExclusiveBetween(0,20)
               .WithMessage("Ingresar un puntaje válido");

            RuleFor(x => x.NotaMinimaExamen)
                 .NotEmpty()
                .WithMessage("Ingresar una nota mínima");
            RuleFor(x => x.NotaMinimaExamen)
                .ExclusiveBetween(0, 20)
               .WithMessage("Ingresar una nota mínima válido");

        }

    }

}
