

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
            
           
           
           
        }

    }
}
