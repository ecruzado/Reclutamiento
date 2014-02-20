

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class PasswordValidator : AbstractValidator<Password>
    {
         
        public PasswordValidator()
        {

          RuleFor(x => x.PassAnterior)
                .NotEmpty()
                .WithMessage("Ingresar contraseña anterior");

          RuleFor(x => x.PassNuevo)
                .NotEmpty()
                .WithMessage("Ingresar nueva contraseña");

          RuleFor(x => x.PassConfirma)
                .NotEmpty()
                .WithMessage("Confirme la nueva contraseña")
                .Equal(x => x.PassNuevo)
                .WithMessage("la contraseña no coincide");


        }
    }
}
