# Random bits and pieces

Things that might be done:

  - Unity object that gradually clears an opaque screen to reveal a map: `ScreenClearer`, with these public variables:

      * `screen` the image that is gradually being made transparent
      * `mask` a GameObject representing the bitmask/flashlight, which can be moved by other components
      * `mask_bits` the actual bitmask, which is a child of `mask`?

    On every frame the `Update` method uses the `mask_bits` to change the opacity of the `screen`
    
  - Unity object that stores the map, with art and permissible paths.  This could be prototyped as an object that lets you go anywhere.

    This would need a method that you can send a source location and a vector to, and get back the location arrived at.  Possibly also with an indication of whether you hit a wall
    
  - Unity object to represent snow?

  - Unity object to represent snowplow

  - Snowplow controller that supports WASD (forward, backward, turn) controls
