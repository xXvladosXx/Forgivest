﻿using System;
using UnityEngine;

namespace InventorySystem.Core
{
    public interface IInteractable
    {
        public GameObject GameObject { get; }
        public void Interact();
        public event Action OnDestroyed;
    }
}