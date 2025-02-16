# Random bits and pieces

(See also _[Thoughts about Unity's programming model](UNITY.md)_.)

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
        probably find some packages that allow us to control the Snowplow with WASD/arrows.  **Update:** I've added a script `PlayerMovement` ---NR
  -Will have some basic art and different side views
  
*Map*
  -Starts as as a completely dark map, the Headlight from the Snowplow will reveal squares 
  along the way

*Snow*
  -The objects that will disappear onCollision with a Snowplow on the Map
  -In the future, will increase sscore

Agreed ideas with all above from Norman!

# Kaelen's Working Ideas
Basically just trying to compile all the ideas and adding some of my own:

*Snowplow Game Object*
  - Controlled with Game Controller Object
  - Collider(s)
      * When collides with snow
      * When collides with certain points on the map? 
      * maybe this is how we can do the gradual reveal of the map? 
      * When collides with edges of paths on map
  - Light?
     * this may or may not be a part of the snowplow object, it might be part of the map object instead (gradually revealing map through mask) and just appear as though it is part of the snowplow object

*Game Controller Object*

  - WASD to control Snowplow [**Update:** See `PlayerMovement` script]

*Snow Game Object*

  - Collider
      * dissapears on collision with snowplow
  - Need to figure out how we are counting "snow cleared"
      * maybe it is every block cleared?
  - Needs to "spawn" before it is visible to player so it appears as though the snow "exists" there
      * spawn off screen or out of spotlight

*Map*
  - Dark map
  - Only revealed with headlight from snowplow
    -mask atop map?
    -Has collider points or some other thing that keeps track of where snowplow is and what to reveal
    -Unity discussion about this: https://discussions.unity.com/t/gradual-appearance-of-objects/671294 
      they are talking about 3D game but it might still be relevant to us
  - Has permissible paths
    -some sort of collider? or some other type of constraint on where our snowplow and snow can be on the map
  - Advice from ChatGPT on the map: https://chatgpt.com/share/67afcd0a-03fc-800c-9db1-566001833b7b

~UI/UX/Gameplay~
*Scoreboard*
  -Time left and/or how many streets still need to be cleared
  -Snow cleared
    increases with each "snow" cleared
*Play/Pause/Home*
  -Standard game UI components
*Game Over/Game Start*
  - When is game over?
    -when user chooses to quit, when __ streets are successfully cleared, when time runs out, when level is cleared
  -Can the user win/lose or is it just an infinite game?

# Possible Game Mechanics
- Katamari-like snowball building
- Salt and snowplow, one must complete before the other
- Night creatures chase you at night
- Sliding on ice
- Clear the map as quickly as possible/before the sun rises
- Obstacles in path
- Try not hit other cars, pedestrians, cyclists
- running out of health/freeze meter/Vehicle can take damage and not clear snow as well
- There might be a vehicle part collectibles throughout the map so if you collected say 3 you can auto repair yourself if you are damaged.
- Chill snowplow game where you explore the town and discover a mystery


