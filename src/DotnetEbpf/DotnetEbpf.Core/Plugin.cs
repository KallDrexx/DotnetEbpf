using Dntc.Common;
using Dntc.Common.Definitions;
using DotnetEbpf.Core.Maps;

namespace DotnetEbpf.Core;

public class Plugin : ITranspilerPlugin
{
    public bool BypassBuiltInNativeDefinitions => true;
    
    public void Customize(TranspilerContext context)
    {
        AddNativeTypes(context);
        context.Definers.Add(new BpfMap.FieldDefiner(context.DefinitionCatalog));
        context.ConversionInfoCreator.AddMethodMutator(new BpfSectionAttribute.Mutator());
        context.ConversionInfoCreator.AddFieldMutator(new BpfSectionAttribute.Mutator());
        context.ConversionInfoCreator.AddFieldMutator(new BpfLicenseAttribute.Mutator(context.ConversionCatalog));
        context.ConversionInfoCreator.AddFieldMutator(new BpfMap.FieldConversionMutator());
        context.ConversionInfoCreator.AddFieldMutator(new BpfArrayAttribute.Mutator(context.ConversionCatalog));
    }

    private static void AddNativeTypes(TranspilerContext context)
    {
        context.DefinitionCatalog.Add([
            new NativeDefinedType(
                new IlTypeName(typeof(int).FullName!),
                new HeaderName("vmlinux.h"),
                new CTypeName("__s32"),
                []),
            
            new NativeDefinedType(
                new IlTypeName(typeof(uint).FullName!),
                new HeaderName("vmlinux.h"),
                new CTypeName("__u32"),
                []),
            
            new NativeDefinedType(
                new IlTypeName(typeof(long).FullName!),
                new HeaderName("vmlinux.h"),
                new CTypeName("__s64"),
                []),
            
            new NativeDefinedType(
                new IlTypeName(typeof(char).FullName!),
                null,
                new CTypeName("char"),
                []),
            
            new NativeDefinedType(
                new IlTypeName(typeof(string).FullName!),
                null,
                new CTypeName("char*"),
                []),
            
            new NativeDefinedType(
                new IlTypeName(typeof(void).FullName!),
                null,
                new CTypeName("void"),
                []),
            
            new NativeDefinedType(
                new IlTypeName(typeof(sbyte).FullName!),
                null,
                new CTypeName("__s8"),
                []),

            new NativeDefinedType(
                new IlTypeName(typeof(byte).FullName!),
                null,
                new CTypeName("__u8"),
                []),
                
            new NativeDefinedType(
                new IlTypeName(typeof(bool).FullName!),
                null,
                new CTypeName("bool"),
                []),

            new NativeDefinedType(
                new IlTypeName(typeof(ulong).FullName!),
                null,
                new CTypeName("__u64"),
                []),

            new NativeDefinedType(
                new IlTypeName(typeof(Type).FullName!),
                null,
                new CTypeName("<invalid>"), // just needs to exist for typeof() support
                []),
        ]);
    }
}