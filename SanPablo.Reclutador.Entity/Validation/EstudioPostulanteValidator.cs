namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class EstudioPostulanteValidator : AbstractValidator<EstudioPostulante>
    {
        public EstudioPostulanteValidator()
        {
            RuleFor(x => x.TipTipoInstitucion)
                .NotEmpty()
                .WithMessage("Ingresar tipo de Institución");

            RuleFor(x => x.TipoArea)
                .NotEmpty()
                .WithMessage("Seleccionar área de estudio");

            RuleFor(x => x.TipoNombreInstitucion)
                .NotEmpty()
                .WithMessage("Seleccionar nombre de la institución");

            RuleFor(x => x.TipoEducacion)
                .NotEmpty()
                .WithMessage("Seleccionar tipo de educacion");
        }
    }
}
