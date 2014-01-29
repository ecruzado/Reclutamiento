

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class ExamenValidator : AbstractValidator<Examen>
    {
        public ExamenValidator()
        {
            RuleFor(x => x.DescExamen)
               .NotEmpty()
               .WithMessage("Ingresar descripción");

            RuleFor(x => x.NomExamen)
                .NotEmpty()
                .WithMessage("Ingresar nombre");

            RuleFor(x => x.TipExamen)
                 .NotEqual("0")
                .WithMessage("Seleccione el tipo de examen");

        }

    }

}
