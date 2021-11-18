# Merge game description

  ## UI
There are two screens in the game. Intro screen and gameplay screen. On the Intro screen, there is a "Start" button on clicking which Gameplay screen is opened. Gameplay screen game board size is dynamic, can be changed in the unity inspector (you can set the number of rows and individualy the number of slots for each row).
  ## Gameplay
At the start of the game, you have a clear board and the box generator under the board. When you tap on that generator several times, it drops a box on the board. Then tap the box and get a Unit item(first level). 

Your task is to create and join those units of the same level. To Merge one needs two (2) pieces of the same level Unit item and pull one on top of each other. The items now merge to give the next level Unit item. It is not enough to put the items next to each other, they have to be in the same field.

### The project also includes:

1. Save/Load system (board state) as JSON data format,
2. Adaptive UI for each phone size(FullHD(16/9), iPhone X, iPad Pro),
3. Portrait mode,
4. Safe area,
5. Draggable unit elements,
6. Progress bar (incremented after merge unit) + Game progress (levels),
7. Showes the "Congratulation" popup after the progress bar is full. Resets progress bar after popup closed,
8. Each unity has level and progress weight,
10. The scriptable object contains progress bar weight for each unit level (max 10 levels),
11. Board and units are non UI elements (has Transform component),
12. Unit item show his level (world text in the right bottom side),
13. Generator box (generating new unit item after 10 seconds countdown, user tap is boost countdown timer) UI element,
14. Showes the second countdown on the generator box,
15. Max unit level is 10 (warning message appears, when trying to merge),
16. Codes are documented so that other programmers could easily navigate in the solution.
