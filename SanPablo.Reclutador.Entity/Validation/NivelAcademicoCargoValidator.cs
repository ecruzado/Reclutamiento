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
                .NotEqual("00").When(x=>x.TipoEducacion.Equals("02") ||x.TipoEducacion.Equals("03")||x.TipoEducacion.Equals("04"))
                .WithMessage("Ingresar un área de Estudio");

            RuleFor(x => x.TipoNivelAlcanzado)
                .NotEqual("00")
                .WithMessage("Seleccione el Nivel Alcanzado");

            RuleFor(x => x.PuntajeNivelEstudio)
                .NotEmpty()
                .WithMessage("Ingresar Puntaje");

            RuleFor(x => x.PuntajeNivelEstudio)
                .InclusiveBetween(0, 10)
                .WithMessage("0 - 10");

            RuleFor(x => x.CicloSemestre)
               .InclusiveBetween(0, 12)
               .WithMessage("Ingresar un ciclo/Semestre válido");

         }

 
    }
}
