using System;
using System.Runtime.Serialization;

namespace Windows2MonitorLibrary.Utility
{
    [Serializable]
    internal class MonitorReferencedNotFoundException : Exception
    {
        public MonitorReferencedNotFoundException()
        {
        }

        public MonitorReferencedNotFoundException(string message) : base(message)
        {
        }

        public MonitorReferencedNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MonitorReferencedNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}