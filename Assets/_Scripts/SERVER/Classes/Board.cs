using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wfMultiplayer
{
    public class Board
    {
        public List<List<char>> boardChars;
        public Board(List<List<char>> boardChars)
        {
            this.boardChars = boardChars;
        }

        public override string ToString()
        {
            string Output = "";
            foreach (List<char> row in boardChars)
            {
                Output += "\n";
                foreach (char c in row)
                {
                    Output += $"[{c}]";
                }
                
            }
            Output += "\n";
            return Output;
        }
    }

}
