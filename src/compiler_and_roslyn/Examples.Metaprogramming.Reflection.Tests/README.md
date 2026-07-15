# Examples.Metaprogramming.Reflection.Tests

## Overview and Purpose

This project includes code to experiment with and learn how to implement metaprogramming
using `System.Reflection` with an Xunit runner.

## Test Target

- [System.Reflection namespace](https://learn.microsoft.com/ja-jp/dotnet/api/system.reflection)

Contains types that retrieve information about assemblies, modules, members, parameters,
and other entities in managed code by examining their metadata.

## Test Index

### Reflection Learns

- [RuntimeTypesTests.cs](./Learns/RuntimeTypesTests.cs)

   Tests that demonstrates how to use reflection to retrieve type information at runtime.

- [GenericTypeDefinitionTests.cs](./Learns/GenericTypeDefinisionTests.cs)

  Test using reflection to create generic type information at runtime.

- [HowToSelectionTypesTests.cs](./Learns/HowToSelectionTypesTests.cs)

  Tests that demonstrates how to use reflection to select types based on certain criteria.

## References

- [Reflection in .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/fundamentals/reflection/overview)
