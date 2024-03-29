###### Assignment #2
## Lets play

### Goals:
The goal of this assignment is to implement a console-based Reversi (Othello) game using the Model-View-Controller (MVC) design pattern. By completing this assignment, students will gain a better understanding of MVC pattern and its practical application in developing software systems.


### Task
1. Implement a [Reversi](https://en.wikipedia.org/wiki/Reversi) game playable through the command-line interface (CLI). There are lot of places on the  internet where you can try playing reversi, e.g. [here](https://cardgames.io/reversi/) or [here](https://www.crazygames.com/game/reversi-online)
2. The game should adhere to the rules of Reversi, also known as Othello.
3. Players should be able to make moves by inputting coordinates for their desired move.
4. The game must support player vs. player (PvP) and player vs. simple bot (PvE) modes.
5. User must be able to start a new game after completing previous without restarting an app.
6. To simulate real-world interaction, in PvE mode little random delay (from one to few seconds) must be applied after player's move before bots move
7. Active player can undo a move during 3 seconds after making it AND while opponent did not make his turn.
8. Move duration must be limited to 20 seconds. Random move must be performed if user failed to make a move during this time.
9. Player can ask for a hint and get all possible moves highlighted.
10. Include comprehensive unit tests to validate the functionality of the game components.  Unit tests should cover critical aspects such as move validation, game state transitions, and edge cases. 


### Learning materials
There is a lot of confusing materials about MVC on the internet, I recommend to start with original [explanation by Martin Fowler](https://martinfowler.com/eaaDev/uiArchs.html), author of Refactoring book and a short video fragment from [Robert Martin talk](https://youtu.be/Nsjsiz2A9mg?si=CobGPXOk6evh2wEr&t=1893) on a conference.

After this you can read other materials on the internet but be aware that you can find a lot of wrong arrows and ideas.
