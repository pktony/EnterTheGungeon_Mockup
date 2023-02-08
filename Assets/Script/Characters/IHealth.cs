
public interface IHealth
{
    public int HP { get; set; }
    public int MaxHP { get; }

    public System.Action<int, int> OnTakeDamage { get; set; }
    public System.Action<int> OnHPUp { get; set; }
}
