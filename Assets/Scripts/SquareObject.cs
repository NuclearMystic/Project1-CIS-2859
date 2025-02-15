public class SquareObject : ArenaShape
{
    private void Start()
    {
        StartCoroutine(ShrinkOverTime());
        health = 20;
    }
}
