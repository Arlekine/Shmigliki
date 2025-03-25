using EventBusSystem;

namespace Shmigliki.Gameplay
{
    public interface IMonsterCollisionHandler : IGlobalSubscriber
    {
        void OnMonsterCollided(Monster monsterA, Monster monsterB);
    }
}