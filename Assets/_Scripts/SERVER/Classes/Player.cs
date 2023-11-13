using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace wfMultiplayer
{
    public class Player
    {

        public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
        public ushort Id { get; set; }
        public string Username { get; set; }

        public bool InMatch = false;

        public Player(ushort Id, string Username)
        {
            this.Id = Id;
            this.Username = Username;
            InMatch = false;
        }

        public override string ToString()
        {
            return "Player [" + Username + ":" + Id + "]";
        }
    }
}

