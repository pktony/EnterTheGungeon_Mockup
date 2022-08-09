
public interface IHealth
{
    public int HP { get; set; }
    public int MaxHP { get; }

    public System.Action onTakeDamage { get; set; }
}
