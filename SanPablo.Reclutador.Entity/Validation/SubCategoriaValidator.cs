

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
                .WithMessage("Ingresar nombre subCateoría");

            RuleFor(x => x.DESCSUBCATEGORIA)
                .NotEmpty()
                .WithMessage("Ingresar descripción subCategoría");

            RuleFor(x => x.TIEMPO)
                .InclusiveBetween(1, 99999)
                .WithMessage("Ingresar el periodo de duracíon");

            
           
        }

    }
}
