

namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;
    
    public class CvPostulanteValidator : AbstractValidator<CvPostulante>
    {
        public CvPostulanteValidator()
        {


            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage("Ingresar nombre");

            RuleFor(x => x.ApePaterno)
                .NotEmpty()
                .WithMessage("Ingresar apellido paterno");

            RuleFor(x => x.ApeMaterno)
               .NotEmpty()
                .WithMessage("Ingresar apellido materno");
            RuleFor(x => x.Dni)
              .NotEmpty()
               .WithMessage("Ingresar dni");
            RuleFor(x => x.Telefono)
              .NotEmpty()
               .WithMessage("Ingresar telefono");
            RuleFor(x => x.Fechacita)
              .NotEmpty()
               .WithMessage("Ingresar fecha de cita");
            RuleFor(x => x.HoraCita)
              .NotEmpty()
               .WithMessage("Ingresar hora de cita");

        }

    }

}
