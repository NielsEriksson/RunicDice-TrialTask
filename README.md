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
- Player melee or projectile attacks.
- Player and enemy health systems.
- Enemy attacks and health UI.

### 4. Inventory System
- Collectible items.
- Item pickup using E.
- Inventory UI toggled using I.
- Item usage, such as healing and equipment.

### 5. Dialogue System
- NPC interaction using E.
- Dialogue UI and branching choices.
- Custom Unity Editor tool for creating dialogue sequences.

## Controls

| Input | Action |
|---|---|
| WASD / Arrow Keys | Move |
| E | Interact / Pick up items |
| I | Toggle inventory |
| Space / Mouse Click | Attack |

## Project Structure

```text
Assets/
├── Animations/
├── Art/
├── Audio/
├── Materials/
├── Prefabs/
├── Scenes/
├── Scripts/
│   ├── Player/
│   ├── Enemy/
│   ├── Combat/
│   ├── Inventory/
│   ├── Dialogue/
│   └── UI/
└── Settings/
```

## Assets & Credits

Third-party assets will be credited here where required by their respective licenses.

## AI Usage

AI may be used for research, brainstorming, and troubleshooting. All generated suggestions will be reviewed and verified during development.

## Author

**Developer:** [Niels Eriksson]

**Project:** Isometric2DGame

**Purpose:** RDE Programming Trial
