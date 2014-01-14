using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SanPablo.Reclutador.Entity;
using SanPablo.Reclutador.Entity.Validation;
using FluentValidation.Results;
using FluentValidation;

namespace SanPablo.Reclutador.Test.Model
{
    [TestClass]
    public class SedeValidarTest
    {
        [TestMethod]
        public void SedeValidator_ValidaEntidadValida()
        {
            Sede sede = GetSedeValida();

            SedeValidator sedeValidator = new SedeValidator();

            sedeValidator.Validate(sede, "CodigoSede", "DescripcionSede");

            ValidationResult validationResult = sedeValidator.Validate(sede);

            Assert.IsTrue(validationResult.IsValid);
        }


        [TestMethod]
        public void SedeValidator_CampoCodigo_NoAceptaValorNulo()
        {
            Sede sede = GetSedeValida();
            sede.CodigoSede = null;

            SedeValidator sedeValidator = new SedeValidator();
            ValidationResult validationResult = sedeValidator.Validate(sede);

            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void SedeValidator_CampoCodigo_NoAceptaValoresConMasDeDosCaracteres()
        {
            Sede sede = GetSedeValida();
            sede.CodigoSede = "000";

            SedeValidator sedeValidator = new SedeValidator();
            ValidationResult validationResult = sedeValidator.Validate(sede);

            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void SedeValidator_CampoDescripcion_NoAceptaValoresNulos()
        {
            Sede sede = GetSedeValida();
            sede.DescripcionSede = null;

            SedeValidator sedeValidator = new SedeValidator();
            ValidationResult validationResult = sedeValidator.Validate(sede);

            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void SedeValidator_CampoDescripcion_NoAceptaValoresVacios()
        {
            Sede sede = GetSedeValida();
            sede.DescripcionSede = "";

            SedeValidator sedeValidator = new SedeValidator();
            ValidationResult validationResult = sedeValidator.Validate(sede);

            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(2, validationResult.Errors.Count);
        }

        [TestMethod]
        public void SedeValidator_CampoDescripcion_NoAceptaValoresConMasDe400Caracteres()
        {
            Sede sede = GetSedeValida();
            sede.DescripcionSede = "a".PadLeft(401);

            SedeValidator sedeValidator = new SedeValidator();
            ValidationResult validationResult = sedeValidator.Validate(sede);

            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void SedeValidator_CampoEstado_NoAceptaValoresNulos()
        {
            Sede sede = GetSedeValida();
            sede.EstadoRegistro = null;

            SedeValidator sedeValidator = new SedeValidator();
            ValidationResult validationResult = sedeValidator.Validate(sede);

            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        [TestMethod]
        public void SedeValidator_CampoEstado_NoAceptaValoresConMasDe2Caracteres()
        {
            Sede sede = GetSedeValida();
            sede.EstadoRegistro = "AB";

            SedeValidator sedeValidator = new SedeValidator();
            ValidationResult validationResult = sedeValidator.Validate(sede);

            Assert.IsFalse(validationResult.IsValid);
            Assert.AreEqual(1, validationResult.Errors.Count);
        }

        private Sede GetSedeValida() 
        {
            return new Sede() 
            { 
                DescripcionSede = "descripcion dummy",
                EstadoRegistro = "A",
                CodigoSede = "01",
                FechaCreacion = new DateTime(2013,1,1)
            };
        }
    }
}
