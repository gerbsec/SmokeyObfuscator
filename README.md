Here's a revised README for the `SmokeyObfuscator`, now updated to reflect its new command-line interface functionality:

---

# SmokeyObfuscator - Universal .NET C# Obfuscator for Windows

SmokeyObfuscator is a powerful tool designed to obfuscate .NET C# assemblies, enhancing security for your applications by making it difficult for reverse engineers to understand and tamper with your code.

## Features

- **Universal Compatibility**: Works seamlessly with .NET C# assemblies targeting various versions of the .NET Framework.
- **Advanced Obfuscation Techniques**: Employs sophisticated obfuscation techniques to scramble your code, making it extremely challenging to reverse engineer.
- **Enhanced Security**: Protects your intellectual property and sensitive algorithms from being easily understood or stolen.
- **Command-Line Interface**: Provides a command-line interface for easy integration into build processes and automation scripts.
- **Customization Options**: Offers a range of customization options to tailor the obfuscation process to your specific needs.

## Installation

1. Download the latest release of SmokeyObfuscator from the [Releases](https://github.com/gerbsec/SmokeyObfuscator/releases) page.
2. Extract the downloaded ZIP file to a directory on your Windows machine.

## Usage

SmokeyObfuscator is operated entirely from the command line, providing a straightforward way to obfuscate assemblies as part of automated scripts or manual operations.

### Basic Commands

- **Obfuscate a Directory**:
  ```
  SmokeyObfuscator.exe -d <path-to-directory>
  ```
  This command will obfuscate all compatible files within the specified directory.

- **Obfuscate a Single File**:
  ```
  SmokeyObfuscator.exe -f <path-to-file>
  ```
  Use this command to obfuscate a specific file.

- **Help**:
  ```
  SmokeyObfuscator.exe -h
  ```
  Displays usage information and help about the command-line arguments.

### Example

To obfuscate all files in a directory called `bin/Release`, navigate to the directory where `SmokeyObfuscator.exe` is located and run:
```
SmokeyObfuscator.exe -d "C:\Path\To\Your\Project\bin\Release"
```

To obfuscate a single assembly named `MyApp.exe` in the directory `bin/Release`, run:
```
SmokeyObfuscator.exe -f "C:\Path\To\Your\Project\bin\Release\MyApp.exe"
```

## Support

For any issues, bugs, or feature requests, please [open an issue](https://github.com/gerbsec/SmokeyObfuscator/issues) on GitHub. Your feedback helps make SmokeyObfuscator better for everyone.

---

This README provides all necessary details for both new and existing users, emphasizing the switch to a command-line interface which aligns with modern development workflows and automation practices.
