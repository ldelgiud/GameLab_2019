namespace Hazmat.Components
{
    public struct MenuPulseComponent
    {
        public bool grow;
        public float scale;
        public float speed;

        public MenuPulseComponent(bool grow, float speed)
        {
            this.grow = grow;
            this.scale = 1.0f;
            this.speed = speed;
        }
    }
}
