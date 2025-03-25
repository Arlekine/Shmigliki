using UnityEngine;

namespace Shmigliki.Gameplay
{
    public interface IMonsterFactory
    {
        Monster Create(int initialLevel, Vector3 position);
    }
}