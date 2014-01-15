
namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;
    
    public class CriterioValidator : AbstractValidator<Criterio>
    {

        public CriterioValidator()
        {
            RuleFor(x => x.TipoCriterio)
                .NotEqual("0")
                .WithMessage("Ingresar tipo criterio");
            
            RuleFor(x => x.NombreCriterio)
                .NotEmpty()
                .WithMessage("Ingresar Pregunta");

            RuleFor(x => x.TipoMedicion)
                .NotEqual("0")
                .WithMessage ("Ingresar tipo Medición");
            
            RuleFor(x => x.TipoModoRegistro)
                .NotEqual("0")
                .WithMessage("Ingresar modo de registro");

            RuleFor(x => x.Calificacion)
                .NotEqual(0)
                .WithMessage("Ingresar Tipo de calificacion");
           
        }

    }
}
