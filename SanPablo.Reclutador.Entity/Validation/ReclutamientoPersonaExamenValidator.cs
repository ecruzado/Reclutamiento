
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
                .WithMessage("Debe ingresar responsable de la evaluacion");
            RuleFor(x => x.IdeUsuarioResponsable)
                .NotEqual(0)
                .WithMessage("Debe ingresar un responsable de la evaluacion");

            RuleFor(x => x.Observacion)
                .NotEmpty()
                .WithMessage("Debe ingresar una observacion para la evaluacion");
            RuleFor(x => x.Observacion)
                .Length(1,255)
                .WithMessage("No debe sobrepasar los 255 caracteres");

            RuleFor(x => x.ComentarioResultado)
                .NotEmpty()
                .WithMessage("Debe ingresar el comentario del resultado");
            RuleFor(x => x.ComentarioResultado)
                .Length(1,255)
                .WithMessage("No debe sobrepasar los 255 caracteres");

        }

    }
}
