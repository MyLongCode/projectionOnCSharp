
using Inheritance.Geometry.Virtual;
using System.IO;

namespace Inheritance.Geometry.Visitor;

public abstract class Body
{
	public Vector3 Position { get; }

	protected Body(Vector3 position)
	{
		Position = position;
	}
    public abstract Body Accept(IVisitor visitor);
}

public class Ball : Body
{
	public double Radius { get; }

	public Ball(Vector3 position, double radius) : base(position)
	{
		Radius = radius;
	}

	public override Body Accept(IVisitor visitor)
	{
        return visitor.Visit(this);
    }
}

public class RectangularCuboid : Body
{
	public double SizeX { get; }
	public double SizeY { get; }
	public double SizeZ { get; }

	public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
	{
		SizeX = sizeX;
		SizeY = sizeY;
		SizeZ = sizeZ;
	}
    public override Body Accept(IVisitor visitor)
    {
       return visitor.Visit(this);
    }
}

public class Cylinder : Body
{
	public double SizeZ { get; }

	public double Radius { get; }

	public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
	{
		SizeZ = sizeZ;
		Radius = radius;
	}
    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class CompoundBody : Body
{
	public IReadOnlyList<Body> Parts { get; }

	public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
	{
		Parts = parts;
	}
    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public interface IVisitor
{
    Body Visit(CompoundBody compoundBody);
    Body Visit(Cylinder cylinder);
    Body Visit(Ball ball);
    Body Visit(RectangularCuboid rectangularCuboid);
}

public class BoundingBoxVisitor : IVisitor
{
	public Body Visit(CompoundBody compoundBody)
	{
		var rectangles = compoundBody.Parts
			.Select(p => p.Accept(this) as RectangularCuboid)
			.ToArray();
        var maxX = rectangles.Max(p => p.Position.X + p.SizeX / 2);
        var minX = rectangles.Min(p => p.Position.X - p.SizeX / 2);

        var maxY = rectangles.Max(p => p.Position.Y + p.SizeY / 2);
        var minY = rectangles.Min(p => p.Position.Y - p.SizeY / 2);

        var maxZ = rectangles.Max(p => p.Position.Z + p.SizeZ / 2);
        var minZ = rectangles.Min(p => p.Position.Z - p.SizeZ / 2);

        var sizeX = maxX - minX;
        var sizeY = maxY - minY;
        var sizeZ = maxZ - minZ;
        var newPosition = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, (maxZ + minZ) / 2);

        return new RectangularCuboid(newPosition, sizeX, sizeY, sizeZ);
    }

	public Body Visit(Cylinder cylinder)
	{
        var diameter = cylinder.Radius * 2;
        return new RectangularCuboid(cylinder.Position, diameter, diameter, cylinder.SizeZ);
    }

	public Body Visit(Ball ball)
	{
        double side = ball.Radius * 2;
        return new RectangularCuboid(ball.Position, side, side, side);
    }

	public Body Visit(RectangularCuboid rectangularCuboid)
	{
        return rectangularCuboid;
    }
}

public class BoxifyVisitor : IVisitor
{
	public Body Visit(CompoundBody compoundBody)
	{
        return new CompoundBody(
               compoundBody.Parts
               .Select(s => s.Accept(this))
               .ToList());
    }

	public Body Visit(Cylinder cylinder)
	{
		return cylinder.Accept(this);
	}
	public Body Visit(Ball ball)
	{
        return ball.Accept(this);
    }

	public Body Visit(RectangularCuboid rectangularCuboid)
	{
        return rectangularCuboid.Accept(this);
    }
}