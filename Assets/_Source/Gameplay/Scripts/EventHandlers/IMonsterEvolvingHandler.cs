using EventBusSystem;

namespace Shmigliki.Gameplay
{
    public interface IMonsterEvolvingHandler : IGlobalSubscriber
    {
        void OnMonsterEvolved(Monster evolvedMonster, Monster eatenMosnter);
    }
}