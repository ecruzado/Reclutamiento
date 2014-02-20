

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class UsuarioValidator : AbstractValidator<Usuario>
    {

        public UsuarioValidator()
        {
            RuleFor(x => x.DscApeMaterno)
                 .NotEmpty()
                 .WithMessage("Ingresar apellido paterno");
            RuleFor(x => x.DscApeMaterno)
                .Length(1, 25)
                .WithMessage("Ingresar apellido paterno con menos de 25 caracteres");

            RuleFor(x => x.DscApePaterno)
                .NotEmpty()
                .WithMessage("Ingresar apellido materno");
            RuleFor(x => x.DscApePaterno)
                .Length(1, 25)
                .WithMessage("Ingresar apellido materno con menos de 25 caracteres");

            RuleFor(x => x.CodUsuario)
                 .NotEmpty()
                 .WithMessage("Ingresar código de usuario");
            
            RuleFor(x => x.CodContrasena)
                 .NotEmpty()
                 .WithMessage("Ingresar contraseña");
            RuleFor(x => x.CodContrasena)
                .Length(1, 10)
                .WithMessage("Ingresar contraseña con menos de 10 caracteres");

            RuleFor(x => x.DscNombres)
                 .NotEmpty()
                 .WithMessage("Ingresar nombre de usuario");
            RuleFor(x => x.DscNombres)
                .Length(1, 25)
                .WithMessage("Ingresar nombre con menos de 25 caracteres");

          
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Ingrese email valido");

            RuleFor(x => x.Telefono)
                 .NotEmpty()
                 .WithMessage("Ingresar numero de telefono");

           
        }
    }
}
