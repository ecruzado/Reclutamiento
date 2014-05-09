

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

            RuleFor(x => x.IdeExamen)
                .NotEqual(0)
                .WithMessage("Seleccionar un Examen");

            RuleFor(x => x.NotaMinimaExamen)
                 .NotEmpty()
                .WithMessage("Ingresar una nota mínima");
            RuleFor(x => x.NotaMinimaExamen)
                .InclusiveBetween(0, 20)
               .WithMessage("Ingresar un nota mínima entre 0 y 20");

        }

    }

}
