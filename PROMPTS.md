Please help me define a Unity script `ScreenClearer` that helps clear an opaque screen to reveal a map.  The script should have these public variables:

      * `screen` a game object holding the image that is gradually being made transparent
      * `mask_parent` a GameObject that should have a bitmap as one of its children

At each update, the `ScreenClearer` host object should examine its location and overlay the bitmap on top of the screen.  Then, for every bit in the bitmap that is set, it should clear the alpha channel of the corresponding pixel on the `screen`.


# Fog of war

The game concept is to start with a map that is completely obscured by fog.  During the game the player will drive a snowplow around the map, and as the snowplow moves, its headlights will penetrate the fog and reveal the map.

The display will show four textures: map at the back, fog in front of the map, headlights in front of the fog, and snowplow in front of the headlights.

Right now I've got a game object each for the map, the fog, and the snowplow.  Each one has the art (texture) as a child.  The snowplow has its art as a child, and the snowplow can either have a headlight object as a child or just the headlight art as a child.

The effect I'm looking for is a sort of "fog of war" effect: whereever headlights pass over the fog, they turn the fog transparent, and the fog stays transparent until the game is restarted.  Oh, and I will want multiple snowplows, each with its own headlight.

Ideally I would like a single C# script that can form a component to go on the headlight object.  That component would refer to the fog and would gradually clear it.

I'd like your opinion on this design.  (I am looking for a design that stays relatively simple and does not use a zillion features.)  And then I'd like help writing that C# script.

Finally, if you write any C# code for me, please put a comment at the top saying "// original source: ChatGPT."

> Taking a closer look
> 
> I’m considering safety and efficiency for air conditioning repairs in high-rise buildings, aiming to minimize risks and ensure smooth functioning.
