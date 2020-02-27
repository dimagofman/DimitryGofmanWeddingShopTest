using System;

namespace DimitryGofmanWeddingShopTest.Classes
{
	/// <summary>Thrown when a collision is detected</summary>
	internal class ExceptionCollision : Exception
	{
		/// <summary>The rover that collided</summary>
		public Rover Rover { get; }

		/// <summary>The other rover the current rover collided with or if this is null then collision is with the edges</summary>
		public ExceptionCollision(Rover rover)
		{
			this.Rover = rover;
		}
	}
}