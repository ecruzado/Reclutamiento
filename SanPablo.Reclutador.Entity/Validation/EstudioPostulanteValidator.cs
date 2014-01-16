namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class EstudioPostulanteValidator : AbstractValidator<EstudioPostulante>
    {
        public EstudioPostulanteValidator()
        {
            RuleFor(x => x.TipTipoInstitucion)
                .NotEqual("00")
                .WithMessage("Ingresar tipo de Institución");

            RuleFor(x => x.TipoNombreInstitucion)
                .NotEqual("00")
                .WithMessage("Seleccionar nombre de la institución");

            RuleFor(x => x.NombreInstitucion)
                .Length(25, 100)
                .WithMessage("Ingresar Nombre Universidad con longitud entre 25 a 100 caracteres");

            RuleFor(x => x.TipoArea)
                .NotEqual("00")
                .WithMessage("Seleccionar área de estudio");

            RuleFor(x => x.TipoEducacion)
                .NotEqual("00")
                .WithMessage("Seleccionar tipo de educación");

            RuleFor(x => x.TipoNivelAlcanzado)
                .NotEqual("00")
                .WithMessage("Seleccionar Nivel Alcanzado");

            RuleFor(x => x.FechaEstudioInicio)
                .NotEmpty()
                .WithMessage("Ingresar Fecha de Inicio");
            RuleFor(x => x.FechaEstudioInicio)
                .GreaterThan(new DateTime(1900, 01, 01))
                .WithMessage("Ingresar Fecha de Inicio Válido");

            RuleFor(x => x.FechaEstudioFin  )
                .GreaterThan(new DateTime(1900, 01, 01))
                .WithMessage("Ingresar Fecha de Fin Válido");


        }
    }
}
