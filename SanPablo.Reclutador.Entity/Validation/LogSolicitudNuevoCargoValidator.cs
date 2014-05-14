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
                .NotEmpty().When(x => x.Aprobado.Equals(false))
                .WithMessage("Debe ingresar una observación");
            
            RuleFor(x => x.Observacion)
                .Length(0,250)
                .WithMessage("Máx. 250 caracteres");


         }

 
    }
}
