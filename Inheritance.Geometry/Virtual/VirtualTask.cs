namespace Inheritance.Geometry.Virtual;

public class Body
{
	public Vector3 Position { get; }

	protected Body(Vector3 position)
	{
		Position = position;
	}

	public bool ContainsPoint(Vector3 point)
	{
		if (this is Ball ball)
		{
			var vector = point - Position;
			var length2 = vector.GetLength2();
			return length2 <= ball.Radius * ball.Radius;
		}

		if (this is RectangularCuboid cuboid)
		{
			var minPoint = new Vector3(
				Position.X - cuboid.SizeX / 2,
				Position.Y - cuboid.SizeY / 2,
				Position.Z - cuboid.SizeZ / 2);
			var maxPoint = new Vector3(
				Position.X + cuboid.SizeX / 2,
				Position.Y + cuboid.SizeY / 2,
				Position.Z + cuboid.SizeZ / 2);

			return point >= minPoint && point <= maxPoint;
		}

		if (this is Cylinder cylinder)
		{
			var vectorX = point.X - Position.X;
			var vectorY = point.Y - Position.Y;
			var length2 = vectorX * vectorX + vectorY * vectorY;
			var minZ = Position.Z - cylinder.SizeZ / 2;
			var maxZ = minZ + cylinder.SizeZ;

			return length2 <= cylinder.Radius * cylinder.Radius && point.Z >= minZ && point.Z <= maxZ;
		}

		if (this is CompoundBody compound)
		{
			return compound.Parts.Any(body => body.ContainsPoint(point));
		}

		throw new NotImplementedException("Unknown figure!");
	}

	public RectangularCuboid GetBoundingBox()
	{
		throw new NotImplementedException("TODO");
	}
}

public class Ball : Body
{
	public double Radius { get; }

	public Ball(Vector3 position, double radius) : base(position)
	{
		Radius = radius;
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
}

public class CompoundBody : Body
{
	public IReadOnlyList<Body> Parts { get; }

	public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
	{
		Parts = parts;
	}
}