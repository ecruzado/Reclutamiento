using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Entity.Validation
{
    public class SedeValidator : AbstractValidator<Sede>
    {
        public SedeValidator()
        {
            RuleFor(s => s.CodigoSede).NotEmpty();
            RuleFor(s => s.CodigoSede)
                .Length(1, 2)
                .WithMessage("Codigo acepta un máximo de 2 caracteres");
            RuleFor(s => s.DescripcionSede)
                .NotEmpty()
                .WithMessage("Ingresar descripción");
            RuleFor(s => s.DescripcionSede)
                .Length(1, 400)
                .WithMessage("Descripción acepta un máximo de 400 caracteres");
            RuleFor(s => s.EstadoRegistro)
                .NotNull()
                .WithMessage("Ingresar estado");
            RuleFor(s => s.EstadoRegistro)
                .Length(1)
                .WithMessage("Estado acepta 1 caracter");
            RuleFor(s => s.FechaCreacion)
                .NotNull()
                .WithMessage("Ingresar fecha creación");
            RuleFor(s => s.FechaCreacion)
                .NotEqual(DateTime.MinValue)
                .WithMessage("Ingresar fecha de creación valida");
        }
    }
}