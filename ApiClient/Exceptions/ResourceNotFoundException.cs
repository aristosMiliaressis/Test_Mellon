using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClient.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public string Resource { get; set; }

        public ResourceNotFoundException() { }

        public ResourceNotFoundException(string resource)
            : base("Resource not found!")
        {
            Resource = resource;
        }
    }
}
