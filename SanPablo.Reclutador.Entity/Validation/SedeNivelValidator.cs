namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class SedeNivelValidator : AbstractValidator<SedeNivel>
    {
        public SedeNivelValidator()
        {
           
            RuleFor(x => x.IDEDEPENDENCIA)
                .NotEqual(0)
                .WithMessage("Seleccionar una dependencia");

            RuleFor(x => x.IDEDEPARTAMENTO)
                .NotEqual(0)
                .WithMessage("Seleccionar un departamento");

            RuleFor(x => x.IDEAREA)
                .NotEqual(0)
                .WithMessage("Seleccionar una área");
            
        }
    }
}