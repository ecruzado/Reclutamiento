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
                .WithMessage("Seleccionar Distrito");

            RuleFor(x => x.IdeDepartamento)
                .NotEqual(0)
                .WithMessage("Seleccionar Departamento");

            RuleFor(x => x.IdeProvincia)
                .NotEqual(0)
                .WithMessage("Seleccionar Provincia");

            RuleFor(x => x.PuntajeUbigeo)
                .NotEmpty()
                .WithMessage("Ingresar Puntaje");
            RuleFor(x => x.PuntajeUbigeo)
                .InclusiveBetween(0, 10)
                .WithMessage("0 - 10");

         }

 
    }
}
