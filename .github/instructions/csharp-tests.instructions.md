---
description: C# xUnit tests.
applyTo: "src/**/*{Test,Tests}.cs"
---

# C# Testing Instructions

- Use xUnit v3 with Microsoft.Testing.Platform.
- Run tests for changed files first; expand scope only if needed.
- Use BDD-style test names.
  - `When_SigningAndVerifying_Then_Success`
  - `When_ExportedAndImported_Then_PrivateKeyIsRestored`
- For unit tests where the target is clearly defined, create an inner class named after the target and place the tests within it. Examples:
  - For constructor variations: `public sealed class Constructor`
  - For the "InvokeAsync" method: `public sealed class InvokeAsyncMethod`
- Use `TestContext.Current.CancellationToken` for async tests.
- Use `Assert.SkipUnless(condition, reason)` for environment-dependent tests.
- Implement `IAsyncLifetime` when setup/teardown performs async I/O; otherwise use `IDisposable`.
- Do not commit certificates, private keys, or other sensitive test artifacts.
- Generate test assets with idempotent scripts in `scripts/` and load them from `TEST_ASSETS_PATH`.
