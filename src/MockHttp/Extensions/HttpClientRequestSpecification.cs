using System;
using System.Net.Http;
using AutoFixture.Kernel;

namespace AutoFixture.Extensions
{
    public class HttpClientRequestSpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
        {
            return request is Type type && type == typeof(HttpClient);
        }
    }
}
