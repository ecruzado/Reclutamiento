namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System.Globalization;
    using System;

    public class OfrecemosCargoValidator : AbstractValidator<OfrecemosCargo>
    {
        public OfrecemosCargoValidator()
        {
            RuleFor(x => x.TipoOfrecimiento)
                .NotEqual("00")
                .WithMessage("Seleccionar un ofrecimiento");

         }

 
    }
}
