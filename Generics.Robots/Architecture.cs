namespace Generics.Robots;

public interface IRobotAI<out T> 
	where T : IMoveCommand
{
	T GetCommand();
}

public abstract class RobotAI<T> : IRobotAI<T>
        where T : IMoveCommand
{
    protected int counter = 1;

    public abstract T GetCommand();
}

public class ShooterAI : RobotAI<ShooterCommand>
{
	int counter = 1;

    public override ShooterCommand GetCommand() => ShooterCommand.ForCounter(counter++);
}

public class BuilderAI : RobotAI<BuilderCommand>
{
    public override BuilderCommand GetCommand() => BuilderCommand.ForCounter(counter++);
}

public interface IDevice <in T> 
	where T : IMoveCommand
{
	string ExecuteCommand(T command);
}

public abstract class Device<T> : IDevice<T> 
	where T : IMoveCommand
{
    public abstract string ExecuteCommand(T command);
}

public class Mover : Device<IMoveCommand>
{
    public override string ExecuteCommand(IMoveCommand command)
    {
        if (command == null)
            throw new ArgumentException();
        return $"MOV {command.Destination.X}, {command.Destination.Y}";
    }
}

public class ShooterMover : Device<IMoveCommand>
{
	public override string ExecuteCommand(IMoveCommand _command)
	{
		var command = _command as IShooterMoveCommand;
		if (command == null)
			throw new ArgumentException();
		var hide = command.ShouldHide ? "YES" : "NO";
		return $"MOV {command.Destination.X}, {command.Destination.Y}, USE COVER {hide}";
	}
}

public class Robot<T> where T : IMoveCommand
{
    IRobotAI<T> ai;
    IDevice<T> device;

    public Robot(IRobotAI<T> ai, IDevice<T> executor)
    {
        this.ai = ai;
        this.device = executor;
    }

    public IEnumerable<string> Start(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            var command = ai.GetCommand();
            if (command == null)
                break;
            yield return device.ExecuteCommand(command);
        }
    }
}

public static class Robot
{
    public static Robot<IMoveCommand> Create<T>(IRobotAI<IMoveCommand> ai, IDevice<IMoveCommand> device)
    {
        return new Robot<IMoveCommand>(ai, device);
    }
}