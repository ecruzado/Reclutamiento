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
    public class ConocimientoGeneralPostulanteValidatorTest
    {
        private ConocimientoGeneralPostulanteValidator validator;

        [TestInitialize]
        public void Iniciar()
        {
            validator = new ConocimientoGeneralPostulanteValidator();
        }

        [TestMethod]
        public void Tipo_de_tipo_conocimiento_ofimatica_no_puede_ser_cero()
        {
            validator
                 .ShouldHaveValidationErrorFor(x => x.TipoConocimientoOfimatica, "00");
        }

        [TestMethod]
        public void Tipo_nombre_ofimatica_no_puede_ser_cero()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoNombreOfimatica, "00");
        }

        [TestMethod]
        public void Tipo_nivel_alcanzado_no_puede_ser_cero()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoNivelConocimiento, "00");
        }
    }
}
