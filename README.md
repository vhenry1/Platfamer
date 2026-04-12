# Platfarmer

Combine farming and co-op platformer in this fast-paced race to the clouds in Platfarmer. Plant and water a beanstalk, either climb or harvest for money to reach the next section. Use fertilizer to grow the plants. Buy the fertilizer with the money you get from selling the plants.


## Setup

### Prerequisites
- Unity 2022.3 LTS or later
- Git

### Installation

1. Clone this repository:
```bash
git clone https://github.com/vhenry1/platfarmer.git
```
3. Open the project in Unity Hub
4. Open the MainMenu scene in Assets/Scenes/
5. Press Play to test in the editor

## How to Play

### Controls
- WASD: Move
- E: Plant Seeds
- Q: Grow Seeds to Plants
- H: Harvest Plants

### Objective
Make it through the platformer section and farm your way up. 

## Project Structure

platfarmer/
├── README.md
├── Assets/
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── Lobby.unity
│   │   ├── Level1.unity
|   |   └── GameOver.unity
│   ├── Scripts/
│   │   ├── Managers/
│   │   ├── Player/
│   │   ├── Shop/
│   │   └── UI/
│   ├── Prefabs/
│   └── Audio/
├── Packages/
└── ProjectSettings/

## Technical Implementation

**Singleton Pattern**
- Location: Assets/Scripts/Managers/GameManager.cs
- Description: Manages game state across scenes

**Delegate**
- Location: Assets/Scripts/Player/Health.cs
- Description: OnPlayerDamaged event notifies UI when player takes damage

**Object Pool Pattern**
- Location: Assets/Scripts/Combat/CoinManager.cs
- Description: Pools coin instances

## Known Issues

- Multiplayer issues 
- Issues with graphics spawn locations when growing.

## Future Enhancements (Final Submission)

- Multiplayer
- Pause menu with settings
- Better UI

## Technologies Used

- **Unity 2022.3 LTS**: Game engine
- **Netcode for GameObjects**: Multiplayer networking
- **TextMeshPro**: UI text rendering


