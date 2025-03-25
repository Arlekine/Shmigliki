namespace Shmigliki.Gameplay
{
    public class Business : Special
    {
        public int AdditionalPoints { get; set; }

        public Business(int additionalPoints)
        {
            AdditionalPoints = additionalPoints;
        }

        public override void OnAdded(Monster monster)
        {}

        public override void OnRemoved(Monster monster)
        {}
}
}