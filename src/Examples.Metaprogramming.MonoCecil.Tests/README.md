# Examples.Metaprogramming.MonoCecil.Tests

## Overview and Purpose

This project includes code to experiment with and learn how to implement metaprogramming
using `Mono.Cecil` with an Xunit runner.

In particular, I've heard that modifying DLLs using `Mono.Cecil` is sometimes referred to as "alchemy."

## Test Target

- [Mono.Cecil](https://www.mono-project.com/docs/tools+libraries/libraries/Mono.Cecil/)

Cecil is a library written by Jb Evain to generate and inspect programs and libraries in the ECMA CIL format.

With Cecil, you can load existing managed assemblies, browse all the contained types,
modify them on the fly and save back to the disk the modified assembly.
