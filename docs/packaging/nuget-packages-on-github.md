# NuGet packages on github

## Create package project

### 1. Add nuget property in csproj

```xml
  <PropertyGroup>
    <!-- Properties related to NuGet packaging: -->
    <IsPackable>True</IsPackable>
    <PackageId>SleighBells.Examples</PackageId>
    <Authors>akira suzuki</Authors>
    <Company>suzu-devworks</Company>
    <Version>1.0.0-alpha</Version>
    <Product>SleighBells.Examples</Product>
    <RepositoryUrl>https://github.com/suzu-devworks/examples-dotnet</RepositoryUrl>
    <PackageProjectUrl>https://github.com/suzu-devworks/examples-dotnet</PackageProjectUrl>
    <PackageTags>local;learning</PackageTags>
    <Description>
      Libraries for learning dotnet programming.
    </Description>
  </PropertyGroup>
```

### 2. Create package(\*.nupkg)

file destinatioin to `bin/Release/`

```shell
dotnet pack --configuration Release
```

## Manual publish package.

### 1. Creating Github personal access token (classic).

- [Creating a personal access token](https://docs.github.com/ja/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token)

### 2. Edit `nuget.config`

I don't want to hardcode the access token in the nuget.config file, so I only describe the package source here.

```shell
dotnet new nugetconfig
```

```diff
 <?xml version="1.0" encoding="utf-8"?>
 <configuration>
   <packageSources>
     <!--To inherit the global NuGet package sources remove the <clear/> line below -->
     <clear />
+    <add key="github" value="https://nuget.pkg.github.com/suzu-devworks/index.json" />
     <add key="nuget" value="https://api.nuget.org/v3/index.json" />
   </packageSources>
 </configuration>
```

### 3. Push package

```shell
dotnet nuget push "src/Examples.Core/bin/Release/SW.Examples.Core.1.0.0-alpha.nupkg"  --api-key YOUR_PERSONAL_ACCESS_TOKEN --source "github"
```

## Publishing and installing a package with GitHub Actions

### 1. Edit `.github/workflows/dotnet-package.yml`

- [see...](/.github/workflows/dotnet-package.yml)

### 2. Setting `GITHUB_TOKEN`

- Repository settings > Actions > Workflow permissions
- Select "Read and write permissions"

## References

- [dotnet CLI を使用して NuGet パッケージを作成する](https://learn.microsoft.com/ja-jp/nuget/create-packages/creating-a-package-dotnet-cli)
- [パッケージ作成のベスト プラクティス](https://learn.microsoft.com/ja-jp/nuget/create-packages/package-authoring-best-practices)
- [Working with the NuGet registry](https://docs.github.com/ja/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry)
