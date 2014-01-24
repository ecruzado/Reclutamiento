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
                .WithMessage("Ingresar la descripcion");
            RuleFor(x => x.DescripcionDiscapacidad)
                .Length(5, 50)
                .WithMessage("Maximo de caracteres permitidos es de 50");

        }

    }
}
