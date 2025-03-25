using EventBusSystem;
using Mono.Cecil;
using UnityEngine;

namespace Shmigliki.Gameplay
{
    public interface IComboHandler : IGlobalSubscriber
    {
        void OnCombo(Vector3 position, int comboCount);
    }
}