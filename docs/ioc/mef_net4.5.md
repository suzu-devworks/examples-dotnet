# MEF in .NET4.5 (System.ComponentModel.Composition.*)

This namespace provides classes that constitute the core of the Managed Extensibility Framework, or MEF.

* Since .NET Framework 4.5

Support for:

* Attribute-less registration ([RegistrationBuilder](https://docs.microsoft.com/ja-jp/dotnet/api/system.componentmodel.composition.registration.registrationbuilder))
* Finer-Grained Control Over Lifetime ([ExportFactory&lt;T&gt;](https://docs.microsoft.com/ja-jp/dotnet/api/system.componentmodel.composition.exportfactory-1))
* Diagnostics improvements ([CompositionOptions.DisableSilentRejection](https://docs.microsoft.com/ja-jp/dotnet/api/system.componentmodel.composition.hosting.compositionoptions))

Sometimes called MEF2 or MEF.

## Setup

```shell
dotnet add package System.ComponentModel.Composition.Registration
```

## References

* [Nuget ...](https://www.nuget.org/packages/System.ComponentModel.Composition.Registration/)
* [Docs ...](https://docs.microsoft.com/ja-jp/dotnet/api/system.componentmodel.composition)
* https://docs.microsoft.com/ja-jp/archive/msdn-magazine/2012/june/clr-an-attribute-free-approach-to-configuring-mef
* https://www.codeproject.com/Tips/488513/MEF-in-NET-4-5
* https://qiita.com/ousttrue/items/8da18f91fed8642abe5f
