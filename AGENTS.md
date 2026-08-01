# AGENTS

## Purpose

- Existing implementations are preserved as learning history.
- Improvements may be added when better approaches are discovered.

## Project Structure

- Application source code, libraries, and tests are located under the `/src` directory.
- Subdirectories categorized by learning topic are created directly under `/src`,
  and individual projects are stored within them.

## Architecture

- Learning examples are primarily implemented as test code and verified using a test runner.
- If a scenario cannot be expressed via test code, the code is created in a separate project, such as a console application.
- Small learning examples may be implemented directly in test projects.
- Shared code is created as a separate project and referenced from the test code.
- Separate projects are created when examples require incompatible dependencies or configurations.

## Design Principles

- Use English for all source code, comments, and documentation.
- Preserve existing conventions.
- Prefer simple implementations.
- Prefer current language features when they improve readability or maintainability.

## Workflow

- Keep diffs minimal and reviewable.
- For complex, ambiguous, or high-impact tasks, align on the approach before making substantial changes.
- Do not add or update dependencies without confirmation.
- Ask before breaking changes.
- Validate changes after editing.

## Boundaries

- Do not create pull requests or perform remote repository operations unless instructed.

## References

Consult the relevant instruction or skill for language-specific rules, documentation, testing, or repository-specific workflows.
