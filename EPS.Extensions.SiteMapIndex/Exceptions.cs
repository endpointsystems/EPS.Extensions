using System;

namespace EPS.Extensions.SiteMapIndex
{
    public class SiteMapException: ApplicationException
    {
        public SiteMapException(string message) : base(message){}
        public SiteMapException(string message, Exception innerException): base(message,innerException){}
        public SiteMapException()
        {}
    }
}
