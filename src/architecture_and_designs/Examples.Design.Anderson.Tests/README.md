# Examples.Design.Anderson.Tests

## Piikokku Anderson's Domain-Driven Design

- <https://anderson02.com/ddd-menu/>

### Hexagonal Architecture

Domain-Driven Development originally recommended a "layered architecture."

- User interface layer
- Application layer
- Domain layer
- Infrastructure layer

Everything depends only on the lower layer.

However, this dependency relationship makes it impossible to refer to the domain definition in the infrastructure,
which causes inconvenience. Therefore, an architecture with the domain layer at the top is recommended.
This is called dependency inversion, but we will leave the details aside.

I'm not sure if it's an evolution of layered architecture, but conceptually, hexagonal architecture is the best at
the moment, and it's said to represent Domain Driven Development.

The idea is to define the domain layer and then write other technical elements outside of it.

### Domain logic and client code

Domain logic is business logic. It is something that is defined by the business. It is something that is decided.

Application logic is also called client code. Application logic is the code that makes your app work.

**The client code should not have any knowledge.**

### Entity

In Domain Driven Development, an Entity is a meaningful piece of data, which can be thought of as a single row of data
in a database.

The rule for an entity is that it must be unique. Even if there are multiple identical entities, it is necessary
to be able to identify one of them.

### ValueObject

The characteristics of ValueObject are as follows:

- Immutable classes
- ValueObjects with the same value are considered to be the same.
- No uniqueness

> [!NOTE]
> To prevent inheritance, we set it to "sealed" and receive a value in the constructor.

### Repository

A repository is an interface that is placed at the boundary between logic that contacts the outside world and logic
that does not when an application contacts the outside world.

Interfaces are placed in the Domain layer.

Implementation is done in the Infrastructure layer.

### Factory

The creation of instances is left to a factory class. A factory is created to allow classes that implement these
interfaces to be branched based on certain criteria.

> [!NOTE]
> Create a class called "Factories" directly under Infrastructure.  
> The access modifier of the concrete repository class is set to internal to ensure that it is only accessed by the factory.
