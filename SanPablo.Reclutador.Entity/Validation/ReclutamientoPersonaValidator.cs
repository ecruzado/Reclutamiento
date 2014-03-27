

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
                    .WithMessage("Ingresar el motivo de cierre de la solicitud");

            RuleFor(x => x.Comentario)
                    .NotEmpty()
                    .WithMessage("Ingrese un comenteatio");
        
        }
    }

}
