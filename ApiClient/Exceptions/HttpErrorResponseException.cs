using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ApiClient.Exceptions
{
    public class HttpErrorResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public HttpErrorResponseException(string exMessage, HttpStatusCode statusCode)
            : base(exMessage)
        {
            StatusCode = statusCode;
        }
    }
}
