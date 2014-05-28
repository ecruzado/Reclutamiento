

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
               .WithMessage("Ingresar DNI");
            RuleFor(x => x.Dni)
               .Length(8)
               .WithMessage("Ingresar un DNI válido");

            RuleFor(x => x.Telefono)
              .NotEmpty()
               .WithMessage("Ingresar teléfono");
            RuleFor(x => x.Telefono)
              .Length(7,9)
              .WithMessage("Ingresar un teléfono válido");
            //RuleFor(x => x.Fechacita)
            //  .NotEmpty()
            //   .WithMessage("Ingresar fecha de cita");
            //RuleFor(x => x.HoraCita)
            //  .NotEmpty()
            //   .WithMessage("Ingresar hora de cita");

        }

    }

}
