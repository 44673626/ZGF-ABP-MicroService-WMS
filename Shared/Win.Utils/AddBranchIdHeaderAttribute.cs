using System;

namespace Win.Utils
{
    public class AddBranchIdHeaderAttribute : Attribute
    {
        public string HeaderName { get; }
        public string Description { get; }
        public string DefaultValue { get; }
        public bool IsRequired { get; }

        public AddBranchIdHeaderAttribute(bool isRequired = false,string headerName=null, string description = null, string defaultValue = null)
        {
            HeaderName = headerName;
            Description = description;
            DefaultValue = defaultValue;
            IsRequired = isRequired;
        }

        

    }
}