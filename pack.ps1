# Create the NuGet packages.
nuget pack Src/LandmarkDevs.Core.Infrastructure/LandmarkDevs.Core.Infrastructure.csproj -Build -Properties Configuration=Release
nuget pack Src/LandmarkDevs.Core.Prism/LandmarkDevs.Core.Prism.csproj -Build -Properties Configuration=Release
nuget pack Src/LandmarkDevs.Core.Security/LandmarkDevs.Core.Security.csproj -Build -Properties Configuration=Release
nuget pack Src/LandmarkDevs.Core.Shared/LandmarkDevs.Core.Shared.csproj -Build -Properties Configuration=Release
nuget pack Src/LandmarkDevs.Core.Telemetry/LandmarkDevs.Core.Telemetry.csproj -Build -Properties Configuration=Release