# MRTK3-SG

## About
This project is intended to create a basic Unity Application with the MRTK3 and the Sketching Geometry Unity Package.
Target of this project is the HoloLens2.

## Installation
The required MRTK3 packages are bundled with this project,
but you can also get them through the [MixedRealityFeatureTool](https://www.microsoft.com/en-us/download/details.aspx?id=102778) </br>
You can get the SketchingGeometry Package from [here](https://github.com/hunsri/VRSketchingGeometryUP/).

After that you should be able to run the application in the editor.
You can find a working `PlaygroundScene` under `Assets/Scenes`

If you want to build and deploy the application to the HoloLens2, you can follow [this guide](https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk3-overview/deployment/hololens2-deployment)

## Controls

### Editor
- hold the Space key and hold the left mouse button to draw a line

### HoloLens
- with your right hand:
  - do a pinching gesture by touching your thumb with your index finger to draw a line

## Troubleshooting
- when exporting a build via Visual Studio it can be necessary to add the `-pin` Command Line Argument when the HoloLens hasn't been paired yet

## Tips for developing your own scenes

A good overview over the functionalities of the MRTK3 can be found [here](https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk3-overview/) </br>


A scene needs the MRTK CR Rig Prefab, you can find it under `Packages/com.microsoft.mrtk.input/Assets/Prefabs/MRTK XR Rig.prefab` </br>
For in editor running, it is also recommended to place the MRTK InputSimulator into a scene `Packages/com.microsoft.mrtk.input/Simulation/Prefabs/MRTKInputSimulator.prefab` 