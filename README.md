# SmokeyObfuscator

SmokeyObfuscator is a lightweight .NET assembly obfuscation utility built around dnlib. It is designed to help protect .NET applications by applying a small set of transformation passes to method names, string payloads, and numeric constants.

## What it does

- Obfuscates .NET assemblies from a file or directory input
- Handles `.exe` and `.dll` files
- Applies multiple protections in a single pipeline
- Supports writing transformed output to a separate output directory

## Current protections

- `NumberChanger`: rewrites literal integer values using arithmetic-based obfuscation
- `Strings`: base64-encodes string literals and injects a decoder path
- `ProxyInts`: redirects literal values through generated helper methods
- `HideMethods`: renames methods and reduces obvious signatures in the output assembly

## Usage

### Obfuscate a single file

```bat
SmokeyObfuscator.exe -f "C:\Path\To\YourApp.exe"
```

### Obfuscate a directory

```bat
SmokeyObfuscator.exe -d "C:\Path\To\bin\Release" 
```

### Write to a separate output directory

```bat
SmokeyObfuscator.exe -f "C:\Path\To\YourApp.exe" -o "C:\Path\To\Output"
```

### Show help

```bat
SmokeyObfuscator.exe -h
```

## Notes

This project is intentionally lightweight and focused on pipeline simplicity. It is best suited for experimentation, training, or pre-release hardening rather than production-grade commercial obfuscation.

## Building

The project targets .NET Framework 4.8, so it requires the .NET Framework 4.8 Developer Pack to build on Windows. The code is intended to run on Windows-based .NET tooling environments.
