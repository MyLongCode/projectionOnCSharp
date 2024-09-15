namespace Inheritance.MapObjects;


public interface IEntity
{
    public Army Army { get; set; }
}

public interface IItem
{
    public int Owner { get; set; }
}

public interface ITreasure
{
    public Treasure Treasure { get; set; }
}

public class Dwelling : IItem
{
	public int Owner { get; set; }
}

public class Mine : IItem, ITreasure, IEntity
{
	public int Owner { get; set; }
	public Army Army { get; set; }
	public Treasure Treasure { get; set; }
}

public class Creeps : IEntity, ITreasure
{
	public Army Army { get; set; }
	public Treasure Treasure { get; set; }
}

public class Wolves : IEntity
{
	public Army Army { get; set; }
}

public class ResourcePile : ITreasure
{
	public Treasure Treasure { get; set; }
}

public static class Interaction
{
	public static void Make(Player player, object mapObject)
	{
        if (mapObject is IEntity entityObj)
        {
            if (!player.CanBeat(entityObj.Army))
			{
                player.Die();
                return;
            }
                
        }
        
		if (mapObject is IItem itemObj)
		{
			itemObj.Owner = player.Id;
		}

		if (mapObject is ITreasure treasureObj)
		{
			player.Consume(treasureObj.Treasure);
		}


		return;
	}
}