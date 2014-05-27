

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;
    
    public class ReclutamientoPersonaValidator : AbstractValidator<ReclutamientoPersona>
    {

        public ReclutamientoPersonaValidator()
        {
            RuleFor(x => x.MotivoCierre)
                    .NotEmpty()
                    .WithMessage("Ingresar el motivo");

            RuleFor(x => x.Comentario)
                    .NotEmpty()
                    .WithMessage("Ingrese un comentario");
        
        }
    }

}
