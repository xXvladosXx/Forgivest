using System.Collections.Generic;

namespace GameCore.Data
{
    public interface IPersistentProgressService
    {
        public PlayerProgress PlayerProgress { get; set; }
    }
}