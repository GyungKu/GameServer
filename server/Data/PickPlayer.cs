namespace server.Data;

public class PickPlayer
{
    public int PlayerId { get; set; }
    public int PickedCharacterId { get; set; } = -1;
    public Team Team { get; set; }
    
}