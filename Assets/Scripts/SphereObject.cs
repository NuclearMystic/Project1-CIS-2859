public class SphereObject : ArenaShape
{
    private void Start()
    {
        StartCoroutine(ShrinkOverTime());
        health = 1;
    }
}
