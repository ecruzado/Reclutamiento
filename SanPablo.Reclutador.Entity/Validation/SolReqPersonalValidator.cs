namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class SolReqPersonalValidator : AbstractValidator<SolReqPersonal>
    {
        public SolReqPersonalValidator()
        {

            RuleFor(x => x.IdeCargo)
                .NotEqual(0)
                .WithMessage("Seleccion el cargo del cual se solicita ampliacion");

            RuleFor(x => x.NumVacantes)
                .InclusiveBetween(0, 99)
                .WithMessage("Ingrese un número de vacantes válido");

            RuleFor(x => x.Observacion)
                .Length(0, 255)
                .WithMessage("El número máximo de caracteres permitido paea este campo es 255");

            RuleFor(x => x.Motivo)
                .Length(0, 255)
                .WithMessage("El número máximo de caracteres permitido paea este campo es 255");

            RuleFor(x => x.EstadoActivo)
               .NotEmpty()
               .WithMessage("Estado inválido");


        }
    }
}
