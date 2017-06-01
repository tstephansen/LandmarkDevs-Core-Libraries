# LandmarkDevs Core Libraries

This project contains three libraries that are most commonly used in WPF applications that I create.

## Summary
### Infrastructure Library
This library contains application Observable and RelayCommand classes for ViewModels, error tracking, and logging.

### Prism Library
This library is useful when using the [Prism Library](https://github.com/PrismLibrary) in an application.

### Security Library
This library contains security functions for WPF applications. The classes in the Crypto folder were taken from the [ASP.NET Core Identiy](https://github.com/aspnet/Identity) repository and added here to reduce the dependencies required in whatever application the library is used in. All credit for that belongs to Microsoft. That code is mainly used for hashing passwords.

### Shared Library
This library contains a Guid Generator that was written by [fluentcassandra](https://github.com/fluentcassandra/fluentcassandra), a change tracking model, a tracking message model, some extension methods, and a helper class used for progress reporting during async operations.

### Telemetry Library
This library contains classes useful for telemetry tracking with HockeyApp.

## NuGet
To create a NuGet package from these libraries run the powershell script in the solution directory.  
`.\pack.ps1`