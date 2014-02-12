namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class DiscapacidadCargoValidator : AbstractValidator<DiscapacidadCargo>
    {
        public DiscapacidadCargoValidator()
        {
            RuleFor(x => x.TipoDiscapacidad)
                .NotEqual("00")
                .WithMessage("Seleccionar un Horario");

            RuleFor(x => x.PuntajeDiscapacidad)
                .NotEmpty()
                .WithMessage("Ingresar Puntaje");
            RuleFor(x => x.PuntajeDiscapacidad)
                .InclusiveBetween(0, 20)
                .WithMessage("Ingresar un puntaje válido");

         }

 
    }
}
