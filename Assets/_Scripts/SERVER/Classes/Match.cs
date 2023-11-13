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
            Debug.Log("Beginning match creation process...");
            this.gameBoard = gameBoard;
            this.players = players;
            playerTurnIndex = 0;
            Debug.Log("Match initialized.");
        }
        public List<List<char>> GetGameBoard() { return gameBoard; }

        public char GetGameBoardItem(int x, int y) { return gameBoard[x][y]; }

        public void SetGameBoardItem(int x, int y, char c)
        {
            gameBoard[x][y] = c;
        }
        public List<Player> GetPlayers()
        {
            return players;
        }
        public override string ToString()
        {
            string playerText = "[";
            foreach (var player in players)
            {
                playerText += player.ToString() + ", ";
            }
            playerText = playerText.Substring(0, playerText.Length - 3) + "]";
            return playerText;
        }
    }
}