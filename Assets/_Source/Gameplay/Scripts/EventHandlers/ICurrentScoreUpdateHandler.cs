using EventBusSystem;

namespace Shmigliki.Gameplay
{
    public interface ICurrentScoreUpdateHandler : IGlobalSubscriber
    {
        void OnCurrentScoreUpdate(int newCurrentScore);
    }
}