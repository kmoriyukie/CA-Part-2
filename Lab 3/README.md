# LAB 3 & 4

## Scenes are located in Assets/Scenes
## Edited scripts are located in Assets/ToDo

## Lab 3
### A_Star.cs contains the implementation for the A_Star pathfinding algorithm, and I attempted to implement the Anytime A* algorithm (file name "ARA.cs"), but was unsuccessful. Thus, I chose to implement Jump Point Search as a secondary pathfinding algorithm, using the pseudocode described in the article "Online Graph Pruning for Pathfinding on Grid Maps", by Daniel Harabor and Alban Grastien. 
### Scene "Lab3, Grid" demonstrates that the JPS algorithm works, but only shows a grid with the path highlighted. You can press SPACE for a new map and new path. Scene "Lab3_Crowd" also demonstrates that the JPS works, but with the crowd of objects used in Lab 2. If desired, you can substitute the member "Grid_JumpAhead gas" from PathManager in Simulator.cs with "Grid_A_Star gas" to test the A_Star algorithm.
### The "main" script for "Lab3, Grid" is JumpAheadScriptGrid.cs. For "Lab3_Crowd", the main script is "Simulator.cs".

## Lab 4 
### There are two scenes for this lab, "Lab4_SingleIndividual", which demonstrates the steering feature. While the scene "Lab 4" demonstrates the steering feature and obstacle avoidance.
### I started by implementing the steering feature, which can be found in the class PathManager_Lab4, method updateVelocity. Then, for the obstacle avoidance, I created a capsule collider with its length to parallel to the forward vector of each agent. Then, whenever a collision is detected, I use the evade formula so the agents avoid each other.