using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Expressions;
using Mono.Cecil;

namespace DotnetEbpf.Core;

[AttributeUsage(AttributeTargets.Field)]
public class BpfLicenseAttribute(string license) : Attribute
{
    public const string DualBsdGpl = "Dual BSD/GPL";

    public class Mutator : IFieldConversionMutator
    {
        private readonly ConversionCatalog _conversionCatalog;

        public Mutator(ConversionCatalog conversionCatalog)
        {
            _conversionCatalog = conversionCatalog;
        }

        public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>([
            new IlTypeName(typeof(char).FullName!),
        ]);

        public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition field)
        {
            var attribute = field
                .CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == typeof(BpfLicenseAttribute).FullName);

            if (attribute == null)
            {
                return;
            }
            
            var license = attribute.ConstructorArguments[0].Value.ToString();
            conversionInfo.AttributeText = "SEC(\"license\")";
            conversionInfo.StaticItemSize = license!.Length + 1; // +1 for null terminator

            var returnType = _conversionCatalog.Find(new IlTypeName(typeof(char).FullName!));
            conversionInfo.FieldTypeConversionInfo = returnType;
            
            var expression = new LiteralValueExpression($"\"{license}\"", returnType);
            conversionInfo.InitialValue = expression;
        }
    }
}