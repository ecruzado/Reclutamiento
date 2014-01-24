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
    public class ParientePostulanteValidatorTest
    {
        private ParientePostulanteValidator validator;

        [TestInitialize]
        public void Iniciar()
        {
            validator = new ParientePostulanteValidator();
        }

        [TestMethod]
        public void Apellido_Paterno_no_puede_ser_nulo_o_vacio()
        {

            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoPaterno, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoPaterno, "");
        }

        [TestMethod]
        public void ApellidoPaterno_no_puede_aceptar_mas_de_50_caracteres()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoPaterno, "a".PadRight(50));
        }


        [TestMethod]
        public void Apellido_Materno_no_puede_ser_nulo_o_vacio()
        {

            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoMaterno, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoMaterno, "");
        }

        [TestMethod]
        public void ApellidoMaterno_no_puede_aceptar_mas_de_50_caracteres()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoMaterno, "a".PadRight(50));
        }

        [TestMethod]
        public void Nombres_no_puede_ser_nulo_o_vacio()
        {

            validator
                .ShouldHaveValidationErrorFor(x => x.Nombres, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.Nombres, "");
        }

        [TestMethod]
        public void Nombres_no_puede_aceptar_mas_de_50_caracteres()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.Nombres, "a".PadRight(50));
        }

        
        [TestMethod]
        public void FechaNacimiento_no_puede_aceptar_valores_menores_a_01_01_1900()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.FechaNacimiento, DateTime.MinValue);
        }


        [TestMethod]
        public void Tipo_Vinculo_no_puede_aceptar_valores_nulos()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoDeVinculo, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoDeVinculo, string.Empty);
        }
    }
}
