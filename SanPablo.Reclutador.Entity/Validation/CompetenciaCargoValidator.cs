namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class CompetenciaCargoValidator : AbstractValidator<CompetenciaCargo>
    {
        public CompetenciaCargoValidator()
        {
            RuleFor(x => x.TipoCompetencia)
                .NotEqual("00")
                .WithMessage("Seleccionar una competenecia");


            RuleFor(x => x.Puntaje)
               .NotEmpty()
               .WithMessage("Ingresar Puntaje");
            RuleFor(x => x.Puntaje)
                .InclusiveBetween(0,10)
                .WithMessage("0 - 10");

         }

 
    }
}
