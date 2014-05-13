

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class SubCategoriaValidator : AbstractValidator<SubCategoria>
    {
        public SubCategoriaValidator()
        {
            
            RuleFor(x => x.NOMSUBCATEGORIA)
                .NotEmpty()
                .WithMessage("Ingresar nombre");

            RuleFor(x => x.DESCSUBCATEGORIA)
                .NotEmpty()
                .WithMessage("Ingresar descripción");

            RuleFor(x => x.TIEMPO)
                .InclusiveBetween(1, 99999)
                .WithMessage("Ingresar el período de duración");

            
           
        }

    }
}
