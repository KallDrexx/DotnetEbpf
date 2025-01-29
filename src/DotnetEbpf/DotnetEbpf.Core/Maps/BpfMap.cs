using Dntc.Attributes;
using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Dntc.Common.Definitions;
using Dntc.Common.Definitions.Definers;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace DotnetEbpf.Core.Maps;

public struct BpfMap
{
    public class FieldDefiner : IDotNetFieldDefiner
    {
        private readonly DefinitionCatalog _definitionCatalog;

        public FieldDefiner(DefinitionCatalog definitionCatalog)
        {
            _definitionCatalog = definitionCatalog;
        }

        public DefinedField? Define(FieldDefinition fieldDefinition)
        {
            if (fieldDefinition.FieldType.FullName != typeof(BpfMap).FullName)
            {
                return null;
            }

            var fieldType = BpfMapCustomDefinedType.CreateForField(fieldDefinition);
            if (fieldType == null)
            {
                return null;
            }

            var fieldName = fieldDefinition.Name;
            var customNameAttribute = fieldDefinition.CustomAttributes
                .SingleOrDefault(x => x.AttributeType.FullName == typeof(CustomFieldNameAttribute).FullName);

            if (customNameAttribute != null)
            {
                fieldName = customNameAttribute.ConstructorArguments[0].Value.ToString()!;
            }
            
            _definitionCatalog.Add([fieldType]);

            return new BpfMapField(
                fieldDefinition,
                fieldType.HeaderName,
                fieldType.SourceFileName,
                new CFieldName(Utils.MakeValidCName(fieldName)),
                new IlFieldId(fieldDefinition.FullName),
                fieldType,
                true,
                []);
        }

        private class BpfMapField : CustomDefinedField
        {
            private readonly BpfMapCustomDefinedType _fieldType;

            public BpfMapField(
                FieldDefinition? originalField,
                HeaderName? declaredInHeader,
                CSourceFileName? declaredInSourceFileName,
                CFieldName nativeName,
                IlFieldId name,
                BpfMapCustomDefinedType fieldType,
                bool isGlobal,
                IReadOnlyList<HeaderName>? referencedHeaders = null)
                : base(originalField, declaredInHeader, declaredInSourceFileName, nativeName, name, fieldType.IlName,
                    isGlobal, referencedHeaders)
            {
                _fieldType = fieldType;
                
                
            }

            public override CustomCodeStatementSet GetCustomDeclaration()
            {
                return new CustomCodeStatementSet($"{_fieldType.NativeName} {NativeName}");
            }
        }
    }
    
    public class FieldConversionMutator : IFieldConversionMutator
    {
        public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();
        
        public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition fieldDefinition)
        {
            if (!fieldDefinition.IsStatic || fieldDefinition.FieldType.FullName != typeof(BpfMap).FullName)
            {
                return;
            }

            conversionInfo.AttributeText = "SEC(\".maps\")";
        }
    }
}