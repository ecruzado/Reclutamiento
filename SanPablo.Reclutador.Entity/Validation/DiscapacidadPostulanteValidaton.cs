namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class DiscapacidadPostulanteValidator : AbstractValidator<DiscapacidadPostulante>
    {
        public DiscapacidadPostulanteValidator()
        {

            RuleFor(x => x.TipoDiscapacidad)
                .NotEqual("00")
                .WithMessage("Seleccionar un tipo de discapacidad");
           

            RuleFor(x => x.DescripcionDiscapacidad)
                .NotEmpty()
                .WithMessage("Ingresar la descripción");
            RuleFor(x => x.DescripcionDiscapacidad)
                .Length(1, 50)
                .WithMessage("Máx. 50 caracteres");

        }

    }
}
