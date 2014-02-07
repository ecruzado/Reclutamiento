

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
                .NotEqual(0)
                .WithMessage("Ingresar el periodo de duracíon");

       
            
           
        }

    }
}
