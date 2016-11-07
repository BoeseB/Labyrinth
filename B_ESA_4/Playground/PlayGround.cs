﻿using System.Drawing;

namespace B_ESA_4.Playground
{
    public class PlayGround 
    {
        private readonly PlaygroundVisual _playgroundVisual;

        private string[,] _playgroundData { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public PlaygroundVisual PlaygroundVisual
        {
            get { return _playgroundVisual; }
        }

        public PlayGround(string[,] playgroundData)
        {
            _playgroundData = playgroundData;
            _playgroundVisual = new PlaygroundVisual(this);
        }

        public bool StillContainsItem()
        {
            bool result = false;
            for (int column = 0; column < _playgroundData.GetLength(0); column++)
            {
                for (int row = 0; row < _playgroundData.GetLength(1); row++)
                {
                    if (_playgroundData[column, row] == "o" || _playgroundData[column, row] == ".")
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public Field this[Point location]
        {
            get { return new EmptyField(); }
        }

        public Field this[int x, int y]
        {
            get
            {
                return new EmptyField();
            }
        }
    }  
}
