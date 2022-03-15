# Unity3D-InGameHierarchy
![image](https://user-images.githubusercontent.com/21270834/158336076-6ac15b53-5ce4-4988-853a-78617c1dc6bf.png)
Adds a Hierarchy in game, similar to the Unity Editor Hierarchy

Current Featuers:
Creating Folders
Dragging and Dropping to reorder/reparent objects
Rename object with double click
Saving to JSON file
Loading from JSON file

TODO:
Add OnSelect/OnDeselect event to SerializedObject and serialize it
Add examples of SerializedObject overrides (ex: Images, Text files)
Replace Unity default skybox with something more appealing

How To Work With It:
Simply override SerializedObject with anything you like (ex: SerializedImageObject) and add a button to spawn that type of object.
