# 3D Action Combat Prototype (Unity)

![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Mac-lightgrey)
![Engine](https://img.shields.io/badge/Engine-Unity%203D-black)
![Language](https://img.shields.io/badge/Language-C%23-blue)

A 3D action combat prototype developed in Unity, demonstrating core gameplay features such as tight parry windows, enemy state management, and impact-driven game feel. Developed as a technical demonstration for gameplay engineering roles.

**🎥 [Watch the 60-Second Gameplay Demonstration Here](https://youtu.be/hFogup_xVws)**

## Core Features

### Player Mechanics & Combat System
* **Dynamic Melee System:** Event-driven weapon hit detection (`OnSignificantHit`) preventing multiple damage registers per swing.
* **Precision Guard & Parry:** Active blocking system with damage mitigation. Incorporates a 0.25-second precise parry window that completely negates damage and triggers a counter-stun on the attacker.
* **Game Feel & Polish:** Implemented custom Coroutine-based "Hit-Stop" (time scaling) to emphasize heavy impacts and successful parries, paired with custom Particle System VFX for action-oriented visual feedback.

### Enemy AI & State Management
* **NavMesh Tracking:** Integrated Unity's `NavMeshAgent` for dynamic player tracking and pathfinding within chase radius.
* **Non-Destructive State Machine:** Managed through Animator State blending and IK Rigging constraints. Safely handles state transitions (Chase -> Attack -> Stunned -> Death) without null-reference exceptions.

### Camera & UI Systems
* **Dual Camera System:** Cinemachine-driven Free Look camera for exploration and a seamless Lock-On system for combat encounters.
* **World Space UI:** Custom Billboard scripts applied to enemy health bars to ensure UI elements track accurately in 3D space and face the main camera regardless of player positioning.
* **Damage Feedback:** Instantiated UI floating damage text numbers dynamically generated at the point of impact.

## Technical Implementation Highlights
* Extensively utilized **Animation Events** and **StateMachineBehaviours** to separate game logic from timeline execution.
* Subscribed to C# events for weapon collision, keeping the `Update()` loops clean and highly performant.
* Utilized **Animation Rigging (Inverse Kinematics)** to dynamically glue character hands to weapon grips during complex blended animations.

## Controls
* **Movement:** W, A, S, D
* **Camera:** Mouse
* **Attack:** Left Mouse Button
* **Block / Parry:** Right Mouse Button (Hold)
* **Lock-On:** Q

## Getting Started
1. Clone the repository.
2. Open the project in Unity (Version 2022.3 LTS or higher recommended).
3. Navigate to `Assets/Scenes/` and open the `GameScene`.
4. Press Play in the Editor to test the mechanics, or build a standalone executable via `File > Build Settings`.
