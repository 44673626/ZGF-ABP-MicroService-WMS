using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Win.Utils
{
    public class BranchHeaderParameter : IOperationFilter
    {
        private readonly string _defaultValue;

        protected BranchHeaderParameter()
        {

        }

        public BranchHeaderParameter(string defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            if (context.MethodInfo.GetCustomAttribute(typeof(AddBranchIdHeaderAttribute)) is
                AddBranchIdHeaderAttribute attribute)
            {
                var existingParam = operation.Parameters.FirstOrDefault(p =>
                    p.In == ParameterLocation.Header && p.Name == attribute.HeaderName);
                if (existingParam != null) // remove description from [FromHeader] argument attribute
                {
                    operation.Parameters.Remove(existingParam);
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = string.IsNullOrEmpty(attribute.HeaderName)
                        ? BranchHeaderConsts.HeaderName
                        : attribute.HeaderName,
                    In = ParameterLocation.Header,
                    Description = string.IsNullOrEmpty(attribute.Description)
                        ? BranchHeaderConsts.HeaderDescription
                        : attribute.Description,
                    Required = attribute.IsRequired,
                    Schema = string.IsNullOrEmpty(attribute.DefaultValue)
                        ? string.IsNullOrEmpty(_defaultValue)
                            ? null
                            : new OpenApiSchema
                            {
                                Type = "String",
                                Default = new OpenApiString(_defaultValue)
                            }
                        : new OpenApiSchema
                        {
                            Type = "String",
                            Default = new OpenApiString(attribute.DefaultValue)
                        }
                });
            }
            else
            {
                var existingParam = operation.Parameters.FirstOrDefault(p =>
                    p.In == ParameterLocation.Header && p.Name == BranchHeaderConsts.HeaderName);
                if (existingParam != null) // remove description from [FromHeader] argument attribute
                {
                    operation.Parameters.Remove(existingParam);
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name =  BranchHeaderConsts.HeaderName,
                    In = ParameterLocation.Header,
                    Description =  BranchHeaderConsts.HeaderDescription,
                    Required = false,
                    Schema =  new OpenApiSchema
                    {
                        Type = "String",
                        Default = new OpenApiString(_defaultValue)
                    }
                });
            }

        }
    }
}