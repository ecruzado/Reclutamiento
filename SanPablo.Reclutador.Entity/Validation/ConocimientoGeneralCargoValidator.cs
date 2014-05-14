namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class ConocimientoGeneralCargoValidator : AbstractValidator<ConocimientoGeneralCargo>
    {
        public ConocimientoGeneralCargoValidator()
        {
            RuleFor(x => x.TipoConocimientoOfimatica)
                .NotEqual("00")
                .WithMessage("Seleccionar un tipo de conocimiento");

            RuleFor(x => x.TipoConocimientoIdioma)
                .NotEqual("00")
                .WithMessage("Seleccionar un tipo de idioma ");

            RuleFor(x => x.TipoConocimientoGeneral)
                .NotEqual("00")
                .WithMessage("Seleccionar un tipo de conocimiento");

            RuleFor(x => x.TipoIdioma)
                .NotEqual("00")
                .WithMessage("Seleccionar una descripción");

            RuleFor(x => x.TipoNombreOfimatica)
                .NotEqual("00")
                .WithMessage("Seleccionar una descripción");

            RuleFor(x => x.TipoNombreConocimientoGeneral)
                .NotEqual("00")
                .WithMessage("Seleccionar una descripción");

            RuleFor(x => x.TipoNivelConocimiento)
                .NotEqual("00")
                .WithMessage("Seleccionar un nivel de conocimiento");

            RuleFor(x => x.PuntajeConocimiento)
                .NotEmpty()
                .WithMessage("Ingresar Puntaje");
            RuleFor(x => x.PuntajeConocimiento)
                .InclusiveBetween(0, 10)
                .WithMessage("0 a 10");

         }

 
    }
}
