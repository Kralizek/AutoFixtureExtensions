using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Kralizek.AutoFixture.Extensions.Internal;

public class MapFieldBehavior : ISpecimenBuilderTransformation
{
    public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
    {
        return new Postprocessor(builder, new DelegatingSpecimenCommand(FillMapFields));
    }

    private void FillMapFields(object? specimen, ISpecimenContext context)
    {
        if (specimen is not IMessage message)
            return;

        foreach (var prop in specimen.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = prop.GetValue(specimen);
            if (value is null)
                continue;

            var type = value.GetType();
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(MapField<,>))
                continue;

            var args = type.GetGenericArguments();
            if (args[0] != typeof(string))
                continue;

            // Skip already filled maps
            if (((IEnumerable)value).Cast<object>().Any())
                continue;

            var valueType = args[1];
            var dictType = typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType);
            var addMethod = type.GetMethod("Add", [typeof(IDictionary<,>).MakeGenericType(typeof(string), valueType)]);

            if (addMethod == null)
                continue;

            var dict = context.Resolve(dictType);

            var countProp = dict?.GetType().GetProperty("Count");
            if (dict is null || (int)(countProp?.GetValue(dict) ?? 0) == 0)
                continue;

            addMethod.Invoke(value, [dict]);
        }
    }
}