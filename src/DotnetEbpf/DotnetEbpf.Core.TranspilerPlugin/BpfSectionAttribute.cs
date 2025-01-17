using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;

namespace DotnetEbpf.Core.TranspilerPlugin;

/// <summary>
/// Specifies that the method it is attached to should use the
/// `SEC` macro to define what section the method should reside in.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
public class BpfSectionAttribute(string section) : Attribute
{
    public class Mutator : IMethodConversionMutator, IGlobalConversionMutator
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

        public void Mutate(GlobalConversionInfo conversionInfo, DotNetDefinedGlobal global)
        {
            var attribute = global.Definition
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