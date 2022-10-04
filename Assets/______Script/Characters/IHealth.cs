
public interface IHealth
{
    public int HP { get; set; }
    public int MaxHP { get; }

    public System.Action OnTakeDamage { get; set; }
    public System.Action OnHPUp { get; set; }
}
