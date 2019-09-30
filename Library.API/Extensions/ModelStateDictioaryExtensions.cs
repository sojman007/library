using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Library.API.Extensions
{
    public static class ModelStateDictioaryExtensions
    {
        public static IEnumerable<string> GetErrorsAsList(this ModelStateDictionary modelState)
        {
            if (modelState == null || !modelState.Values.Any())
                return new List<string>();

            var errors = modelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage));
            var error = errors.Where(x => !string.IsNullOrWhiteSpace(x));

            if (!error.Any())
                error = modelState.Values.SelectMany(x => x.Errors.Select(y => y.Exception.Message));

            return error;
        }
    }
}
