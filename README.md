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






# Peter's Working Ideas

For G.O.'s and Components I think we would need:

*Snowplow*
  -This will be the main GameObject for a snowplow, something that can be easily duplicated if 
        that is an avenue we wish to pursue
  -Will have a Headlight/Light object at a minimum
  -This will likely be the 2D version of some kind of Game Controller object we tried to use 
        previously. Now that I think of it, Game Controller was used for 3D, so we can 
        probably find some packages that allow us to control the Snowplow with WASD/arrows
  -Will have some basic art and different side views
  
*Map*
  -Starts as as a completely dark map, the Headlight from the Snowplow will reveal squares 
  along the way

*Snow*
  -The objects that will disappear onCollision with a Snowplow on the Map
  -In the future, will increase sscore

Agreed ideas with all above from Norman!



