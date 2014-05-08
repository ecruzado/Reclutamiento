namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class ParientePostulanteValidator : AbstractValidator<ParientePostulante>
    {
        public ParientePostulanteValidator()
        {

            RuleFor(x => x.ApellidoPaterno)
                .NotEmpty()
                .WithMessage("Ingresar el Apellido Paterno del pariente");
            RuleFor(x => x.ApellidoPaterno)
                .Length(1, 50)
                .WithMessage("Máx. 50 caracteres");

            RuleFor(x => x.ApellidoMaterno)
                .NotEmpty()
                .WithMessage("Ingresar el Apellido Materno del pariente");
            RuleFor(x => x.ApellidoMaterno)
                .Length(1, 50)
                .WithMessage("Máx. 50 caracteres");

            RuleFor(x => x.Nombres)
               .NotEmpty()
               .WithMessage("Ingresar el nombre  del pariente");
            RuleFor(x => x.Nombres)
                .Length(1, 50)
                .WithMessage("Máx. 50 caracteres");

            RuleFor(x => x.TipoDeVinculo)
                .NotEqual("00")
                .WithMessage("Seleccione vinculo de parentesco");

            RuleFor(x => x.FechaNacimiento)
                .NotEmpty().When(x => x.TipoDeVinculo.Equals(TipoVinculo.Hijo))
                .WithMessage("La fecha de nacimiento en caso de hijos es obligatoria");

        }

    }
}
