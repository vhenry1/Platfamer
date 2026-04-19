# Platfarmer

Combine farming and co-op platformer in this fast-paced race to the clouds in Platfarmer. Gather coins and grow crops to gain 150 points in each level. Reach 300 points to win and reach the clouds. 

Documentation Video
[Link Text](https://www.loom.com/share/9c371a3427a74fe09972f31f29edc4ae)


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
- Space: Jump
- E: Plant Seeds
- Q: Grow Seeds to Plants
- H: Harvest Plants
- P: Open and Close Pause Menu

### Objective
Make it through the platformer section and farm to gain more money and points. 

## Project Structure

platfarmer/
├── README.md
├── Assets/
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── Lobby.unity
│   │   ├── Level1.unity
│   │   ├── Level2.unity
│   │   ├── Bootstrapper.unity
│   │   ├── HighScores.unity
│   │   ├── UIScene.unity
│   │   ├── WinScreen.unity
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
- Location: Assets/Scripts/GameManager.cs
- Description: Manages game state across scenes

**Singleton Pattern**
- Location: Assets/Scripts/AudioManager.cs
- Description: Manages audio across game scenes

**Delegate**
- Location: Assets/Scripts/PlayerController/Health.cs
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

## Technologies Used

- **Unity 2022.3 LTS**: Game engine
- **Netcode for GameObjects**: Multiplayer networking
- **TextMeshPro**: UI text rendering
- **SceneManagement**: Moving between scenes with persistence


