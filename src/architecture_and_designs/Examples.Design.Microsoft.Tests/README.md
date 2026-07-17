# Examples.Design.Microsoft.Tests

## Tackle Business Complexity in a Microservice with DDD and CQRS Patterns

- <https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/>

### CQS/CQRS

CQRS is an architectural pattern that separates the models for reading and writing data.
It means that the operations of the system can be clearly separated into two separate categories:

- `Query`: These queries return results and don't change the state of the system. They also don't have side effects.
- `Command`: These commands change the state of the system.

CQS is a simple concept: methods in the same object are queries or commands.

### Entity

An entity represents a domain object and is defined primarily by its identity, persistence, and persistence over time,
not solely by the attributes that make it up. As Eric Evans says, "An object that is defined primarily by its identity
is called an entity." Entities are very important in a domain model because they are the foundation of the model.
Therefore, entities must be identified and designed carefully.

> An entity's identity can cross multiple microservices or Bounded Contexts.

### Value Object

A value object is an object that doesn't have a conceptual identity that describes an aspect of your domain.
It's an object that you instantiate to represent a design element that's only of concern to you temporarily.
You need to be concerned with "what" it is, not "who" it is.

> [!NOTE]
> EF Core 2.0 and later versions include Owned Entities, which make it easier to work with value objects, as described
> in more detail below.

### Aggregate

The finer grained DDD unit is the aggregate, which describes a cluster or group of entities and behaviors that can be
treated as a cohesive unit.

> Typically you define aggregates based on the transactions you require.

### Aggregate Root or Root Entity

An aggregate consists of one or more entities called aggregate roots (also known as root entities or primary entities).
An aggregate can also have multiple child entities and value objects, all of which work together to implement
the required behavior or transaction.

> The purpose of the aggregate root is to maintain the consistency of the aggregate.

### Use enumeration classes instead of enum types

Problems with enumerations:

- Enumeration-related behavior is scattered throughout the application. This leads to an increase in switch statements
  in various places.
- Adding new enumeration values ​​requires refactoring (shotgun surgery)
  > Check the default: of all switch statements when adding an enumeration.
- Enumerations do not follow the open-closed principle

## eShopOnContainers

- <https://github.com/dotnet-architecture/eShopOnContainers>
