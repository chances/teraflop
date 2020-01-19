all: build
.DEFAULT_GOAL := build

build:
	dotnet build
.PHONY: build
