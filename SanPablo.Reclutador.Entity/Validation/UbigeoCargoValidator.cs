namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class UbigeoCargoValidator : AbstractValidator<UbigeoCargo>
    {
        public UbigeoCargoValidator()
        {
            RuleFor(x => x.IdeUbigeo)
                .NotEqual(0)
                .WithMessage("Seleccionar Departamento, Provincia y Distrito");

            RuleFor(x => x.PuntajeUbigeo)
                .NotEmpty()
                .WithMessage("Ingresar Puntaje");
            RuleFor(x => x.PuntajeUbigeo)
                .InclusiveBetween(0, 10)
                .WithMessage("Ingresar un puntaje entre 0 y 10");

         }

 
    }
}
