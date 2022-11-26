﻿using AttackSystem.Core;
using Utilities;

namespace GameCore.StateMachine
{
    public class PlayerObserver : IPlayerObserver
    {
        public DamageHandler DamageHandler { get; set; }
        public PlayerInputProvider PlayerInputProvider { get; set; }
    }
}