using EventBusSystem;

namespace Shmigliki.Gameplay
{
    public interface IMaxScoreUpdateHandler : IGlobalSubscriber
    {
        void OnMaxScoreUpdate(int newMaxScore);
    }
}