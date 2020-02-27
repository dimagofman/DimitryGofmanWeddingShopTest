using System;
using System.Linq;

namespace DimitryGofmanWeddingShopTest.Classes
{
	/// <summary>This represents the simulation area with rovers</summary>
	class Plateau
	{
		public int Width { get; }
		public int Height { get; }

		public Rovers Rovers { get; private set; }

		public Plateau(int width, int height, Rovers rovers)
		{
			if (width <= 0 || height <= 0 || rovers == null || rovers.Count() == 0) throw new Exception("Invalid parameters");

			this.Width = width;
			this.Height = height;
			this.Rovers = rovers;
		}

		/// <summary>Execute commands on each rover</summary>
		public void Simulate(bool enableCollision)
		{
			foreach (var rover in this.Rovers)
				foreach (var cmd in rover.Commands)
					rover.Execute(cmd, enableCollision, this.Rovers);
		}

		/// <summary>Outputs a report of current positions</summary>
		public string ReportCurrentPositions() =>
			string.Join(Environment.NewLine, from rover in Rovers
											 select $"{rover.CurrentState.X} {rover.CurrentState.Y} {Utils.GetDescription(rover.CurrentState.Orientation)}");

	}
}
