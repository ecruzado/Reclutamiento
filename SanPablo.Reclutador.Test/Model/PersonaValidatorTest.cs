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
    public class PersonaValidatorTest
    {
        private PersonaValidator validator;
        
        [TestInitialize]
        public void Iniciar() 
        {
            validator = new PersonaValidator();
        }

        [TestMethod]
        public void TipoDocumento_no_puede_ser_nulo_o_vacio() 
        {
            
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoDocumento, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoDocumento, "");
        }

        [TestMethod]
        public void NumeroDocumento_no_puede_ser_nulo_o_vacio()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.NumeroDocumento, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.NumeroDocumento, "");
        }

        [TestMethod]
        public void NumeroDocumento_no_puede_aceptar_mas_de_8_caracteres()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.NumeroDocumento, "0".PadRight(9));
            validator
                .ShouldHaveValidationErrorFor(x => x.NumeroDocumento, "0".PadRight(7));
        }

        [TestMethod]
        public void ApellidoPaterno_no_puede_aceptar_nulo_o_vacio()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoPaterno, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoPaterno, "");
        }

        [TestMethod]
        public void ApellidoPaterno_no_puede_aceptar_mas_de_25_caracteres()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoPaterno, "a".PadRight(26));
        }

        [TestMethod]
        public void ApellidoMaterno_no_puede_aceptar_nulo_o_vacio()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoMaterno, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoMaterno, "");
        }

        [TestMethod]
        public void ApellidoMaterno_no_puede_aceptar_mas_de_25_caracteres()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.ApellidoMaterno, "a".PadRight(26));
        }

        [TestMethod]
        public void PrimerNombre_no_puede_aceptar_nulo_o_vacio()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.PrimerNombre, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.PrimerNombre, "");
        }

        [TestMethod]
        public void PrimerNombre_no_puede_aceptar_mas_de_25_caracteres()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.PrimerNombre, "a".PadRight(26));
        }

        [TestMethod]
        public void FechaNacimiento_no_puede_aceptar_valores_menores_a_01_01_1900()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.FechaNacimiento, DateTime.MinValue);
        }

        [TestMethod]
        public void Sexo_no_puede_aceptar_valores_nulos()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.IndicadorSexo, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.IndicadorSexo, string.Empty);
        }

        [TestMethod]
        public void TipoEstadoCivil_no_puede_aceptar_valores_nulos()
        {
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoEstadoCivil, null as string);
            validator
                .ShouldHaveValidationErrorFor(x => x.TipoEstadoCivil, string.Empty);
        }
    }
}
