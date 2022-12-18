using UI.Menu;
using UnityEngine;

namespace UI.Core
{
    public class GameplayPanel : Panel
    {
        [field: SerializeField] public NewSaveMenu NewSaveMenu { get; private set; }
    }
}