# Isometric2DGame

A 2D isometric game developed in Unity as part of the RDE Programming Trial. The project focuses on gameplay programming, AI behavior, and implementing core game systems using C#.

## Technologies
- Unity 6000.4.1f1 (2D URP)
- C#
- Unity New Input System
- Rigidbody2D and Collider2D
- Git & GitHub

## Features

### 1. Player Movement
- WASD and arrow-key movement.
- Dynamic Rigidbody2D movement.
- Collision detection.
- Isometric camera follow.

### 2. Enemy AI
- State machine with four states:
  - Idle
  - Patrol
  - Chase
  - Attack
- Player detection and state transitions.

### 3. Combat System
- Player projectile attacks.
- Player and enemy health systems.
- Enemy attacks and health UI.

### 4. Inventory System
- Collectible items.
- Item pickup using E.
- Inventory UI toggled using I.
- Item usage, such as healing (using the apple will heal you, where as using the bomb will make it blow up in your hands if youre not careful).


## Controls

| Input | Action |
|---|---|
| WASD / Arrow Keys | Move |
| E | Interact / Pick up items |
| I | Toggle inventory |
| Mouse Click | Attack |

## Project Structure

```text
Assets/
├── Animations/
├── Art/
├── Miscellaneous/
├── Prefabs/
├── Scenes/
├── ScriptableObjects/
├── Scripts/
│   ├── Camera/
│   ├── Enemy/
│   ├── Inventory/
│   ├── Player/
└── Settings/
```

## Assets & Credits

Third-party assets will be credited here where required by their respective licenses.

## AI Usage

AI may be used for research, brainstorming, and troubleshooting. All generated suggestions will be reviewed and verified during development.
AI was used to generate the UI components (Inventory and Healthbar)

## Author

**Developer:** [Niels Eriksson]

**Project:** Isometric2DGame

**Purpose:** RDE Programming Trial
