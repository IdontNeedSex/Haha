/// <summary>
/// The Player class represents the players data like healthpoints and money
/// </summary>
public class Player
{
    public Player(int healthPoints, int startMoney)
    {
        HealthPoints = healthPoints;
        Money = startMoney;
    }

    public int HealthPoints { get; private set; }

    public int Money { get; set; }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void Hit(int damage)
    {
        HealthPoints -= damage;
    }

    public bool IsPlayerDead()
    {
        return HealthPoints <= 0;
    }
}