namespace SanPablo.Reclutador.Entity.Validation
{
    using FluentValidation;
    using System;

    public class PostulanteValidator : AbstractValidator<Postulante>
    {
        public PostulanteValidator()
        {
            RuleFor(x => x.TipoDocumento)
                .NotEmpty()
                .WithMessage("Ingresar tipo documento");
            
            RuleFor(x => x.NumeroDocumento)
                .NotEmpty()
                .WithMessage("Ingresar número documento");
            RuleFor(x => x.NumeroDocumento)
                .Length(8)
                .WithMessage("Ingresar número documento de 8 dígitos");

            RuleFor(x => x.ApellidoPaterno)
                .NotEmpty()
                .WithMessage ("Ingresar apellido paterno");
            RuleFor(x => x.ApellidoPaterno)
                .Length(1,25)
                .WithMessage("Ingresar apellido paterno con menos de 25 caracteres");

            RuleFor(x => x.ApellidoMaterno)
                .NotEmpty()
                .WithMessage("Ingresar apellido materno");
            RuleFor(x => x.ApellidoMaterno)
                .Length(1, 25)
                .WithMessage("Ingresar apellido materno con menos de 25 caracteres");

            RuleFor(x => x.PrimerNombre)
                .NotEmpty()
                .WithMessage("Ingresar primer nombre");
            RuleFor(x => x.PrimerNombre)
                .Length(1, 25)
                .WithMessage("Ingresar primer nombre con menos de 25 caracteres");

            RuleFor(x => x.SegundoNombre)
                .Length(0, 25)
                .WithMessage("Ingresar primer nombre con menos de 25 caracteres");

            RuleFor(x => x.FechaNacimiento)
                .NotEmpty()
                .WithMessage("Ingresar fecha de nacimiento");
            //RuleFor(x => x.FechaNacimiento)
            //    .GreaterThan( DateTime.Now.AddYears(-18))
            //    .WithMessage("El postulante debe ser mayor de edad");

            //RuleFor(x => x.FechaNacimiento)
            //    .Must(BeAValidDate)
            //    .WithMessage("El Postulante debe ser mayor de edad");


            RuleFor(x => x.IndicadorSexo)
                .NotEmpty()
                .WithMessage("Ingresar sexo");
            RuleFor(x => x.IndicadorSexo)
                .NotEqual("0")
                .WithMessage("Ingresar sexo");
            
            RuleFor(x => x.TipoEstadoCivil)
                .NotEqual("00")
                .WithMessage("Ingresar Estado Civil");

            RuleFor(x => x.IdeUbigeo)
                .NotEqual(0)
                .WithMessage("Ingresar Distrito de residencia");

            RuleFor(x => x.Correo)
                .EmailAddress()
                .WithMessage("Ingresar Correo Valido");

            RuleFor(x => x.TipoVia)
                .NotEqual("00")
                .WithMessage("Ingresar Tipo Via");

            RuleFor(x => x.NumeroDireccion)
                .InclusiveBetween(0, 999999)
                .WithMessage("Ingresar Numero Valido");

            RuleFor(x => x.TipoSalario)
                .NotEqual("0")
                .WithMessage("Seleccionar  su espectativa de Sueldo Bruto");

            RuleFor(x => x.TipoDisponibilidadTrabajo)
                .NotEqual("0")
                .WithMessage("Seleccionar su Disponibilidad para trabajar");

            RuleFor(x => x.TipoDisponibilidadHorario)
                .NotEqual("0")
                .WithMessage("Seleccionar su Disponibilidad de horario");

            RuleFor(x => x.TipoComoSeEntero)
                .NotEmpty()
                .WithMessage("Seleccionar como se entero de la convocatoria");

            RuleFor(x => x.TelefonoMovil)
               .InclusiveBetween(900000000, 999999999)
               .WithMessage("Ingresar telefono movil válido");


            RuleFor(x => x.Observacion)
                .Length(0, 255)
                .WithMessage("La observacion no debe sobrepasar los 255 caracteres");


            RuleFor(x => x.ReferenciaDireccion)
                .NotEmpty()
                .WithMessage("Ingresar una referencia");
            RuleFor(x => x.ReferenciaDireccion)
                .Length(5,100)
                .WithMessage("Ingrese una referencia válida de hasta 100 caracteres");
            //RuleFor(x => x.TelefonoFijo)
            //    .Must()
            //    .withMessage("Ingresar un número telefonico válido");
        }
    }
}