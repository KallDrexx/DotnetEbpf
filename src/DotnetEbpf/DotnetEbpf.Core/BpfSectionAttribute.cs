using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;
using Mono.Cecil;

namespace DotnetEbpf.Core;

/// <summary>
/// Specifies that the method it is attached to should use the
/// `SEC` macro to define what section the method should reside in.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
public class BpfSectionAttribute(string section) : Attribute
{
    public class Mutator : IMethodConversionMutator, IFieldConversionMutator
    {
        public void Mutate(MethodConversionInfo conversionInfo, DotNetDefinedMethod method)
        {
            var attribute = method.Definition
                .CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == typeof(BpfSectionAttribute).FullName);

            if (attribute == null)
            {
                return;
            }

            var name = attribute.ConstructorArguments[0].Value.ToString();

            conversionInfo.AttributeText = $"SEC(\"{name}\")";
        }

        public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

        public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition field)
        {
            var attribute = field
                .CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == typeof(BpfSectionAttribute).FullName);

            if (attribute == null)
            {
                return;
            }

            var name = attribute.ConstructorArguments[0].Value.ToString();

            conversionInfo.AttributeText = $"SEC(\"{name}\")";
        }
    }
}