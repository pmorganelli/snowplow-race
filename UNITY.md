# Thoughts about Unity's programming model

## Game objects

A `GameObject` has a unique **parent** and one or more **components**.

  - The parent may be a scene or another `GameObject`.

  - The components always include a `Transform` component.  This component, *composed with the transforms of the parents*, determines the object's position, orientation, and scaling in the scene.

  - A component supplies behavior.

  - A component may include *slots*.  A slot my contain a scalar variable, a reference to a `GameObject`, or presumably other stuff.

  - One key component type is the **C# Script**.  We will be writing these.

## Scripts and instances

Every script has zero or more **public variables**.  Each variable corresponds to a slot visible in the Inspector.  _The slot on the script itself should never be altered in the Inspector_.

A script is **instantiated** by dragging it onto a `GameObject`.  This object is called the **host**.  The public variables of the instance become slots on the host object. _These are the slots that should be altered in the Inspector._

## Designing with objects and components

We can think of a script (or other component) as being a little like a function:

  - When the containing object's slots are filled in the Inspector, that is like passing actual parameters.
  
  - The script has a specification or contract that explains what behavior it adds, expressed only in terms of the public variables and the host object.

To understand the design of a Unity game requires two things:

  - There must be a list of all the components and their contracts.

  - There must be a tree (Hierarchy) of game objects, each of which has a known set of components.
  
