using System;
using DimitryGofmanWeddingShopTest.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
	[TestClass]
	public class UnitTest1
	{


		[TestMethod]
		public void TestMethod1()
		{
		}



		[TestMethod]
		public void Test_RoverState_Constructor()
		{
			const int x = 5, y = 5;
			const Utils.Orientation orientation = Utils.Orientation.South;
			var state = new RoverState(x, y, orientation);
			Assert.AreEqual(state.X, x);
			Assert.AreEqual(state.Y, y);
			Assert.AreEqual(state.Orientation, orientation);
		}

		[TestMethod]
		public void Test_RoverState_Overrides()
		{
			const int x = 5, y = 5;
			const Utils.Orientation orientation = Utils.Orientation.South;

			var state1 = new RoverState(x, y, orientation);
			var state2 = new RoverState(x, y, orientation);

			Assert.IsTrue(state1 == state2);

			state2 = state1.Clone();

			Assert.IsTrue(state1 == state2 && !Object.ReferenceEquals(state1, state2));

			Assert.IsTrue(state1.ToString().Equals($"{x} {y} {Utils.GetDescription(orientation)}"));

		}

		[TestMethod]
		public void Test_Rover_Constructor()
		{

			const int x = 3, y = 3, w = 10, h = 10;
			const Utils.Orientation orientation = Utils.Orientation.East;

			var rover = new Rover(x, y, orientation, new RoverCommands(), w, h);

			Assert.IsNotNull(rover.CurrentState);
			Assert.IsNotNull(rover.Commands);
			Assert.IsNotNull(rover.StartingState);
			Assert.IsNotNull(rover.StateHistory);

			Assert.AreEqual(rover.CurrentState.X, x);
			Assert.AreEqual(rover.CurrentState.Y, y);
			Assert.AreEqual(rover.CurrentState.Orientation, orientation);

		}

		[TestMethod]
		public void Test_Rover_Commands1()
		{
			const int inX = 1, inY = 2, w = 5, h = 5;
			const Utils.Orientation inOrientation = Utils.Orientation.East;
			const string commandInput = "LMLMLMLMM";

			const int outX = 1, outY = 3;
			const Utils.Orientation outOrientation = Utils.Orientation.North;

			RoverCommands.TryParse(commandInput, out RoverCommands commands);

			var rover = new Rover(inX, inY, inOrientation, commands, w, h);

			foreach (var cmd in rover.Commands)
				rover.Execute(cmd, false, null);

			Assert.AreEqual(rover.CurrentState.X, outX);
			Assert.AreEqual(rover.CurrentState.Y, outY);
			Assert.AreEqual(rover.CurrentState.Orientation, outOrientation);

		}
		[TestMethod]
		public void Test_Rover_Commands2()
		{
			const int inX = 3, inY = 3, w = 5, h = 5;
			const Utils.Orientation inOrientation = Utils.Orientation.East;
			const string commandInput = "MMRMMRMRRM";

			const int outX = 5, outY = 1;
			const Utils.Orientation outOrientation = Utils.Orientation.East;

			RoverCommands.TryParse(commandInput, out RoverCommands commands);

			var rover = new Rover(inX, inY, inOrientation, commands, w, h);

			foreach (var cmd in rover.Commands)
				rover.Execute(cmd, false, null);

			Assert.AreEqual(rover.CurrentState.X, outX);
			Assert.AreEqual(rover.CurrentState.Y, outY);
			Assert.AreEqual(rover.CurrentState.Orientation, outOrientation);

		}










	}
}
