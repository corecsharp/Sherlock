using FluentValidation.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System;

namespace Sherlock.Framework.Web.Validation
{
    public abstract class FluentValidationClientModelValidator<TValidator> : IClientModelValidator
        where TValidator : IPropertyValidator
    {

        public FluentValidationClientModelValidator(TValidator validator)
        {
            this.Validator = validator;
        }

        protected TValidator Validator { get; }

        public abstract void AddValidation(ClientModelValidationContext context);
        
        protected virtual string GetErrorMessage(ModelMetadata modelMetadata)
        {
            return this.Validator.ErrorMessageSource.GetString(null);
        }

    }
}
