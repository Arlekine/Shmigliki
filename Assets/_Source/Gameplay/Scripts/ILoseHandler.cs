using EventBusSystem;

namespace Shmigliki.Gameplay
{
    public interface ILoseHandler : IGlobalSubscriber
    {
        void OnLost();
    }
}