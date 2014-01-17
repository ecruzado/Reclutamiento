namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class ExperienciaPostulanteValidator : AbstractValidator<ExperienciaPostulante>
    {
        public ExperienciaPostulanteValidator()
        {
            RuleFor(x => x.NombreEmpresa)
                .Length(10,100)
                .WithMessage("Ingresar Empresa con longitud entre 10 a 100 caracteres");

            RuleFor(x => x.TipoCargoTrabajo)
                .NotEqual("00")
                .WithMessage("Seleccionar el cargo que desempeñaba");

            RuleFor(x => x.FechaTrabajoInicio)
                .NotEmpty()
                .WithMessage("Ingresar la fecha de inicio de trabajo");
            RuleFor(x => x.FechaTrabajoInicio)
                .GreaterThan(new DateTime(1990,01,01))
                .WithMessage("Ingresar una Fecha Inicio válida");

            RuleFor(x => x.FechaTrabajoFin)
               .GreaterThan(new DateTime(1990, 01, 01))
               .WithMessage("Ingresar una Fecha Fin válida");

            RuleFor(x => x.NombreReferente)
                .NotEmpty()
                .WithMessage("Ingresar Nombre de referencia");

            RuleFor(x => x.TipoCargoTrabajoReferente)
                .NotEqual("00")
                .WithMessage("Seleccionar el cargo de la persona de referencia");

            RuleFor(x => x.NumeroFijoInstitucionReferente)
                .NotEmpty()
                .WithMessage("Ingresar telefono fijo de la institucion");
            RuleFor(x => x.NumeroFijoInstitucionReferente)
                .InclusiveBetween(0, 999999)
                .WithMessage("Ingresar telefono fijo válido");


        }
    }
}
