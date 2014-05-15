namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class HorarioCargoValidator : AbstractValidator<HorarioCargo>
    {
        public HorarioCargoValidator()
        {
            RuleFor(x => x.TipoHorario)
                .NotEqual("0")
                .WithMessage("Seleccionar un Horario");

            RuleFor(x => x.PuntajeHorario)
                .NotEmpty()
                .WithMessage("Ingresar Puntaje");

            RuleFor(x => x.PuntajeHorario)
                .InclusiveBetween(-1, 10)
                .WithMessage("0 - 10");

         }

 
    }
}
