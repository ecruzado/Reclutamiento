
namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class ConocimientoGeneralPostulanteValidator : AbstractValidator<ConocimientoGeneralPostulante>
    {
        public ConocimientoGeneralPostulanteValidator()
        {

            RuleFor(x => x.TipoConocimientoOfimatica)
                .NotEqual("00")
                .WithMessage("Seleccionar el tipo de conocimiento");

            RuleFor(x => x.TipoNombreOfimatica)
                .NotEqual("00")
                .WithMessage("Seleccione una descripcion");

            RuleFor(x => x.TipoNivelConocimiento)
                .NotEqual("00")
                .WithMessage("Seleccione el nivel de conocimiento");

            RuleFor(x => x.TipoIdioma)
                .NotEqual("00")
                .WithMessage("Seleccione un Idioma");

            RuleFor(x => x.TipoConocimientoIdioma)
                .NotEqual("00")
                .WithMessage("Seleccione el tipo de conocimiento");

            RuleFor(x => x.TipoConocimientoGeneral)
                .NotEqual("00")
                .WithMessage("Seleccione el tipo de Conocimiento");

            RuleFor(x => x.TipoNombreConocimientoGeneral)
                .NotEqual("00")
                .WithMessage("Seleccione el nombre de conocimiento");

            RuleFor(x => x.NombreConocimientoGeneral)
                .Length(5, 50)
                .WithMessage("El max de caracteres permitidos es 50");


        }

    }
}
