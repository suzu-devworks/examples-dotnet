# Examples.Design.Gof.Tests

## Design Patterns: Elements of Reusable Object-Oriented Software

Design Patterns: Elements of Reusable Object-Oriented Software (1994) is a software engineering book describing software
design patterns. The book was written by Erich Gamma, Richard Helm, Ralph Johnson, and John Vlissides, with a foreword
by Grady Booch. The book is divided into two parts, with the first two chapters exploring the capabilities and pitfalls
of object-oriented programming, and the remaining chapters describing 23 classic software design patterns.

<!-- spell-checker:words Vlissides -->
<!-- spell-checker:words Booch -->

> [Design patterns] solve specific design problems and make object-oriented designs more flexible, elegant, and
> ultimately reusable. They help designers reuse successful designs by basing new designs on prior experience.
> A designer who is familiar with such patterns can apply them immediately to design problems without having
> to rediscover them.

## Patterns by type

### Creational

<!-- spell-checker:words Creational -->

Creational patterns provide the capability to create objects based on a required criterion and in a controlled way.

- ***Abstract factory*** groups object factories that have a common theme.
- ***Builder*** constructs complex objects by separating construction and representation.
- ***Factory method*** creates objects without specifying the exact class to create.
- ***Prototype*** creates objects by cloning an existing object.
- ***Singleton*** restricts object creation for a class to only one instance.

### Structural

Structural patterns concern class and object composition. They use inheritance to compose interfaces and define ways
to compose objects to obtain new functionality.

- ***Adapter*** allows classes with incompatible interfaces to work together by wrapping its own interface around that
  of an already existing class.
- ***Bridge*** decouples an abstraction from its implementation so that the two can vary independently.
- ***Composite*** composes zero-or-more similar objects so that they can be manipulated as one object.
- ***Decorator*** dynamically adds/overrides behavior in an existing method of an object.
- ***Facade*** provides a simplified interface to a large body of code.
- ***Flyweight*** reduces the cost of creating and manipulating a large number of similar objects.
- ***Proxy*** provides a placeholder for another object to control access, reduce cost, and reduce complexity.

### Behavioral

Most behavioral design patterns are specifically concerned with communication between objects.

- ***Chain of responsibility*** delegates commands to a chain of processing objects.
- ***Command*** creates objects that encapsulate actions and parameters.
- ***Interpreter*** implements a specialized language.
- ***Iterator*** accesses the elements of an object sequentially without exposing its underlying representation.
- ***Mediator*** allows loose coupling between classes by being the only class that has detailed knowledge of their methods.
- ***Memento*** provides the ability to restore an object to its previous state (undo).
- ***Observer*** is a publish/subscribe pattern, which allows a number of observer objects to see an event.
- ***State*** allows an object to alter its behavior when its internal state changes.
- ***Strategy*** allows one of a family of algorithms to be selected on-the-fly at runtime.
- ***Template method*** defines the skeleton of an algorithm as an abstract class, allowing its subclasses to provide
  concrete behavior.
- ***Visitor*** separates an algorithm from an object structure by moving the hierarchy of methods into one object.
