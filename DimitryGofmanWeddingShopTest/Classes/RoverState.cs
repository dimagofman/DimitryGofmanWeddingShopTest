using System;

namespace DimitryGofmanWeddingShopTest.Classes
{
	public class RoverState
	{
		public int X { get; }
		public int Y { get; }
		public Utils.Orientation Orientation { get; }

		public RoverState(int x, int y, Utils.Orientation orientation)
		{
			this.X = x;
			this.Y = y;
			this.Orientation = orientation;
		}

		public RoverState Clone() => new RoverState(X, Y, Orientation);
		public override string ToString() => $"{X} {Y} {Utils.GetDescription(Orientation)}";
		public override bool Equals(object otherState) => this.Equals(otherState as RoverState);
		public bool Equals(RoverState otherState)
		{

			if (ReferenceEquals(otherState, null)) return false;

			if (Object.ReferenceEquals(this, otherState)) return true;

			if (this.GetType() != otherState.GetType()) return false;

			//only compare position, not orientation, used for collision detection
			return X == otherState.X && Y == otherState.Y;// && Orientation == otherState.Orientation;

		}
		public static bool operator ==(RoverState state1, RoverState state2) => state1.Equals(state2);
		public static bool operator !=(RoverState state1, RoverState state2) => !state1.Equals(state2);
		public override int GetHashCode() => this.ToString().GetHashCode();

	}
}
