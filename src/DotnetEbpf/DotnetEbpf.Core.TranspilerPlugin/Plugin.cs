using System.Reflection.Metadata;
using Dntc.Common;
using Dntc.Common.Definitions;

namespace DotnetEbpf.Core.TranspilerPlugin;

public class Plugin : ITranspilerPlugin
{
    public bool BypassBuiltInNativeDefinitions => true;
    
    public void Customize(TranspilerContext context)
    {
        AddNativeTypes(context);
        context.ConversionInfoCreator.AddMethodMutator(new BpfSectionAttribute.Mutator());
        context.ConversionInfoCreator.AddGlobalMutator(new BpfSectionAttribute.Mutator());
        context.ConversionInfoCreator.AddGlobalMutator(new BpfLicenseAttribute.Mutator(context.ConversionCatalog));
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
                new IlTypeName(typeof(bool).FullName!),
                null,
                new CTypeName("bool"),
                []),
        ]);
    }
}