
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
                .WithMessage("El campo no debe sobrepasar los 255 caracteres");

            RuleFor(x => x.FuncionCargo)
                .NotEmpty()
                .WithMessage("Ingresar las Funciones");
            RuleFor(x => x.FuncionCargo)
                .Length(1, 255)
                .WithMessage("El campo no debe sobrepasar los 255 caracteres");

            RuleFor(x => x.PuntajeTotalPostulanteInterno)
                .NotEmpty()
                .WithMessage("Debe ingresar el puntaje para el postulante interno");
            RuleFor(x => x.PuntajeTotalPostulanteInterno)
                .ExclusiveBetween(0,20)
                .WithMessage("el rango de puntaje es de 0 a 20");

            RuleFor(x => x.EdadInicio)
                .NotEmpty()
                .WithMessage("Debe ingresar el inicio de rango de edad");
            RuleFor(x => x.EdadInicio)
                .ExclusiveBetween(18, 99)
                .WithMessage("Verifíque el dato ingresado");

            RuleFor(x => x.EdadFin)
                .NotEmpty()
                .WithMessage("Debe ingresar el fin de rango de edad");
            RuleFor(x => x.EdadFin)
                .ExclusiveBetween(18, 99)
                .WithMessage("Verifíque el dato ingresado");

            RuleFor(x => x.PuntajeEdad)
                .NotEmpty()
                .WithMessage("Debe ingresar puntaje de edad");
            RuleFor(x => x.PuntajeEdad)
                .ExclusiveBetween(0,20)
                .WithMessage("El rango de puntaje es de 0 a 20");

            RuleFor(x => x.Sexo)
                .NotEqual("0")
                .WithMessage("Seleccionar Sexo");

            RuleFor(x => x.PuntajeSexo)
                 .NotEmpty()
                 .WithMessage("Debe ingresar puntaje de sexo");
            RuleFor(x => x.PuntajeSexo)
                .ExclusiveBetween(0, 20)
                .WithMessage("El rango de puntaje es de 0 a 20");

            RuleFor(x => x.TipoRequerimiento)
                .NotEqual("00")
                .WithMessage("Seleccionar el tipo de Requerimiento");

            RuleFor(x => x.PuntajeMinimoPostulanteInterno)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Ingresar puntaje válido")
                .LessThanOrEqualTo(x => x.PuntajeTotalPostulanteInterno)
                .WithMessage("No puede sobreparar el puntaje máx de Postulante interno");

        }

    }
}
