using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wfMultiplayer;

namespace wfMultiplayer
{
    public class Match
    {
        private List<List<char>> gameBoard;
        private List<Player> players;
        int playerTurnIndex;

        public Match(List<List<char>> gameBoard, List<Player> players)
        {
            this.gameBoard = gameBoard;
            this.players = players;
            playerTurnIndex = 0;
        }
        public List<List<char>> GetGameBoard() { return gameBoard; }

        public char GetGameBoardItem(int x, int y) { return gameBoard[x][y]; }

        public void SetGameBoardItem(int x, int y, char c)
        {
            gameBoard[x][y] = c;
        }
      
    }
}