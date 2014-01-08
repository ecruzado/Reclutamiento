namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class EstudioPostulanteValidator : AbstractValidator<EstudioPostulante>
    {
        public EstudioPostulanteValidator()
        {
            RuleFor(x => x.tipTipoInstitucion)
                .NotEmpty()
                .WithMessage("Ingresar tipo de Institución");

            RuleFor(x => x.tipoArea)
                .NotEmpty()
                .WithMessage("Seleccionar área de estudio");

            RuleFor(x => x.tipNombreInstitucion)
                .NotEmpty()
                .WithMessage("Seleccionar nombre de la institución");

            RuleFor(x => x.tipoEducacion)
                .NotEmpty()
                .WithMessage("Seleccionar tipo de educacion");
        }
    }
}
