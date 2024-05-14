### Software functionality

The game has 4 difficulty levels namely beginner, comfortable, normal and hard. When choosing the difficulty of the game, it depends on how many moves, hints and open cells will be provided to the user.

#### The game also features:
+ Hints, namely opening a random cell on the field.
+ Clearing the field, the user can clear all numbers previously entered by the user.
+ A pause that stops the timer and has an option to return to the menu, continue the game and an option to start a new game.
+ The user can also continue the game after closing the application, the result, time and number of moves are saved.


### Programming Principles

#### SRP
Each class in the code performs only one function.

#### DRY
The code that creates the TextBox text blocks to represent the game is generated in a loop instead of repeating the code for each block individually.

#### KISS
The code adheres to simplicity and transparency, avoiding unnecessary complexity.

#### YAGNI 
The code is implemented taking into account the real needs of the program, avoiding redundant functionality.


### Design Patterns

#### Factory Method
Used to create instances of the [Sudoku](Sudoku.cs) class. The `CreateSudoku` method of the SudokuFactory inner class is used to create a new Sudoku instance.

#### Decorator
The decorator pattern is used to change the appearance of text boxes (TextBox) in the `SudokuCellDecorator class`. Depending on the number in the cell, the mode for the text box is set: number mode or empty mode.

#### Memento
The snapshot pattern is used to save the Sudoku state in the `SudokuMemento class`. This allows you to save the current state and restore it later as needed.

### Refactoring Techniques

#### Extract Method
Some parts of the code that perform certain operations have been extracted into separate methods to improve readability and support the principle of single responsibility.

#### Extract Variable
Some complex expressions have been replaced by variables with understandable names to make the code easier to understand.

#### Rename Method
The program implements methods with understandable names
