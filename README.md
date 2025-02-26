# State of the prefabs

We have several snowplow prefabs in various states of debugging.
The one that is most up to date is the `SimpleOversizedYellowPlow`.
I just updated it with the changes that are used in Monday's demo.


# How the game objects work

## Tutorial popups

The tutorial popups are demonstrated in the scene `NRPopupDemo`. They rely on two scripts:

  - `Postable` adds a component to an object that can be made to appear and disappear at runtime.  To make it appear, send it a `Post()` message (via method).  To make it disappear, sent `Unpost()`.

  - `ColliderPopup` has a postable object, to which it sends `Post` when the collider is entered and `Unpost` when the collider is exited.
  
They use these prefabs:

  - `Popup` is a UI element that has a background and some text.  It is based on the score object from the Week 1 tutorial.

  - `TutorialPopup` is a collider that will make an associated popup visible when the collider is entered.


To create a tutorial popup:

  - Drag the `TutorialPopup` collider into the scene.  Adjust the collider size, position, and angle to where you want it to appear.  The popup will post when there is a vehicle inside the collider.  Rename the object to give a hint as to what it is supposed to pop up.
  
  - Drag a `Popup` prefab into the UI Canvas, making it a child of the canvas.  Adjust its position to where you want it to pop up on the screen.  Rename the object to give a hint as to what text it pops up.
  
  - If the popup is supposed to be visible at startup, check the box in its `Postable` component.

  - Now go to the `Text` child of the `Popup` object and set the text.


# Scene sketches

This is an attempt to list all the pieces that would need to go into a Level 1 scene that supports our play narrative.

  - Canvas (object that holds everything in "on screen" as opposed to "in the world")
    
      * TutorialPopups (container for the popups)

          - Popup W (has a `Postable` component, visible on startup)
          - ... other popups, not visible on startup ...
          
      * Score

  - GameHandler (unclear what goes here)

  - Tutorial Colliders

      * Trigger W (has `ColliderPopup`) component, slot filled with Popup W
      * ... triggers for other popups ...
      
  - Map (has collider component, maybe polygon colliders?)

      * Background art?
      * Possible scrim or other means of highlighting road position
      * Snow (container for each snow pile)
          - Snow pile 1 (has collider, when player hits, get score and animation)
          - ... more snow piles ...

  - Snowplow (has rigid body, circle collider components)

      * Headlights
      * Plow art
      * Some means of carrying illumination
      
  - Darkness (superposed over the map)
  
Order in layer might look like this:

  - Order 0: map background art
  - Order 10: road highlights
  - Order 20: snow
  - Order 30: plow headlights
  - Order 31: plow art
  


# Game Narrative

Two players are using snowplows to clear the streets of Pleasantville.
Whoever clears the most snow wins.

 1. The world consists of a map on which there are roads.  Some roads have snow on them.
 
 2. Snowplows can travel on roads.  (One day snowplows may be able to travel off road, but there would be consequences.)
 
 3. In the beginning, the town is shrouded in darkness.  The only portion of the map that can be seen is what is illuminated by the headlights of the snowplows.  Headlights shine out ahead of the plow in the direction of travel.

 4. If a part of the map is ever illuminated by headlights, that part of the map stays illuminated forever.
 
 5. Snowplows move using WASD or IJKL keys.  Snow is cleared by moving a plow over it.
 
## The lens of player choice

  - Initially the player's only choices are where to go: *what route to take* to clear as much snow as possible before the opponent gets there.
  
  - Some other interesting choices might be possible, for example:

      * Turn on high beams to illuminate more space ahead, at the cost of moving more slowly.
      
      * Raise the plow in order to move faster, at the cost of not being able to pick up any snow (useful on streets that have already cleared).
      
  - There could be temporary or unexpected hazards, like stranded motorists or lost kittens.


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


