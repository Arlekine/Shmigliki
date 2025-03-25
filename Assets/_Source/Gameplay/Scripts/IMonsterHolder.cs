using System.Collections.Generic;
using UnityEngine;

namespace Shmigliki.Gameplay
{
    public interface IMonsterHolder
    {
        void SetInitialMonsters(IEnumerable<Monster> monsters);
        IReadOnlyList<Monster> CurrentMonsters { get; }
    }
}