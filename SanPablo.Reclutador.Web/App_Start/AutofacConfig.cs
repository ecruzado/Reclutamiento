namespace SanPablo.Reclutador.Web.App_Start
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using FluentValidation.Internal;
    using FluentValidation.Mvc;
    using FluentValidation.Validators;
    using NHibernate;
    using SanPablo.Reclutador.Web.Controllers;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;

    public class AutofacConfig
    {
        public static void Config()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetAssembly(typeof(HomeController)));

            builder.Register(x => new NHibernateConfigurator().GetSessionFactory())
                .SingleInstance();
            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession())
                .As<ISession>().InstancePerHttpRequest();

            builder.RegisterModule(new RepositoryComponentModule());
            builder.RegisterModule(new ValidatorComponentModule());

            builder.RegisterModule(new AutofacWebTypesModule());
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            // Set up the FluentValidation provider factory and add it as a Model validator
            var fluentValidationModelValidatorProvider = new FluentValidationModelValidatorProvider(new AutofacValidatorFactory(container));
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            fluentValidationModelValidatorProvider.AddImplicitRequiredValidator = false;
            ModelValidatorProviders.Providers.Add(fluentValidationModelValidatorProvider);
            fluentValidationModelValidatorProvider.Add(typeof(NotEqualValidator), (metadata, context, description, validator) => new NotEqualPropertyValidator(metadata, context, description, validator));
            fluentValidationModelValidatorProvider.Add(typeof(LessThanOrEqualValidator), (metadata, context, rule, validator) => new LessThanOrEqualToFluentValidationPropertyValidator(metadata, context, rule, validator));
        }
    }

    public class NotEqualPropertyValidator : FluentValidationPropertyValidator
    {

        public NotEqualPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator)
            : base(metadata, controllerContext, rule, validator)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules()) yield break;

            var formatter = new MessageFormatter().AppendPropertyName(Rule.PropertyName);
            string message = formatter.BuildMessage(Validator.ErrorMessageSource.GetString());
            var rule = new ModelClientValidationRule
            {
                ValidationType = "notequal",
                ErrorMessage = message
            };
            rule.ValidationParameters["field"] = String.Format("{0}", ((NotEqualValidator)Validator).ValueToCompare);
            //rule.ValidationParameters["field"] = "0";
            yield return rule;
        }
    }


    public class LessThanOrEqualToFluentValidationPropertyValidator : FluentValidationPropertyValidator
    {
        public LessThanOrEqualToFluentValidationPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator)
            : base(metadata, controllerContext, rule, validator)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!this.ShouldGenerateClientSideRules())
            {
                yield break;
            }

            var validator = Validator as LessThanOrEqualValidator;

            var errorMessage = new MessageFormatter()
                .AppendPropertyName(this.Rule.GetDisplayName())
                .BuildMessage(validator.ErrorMessageSource.GetString());

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = errorMessage,
                ValidationType = "lessthanorequaldate"
            };
            rule.ValidationParameters["other"] = CompareAttribute.FormatPropertyForClientValidation(validator.MemberToCompare.Name);
            yield return rule;
        }
    }
}