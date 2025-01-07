# SPDX-License-Identifier: (LGPL-2.1 OR BSD-2-Clause)
# Heavily based on libbpf-bootstrap Makefile

.SUFFIXES:

DOTNET ?= dotnet
OUTPUT := .output

# List of different application targets that can be invoked
APPS = minimal

# Dotnet project directories to check for changes of
DNTC_TOOL_DIR := ../dntc
SRC_ROOT = ../src/DotnetEbpf
EXAMPLES_DIR = $(SRC_ROOT)/DotnetEbpf.Examples
CORE_DIR = $(SRC_ROOT)/DotnetEbpf.Core

DNTC_TOOL_CSPROJ = $(DNTC_TOOL_DIR)/Dntc.Cli/Dntc.Cli.csproj
MANIFEST_DIR = $(EXAMPLES_DIR)

EXAMPLE_ARTIFACTS_DIR = $(SRC_ROOT)/artifacts/bin/DotnetEbpf.Examples/release
EXAMPLE_DLL = $(EXAMPLE_ARTIFACTS_DIR)/DotnetEbpf.Examples.dll
EXAMPLE_CSPROJ = $(EXAMPLES_DIR)/DotnetEbpf.Examples.csproj
CORE_DLL = $(EXAMPLE_ARTIFACTS_DIR)/DotnetEbpf.Core.dll

# libbpf vars
CLANG ?= clang
LIBBPF_SRC := $(abspath ../libbpf/src)
LIBBPF_OBJ := $(abspath $(OUTPUT)/libbpf.a)
ARCH ?= $(shell uname -m | sed 's/x86_64/x86/' \
			 | sed 's/arm.*/arm/' \
			 | sed 's/aarch64/arm64/' \
			 | sed 's/ppc64le/powerpc/' \
			 | sed 's/mips.*/mips/' \
			 | sed 's/riscv64/riscv/' \
			 | sed 's/loongarch64/loongarch/')
VMLINUX := ../vmlinux.h/include/$(ARCH)/vmlinux.h
LIBBLAZESYM_SRC := $(abspath ../../blazesym/)
LIBBLAZESYM_INC := $(abspath $(LIBBLAZESYM_SRC)/capi/include)
LIBBLAZESYM_OBJ := $(abspath $(OUTPUT)/libblazesym_c.a)
# Use our own libbpf API headers and Linux UAPI headers distributed with
# libbpf to avoid dependency on system-wide headers, which could be missing or
# outdated
INCLUDES := -I$(OUTPUT) -I../libbpf/include/uapi -I$(dir $(VMLINUX)) -I$(LIBBLAZESYM_INC)

# Get Clang's default includes on this system. We'll explicitly add these dirs
# to the includes list when compiling with `-target bpf` because otherwise some
# architecture-specific dirs will be "missing" on some architectures/distros -
# headers such as asm/types.h, asm/byteorder.h, asm/socket.h, asm/sockios.h,
# sys/cdefs.h etc. might be missing.
#
# Use '-idirafter': Don't interfere with include mechanics except where the
# build would have failed anyways.
CLANG_BPF_SYS_INCLUDES ?= $(shell $(CLANG) -v -E - </dev/null 2>&1 \
	| sed -n '/<...> search starts here:/,/End of search list./{ s| \(/.*\)|-idirafter \1|p }')

# Build the specified application
$(APPS): %: $(OUTPUT)/%.bpf.o $(LIBBPF_OBJ) 
	@echo Test

# Build the DotnetEbpf.Examples project
$(EXAMPLE_DLL): $(shell find $(EXAMPLES_DIR) $(DNTC_TOOL_DIR) $(CORE_DIR)) 
	$(DOTNET) build -c Release $(EXAMPLE_CSPROJ)

# Transpile via a manifest with the same name as the target. It's expected that the manifests
# will produce a single file output with the name of the target plus ".bpf.c"
$(OUTPUT)/%.bpf.c: $(EXAMPLE_DLL) $(EXAMPLES_DIR)/%.manifest.json %.c | $(OUTPUT)
	$(DOTNET) run --project $(DNTC_TOOL_CSPROJ) -- $(word 2, $^)

# Build the BPF code	
#$(OUTPUT)/%.bpf.o: $(OUTPUT)/%.bpf.c $(LIBBPF_OBJ) $(VMLINUX) | $(OUTPUT)
.output/minimal.bpf.o: .output/minimal.bpf.c $(LIBBPF_OBJ) $(VMLINUX) | $(OUTPUT) 
	$(CLANG) -g -O2 -target bpf -D__TARGET_ARCH_$(ARCH)		      \
			 $(INCLUDES) $(CLANG_BPF_SYS_INCLUDES)		      \
			 -c $(filter %.c,$^) -o $(patsubst %.bpf.o,%.tmp.bpf.o,$@)
	$(BPFTOOL) gen object $@ $(patsubst %.bpf.o,%.tmp.bpf.o,$@)

# Build libbpf
$(LIBBPF_OBJ): $(wildcard $(LIBBPF_SRC)/*.[ch] $(LIBBPF_SRC)/Makefile) | $(OUTPUT)/libbpf
	$(MAKE) -C $(LIBBPF_SRC) BUILD_STATIC_ONLY=1 \
		    OBJDIR=$(dir $@)/libbpf DESTDIR=$(dir $@) \
		    INCLUDEDIR= LIBDIR= UAPIDIR=  \
		    install	

# Required directory creation
$(OUTPUT) $(OUTPUT)/libbpf:
	mkdir -p $(OUTPUT)
	
.phony: clean
clean:
	$(DOTNET) clean -c Release $(SRC_ROOT)/*.sln
	$(DOTNET) clean -c Debug $(SRC_ROOT)/*.sln
	rm -rf $(OUTPUT)

# delete failed targets
.DELETE_ON_ERROR:

# keep intermediate (.skel.h, .bpf.o, etc) targets
.SECONDARY: