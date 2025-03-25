using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Shmigliki.Application
{
    [DefaultExecutionOrder(int.MinValue)]
    public class ApplicationQuitNotifier : MonoBehaviour
    {
        private List<IApplicationQuit> _preDestroys;

        [Inject]
        public void Initialize(List<IApplicationQuit> preDestroys)
        {
            _preDestroys = preDestroys;
        }

        public void OnApplicationQuit()
        {
            _preDestroys.ForEach(x => x.OnApplicationQuit());
        }
    }
}