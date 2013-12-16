namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class PersonaValidator : AbstractValidator<Persona>
    {
        public PersonaValidator()
        {
            RuleFor(x => x.TipoDocumento)
                .NotEmpty()
                .WithMessage("Ingresar tipo documento");
            
            RuleFor(x => x.NumeroDocumento)
                .NotEmpty()
                .WithMessage("Ingresar número documento");
            RuleFor(x => x.NumeroDocumento)
                .Length(8)
                .WithMessage("Ingresar número documento de 8 dígitos");

            RuleFor(x => x.ApellidoPaterno)
                .NotEmpty()
                .WithMessage("Ingresar apellido paterno");
            RuleFor(x => x.ApellidoPaterno)
                .Length(1,25)
                .WithMessage("Ingresar apellido paterno con menos de 25 caracteres");

            RuleFor(x => x.ApellidoMaterno)
                .NotEmpty()
                .WithMessage("Ingresar apellido materno");
            RuleFor(x => x.ApellidoMaterno)
                .Length(1, 25)
                .WithMessage("Ingresar apellido materno con menos de 25 caracteres");

            RuleFor(x => x.PrimerNombre)
                .NotEmpty()
                .WithMessage("Ingresar primer nombre");
            RuleFor(x => x.PrimerNombre)
                .Length(1, 25)
                .WithMessage("Ingresar primer nombre con menos de 25 caracteres");

            RuleFor(x => x.FechaNacimiento)
                .NotEmpty()
                .WithMessage("Ingresar fecha de nacimiento");
            RuleFor(x => x.FechaNacimiento)
                .GreaterThan(new DateTime(1900,1,1))
                .WithMessage("Ingresar fecha de nacimiento válida");

            RuleFor(x => x.IndicadorSexo)
                .NotEmpty()
                .WithMessage("Ingresar sexo");
            
            RuleFor(x => x.TipoEstadoCivil)
                .NotEmpty()
                .WithMessage("Ingresar Estado Civil");
        }
    }
}