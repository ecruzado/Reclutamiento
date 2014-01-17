namespace SanPablo.Reclutador.Test.Model
{
    using FluentValidation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Entity.Validation;
    using FluentValidation.TestHelper;
    using System;
    using FluentValidation.Results;

    [TestClass]
    public class EstudioPostulanteValidatorTest
    {
        private EstudioPostulanteValidator validator;

        [TestInitialize]
        public void Iniciar()
        {
            validator = new EstudioPostulanteValidator();
        }

        [TestMethod]
        public void Tipo_de_tipo_institucion_no_puede_ser_nulo()
        {

           validator
                .ShouldHaveValidationErrorFor(x => x.TipTipoInstitucion, "00");
        }

        [TestMethod]
        public void Tipo_nombre_institucion_no_puede_ser_cero()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoNombreInstitucion, "00");
        }

        [TestMethod]
        public void Tipo_educacion_no_puede_ser_cero()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoEducacion, "00");
        }

        [TestMethod]
        public void Tipo_nivel_alcanzado_no_puede_ser_cero()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoNivelAlcanzado, "00");
        }

        [TestMethod]
        public void Fecha_estudio_inicio_no_puede_fecha_menor_1980_01_01()
        {
             validator
                .ShouldHaveValidationErrorFor(x => x.FechaEstudioInicio, DateTime.MinValue);
        }
    }
}
