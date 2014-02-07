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

         }

 
    }
}
