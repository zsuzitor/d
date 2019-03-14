using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace dip.Models.CustomException
{
    public class NotFoundException:Exception
    {
        public NotFoundException():base() {
        }
        public NotFoundException(string message) : base(message)
        {

        }
        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
        public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}