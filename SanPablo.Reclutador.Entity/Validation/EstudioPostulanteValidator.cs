namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
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
                .Length(10, 100)
                .WithMessage("Ingresar Nombre Universidad con longitud entre 10 a 100 caracteres");

            //RuleFor(x => x.TipoArea)
            //    .NotEqual("00")
            //    .WithMessage("Seleccionar área de estudio");

            RuleFor(x => x.TipoEducacion)
                .NotEqual("00")
                .WithMessage("Seleccionar tipo de educación");

            RuleFor(x => x.TipoNivelAlcanzado)
                .NotEqual("00")
                .WithMessage("Seleccionar Nivel Alcanzado");


            RuleFor(x => x.FechaEstudioInicio)
               .LessThanOrEqualTo(x => x.FechaEstudioFin.Value)
               .When(x => x.ActualmenteEstudiando.Equals(false))
               .WithMessage("Ingresar una fecha final posterior a la fecha inicial");
           
            RuleFor(x => x.FechaEstudioInicio)
                .NotEmpty()
                .WithMessage("Ingresar Fecha de Inicio");
            
            RuleFor(x => x.NombreInstitucion)
                .NotEmpty()
                .When(x => x.TipoNombreInstitucion.Equals("XX"))
                .WithMessage("Ingresar el nombre de la institución");

            RuleFor(x => x.FechaInicio)
                .NotEmpty()
                .WithMessage("Ingresar una fecha");

            When(x => x.ActualmenteEstudiando.Equals(false), () =>
            {
                RuleFor(x => x.FechaFin)
                    .NotEmpty()
                    .WithMessage("Ingresar una fecha fin");
            });
            
            //When(x => x.ActualmenteEstudiando.Equals(false), () =>
            //{
            //    RuleFor(x => x.FechaEstudioFin)
            //        .GreaterThan(x => x.FechaEstudioInicio.Value)
            //        .WithMessage("Ingresar una fecha final válida");
            //});
            

        }

 
    }
}
