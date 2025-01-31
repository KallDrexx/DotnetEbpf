using System.Text;
using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;

namespace DotnetEbpf.Core.Maps;

public class BpfMapCustomDefinedType : CustomDefinedType
{
    private static readonly Dictionary<IlTypeName, BpfMapCustomDefinedType> BpfMaps = new();
    private MapType? _mapType;
    private int? _maxEntries;
    
    private BpfMapCustomDefinedType(
        IlTypeName ilTypeName, 
        HeaderName? headerName, 
        CSourceFileName? sourceFileName, 
        CTypeName nativeName, 
        IReadOnlyList<IlTypeName> otherReferencedTypes, 
        IReadOnlyList<HeaderName> referencedHeaders) 
        : base(ilTypeName, headerName, sourceFileName, nativeName, otherReferencedTypes, referencedHeaders)
    {
    }

    public static BpfMapCustomDefinedType? CreateForField(FieldDefinition field)
    {
        if (!field.IsStatic || field.FieldType.FullName != typeof(BpfMap).FullName)
        {
            // Only global that are of type `BpfMap` are valid
            return null;
        }

        // Every global that's declared as a map needs its own unique type id
        var typeId = new IlTypeName($"{field.DeclaringType.FullName}_{field.Name}");
        var mapType = BpfMaps.GetValueOrDefault(typeId);
        if (mapType != null)
        {
            return mapType;
        }

        var fieldNamespace = Utils.GetNamespace(field.DeclaringType);
        mapType = new BpfMapCustomDefinedType(
            typeId,
            Utils.GetHeaderName(fieldNamespace),
            null,
            new CTypeName(Utils.MakeValidCName(typeId.Value)),
            [],
            [new HeaderName("vmlinux.h")]);
        
        mapType.AssignTypeAttribute(field);
        mapType.AssignMaxEntriesAttribute(field);
        BpfMaps.Add(typeId, mapType);

        return mapType;
    }

    public override CustomCodeStatementSet? GetCustomTypeDeclaration(ConversionCatalog catalog)
    {
        var content = new StringBuilder();
        content.AppendLine("typedef struct {");

        if (_mapType != null)
        {
            content.AppendLine($"\t__uint(type, {_mapType.Value.ToAttributeString()}");
        }

        if (_maxEntries != null)
        {
            content.AppendLine($"\t__uint(max_entries, {_maxEntries.Value}");
        }

        content.AppendLine($"}} {NativeName};");

        return new CustomCodeStatementSet(content.ToString());
    }

    private void AssignTypeAttribute(FieldDefinition fieldDefinition)
    {
        var attribute = fieldDefinition.CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(MapTypeAttribute).FullName);

        if (attribute == null)
        {
            return;
        }

        var type = (MapType)attribute.ConstructorArguments[0].Value;
        _mapType = type;
    }

    private void AssignMaxEntriesAttribute(FieldDefinition fieldDefinition)
    {
        var attribute = fieldDefinition.CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(MaxEntriesAttribute).FullName);

        if (attribute == null)
        {
            return;
        }

        var count = (int)attribute.ConstructorArguments[0].Value;
        _maxEntries = count;
    }
}