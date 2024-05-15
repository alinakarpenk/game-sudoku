using System;
using System.Windows.Controls;

namespace Game_Sudoku.Models
{
    public class Sudoku
    {
        private readonly TextBox[,] _textBoxes;
        private readonly int _hiddenCount;
        private readonly Random _random;

        public int[,] Map { get; private set; }
        public int[,] PreparedMap { get; private set; }
        public int[,] GeneratedMap { get; private set; }
        public MapGenerator MapGenerator { get; private set; }
        public MapPreparer MapPreparer { get; private set; }
        public MapUpdater MapUpdater { get; private set; }
        public MapChecker MapChecker { get; private set; }
        public GameLogic GameLogic { get; private set; }

        public Sudoku(TextBox[,] textBoxes, int hiddenCount = 45)
        {
            _textBoxes = textBoxes;
            _hiddenCount = hiddenCount;
            _random = new Random(Guid.NewGuid().GetHashCode());

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Map = new int[9, 9];
            PreparedMap = new int[9, 9];
            GeneratedMap = new int[9, 9];
            
            MapGenerator = new MapGenerator();
            MapPreparer = new MapPreparer(GeneratedMap, PreparedMap, Map, _hiddenCount, _random);
            MapUpdater = new MapUpdater(Map, GeneratedMap, new SudokuUi(_textBoxes));
            MapChecker = new MapChecker(Map);
            GameLogic = new GameLogic(GeneratedMap, Map, _textBoxes);

            MapGenerator.GenerateMap();
            MapPreparer.PrepareMap();
            MapUpdater.UpdateNumbers();
        }
    }
}