
namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class RolValidator : AbstractValidator<Rol>
    {

        public RolValidator()
        {

            RuleFor(x => x.CodRol)
                .NotEmpty()
                .WithMessage("Ingresar nombre");
            
            RuleFor(x => x.DscRol)
                .NotEmpty()
                .WithMessage("Ingresar descripciòn");

            RuleFor(x => x.FlgSede)
                 .NotEqual("0")
                .WithMessage ("Seleccione si se requiere una sede");

         }

    }
}
