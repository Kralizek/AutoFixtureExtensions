using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public class TypeWithProperty<T>
    {
        public required T Value { get; set;}
    }
}