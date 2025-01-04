DOTNET_CMD=dotnet
MANIFEST_DIR=../src/DotnetEbpf.Examples/
DNTC_TOOL_CSPROJ=../dntc/Dntc.Cli/Dntc.Cli.csproj

minimal: check_dotnet
	$(MAKE) process_manifest CURRENT_MANIFEST_NAME="minimal.manifest.json"

check_dotnet:
	@echo Looking for $(DOTNET_CMD) command
	$(eval EXISTS := $(shell command -v $(DOTNET_CMD) > /dev/null && echo yes))
	@ifeq ($(EXISTS),yes)
		@echo Dotnet cli tool found
	else
#		$(error Dotnet tool not installed or in path)
		@echo test
	endif
	
process_manifest:
	$(eval FULL_MANIFEST_PATH = $(MANIFEST_DIR)$(CURRENT_MANIFEST_NAME))
	@echo Building manifest $(FULL_MANIFEST_PATH)
	@if [ -f $(FULL_MANIFEST_PATH) ]; then \
		echo "Manifest exists"; \
	else \
		$(error Manifest file does not exist!); \
	fi
	#$(DOTNET_CMD) run --project $(DNTC_TOOL_CSPROJ) -- $(FULL_MANIFEST_PATH)
 