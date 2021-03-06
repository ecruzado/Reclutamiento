﻿namespace SanPablo.Reclutador.Entity.Validation
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
                .WithMessage("Ingresar sólo 8 digitos");

            RuleFor(x => x.ApellidoPaterno)
                .NotEmpty()
                .WithMessage ("Ingresar apellido paterno");
            RuleFor(x => x.ApellidoPaterno)
                .Length(1,25)
                .WithMessage("Máx. 25 caracteres");

            RuleFor(x => x.ApellidoMaterno)
                .NotEmpty()
                .WithMessage("Ingresar apellido materno");
            RuleFor(x => x.ApellidoMaterno)
                .Length(1, 25)
                .WithMessage("Máx. 25 caracteres");

            RuleFor(x => x.PrimerNombre)
                .NotEmpty()
                .WithMessage("Ingresar primer nombre");
            RuleFor(x => x.PrimerNombre)
                .Length(1, 25)
                .WithMessage("Máx. 25 caracteres");

            RuleFor(x => x.SegundoNombre)
                .Length(0, 25)
                .WithMessage("Máx. 25 caracteres");

            //RuleFor(x => x.FechaNacimiento)
            //    .NotEmpty()
            //    .WithMessage("Ingresar fecha de nacimiento");


            //RuleFor(x => x.FechaNacimiento)
            //    .LessThanOrEqualTo(DateTime.Now.AddYears(-18)).When(x=>x.FechaNacimiento != null)
            //    .WithMessage("Debes ser mayor de edad");

            //RuleFor(x => x.FechaNacimiento)
            //    .Must(BeAValidDate)
            //    .WithMessage("El Postulante debe ser mayor de edad");

            RuleFor(x => x.NombreVia)
                .Length(0, 100)
                .WithMessage("Máx. 100 caracteres");

            RuleFor(x => x.IndicadorSexo)
                .NotEmpty()
                .WithMessage("Ingresar sexo");
            RuleFor(x => x.IndicadorSexo)
                .NotEqual("0")
                .WithMessage("Ingresar sexo");
            
            RuleFor(x => x.TipoEstadoCivil)
                .NotEqual("0")
                .WithMessage("Ingresar Estado Civil");


            RuleFor(x => x.Pais)
                .NotEqual(9000)
                .WithMessage("Ingresar país de residencia");

            RuleFor(x => x.Departamento)
                .NotEqual(0)
                .WithMessage("Ingresar departamento de residencia");

            RuleFor(x => x.Provincia)
                .NotEqual(0)
                .WithMessage("Ingresar provincia de residencia");

            RuleFor(x => x.IdeUbigeo)
                .NotEqual(0)
                .WithMessage("Ingresar distrito de residencia");

            RuleFor(x => x.Correo)
                .EmailAddress()
                .WithMessage("Ingresar correo válido");

            RuleFor(x => x.TipoVia)
                .NotEqual("0")
                .WithMessage("Ingresar Tipo Via");

            RuleFor(x => x.NumeroDireccion)
                .InclusiveBetween(0, 999999)
                .WithMessage("Ingresar número válido");

            RuleFor(x => x.TipoSalario)
                .NotEqual("0")
                .WithMessage("Seleccionar su sueldo bruto");

            RuleFor(x => x.TipoDisponibilidadTrabajo)
                .NotEqual("0")
                .WithMessage("Seleccionar su disponibilidad para trabajar");

            RuleFor(x => x.TipoDisponibilidadHorario)
                .NotEqual("0")
                .WithMessage("Seleccionar su disponibilidad de horario");

            RuleFor(x => x.TipoComoSeEntero)
                .NotEmpty()
                .WithMessage("Seleccionar como se enteró de la convocatoria");

            RuleFor(x => x.TelefonoMovil)
               .InclusiveBetween(900000000, 999999999)
               .WithMessage("Ingresar teléfono movil válido");


            RuleFor(x => x.TelefonoFijo)
                .NotEmpty().When(x => x.TelefonoMovil == null)
                .WithMessage("Ingresar un numero de contacto movil y/o fijo");

            RuleFor(x => x.Observacion)
                .NotEmpty()
                .WithMessage("Ingresar descripción de Perfil");
            RuleFor(x => x.Observacion)
                .Length(0, 1000)
                .WithMessage("Máx. 1000 caracteres");


            RuleFor(x => x.ReferenciaDireccion)
                .NotEmpty()
                .WithMessage("Ingresar una referencia");
            RuleFor(x => x.ReferenciaDireccion)
                .Length(1,100)
                .WithMessage("Máx. 100 caracteres");

            RuleFor(x => x.NombreZona)
                .Length(0, 100)
                .WithMessage("Máx. 100 caracteres");


            RuleFor(x => x.TipoParienteSede)
                .NotEqual("0").When(x => x.IndicadorParientesCHSP==null?false:x.IndicadorParientesCHSP.Equals("S"))
                .WithMessage("Ingresar la Sede en la que labora");

            RuleFor(x => x.ParienteNombre)
                .NotEmpty().When(x => x.IndicadorParientesCHSP == null ? false : x.IndicadorParientesCHSP.Equals("S"))
                .WithMessage("Ingresar nombre del familiar");

            //RuleFor(x => x.ParienteNombre)
            //    .Length(0,100)
            //    .WithMessage("Máx. 100 caracteres");

            RuleFor(x => x.ParienteCargo)
                .NotEmpty().When(x => x.IndicadorParientesCHSP == null ? false : x.IndicadorParientesCHSP.Equals("S"))
                .WithMessage("Ingresar cargo del familiar");

            //RuleFor(x => x.ParienteCargo)
            //    .Length(0,50)
            //    .WithMessage("Máx. 50 caracteres");

            RuleFor(x => x.DescripcionOtroMedio)
                .NotEmpty().When(x => x.TipoComoSeEntero == null ? false : x.TipoComoSeEntero.Equals("04"))
                .WithMessage("Ingresar descripción");

            //RuleFor(x => x.ParienteCargo)
            //    .Length(1, 50)
            //    .WithMessage("Máx. 50 caracteres");

        }
    }
}