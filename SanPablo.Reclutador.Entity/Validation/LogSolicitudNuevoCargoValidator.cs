namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class LogSolicitudNuevoCargoValidator : AbstractValidator<LogSolicitudNuevoCargo>
    {
        public LogSolicitudNuevoCargoValidator()
        {
            RuleFor(x => x.Observacion)
                .Length(0,250)
                .WithMessage("La observacion no puede tener mas de 250 caracteres");

         }

 
    }
}
