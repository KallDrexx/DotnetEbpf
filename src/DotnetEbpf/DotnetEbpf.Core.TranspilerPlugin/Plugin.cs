using Dntc.Common;

namespace DotnetEbpf.Core.TranspilerPlugin;

public class Plugin : ITranspilerPlugin
{
    public bool BypassBuiltInNativeDefinitions => false;
    
    public void Customize(TranspilerContext context)
    {
        context.ConversionInfoCreator.AddMethodMutator(new BpfSectionAttribute.Mutator());
        context.ConversionInfoCreator.AddGlobalMutator(new BpfSectionAttribute.Mutator());
        context.ConversionInfoCreator.AddGlobalMutator(new BpfLicenseAttribute.Mutator(context.ConversionCatalog));
    }
}