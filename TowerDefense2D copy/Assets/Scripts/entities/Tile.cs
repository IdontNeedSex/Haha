/// <summary>
/// The Tile class stores the data for one specific square on the board. The Tile does not store its position.
/// </summary>
public class Tile
{
    public bool IsBlocked { get; set; }
    public bool IsTowerSet { get; private set; }
    
    public Tile(bool isBlocked)
    {
        IsBlocked = isBlocked;
    }
    
    public bool SetTower()
    {
        if (IsTowerSet || IsBlocked) return false;
        IsTowerSet = true;
        return IsTowerSet;
    }
}