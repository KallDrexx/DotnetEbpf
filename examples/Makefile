DOTNET_CMD=dotnet
MANIFEST_DIR=../src/DotnetEbpf/DotnetEbpf.Examples/
DNTC_TOOL_CSPROJ=../dntc/Dntc.Cli/Dntc.Cli.csproj

minimal: check_dotnet
	$(MAKE) process_manifest CURRENT_MANIFEST_NAME="minimal.manifest.json"

check_dotnet:
	@echo Looking for $(DOTNET_CMD) command
	@command -v $(DOTNET_CMD) > /dev/null || $(MAKE) print_error MSG="Dotnet CLI not installed or in path"
	@echo Dotnet CLI found
	
process_manifest:
	$(eval FULL_MANIFEST_PATH = $(MANIFEST_DIR)$(CURRENT_MANIFEST_NAME))
	@echo Executing DNTC manifest $(FULL_MANIFEST_PATH)
	@test -e $(FULL_MANIFEST_PATH) || $(MAKE) print_error MSG="Manifest file $(FULL_MANIFEST_PATH) not found"
	$(DOTNET_CMD) run --project $(DNTC_TOOL_CSPROJ) -- $(FULL_MANIFEST_PATH)

print_error:
	$(error $(MSG)) 