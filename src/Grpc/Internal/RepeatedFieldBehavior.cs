using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Kralizek.AutoFixture.Extensions.Internal;

public class RepeatedFieldBehavior : ISpecimenBuilderTransformation
{
    public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
    {
        return new Postprocessor(builder, new DelegatingSpecimenCommand(FillRepeatedFields));
    }

    private void FillRepeatedFields(object? specimen, ISpecimenContext context)
    {
        if (specimen is not IMessage message)
            return;

        foreach (var prop in specimen.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = prop.GetValue(specimen);
            if (value is null) continue;

            var type = value.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(RepeatedField<>))
            {
                // Skip if already has items
                var enumerable = (IEnumerable)value;
                if (enumerable.Cast<object>().Any())
                    continue;

                var elementType = type.GetGenericArguments()[0];
                var addRange = type.GetMethod("AddRange", [typeof(IEnumerable<>).MakeGenericType(elementType)])
                                ?? type.GetMethod("AddRange", Type.EmptyTypes); // fallback
                if (addRange is null)
                    continue;

                // Ask AutoFixture for an array of T
                var arrayType = elementType.MakeArrayType();
                var items = context.Resolve(arrayType);
                addRange.Invoke(value, [items]);
            }
        }
    }
}
