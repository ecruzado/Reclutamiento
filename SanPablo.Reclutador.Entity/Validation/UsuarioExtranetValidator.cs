

namespace SanPablo.Reclutador.Entity.Validation
{

    using FluentValidation;
    using System.Globalization;
    using System;

    
    public class UsuarioExtranetValidator : AbstractValidator<UsuarioExtranet>
    {
        public UsuarioExtranetValidator()
        {

            RuleFor(x => x.CodUsuario).NotEmpty().EmailAddress().WithMessage("Ingrese email valido");
            RuleFor(x => x.CodContrasena)
                  .NotEmpty()
                  .WithMessage("Ingresar contraseña");

        }
    }

}
