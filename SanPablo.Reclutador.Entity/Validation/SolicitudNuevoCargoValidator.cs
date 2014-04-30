

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
                .WithMessage("Debe ingresar el codigo del Cargo");
            RuleFor(x => x.CodigoCargo)
                .Length(1, 10)
                .WithMessage("El codigo de cargo debe tener un maximo de 10 caracteres");

            RuleFor(x => x.NombreCargo)
                .Length(3, 50)
                .WithMessage("Ingresar el nombre con un mínimo de 3 y un máximo de 50 caracteres");

            RuleFor(x => x.DescripcionCargo)
                .Length(5, 255)
                .WithMessage("Ingresar la descripcion con un mínimo de 3 y un máximo de 100 caracteres");
 
            RuleFor(x => x.NumeroPosiciones)
                .NotEmpty()
                .WithMessage("Ingresar el número de posiciones para la solicitud");

            RuleFor(x => x.TipoRangoSalarial)
                .NotEqual("00")
                .WithMessage ("Seleccione el rango salarial para el cargo");

            RuleFor(x => x.DescripcionEstudios)
                .NotEmpty()
                .WithMessage("Ingresar una descripcion de estudio");
            RuleFor(x => x.DescripcionEstudios)
                .Length(5,255)
                .WithMessage("Ingresar la descripcion con un mínimo de 5 y un máximo de 255 caracteres");

            RuleFor(x => x.DescripcionFunciones)
                .NotEmpty()
                .WithMessage("Ingresar una descripcion de Funciones");
            RuleFor(x => x.DescripcionFunciones)
                .Length(5, 255)
                .WithMessage("Ingresar la descripcion con un mínimo de 5 y un máximo de 255 caracteres");

            RuleFor(x => x.DescripcionCompetencias)
                .NotEmpty()
                .WithMessage("Ingresar una descripcion de Competencias");
            RuleFor(x => x.DescripcionCompetencias)
                .Length(5, 255)
                .WithMessage("Ingresar la descripcion con un mínimo de 5 y un máximo de 255 caracteres");

            RuleFor(x => x.ObservacionPublicacion)
                .NotEmpty()
                .WithMessage("Ingresar una descripcion de Observaciones");
            RuleFor(x => x.ObservacionPublicacion)
                .Length(5, 255)
                .WithMessage("Ingresar la descripcion con un mínimo de 5 y un máximo de 255 caracteres");

            //RuleFor(X => X.FechaPublicacion)
            //    .NotEmpty()
            //    .WithMessage("Ingresar fecha de publicación");
            //    //.GreaterThan(new DateTime(DateTime.Now()))
            //    //.WithMessage("Ingresar fecha de Publicación válida");

            //RuleFor(X => X.FechaExpiracion)
            //    .NotEmpty()
            //    .WithMessage("Ingresar fecha de Expiración");
               //.GreaterThan(new DateTime(DateTime.Now()))
               //.WithMessage("Ingresar fecha de Expiración válida");

        }
    }
}
