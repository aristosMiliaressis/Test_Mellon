using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClient.Exceptions
{
    public class TransferErrorException : Exception
    {
        public TransferErrorException(Exception innerEx)
            : base("Error occurred while sending request!", innerEx)
        { }

        public TransferErrorException(string exMessage, Exception innerEx)
           : base(exMessage, innerEx)
        { }
    }
}
