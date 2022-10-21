using UnityEngine;

namespace Interaction.Core
{
    public interface IInteractable
    {
        public GameObject GameObject { get; }
        public void Interact();
    }
}