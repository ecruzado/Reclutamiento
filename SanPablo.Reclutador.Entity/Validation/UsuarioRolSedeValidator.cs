
namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class UsuarioRolSedeValidator : AbstractValidator<UsuarioRolSede>
    {

        public UsuarioRolSedeValidator()
        {
            RuleFor(x => x.IdRol)
                 .NotEqual(0)
                 .WithMessage("Seleccionar un usuario");
        }
    }
}
