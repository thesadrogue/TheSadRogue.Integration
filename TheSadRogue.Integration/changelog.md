# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [Unreleased]

None.

## [1.0.0-alpha02] - 2021-10-13

## Added
- Added support for having `RogueLikeMap` as the parent type for `RogueLikeComponentBase` and `RogueLikeComponentBase<T>`
- Added constructors to `RogueLikeCell`, `MemoryAwareRogueLikeCell`, and `RogueLikeEntity` that don't take a mandatory position parameter

## Changed
- `PlayerKeybindingsComponent.MotionHandler` is now a virtual function you can override, instead of an `Action`.
- `RogueLikeComponentBase` accepts any object which implements `IObjectWithComponents` as its parent
    - For `RogueLikeEntity` components, you should now use `RogueLikeComponentBase<IGameObject>` or `RogueLikeComponentBase<RogueLikeEntity>` as appropriate