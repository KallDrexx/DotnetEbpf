.SUFFIXES:

DOTNET_CMD = dotnet

# Dotnet project directories to check for changes of
DNTC_TOOL_DIR = ../dntc
SRC_ROOT = ../src/DotnetEbpf
EXAMPLES_DIR = $(SRC_ROOT)/DotnetEbpf.Examples
CORE_DIR = $(SRC_ROOT)/DotnetEbpf.Core

DNTC_TOOL_CSPROJ = $(DNTC_TOOL_DIR)/Dntc.Cli/Dntc.Cli.csproj
MANIFEST_DIR = $(EXAMPLES_DIR)

EXAMPLE_ARTIFACTS_DIR = $(SRC_ROOT)/artifacts/bin/DotnetEbpf.Examples/release
EXAMPLE_DLL = $(EXAMPLE_ARTIFACTS_DIR)/DotnetEbpf.Examples.dll
EXAMPLE_CSPROJ = $(EXAMPLES_DIR)/DotnetEbpf.Examples.csproj
CORE_DLL = $(EXAMPLE_ARTIFACTS_DIR)/DotnetEbpf.Core.dll

GENERATED_DIR = generated

.PRECIOUS: $(GENERATED_DIR)/%.bpf.c

%: $(GENERATED_DIR)/%.bpf.c 
	@echo Test

$(GENERATED_DIR)/%.bpf.c: $(EXAMPLE_DLL) $(EXAMPLES_DIR)/%.manifest.json %.c $(GENERATED_DIR)
	$(DOTNET_CMD) run --project $(DNTC_TOOL_CSPROJ) -- $(word 2, $^)
	
$(GENERATED_DIR):
	mkdir -p $(GENERATED_DIR)

$(EXAMPLE_DLL): $(shell find $(EXAMPLES_DIR) $(DNTC_TOOL_DIR) $(CORE_DIR)) 
	$(DOTNET_CMD) build -c Release $(EXAMPLE_CSPROJ)
	
.phony: clean
clean:
	$(DOTNET_CMD) clean -c Release $(SRC_ROOT)/*.sln
	$(DOTNET_CMD) clean -c Debug $(SRC_ROOT)/*.sln
	rm $(GENERATED_DIR)/*
