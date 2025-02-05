using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions.CustomDefinedTypes;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace DotnetEbpf.Core;

/// <summary>
/// Specifies that the field is a statically sized array with a size known at compile time.
/// </summary>
/// <param name="numberOfElements"></param>
[AttributeUsage(AttributeTargets.Field)]
public class BpfArrayAttribute(int numberOfElements) : Attribute
{
    public class Mutator : IFieldConversionMutator
    {
        private readonly ConversionCatalog _conversionCatalog;

        public Mutator(ConversionCatalog conversionCatalog)
        {
            _conversionCatalog = conversionCatalog;
        }

        public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

        public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition field)
        {
            var attribute = field
                .CustomAttributes
                .SingleOrDefault(x => x.AttributeType.FullName == typeof(BpfArrayAttribute).FullName);

            if (attribute == null)
            {
                return;
            }

            var fieldType = field.FieldType;
            if (!fieldType.IsArray)
            {
                var message = $"BpfArrayAttribute was attached to the field {field.FullName} but " +
                              $"its field type is not an array type";

                throw new InvalidOperationException(message);
            }

            if (!attribute.HasConstructorArguments || attribute.ConstructorArguments[0].Value is not int size)
            {
                var message = $"Field {field.FullName}'s BpfArrayAttribute constructor did not have a " +
                              $"single integer size value";

                throw new InvalidOperationException(message);
            }

            // We need to get the derived type info of the array's element type, so we can
            // accurately tell the array's type conversion info what it's native name is. This
            // *should* not be a race condition because the dependency graph should have ensured
            // that this field's type info is added to the conversion catalog before the field itself.
            var elementType = new IlTypeName(fieldType.GetElementType().FullName);
            var elementTypeInfo = _conversionCatalog.Find(elementType);

            var sizeType = new IlTypeName(typeof(int).FullName!); // TODO: Figure out a way to make this configurable.
            var definedType = new DefinedType(fieldType, elementTypeInfo, size, sizeType);
            var fieldTypeInfo = new TypeConversionInfo(definedType, false);

            // Replace the field's current conversion info with one based on a statically sized defined type
            conversionInfo.FieldTypeConversionInfo = fieldTypeInfo;
            conversionInfo.StaticItemSize = size;
        }
    }
    
    private class DefinedType : StaticallySizedArrayDefinedType
    {
        public DefinedType(
            TypeReference arrayType,
            TypeConversionInfo elementTypeInfo,
            int size,
            IlTypeName sizeType)
            : base(arrayType, elementTypeInfo, size, sizeType)
        {
        }

        public override CStatementSet GetLengthCheckExpression(
            CBaseExpression arrayLengthField,
            CBaseExpression arrayInstance,
            DereferencedValueExpression index)
        {
            // Return no code for the length check, as ebpf programs can't abort,
            // and array checks are statically checked by the compiler.
            return new CustomCodeStatementSet("");
        }
    }
}