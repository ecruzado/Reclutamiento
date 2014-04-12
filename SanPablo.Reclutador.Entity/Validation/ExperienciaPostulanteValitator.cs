namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class ExperienciaPostulanteValidator : AbstractValidator<ExperienciaPostulante>
    {
        public ExperienciaPostulanteValidator()
        {
            RuleFor(x => x.NombreEmpresa)
               .NotEmpty()
               .WithMessage("Ingresar Nombre de la Empresa");
            RuleFor(x => x.NombreEmpresa)
                .Length(3,100)
                .WithMessage("Ingresar Empresa con longitud entre 3 a 100 caracteres");

            RuleFor(x => x.TipoCargoTrabajo)
                .NotEqual("00")
                .WithMessage("Seleccionar el cargo que desempeñaba");

            RuleFor(x => x.NombreCargoTrabajo)
                .Length(10,50)
                .WithMessage("Ingresar un cargo con longitud entre 10 y 50c caracteres");

            RuleFor(x => x.FechaTrabajoInicio)
                .NotEmpty()
                .WithMessage("Ingresar la fecha de inicio de trabajo");
            RuleFor(x => x.FechaTrabajoInicio)
                .GreaterThan(new DateTime(1900,01,01))
                .WithMessage("Ingresar una Fecha Inicio válida");

            //RuleFor(x => x.FechaTrabajoFin)
            //   .GreaterThan(new DateTime(1990, 01, 01))
            //   .WithMessage("Ingresar una Fecha Fin válida");

            RuleFor(x => x.NombreReferente)
                .NotEmpty()
                .WithMessage("Ingresar Nombre de referencia");

            RuleFor(x => x.TipoCargoTrabajoReferente)
                .NotEqual("00")
                .WithMessage("Seleccionar el cargo de la persona de referencia");

            //RuleFor(x => x.NumeroFijoInstitucionReferente)
            //    .NotEmpty()
            //    .WithMessage("Ingresar telefono fijo de la institucion");
            //RuleFor(x => x.NumeroFijoInstitucionReferente)
            //    .InclusiveBetween(100000, 999999999)
            //    .WithMessage("Ingresar telefono fijo válido");

            RuleFor(x => x.NumeroMovilReferencia)
                .InclusiveBetween(900000000, 999999999)
                .WithMessage("Ingresar telefono movil válido");

            RuleFor(x => x.NumeroAnexoInstitucionReferente)
               .InclusiveBetween(1, 999999)
               .WithMessage("Ingresar anexo válido");

            RuleFor(x => x.CorreoReferente)
                .EmailAddress()
                .WithMessage("Ingresar un correo válido");

            RuleFor(x => x.FechaTrabajoFin)
                .GreaterThan(x => x.FechaTrabajoInicio.Value).When(x => x.FechaTrabajoInicio != null)
                .When(x => x.ActualmenteTrabajando.Equals(false))
                .WithMessage("Ingresar una fecha final válida");


            RuleFor(x => x.FuncionesDesempenadas)
                .NotEmpty()
                .WithMessage("Ingresar la descripcion de las funciones desempeñadas")
                .Length(1, 255)
                .WithMessage("Ingrese las funciones desempeñadas - Máx. 255 caracteres");


            RuleFor(x => x.FechaInicio)
                .NotEmpty()
                .WithMessage("Ingresar la fecha de inicio");
            //When(x => x.ActualmenteTrabajando.Equals(false), () =>
            //{
            //    RuleFor(x => x.FechaTrabajoFin)
            //        .GreaterThan(x => x.FechaTrabajoInicio).When(x=>x.FechaTrabajoInicio != null)
            //        .WithMessage("Ingresar una fecha final válida");
            //});
        }
    }
}
