public class TriangleObject : ArenaShape
{
    private void Start()
    {
        StartCoroutine(ShrinkOverTime());
        health = 10;
    }
}
