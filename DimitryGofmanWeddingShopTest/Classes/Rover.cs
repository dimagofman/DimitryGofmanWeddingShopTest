using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DimitryGofmanWeddingShopTest.Classes
{
	/// <summary>Represents a Rover, it has a starting state a list of commands and ending state. For fun, I've added state history, to keep track of travelled path</summary>
	public class Rover
	{

		/// <summary>Plateau's width and height</summary>
		private readonly int w, h;

		public RoverCommands Commands { get; }

		public RoverState StartingState { get; }

		public RoverState CurrentState { get; private set; }

		/// <summary>Just for fun, keeping track of state changes</summary>
		public List<RoverState> StateHistory { get; private set; } = new List<RoverState>();

		/// <param name="w">Plateau's width</param>
		/// <param name="h">Plateau's height</param>
		public Rover(int x, int y, Utils.Orientation orientation, RoverCommands nav, int w, int h)
		{
			this.Commands = nav ?? throw new ArgumentNullException(nameof(nav));
			this.w = w;
			this.h = h;

			this.StartingState = new RoverState(x, y, orientation);
			this.CurrentState = this.StartingState.Clone();
			StateHistory.Add(this.CurrentState);
		}

		/// <summary>
		/// Execute a given command, when command is executed it is removed from the list and a new state is saved into history. 
		/// For fun I've added collision detection with edges and other rovers
		/// </summary>
		/// <param name="enableCollision">Will throw <see cref="ExceptionCollision"/> if there's a collision</param>
		/// <param name="otherRovers">used for collision detection</param>
		public void Execute(Utils.Command cmd, bool enableCollision, Rovers otherRovers)
		{

			Utils.Orientation nextOrientation =
				cmd == Utils.Command.Left ? Utils.RotateLeft(this.CurrentState.Orientation) :
				cmd == Utils.Command.Right ? Utils.RotateRight(this.CurrentState.Orientation) :
				this.CurrentState.Orientation;

			int dX = cmd == Utils.Command.Move ?
					this.CurrentState.Orientation == Utils.Orientation.East ? +1 :
					this.CurrentState.Orientation == Utils.Orientation.West ? -1 :
					0 : 0;

			int dY = cmd == Utils.Command.Move ?
					this.CurrentState.Orientation == Utils.Orientation.North ? +1 :
					this.CurrentState.Orientation == Utils.Orientation.South ? -1 :
					0 : 0;

			this.CurrentState = new RoverState(this.CurrentState.X + dX, this.CurrentState.Y + dY, nextOrientation);

			StateHistory.Add(this.CurrentState);

			if (enableCollision && (
				this.CurrentState.X <= 0 ||
				this.CurrentState.X >= w ||
				this.CurrentState.Y <= 0 ||
				this.CurrentState.Y >= h ||
				otherRovers != null && otherRovers.Any((rover) => rover != this && rover.CurrentState == this.CurrentState)
				))
				throw new ExceptionCollision(this);


		}

	}
}
