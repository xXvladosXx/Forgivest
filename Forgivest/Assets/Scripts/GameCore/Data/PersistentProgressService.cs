using System.Collections.Generic;

namespace GameCore.Data
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public Dictionary<string, PlayerProgress> PlayerProgresses { get; set; }
        public string Scene { get; set; }
    }
}