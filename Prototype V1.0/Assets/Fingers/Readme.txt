Fingers, by Jeff Johnson
Fingers (c) 2015 Digital Ruby, LLC
http://www.digitalruby.com

Version 1.7.1

See ChangeLog.txt for history.

Fingers is an advanced gesture recognizer system for Unity and any other platform where C# is supported (such as Xamarin). I am using this same code in a native drawing app for Android (You Doodle) and they work great.

If you've used UIGestureRecognizer on iOS, you should feel right at home using Fingers. In fact, Apple's documentation is very relevant to fingers: https://developer.apple.com/library/ios/documentation/UIKit/Reference/UIGestureRecognizer_Class/

Tutorial
--------------------
I've made a video that gives a full run down of this asset. Please view it here: https://youtu.be/97tJz0y52Fw
These tutorials cover creating custom shape gestures: https://youtu.be/7dvP_zhlWvU and https://youtu.be/6JgPYK38G9o
Joystick tutorial: https://youtu.be/_uGy6yAk83s
DPad tutorial: https://youtu.be/kra9zFDhM-8
Please be sure to check out each demo scene as well.

Instructions
--------------------
To get started, perform the following:
- Drag the FingersScriptPrefab object into your first scene. You don't need to do this for additional scenes, as it will live forever.
- In your scripts you can the simply refer to FingersScript.Instance whenever you need to add or remove gestures.
- Add "using DigitalRubyShared;" to the top of your scripts to include the gestures and framework.
- Create some gestures. There are many example scripts and scenes for you to refer to.

Fingers script has these properties:
- Treat mouse as pointer (default is true, useful for testing in the player for some gestures). Disable this if you are using Unity Remote or are running on a touch screen like Surface Pro.
- Simulate mouse with touch - whether to send mouse events for touches. Default is false. You don't need this unless you have legacy code relying on mouse events.
- Pass through objects. Any object in this list will always allow the gesture to execute.
- Touch circles. For debug, if showing touches. Turn off before releasing your game or app. Requires using the prefab.
- Show touches. Default is false. Set to true to debug touches. Requires using the prefab.
- Default DPI. In the event that Unity can't figure out the DPI of the device, use this default value.
- Clear gestures on level load. Default is true. This clears out all gestures when a new scene is loaded.

Event System
--------------------
The gestures work with the Unity event system. Gestures over certain UI elements in a Canvas will be blocked, such as Button, Dropdown, etc. Text elements are always ignored and never block the gesture.

You can add physics raycasters to allow objects not on the Unity UI to also be part of the gesture pass through system. Collider and Collider2D components will not block gestures unless the PlatformSpecificView property on the gesture is not null and does not match the game object with the collider.

Any object in the pass through list of FingersScript will always pass the gesture through.

Options for allowing gestures on UI elements:
- You can set the PlatformSpecificView on your gesture that is the game object that you want to allow gestures on. If the gesture then executes over this game object, the gesture is always allowed. See DemoScriptPlatformSpecificView.cs.
- You can populate the PassThroughObjects property of FingersScript. Any game object in this list will always pass the gesture through.
- You can use the CaptureGestureHandler callback on the fingers script to run custom logic to determine whether the gesture can pass through a UI element. See DemoScript.cs, CaptureGestureHandler function.
- You can use the ComponentTypesToDenyPassThrough and ComponentTypesToIgnorePassThrough properties of FingersScript to customize pass through behavior by adding additional component types.

See the DemoScript.cs file for more details and examples.

Standard Gestures:
--------------------
Once you've added the script, you will need to add some gestures. This will require you to create a C# script of your own and add a reference to the FingersScript object that you added. See the demo script (Demo*.cs) for what this looks like. Remember to add the namespace DigitalRubyShared.

Each gesture has public properties that can configure things such as thresholds for movement, rotation, etc. The defaults should work well for most cases. Fingers works in inches by default.

Please review the Start method of DemoScript.cs to see how gestures are created and added to the finger script. Also watch that tutorial video if you get lost, it will be very helpful.

Other demo scenes show how to use additional helper scripts, such as DemoScenePanScaleRotate and DemoSceneDragDrop.

Custom Shapes:
--------------------
Custom shapes are possible with ShapeGestureRecognizer. This uses a fuzzy image recognition algorithm, and will require you to train it to match variants of your shape. Shapes are defined as a grid of pixels up to 64 pixels in size. Performance is excellent as rows are compared by a simple ulong bitmask comparison.

FingersImageAutomationScene is a great way to rapidly create the code for your shapes. Run the scene. As you draw each gesture, the code gets put into the text box. Click the X in the bottom right to remove the last line if you made a mistake. Up to about 50 lines can go in the text box before Unity starts throwing errors, so copy the code out every so often and clear out the text box.

To learn more about creating custom shape gestures and how to test them and refine them, please watch the tutorial video at https://youtu.be/7dvP_zhlWvU

One Finger Gestures:
--------------------
Scaling and Rotation one finger gestures are available. Please see DemoSceneOneFinger for more details.

Joystick:
--------------------
FingersJoystickScript is a great way to create a joystick. Please see DemoSceneJoystick and DemoScriptJoystick for examples. The joystick features distance limiting and power (moves further as joystick moves away from center).

The joystick now has a prefab! It must be placed under a Canvas.

DPad:
FingersDPadScript allows use of a DPad. You can swap out the images and change the colliders. Please watch the DPad tutorial to see how this works in full.

The DPad now has a prefab! It must be placed under a Canvas.

Demos:
--------------------
I've made several demo scenes. Please check them out as they are great for seeing everything Fingers - Gestures for Unity can do.

Misc:
--------------------
*Note* I don't use anonymous / inline delegates in the demo script as these seem to crash on iOS.

Troubleshooting / FAQ:
--------------------
Q: My gestures aren't working.
A: Did you add a physics and/or physics2d ray caster to your camera?

Q: Help!
A: I'm available to answer your questions or feedback at support@digitalruby.com

Thank you.

- Jeff Johnson, create of Fingers - Gestures for Unity

