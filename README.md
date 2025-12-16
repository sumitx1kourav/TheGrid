# The Grid Assignment

A grid-based tactics prototype built in Unity 6 (6000.0.63f1) as part of a programming assignment. The project demonstrates grid generation, custom editor tooling, pathfinding, and turn-based enemy AI using clean OOP principles.

---

## Overview

This project recreates the core mechanics of a tactics-style game inspired by classic isometric strategy titles:

* A grid-based battlefield
* Player-controlled movement using grid pathfinding
* Obstacles placed using a custom Unity Editor tool
* A turn-based enemy AI that reacts after the player moves

---

## Features Implemented

### Assignment 1 – Grid Block Generation

* Generates a 10×10 grid of tiles at runtime
* Each tile is a separate GameObject with attached TileData
* Mouse hover detection using raycasting
* UI text displays hovered tile coordinates

### Assignment 2 – Obstacles

* Obstacle data stored in a ScriptableObject (ObstacleData)
* Custom Unity Editor Window with a 10×10 toggle grid
* ObstacleManager reads data and spawns obstacle cubes
* Blocked tiles cannot be traversed

### Assignment 3 – Pathfinding

* Player unit moves using grid-based BFS pathfinding
* Movement is shown tile-by-tile using coroutines
* Player cannot move through blocked tiles
* Input is disabled while the unit is moving

### Assignment 4 – Enemy AI

* Enemy AI implements a custom IAI interface
* Enemy uses the same grid and pathfinding logic
* Enemy moves only after the player finishes a move
* Enemy targets one of the four adjacent tiles near the player
* Turn-based behavior with proper separation of responsibilities

---

## Technical Highlights

* No Unity NavMesh used (custom grid-based pathfinding)
* Breadth-First Search (BFS) for shortest-path calculation
* ScriptableObjects used for data persistence
* Custom Editor tooling for level configuration
* Proper use of interfaces and MonoBehaviours

---

## Controls

* Left Mouse Click: Select a tile to move the player
* Player moves first
* Enemy moves automatically after the player finishes

---

## Camera

* Isometric-style camera setup
* Camera positioned to clearly frame the entire grid and units

---

## Unity Version

Unity 6 – 6000.0.63f1

This project is intended to be opened only with the specified Unity version, as required by the assignment.

---

## Project Structure (Key Files)

* GridManager.cs – Grid generation and tile storage
* TileData.cs – Per-tile data (coordinates, blocked state)
* ObstacleData.cs – ScriptableObject storing obstacle layout
* ObstacleEditorWindow.cs – Custom editor tool
* ObstacleManager.cs – Spawns obstacles at runtime
* PlayerMovement.cs – Player input and pathfinding
* EnemyAI.cs – Enemy behavior and movement
* IAI.cs – AI interface

---

## How to Run

1. Clone the repository
2. Open the project in Unity 6 (6000.0.63f1)
3. Open SampleScene
4. Press Play

---

## Notes

* Code is commented throughout for clarity
* Visual assets use simple primitives to focus on logic and structure
* The project emphasizes correctness, readability, and clean architecture

---

## Author

Sumit Kourav

---

Thank you for reviewing this project.
