Please help me define a Unity script `ScreenClearer` that helps clear an opaque screen to reveal a map.  The script should have these public variables:

      * `screen` a game object holding the image that is gradually being made transparent
      * `mask_parent` a GameObject that should have a bitmap as one of its children

At each update, the `ScreenClearer` host object should examine its location and overlay the bitmap on top of the screen.  Then, for every bit in the bitmap that is set, it should clear the alpha channel of the corresponding pixel on the `screen`.
