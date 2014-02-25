

namespace SanPablo.Reclutador.Entity.Validation
{

    using FluentValidation;
    using System.Globalization;
    using System;

    
    public class UsuarioExtranetValidator : AbstractValidator<UsuarioExtranet>
    {
        public UsuarioExtranetValidator()
        {

            RuleFor(x => x.Usuario).NotEmpty().EmailAddress().WithMessage("Ingrese email valido");
            RuleFor(x => x.Password)
                  .NotEmpty()
                  .WithMessage("Ingresar contraseña");



            RuleFor(x => x.PasswordConfirma)
                .NotEmpty()
                .WithMessage("Confirme la nueva contraseña")
                .Equal(x => x.Password)
                .WithMessage("la contraseña no coincide");

        }
    }

}
