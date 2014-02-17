namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class NivelAcademicoCargoValidator : AbstractValidator<NivelAcademicoCargo>
    {
        public NivelAcademicoCargoValidator()
        {
            RuleFor(x => x.TipoEducacion)
                .NotEqual("00")
                .WithMessage("Seleccionar un Tipo de Educación");

            RuleFor(x => x.TipoAreaEstudio)
                .NotEqual("00")
                .WithMessage("Ingresar un área de Estudio");

            RuleFor(x => x.TipoNivelAlcanzado)
                .NotEqual("00")
                .WithMessage("Seleccione el Nivel Alcanzado");

            RuleFor(x => x.PuntajeNivelEstudio)
                .NotEmpty()
                .WithMessage("Ingresar Puntaje");

            RuleFor(x => x.PuntajeNivelEstudio)
                .InclusiveBetween(0, 20)
                .WithMessage("Ingresar un puntaje válido");

            RuleFor(x => x.CicloSemestre)
               .InclusiveBetween(0, 99)
               .WithMessage("Ingresar un Ciclo/Semestre válido");

         }

 
    }
}
