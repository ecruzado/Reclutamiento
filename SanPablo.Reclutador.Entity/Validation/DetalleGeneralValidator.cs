namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class DetalleGeneralValidator : AbstractValidator<DetalleGeneral>
    {
        public DetalleGeneralValidator()
        {
            RuleFor(x => x.Valor)
                .NotEmpty()
                .WithMessage("Debe ingresar un valor referencial");
            RuleFor(x => x.Valor)
                .Length(1,3)
                .WithMessage("El nro máx. de caracteres es de 3");


            RuleFor(x => x.Descripcion)
                .NotEmpty()
                .WithMessage("Ingresar una descripcion");
            

         }

 
    }
}
