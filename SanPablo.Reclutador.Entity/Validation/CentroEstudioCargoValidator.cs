namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class CentroEstudioCargoValidator : AbstractValidator<CentroEstudioCargo>
    {
        public CentroEstudioCargoValidator()
        {
            RuleFor(x => x.TipoCentroEstudio)
                .NotEqual("00")
                .WithMessage("Seleccionar un Tipo de Centro de Estudios");

            RuleFor(x => x.TipoNombreCentroEstudio)
                .NotEqual("00")
                .WithMessage("Seleccionar el nombre de la Institución");

            RuleFor(x => x.PuntajeCentroEstudios)
                .NotEmpty()
                .WithMessage("Ingresar Puntaje");
            RuleFor(x => x.PuntajeCentroEstudios)
                .InclusiveBetween(0, 10)
                .WithMessage("0 a 10");

         }

 
    }
}
