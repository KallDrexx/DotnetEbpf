using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Expressions;

namespace DotnetEbpf.Core.TranspilerPlugin;

[AttributeUsage(AttributeTargets.Field)]
public class BpfLicenseAttribute(string license) : Attribute
{
    public const string DualBsdGpl = "Dual BSD/GPL";

    public class Mutator : IGlobalConversionMutator
    {
        private readonly ConversionCatalog _conversionCatalog;

        public Mutator(ConversionCatalog conversionCatalog)
        {
            _conversionCatalog = conversionCatalog;
        }

        public void Mutate(GlobalConversionInfo conversionInfo, DotNetDefinedGlobal global)
        {
            var attribute = global.Definition
                .CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == typeof(BpfLicenseAttribute).FullName);

            if (attribute == null)
            {
                return;
            }

            conversionInfo.IsNonPointerString = true;
            conversionInfo.AttributeText = "SEC(\"license\")";

            var license = attribute.ConstructorArguments[0].Value.ToString();
            var returnType = _conversionCatalog.Find(new IlTypeName(typeof(string).FullName!));
            var expression = new LiteralValueExpression($"\"{license}\"", returnType);
            conversionInfo.InitialValue = expression;
        }
    }
}