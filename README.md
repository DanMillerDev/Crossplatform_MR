# Cross-Platform Mixed Reality

This repo is a collection of features and utilities for building cross-platform mixed reality applications. I've designed these features to be flexible and easily extendable, encouraging you to customize the scripts to suit your project!

Features are currently tested on 
* [Quest](https://www.meta.com/quest/)
* [Apple Vision Pro](https://www.apple.com/apple-vision-pro/)

with plans to support [Android XR](https://www.android.com/xr/) based devices in the future.
  
> **⚠️ WARNING: The support for Apple Vision Pro uses [Unity PolySpatial](https://unity.com/campaign/spatial).**  
> This requires a Unity Pro, Enterprise or Industry license. This is NOT required for Quest support.


## Poisson Disc Sampling for Surfaces
This feature uses the [poisson disc sampling technique](https://en.wikipedia.org/wiki/Supersampling#Poisson_disk) to distrubute objects on a found surface using [AR Planes](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.1/manual/features/plane-detection/arplane.html) through [AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.1/manual/index.html). It's designed to offer flexibility for random content distribution, unique content per plane alignment, and customizable appearance behavior.
