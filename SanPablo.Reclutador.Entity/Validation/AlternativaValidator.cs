

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class AlternativaValidator : AbstractValidator<Alternativa>
    {
        public AlternativaValidator()
        {
           
            RuleFor(x => x.NombreAlternativa)
                .NotEmpty()
                .WithMessage("Ingresar nombre alternativa");
            
            RuleFor(x => x.Peso)
                .NotEmpty()
                .WithMessage("Ingresar Peso");

            RuleFor(x => x.Peso)
                .NotEqual(0)
                .WithMessage ("Ingresar Peso mayor a cero");
           
        }

    }
}
