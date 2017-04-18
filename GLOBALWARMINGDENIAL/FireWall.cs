namespace GLOBALWARMINGDENIAL
{
    public class FireWall: Sprite
    {
        public FireWall(GlobalWarmingDenial game): base(game)
        {

        }

        public override void Update()
        {
            position.Y += 5;
            base.Update();
        }
    }
}