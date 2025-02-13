# Cross-Platform Mixed Reality

This repo is a collection of features and utilities for building cross-platform mixed reality applications. I've designed these features to be flexible and easily extendable, encouraging you to customize the scripts to suit your project!

Features are currently tested on 
* [Quest](https://www.meta.com/quest/)
* [Apple Vision Pro](https://www.apple.com/apple-vision-pro/)

plans to support [Android XR](https://www.android.com/xr/) based devices in the future.
  
> **⚠️ WARNING: The support for Apple Vision Pro uses [Unity PolySpatial](https://unity.com/campaign/spatial).**  
> This requires a Unity Pro, Enterprise or Industry license. This is NOT required for Quest support.


## Poisson Disc Sampling for Surfaces
![discSampling_quest](https://github.com/user-attachments/assets/c19367c8-5994-4829-b0ce-66c2af13c6e1)

This feature uses the [poisson disc sampling technique](https://en.wikipedia.org/wiki/Supersampling#Poisson_disk) to distrubute objects on a physical surface using [AR Planes](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.1/manual/features/plane-detection/arplane.html) through [AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.1/manual/index.html). It's designed to offer flexibility for random content distribution, unique content per plane alignment, and customizable appearance behavior.

#### [PoissonDiscSampling.cs](https://github.com/DanMillerDev/Crossplatform_MR/blob/main/Assets/Scripts/Poisson/PoissonDiscSampling.cs)
Creates a distibution of 2D points relative to an AR plane and stores them in a list.

#### [PlaceContentOnPlane.cs](https://github.com/DanMillerDev/Crossplatform_MR/blob/main/Assets/Scripts/Poisson/PlaceContentOnPlane.cs)
Instantiates prefabs on the planes based on data from [PoissonDiscSampling.cs](https://github.com/DanMillerDev/Crossplatform_MR/blob/main/Assets/Scripts/Poisson/PoissonDiscSampling.cs) with options for alingment, only being created on unique planes, and to show objects at creation time. There is also a public `UnityEvent` `OnObjectsPlaced` that is Invoked with a reference to the list of created objects and the initial collision position.

#### [AppearanceBehaviorDistance.cs](https://github.com/DanMillerDev/Crossplatform_MR/blob/main/Assets/Scripts/Poisson/AppearanceBehaviorDistance.cs)
Listens to to the `OnObjectsPlaced` event to have a create a unique visual to show the objects on the plane. It sorts the objects in "rings" and then loops through the rings to create a rippling grow effect centered around the collision position.

#### [ScaleAppear.cs](https://github.com/DanMillerDev/Crossplatform_MR/blob/main/Assets/Scripts/Utilities/ScaleAppear.cs)
A utility script for using a lerp to scaling objects based on a in Quart tween.

#### [PlaneCollisionManager.cs](https://github.com/DanMillerDev/Crossplatform_MR/blob/main/Assets/Scripts/Poisson/PlaneCollisionManager.cs)
a utility script for doing collision detection on AR planes and invoking an event with the position and AR plane.


## Utility

#### [HandJointFollow.cs](https://github.com/DanMillerDev/Crossplatform_MR/blob/main/Assets/Scripts/Utilities/HandJointFollow.cs)
A script for moving an object based on XR Hand joint ID pose data. Options for Direct transform setting or rigidbody move position.
