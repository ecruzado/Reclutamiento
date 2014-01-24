

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class CategoriaValidator : AbstractValidator<Categoria>
    {
        public CategoriaValidator()
        {

            RuleFor(x => x.DESCCATEGORIA)
                .NotEmpty()
                .WithMessage("Ingresar descripción");
            
            RuleFor(x => x.NOMCATEGORIA)
                .NotEmpty()
                .WithMessage("Ingresar nombre");

            RuleFor(x => x.TIPCATEGORIA)
                 .NotEqual("0")
                .WithMessage ("Seleccione el tipo de categoría");

            RuleFor(x => x.INSTRUCCIONES)
                  .NotEmpty()
                .WithMessage("Ingresar instrucciones");

            
           

            RuleFor(x => x.TEXTOEJEMPLO)
                  .NotEmpty()
                .WithMessage("Ingresar Texto de ejemplo");
            
            



        }
    }
}
