using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace wfMultiplayer
{
    public class Player
    {
        public string ip { get; set; }
        public string username { get; set; }

        public Player(string ip, string username)
        {
            this.ip = ip;
            this.username = username;
        }


    }
}

