

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class SolicitudNuevoCargoValidator : AbstractValidator<SolicitudNuevoCargo>
    {
        public SolicitudNuevoCargoValidator()
        {

            //RuleFor(x => x.IdeArea)   
            //    .NotEqual(0)
            //    .WithMessage("Seleccionar los datos requeridos ");

            RuleFor(x => x.CodigoCargo)
                .NotEmpty()
                .WithMessage("Debe ingresar el código del cargo");
            RuleFor(x => x.CodigoCargo)
                .Length(1, 10)
                .WithMessage("Máx. 10 caracteres");

            RuleFor(x => x.NombreCargo)
               .NotEmpty()
               .WithMessage("Ingresar el nombre de cargo");
            RuleFor(x => x.NombreCargo)
                .Length(1, 50)
                .WithMessage("Máx. 50 caracteres");

            RuleFor(x => x.NumeroPosiciones)
                .NotEmpty()
                .WithMessage("Ingresar el número de posiciones");
            RuleFor(x => x.NumeroPosiciones)
                .InclusiveBetween(1,999)
                .WithMessage("Ingresar el número de posiciones válido");

            RuleFor(x => x.DescripcionCargo)
                .NotEmpty()
                .WithMessage("Ingresar la descripción de cargo");
            RuleFor(x => x.DescripcionCargo)
                .Length(1, 100)
                .WithMessage("Máx. 100 caracteres");

            RuleFor(x => x.IdeDependencia)
                .NotEqual(0)
                .WithMessage("Seleccione una dependencia");

            RuleFor(x => x.IdeDepartamento)
                .NotEqual(0)
                .WithMessage("Seleccione un departamento");

            RuleFor(x => x.IdeArea)
                .NotEqual(0)
                .WithMessage("Seleccione una área");

            RuleFor(x => x.TipoRangoSalarial)
                .NotEqual("00")
                .WithMessage ("Seleccione el rango salarial");

            RuleFor(x => x.DescripcionEstudios)
                .NotEmpty()
                .WithMessage("Ingrese los estudios");
            RuleFor(x => x.DescripcionEstudios)
                .Length(1,255)
                .WithMessage("Máx. 255 caracteres");

            RuleFor(x => x.DescripcionFunciones)
                .NotEmpty()
                .WithMessage("Ingrese las funciones");
            RuleFor(x => x.DescripcionFunciones)
                .Length(1, 255)
                .WithMessage("Máx. 255 caracteres");

            RuleFor(x => x.DescripcionCompetencias)
                .NotEmpty()
                .WithMessage("Ingrese las competencias");
            RuleFor(x => x.DescripcionCompetencias)
                .Length(1, 255)
                .WithMessage("Máx. 255 caracteres");


            RuleFor(x => x.DescripcionObservaciones)
                .NotEmpty()
                .WithMessage("Ingrese las observaciones adicionales");
            RuleFor(x => x.DescripcionObservaciones)
                .Length(1, 255)
                .WithMessage("Máx. 255 caracteres");


            RuleFor(x => x.FechaPublicacion)
                .NotEmpty()
                .WithMessage("Ingresar fecha de publicación");

            RuleFor(x => x.FechaExpiracion)
                .NotEmpty()
                .WithMessage("Ingresar fecha de expiración");

            RuleFor(x => x.ObservacionPublicacion)
                .NotEmpty()
                .WithMessage("Ingresar una descripcion de Observaciones");
            RuleFor(x => x.ObservacionPublicacion)
                .Length(1, 255)
                .WithMessage("Máx. 255 caracteres");

        }
    }
}
