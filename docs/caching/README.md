# Caching

<!-- ----------------------------------- -->

## Microsoft.Extensions.Caching.Memory

IMemoryCache実装。ASP.NET Core と Microsoft.Extensions.DependencyInjection 統合。
IServiceCollection.AddMemoryCache();
はこれ。

### Setup

```shell
dotnet add package Microsoft.Extensions.Caching.Memory --version 5.0.0
```

### References

* https://docs.microsoft.com/ja-jp/aspnet/core/performance/caching/memory?view=aspnetcore-5.0
* https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.caching.memory.memorycache?view=dotnet-plat-ext-5.0

<!-- ----------------------------------- -->

## System.Runtime.Caching

ASP.NET MVC のキャッシュ(System.Web)をWeb意外に提供したもの。
object 経由アクセスなので、Boxing の考慮がつきまとう。
すでに互換性目的のパッケージのようだ。


### Setup

```shell
dotnet add package System.Runtime.Caching --version 5.0.0
```

### References

* https://qiita.com/wheeliechamp/items/9c4576b44b2e11fd997d
* https://tnakamura.hatenablog.com/entry/20100427/wellcome_system_runtime_caching
