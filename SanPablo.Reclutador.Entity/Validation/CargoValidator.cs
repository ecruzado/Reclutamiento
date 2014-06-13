
namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;
    
    public class CargoValidator : AbstractValidator<Cargo>
    {

        public CargoValidator()
        {
            RuleFor(x => x.ObjetivoCargo)
                .NotEmpty()
                .WithMessage("Ingresar el Objetivos");
            RuleFor(x => x.ObjetivoCargo)
                .Length(1,255)
                .WithMessage("Máx. 255 caracteres");

            RuleFor(x => x.FuncionCargo)
                .NotEmpty()
                .WithMessage("Ingresar las Funciones");
            RuleFor(x => x.FuncionCargo)
                .Length(1, 255)
                .WithMessage("Máx. 255 caracteres");

            //pestaña General

            RuleFor(x => x.PuntajeTotalPostulanteInterno)
                .NotEmpty()
                .WithMessage("0 - 10")
                .InclusiveBetween(0, 10)
                .WithMessage("0 - 10");

            RuleFor(x => x.EdadInicio)
                .NotEmpty()
                .WithMessage("Debe ingresar el inicio de rango de edad");
            RuleFor(x => x.EdadInicio)
                .InclusiveBetween(18, 99)
                .WithMessage("El postulante debe ser mayor de edad");

            RuleFor(x => x.EdadFin)
                .NotEmpty()
                .WithMessage("Debe ingresar el fin de rango de edad");
            RuleFor(x => x.EdadFin)
                .InclusiveBetween(18, 99)
                .WithMessage("El potulante debe ser mayor de edad");

            RuleFor(x => x.PuntajeEdad)
                .NotEmpty()
                .WithMessage("0 - 10")
                .InclusiveBetween(0,10)
                .WithMessage("0 - 10");


            RuleFor(x => x.PuntajeSalario)
                .NotEmpty()
                .WithMessage("0 - 10")
                .InclusiveBetween(0, 10)
                .WithMessage("0 - 10");

            RuleFor(x => x.Sexo)
                .NotEqual("0")
                .WithMessage("Seleccionar Sexo");

            RuleFor(x => x.TipoRangoSalarial)
                .NotEqual("00")
                .WithMessage("Seccionar rango salarial");
            

            RuleFor(x => x.PuntajeSexo)
                 .NotEmpty()
                 .WithMessage("0 - 10")
                .InclusiveBetween(0, 10)
                .WithMessage("0 - 10");

            RuleFor(x => x.TipoRequerimiento)
                .NotEqual("00")
                .WithMessage("Seleccionar el tipo de Requerimiento");

            RuleFor(x => x.PuntajeMinimoGeneral)
                .NotEmpty()
                .WithMessage("Pendiente");

            RuleFor(x => x.PuntajeMinimoExamen)
                .NotEmpty()
                .WithMessage("Pendiente");

            RuleFor(x => x.CantidadPreseleccionados)
                .NotEmpty()
                .WithMessage("Pendiente");
                

            //RuleFor(x => x.PuntajeMinimoPostulanteInterno)
            //    .GreaterThanOrEqualTo(0)
            //    .WithMessage("Ingresar puntaje válido")
            //    .LessThanOrEqualTo(x => x.PuntajeTotalPostulanteInterno)
            //    .WithMessage("No puede sobreparar el puntaje máx de Postulante interno");

            //Datos de mantenimientos

            RuleFor(x => x.NombreCargo)
                .NotEmpty()
                .WithMessage("Ingresar el nombre del cargo");

            RuleFor(x => x.DescripcionCargo)
                .NotEmpty()
                .WithMessage("Ingresar el nombre del cargo");

            RuleFor(x => x.IdeDependencia)
               .NotEqual(0)
               .WithMessage("Seleccione una dependencia");

            RuleFor(x => x.IdeDepartamento)
               .NotEqual(0)
               .WithMessage("Seleccione Departamento");

            RuleFor(x => x.IdeArea)
               .NotEqual(0)
               .WithMessage("Seleccione Área");

        }

    }
}
