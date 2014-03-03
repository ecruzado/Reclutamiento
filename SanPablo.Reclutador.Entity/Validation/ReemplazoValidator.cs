

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    
    public class ReemplazoValidator : AbstractValidator<Reemplazo>
    {
        public ReemplazoValidator()
        {


            RuleFor(x => x.Nombres)
                .NotEmpty()
                .WithMessage("Ingresar nombre");

            RuleFor(x => x.ApeMaterno)
                .NotEmpty()
                .WithMessage("Apellido Paterno");

            RuleFor(x => x.ApePaterno)
                .NotEmpty()
                .WithMessage("Apellido Materno");

            RuleFor(x => x.FecInicioReemplazo)
                .LessThanOrEqualTo(x => x.FecFinalReemplazo)
                .WithMessage("La fecha Inicial debe ser menor a la fecha final")
                .NotEmpty()
                .WithMessage("Ingreser Fecha de inicio de reemplazo");


            RuleFor(x => x.FecFinalReemplazo)
              .NotEmpty()
              .WithMessage("Ingreser Fecha final de reemplazo");
             


        }

    }


}
