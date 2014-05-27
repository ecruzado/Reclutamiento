

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
                 .WithMessage("Ingresar el apellido paterno");
            RuleFor(x => x.DscApeMaterno)
                .Length(1, 25)
                .WithMessage("Máx. 25 caracteres");

            RuleFor(x => x.DscApePaterno)
                .NotEmpty()
                .WithMessage("Ingresar el apellido materno");
            RuleFor(x => x.DscApePaterno)
                .Length(1, 25)
                .WithMessage("Máx. 25 caracteres");

            RuleFor(x => x.CodUsuario)
                 .NotEmpty()
                 .WithMessage("Ingresar usuario");
            
            RuleFor(x => x.CodContrasena)
                 .NotEmpty()
                 .WithMessage("Ingresar contraseña");
            RuleFor(x => x.CodContrasena)
                .Length(1, 10)
                .WithMessage("Máx. 10 caracteres");

            RuleFor(x => x.DscNombres)
                 .NotEmpty()
                 .WithMessage("Ingresar el nombre");
            RuleFor(x => x.DscNombres)
                .Length(1, 25)
                .WithMessage("Máx. 25 caracteres");

          
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Ingresar el email");
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Ingrese email válido");

            RuleFor(x => x.Telefono)
                 .NotEmpty()
                 .WithMessage("Ingresar el telefono");

           
        }
    }
}
