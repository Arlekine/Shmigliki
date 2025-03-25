namespace Shmigliki.Gameplay
{
    public class Frozen : MonsterTag
    {
        public override void OnAdded(Monster monster)
        {
            monster.ActivityBlockers++;
        }

        public override void OnRemoved(Monster monster)
        {
            monster.ActivityBlockers--;
        }
    }
}