README.txt – Assignment 6: AI and Mecanim; Physics, Lights, Textures

====================================================
3 FORMS OF 3D PHYSICS
====================================================

Teleportation Taxis:
There are pairs of taxis located around the city, serving as entry and destination points.  
Each taxi is equipped with a box collider that detects when the player touches it, transporting the player across the city.

Paparazzi:
The paparazzi have box colliders around them.  
When the player collides with a paparazzi, a camera flashing sound is triggered, signaling that points will be deducted from the player's total score.

Boulevard Colliders:
Each boulevard in the city is marked as a “popular street” using box colliders.  
This feature is utilized for the Bayesian network in the game.

Clothing Store:
The clothing store has a box collider.  
When the player collides with it, they can change their outfit to temporarily disguise themselves from fans and paparazzi.

====================================================
THREE LIGHTS
====================================================

Sunrise and Sunset:
The game has a challenge that the user must complete all auditions before sunset. As time goes by in the game, the directional light changes its position to let the user visually see how much time they have left.

City Street Lights and Lamp Posts:
Street lights and lamp posts have been added around the city to not only make the game look visually appealing but also to let the user see when the city gets dark.

Interior Lights for Audition Center:
Each audition center has lights inside so the user can see inside the room.

====================================================
THREE TEXTURES
====================================================

- NPC and player character textures  
- Buildings textures  
- Trees, cars, and other city props  

====================================================
AI TECHNIQUES AND MECANIM ANIMATIONS
====================================================

(Lora): 
Added a traffic AI system using the A* algorithm to create moving car traffic around the city. 
This adds some more obstacles for the players as they have to avoid getting hit and being slowed down by the cars while running away from the paparazzi. 
Also added movement animation to the paparazzi. 

(Nandini): 
Flocking behavior has been implemented for the fans, allowing them to follow the celebrity wherever she goes. 
This introduces an added challenge for the player, who must navigate around the fans to avoid being crowded and slowed down. 
Additionally, a running animation has been added for the fans.

(Vedant): 
In the game, paparazzi use a simplified Bayesian Network to decide whether to chase the player. 
This decision is based on environmental conditions: time of day, street type, and the player's fame level. 
A probability is calculated from a conditional probability table (CPT), and random sampling is used to determine if the chase begins. 
The paparazzi then follow a finite state machine (FSM) with patrol, chase, and search behaviors.

====================================================
SOURCES
====================================================

Traffic system AI: https://github.com/SunnyValleyStudio/Simple-Traffic-System-Unity-2020  
Day night animation: https://discussions.unity.com/t/day-night-cycle/828208  
Bayes network: https://www.cs.ubc.ca/~murphyk/Bayes/bnintro.html#CPTs  
Flocking Algorithm: https://learn.unity.com/tutorial/flocking  
