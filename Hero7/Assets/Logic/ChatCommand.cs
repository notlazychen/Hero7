using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBuf;
using RoyNet.GameServer.Entity;
using UnityEngine;

namespace Logic
{
    public class ChatCommand : Command<Chat_Send>
    {
        public override int Name
        {
            get { return (int) CMD_Chat.Send; } 
        }
        
        public ChatCommand(Action<Chat_Send> onExecute) : base(onExecute)
        {
        }
    }
}
