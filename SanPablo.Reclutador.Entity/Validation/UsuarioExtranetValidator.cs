

namespace SanPablo.Reclutador.Entity.Validation
{

    using FluentValidation;
    using System.Globalization;
    using System;

    
    public class UsuarioExtranetValidator : AbstractValidator<UsuarioExtranet>
    {
        public UsuarioExtranetValidator()
        {

            RuleFor(x => x.Usuario).NotEmpty().EmailAddress().WithMessage("Ingrese email válido.");
            RuleFor(x => x.Password)
                  .NotEmpty()
                  .WithMessage("Ingresar contraseña.")
                 .Length(7, 20)
                 .WithMessage("La contraseña es de 7 a 20 caracteres");



            RuleFor(x => x.PasswordConfirma)
                .NotEmpty()
                .WithMessage("Confirme la nueva contraseña.")
                .Length(7, 20)
                .WithMessage("La contraseña es de 7 a 20 caracteres")
                .Equal(x => x.Password)
                .WithMessage("La contraseña no coincide.");
                 
        }
    }

}
