## Journal Entries
#### Ryan Stonebraker

| Time | Description |
| --- | --- |
| 2/11/2018 - 10:32am | Started work on project. Setup Unity to work with git version control and created empty scenes. |
| 2/11/2018 - 12:17pm | Started setting up the terrain environment and got SteamVR added to game. |
| 2/11/2018 - 2:24pm | Nearly complete with the Fog of War addition to cover parts of the scene. |
| 2/11/2018 - 4:01pm | Got the Fog of War shader and scene ready for initial Oculus Rift VR testing. |
| 2/11/2018 - 6:12pm | Tried to set up the Oculus Rift, but ran into initial problems running the installer due to RAID configuration of hard drives (there is a known bug involving this in the installer). Switched over to testing on the HTC Vive and got a first look into our VR environment. |
| 2/11/2018 - 8:52pm | Working on ship controllers so that each ship can check the board to see whether it is alive. |
| 2/11/2018 - 10:59pm | Working ship controller that can check the board to see if it has been hit and disappears if all of its “spots” have been hit. |
| 2/12/2018 - 12:59am | Ship controller now draws ships to sea in a grid based on their row/col position on the board. |
| 2/12/2018 - 2:13am | Added more functionality to ship controller. Positions relative to a Sea Board and depending on piece location, it snaps the orientation of the pieces at 90 degree intervals. |
| 2/12/2018 - 4:07am | Began integration of all parts for a first Alpha build of Battleship in VR. Peer programmed the integration. |
| 2/12/2018 - 5:30am | Scene set up with both ship controller and board. A bug persists that is causing errors synchronizing them. |
| 2/12/2018 - 1:42pm | Fully working Alpha build. Two boards exist that allow you to drop ships on one (creator mode) and attack on the other (game play mode). The only thing left now is to build simple AI player and possible due epics of adding a web based controller. |
| 2/18/2018 - 9:41pm | Refactored the ship controller class to make it more readable. Also added another ship to the main testing scene. |
| 2/19/2018 - 2:56am | Finished creating a demo video showing off the development on Derelict. Also, collaborated on filling out the README file to have in depth documentation of testing procedures, set up, dependencies, and acknowledgements. |
| 2/23/2018 - 6:34pm | Fixed some bugs associated with SeaBoard drawing and worked on various boolean related bug fixes. |
| 2/24/2018 - 12:47am | Worked on setting up the scene for a demo with full game play. Also added some objects to scene for visual appeal.|
| 2/24/2018 - 11:56am | Made a CEM_DEMO branch to show off at the CEM open house. This build included placing ships back on board for the game to go faster. There is still an issue with a shader preventing building. |
| 2/27/2018 - 6:23pm | Implemented an AI that "knows" where every one of the player's pieces are and hits them a specified percentage of the time instead of randomly guessing. |
| 2/27/2018 - 8:49pm | Fixed some simple bugs with the new AI and went to Chapman to test room setup before presentation. |
| 2/28/2018 - 2:38pm | Got AI (we think?) fully working and fixed several coloring issues associated with it incorrectly choosing miss shots. Also did some strength testing and filtered out unknown objects connecting to nodes on the board. Made a final build that will be used for the demo. |



#### Tristan Van Cise

| Time | Description |
| --- | --- |
| 2/11/2018 - 10:32am | Linked unity project assets and folders to github and set up main project scenes. |
| 2/11/2018 - 12:12pm | Imported assets for SteamVR and setup terrain scene that the player will be standing on. Also planned out an area to put ships. |
| 2/11/2018 - 12:18pm | Added camera rig and steamVR interactions script to Board scene. |
| 2/11/2018 - 2:33pm | Created board with 100 nodes and added colliders that recognize when objects are within their collision boxes. These will be used to link ships to the board and store information about the game state. |
| 2/11/2018 - 4:01pm | Coded two C# scripts, one for the board and another for the nodes. The board script auto populates a list containing GameObject of type Node. The class has mutator and accessor functions for both itself and the nodes. The node class contains a boolean that tracks if it is hit or not and a GameObject that changes its appearance to display the ship on the board. |
| 2/11/2018 - 5:07pm | Added snap to node function and added some grab/physics interactions to the controller and objects |
| 2/11/2018 - 7:21pm | Made the controllers have colliders so they can pick up objects. Also added joint functionality between objects when certain collider intersect with one another. When this event happens, a joint is created between the two objects and only a force > 5000 can break it. |
| 2/12/2018 - 1:00am | Added instant board snapping (fixed the delay between controller release and object snapping to board) |
| 2/11/2018 - 4:07am | Added integration with Ryan’s ship creation class, currently working on putting all the separate pieces (I.E the game board, display board, and script backend) together. |
| 2/18/2018 - 10:10pm | Created a format file for git commit message that allow me to specify a header and subject body with optional issue ticket close as suggested from assigned git commit message writing reading (https://chris.beams.io/posts/git-commit/). Also updated readme to make the commit message more standardized and give credit to the author who inspired us to take on that style. |
| 2/19/2018 - 12:15am | Finished refactoring a majority of the initial code batch. A large majority of functions are now significantly shorter and use helper functions to make the intent of the function much clearer. |
| 2/19/2018 - 12:37am | Finished recording a VR demo video for rough draft demonstration in class tomorrow. We figured it would be more efficient to do that instead of bringing in the entire VR system and gaming tower. |

