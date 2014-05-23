
namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;
    
    public class ReclutamientoPersonaExamenValidator : AbstractValidator<ReclutamientoPersonaExamen>
    {

        public ReclutamientoPersonaExamenValidator()
        {
            RuleFor(x => x.FechaEvaluacion)
                .NotEmpty()
                .WithMessage("Ingresar una fecha de evaluacion");

            RuleFor(x => x.HoraEvaluacion)
                .NotEmpty()
                .WithMessage("Ingresar la hora de evaluacion");


            RuleFor(x => x.IdeUsuarioResponsable)
                .NotEmpty()
                .WithMessage("Ingresar responsable");
            RuleFor(x => x.IdeUsuarioResponsable)
                .NotEqual(0)
                .WithMessage("Ingresar un responsable");

            //RuleFor(x => x.Observacion)
            //    .NotEmpty()
            //    .WithMessage("Ingresar una observacion");


            RuleFor(x => x.ComentarioResultado)
                .NotEmpty()
                .WithMessage("Ingresar el comentario");
            

        }

    }
}
