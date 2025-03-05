# Lessons learned from the midterm project

## Process

  * The official process (with the five pages, the ABCD, and the backlog) seems most useful in the middle and late stages of a project.  In the early stages where you don't know what you want to build, it's less useful.

  * For figuring out what to build, it was very useful to create a detailed narrative that described what the camera would see if you filmed a person playing the game.  The construction of the narrative helped the team reach a common understanding of what we were hoping to build.  And the narrative provides a very concrete set of tasks for the first prototype: build only what you need to realize the narrative.

    If I were doing this again I would be writing multiple narratives throughout the early stages of the process.
    
## Git and github

  * Merge commits are as annoying as I remember them.  The "pull-push" story needs to be replaced with a "fetch-rebase-push" story.  (In default git land, "pull" is a shorthand for "fetch and merge.")  There has *got* to be a way to convince Github Desktop to use fetch-rebase by default when pulling.
  
  * During the very late stages it was hard to keep track of who was doing what.  The github issue tracker could have made this much easier.  For feature issues I would use these labels:

      - New unclaimed feature
      - Claimed
      - Underway
      - Ready for beta
      - Tested

    I would also use the predefined `bug` label.  The other predefined labels seem mostly irrelevant.

## Collaborative protocols

  * Jason recommends a protocol wherein before I work on a scene, I send a message to Slack saying "I'm working on this scene now; nobody else touch it."  Then when I'm finished, I send another message saying I'm finished.  In CS these actions are called _locking_ and _unlocking_.  We didn't try using this protocol, but it might work.

    If I tried this I think I would want to have a scene folder for each person, with the idea that I lock a scene by moving it into my folder, and I unlock/release the scene by moving it back to the common folder.
  
  * We commonly used a protocol wherein I could build on someone else's scene by making a copy and editing it.  This protocol worked well in terms of getting things done without stepping on anyone else's toes, but it did mean that copies of scenes proliferated, and that each copy had a different name.   The chaos became difficult to manage.

## Unity

  * Every game should have a debug/developer mode designed in from the start.  We could have used this to shorten the levels for debugging, without having to scramble to put them back at the last minute.

  * Testing multiple alternatives for gameplay is really useful.  The game and scenes should be designed in such a way that _all_ the alternatives can be loaded at once.  If each alternative requires its own build, that's going to inhibit experimentation.

  * Because collaborative development results in frequent renaming of scenes, any C# code must treat scene names as volatile information that is likely to change.  That means that each scene name should appear *at most once* in the C# code, and all the names should appear in the same place.  A good place is probably public variables on the `GameHandler`, *especially* if those variables can somehow be prevented from being changed in the Inspector.

  * The `GameHandler` script should likely use `public static` variables to store persistent information.

  * When there are alternative versions of a scene, or a new clone of a scene, it can be very easy to forget to fill public-variable slots in the Inspector.  (The Inspector is your friend but also your enemy. Especially at 2:00am.)  When possible, it is better to tag the object that needs to go into a public-variable slot, then use the `Start` method to find the variable using its tag.

    **Caveat:** Only _active_ objects can be found using tags.
    
  * When objects in different scenes share a script component, the way things work makes it hard to share information.  In particular, when changing scenes, _the new scene is not loaded until the `Update` method returns_.  Therefore, within a single method _code following `SceneManager.LoadScene` runs with the old scene still in place._  To run code under the new scene requires placing a callback in the `SceneManager.sceneLoaded` list.  There's an example in the `GameHandler.cs` from the Snowplow Race.

    (If you've taken 105, this is an example of continuation-passing style.  The callback is the continuation.)

  * A good workflow for developing prefabs is to tinker with an _instance_ of a prefab within a private scene.  Then when everything works, the `Apply` option under the `Overrides` dropdown can be used to propagate the changes back to the prefab.
