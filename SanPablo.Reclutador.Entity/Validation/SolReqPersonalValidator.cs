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
                .NotEmpty()
                .WithMessage("Ingrese el número de vacantes");
            RuleFor(x => x.NumVacantes)
                .InclusiveBetween(1, 99)
                .WithMessage("Ingrese un número de vacantes válido");

            RuleFor(x => x.Observacion)
               .NotEmpty()
               .WithMessage("Ingrese la observación");
            RuleFor(x => x.Observacion)
                .Length(1, 255)
                .WithMessage("El número máximo de caracteres permitido paea este campo es 255");

            RuleFor(x => x.Motivo)
              .NotEmpty()
              .WithMessage("Ingrese los motivos del requerimiento");
            RuleFor(x => x.Motivo)
                .Length(1, 255)
                .WithMessage("El número máximo de caracteres permitido paea este campo es 255");

            RuleFor(x => x.EstadoActivo)
               .NotEmpty()
               .WithMessage("Estado inválido");


            RuleFor(x => x.TipPuesto)
                .NotEqual("00")
                .WithMessage("Seleccione una opción");

           
            
            

        }
    }
}
