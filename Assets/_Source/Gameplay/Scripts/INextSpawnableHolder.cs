using UnityEngine;

namespace Shmigliki.Gameplay
{
    public interface INextSpawnableHolder
    {
        Monster GetNextMonster(Vector3 position);
    }
}