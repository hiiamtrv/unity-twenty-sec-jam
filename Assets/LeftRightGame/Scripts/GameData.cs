namespace LeftRightGame
{
    public enum DanceCommand
    {
        None,
        Left,
        Right
    }

    public class DanceUnit
    {
        public DanceCommand command;
        public float delay;
    }
}