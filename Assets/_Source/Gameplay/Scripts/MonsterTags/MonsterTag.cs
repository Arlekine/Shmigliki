namespace Shmigliki.Gameplay
{
    public abstract class MonsterTag
    {
        public abstract void OnAdded(Monster monster);
        public abstract void OnRemoved(Monster monster);
    }
}