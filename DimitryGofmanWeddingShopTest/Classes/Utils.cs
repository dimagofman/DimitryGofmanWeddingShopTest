using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DimitryGofmanWeddingShopTest.Classes
{
	public static class Utils
	{

		/// <summary>Rover orientation, move commands are executed with reference to this</summary>
		public enum Orientation {[Description("N")] North, [Description("E")] East, [Description("S")] South, [Description("W")] West }

		/// <summary>Programmed command for a rover</summary>
		public enum Command { Move, Right, Left }

		/// <summary>Match strings N E S and W to Orientation enum</summary>
		public static bool OrientationTryParse(string input, out Orientation orientation)
		{

			orientation = default;

			if (string.IsNullOrWhiteSpace(input)) return false;

			var chr = char.ToUpperInvariant(input[0]);

			switch (chr)
			{
				case 'N': orientation = Utils.Orientation.North; return true;
				case 'E': orientation = Utils.Orientation.East; return true;
				case 'S': orientation = Utils.Orientation.South; return true;
				case 'W': orientation = Utils.Orientation.West; return true;
			}
			return false;
		}
		/// <summary>Return new orientation to the left of current orientation</summary>
		public static Orientation RotateLeft(Orientation orientation)
		{
			switch (orientation)
			{
				case Orientation.East: return Orientation.North;
				case Orientation.North: return Orientation.West;
				case Orientation.South: return Orientation.East;
				case Orientation.West: return Orientation.South;
			}
			return Orientation.North;
		}
		/// <summary>Return new orientation to the right of current orientation</summary>
		public static Orientation RotateRight(Orientation orientation)
		{
			switch (orientation)
			{
				case Orientation.East: return Orientation.South;
				case Orientation.North: return Orientation.East;
				case Orientation.South: return Orientation.West;
				case Orientation.West: return Orientation.North;
			}
			return Orientation.North;
		}

		/// <summary>
		/// Split the input into lines and process according to spec
		/// </summary>
		/// <returns>grid width and height and rovers preloaded with nav paths</returns>
		/// <remarks>
		/// Test Input:
		/// 5 5
		/// 1 2 N
		/// LMLMLMLMM
		/// 3 3 E
		/// MMRMMRMRRM
		/// Expected Output:
		/// 1 3 N
		/// 5 1 E
		/// </remarks>
		public static (int gridWidth, int gridHeight, Rovers rovers) ProcessInput(string text)
		{

			if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));

			var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

			if (lines.Length == 0) throw new ArgumentException("invalid format, no lines found", nameof(text));

			var splits = lines[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			if (splits.Length != 2) throw new Exception("invalid format - first line, should have 2 numbers");

			if (!int.TryParse(splits[0], out int w) || w <= 0) throw new Exception("invalid format - first line, should have 2 numbers, first part was not a >0 integer");

			if (!int.TryParse(splits[1], out int h) || h <= 0) throw new Exception("invalid format - first line, should have 2 numbers, second part was not a >0 integer");

			if (lines.Length > 2)
			{
				var lst = new List<Rover>();

				for (int i = 1; i < lines.Length; i += 2)
				{

					if (char.IsDigit(lines[i][0]))
					{
						//current line is the starting coordinate of a rover since it's starting with a number

						splits = lines[i].Split(' ');

						if (splits.Length < 3 ||
						 !int.TryParse(splits[0], out int x) ||
						 !int.TryParse(splits[1], out int y) ||
						 !Utils.OrientationTryParse(splits[2], out Utils.Orientation orientation)) throw new Exception("invalid format line: " + i);

						if (x < 0 || x > w || y < 0 || y > h) throw new Exception("invalid position, outside the bounds");


						if (i < lines.Length - 1)
						{
							//nav points must be on the next line
							if (!RoverCommands.TryParse(lines[i + 1], out RoverCommands nav)) throw new Exception("invalid format line: " + i + 1);
							lst.Add(new Rover(x, y, orientation, nav, w, h));
						}

					}

				}

				return (w, h, new Rovers(lst));

			}

			return (w, h, null);
		}

		/// <summary>
		/// Copied from https://www.codementor.io/@cerkit/giving-an-enum-a-string-value-using-the-description-attribute-6b4fwdle0
		/// </summary>
		public static string GetDescription<T>(this T e) where T : IConvertible
		{
			if (e is Enum)
			{
				Type type = e.GetType();
				Array values = System.Enum.GetValues(type);

				foreach (int val in values)
				{
					if (val == e.ToInt32(CultureInfo.InvariantCulture))
					{
						var memInfo = type.GetMember(type.GetEnumName(val));
						var descriptionAttribute = memInfo[0]
							.GetCustomAttributes(typeof(DescriptionAttribute), false)
							.FirstOrDefault() as DescriptionAttribute;

						if (descriptionAttribute != null)

							return descriptionAttribute.Description;

					}
				}
				return e.ToString();
			}

			return null; // could also return string.Empty
		}

		/// <summary>Start the program. Takes input, parses it and checks new locations of the rovers.</summary>
		/// <remarks>
		/// using an async pattern here. When updating control properties you have to use Invoke().
		/// </remarks>
		public static Task RunSimulation(string input, bool enableCollision, TextBox output, PictureBox graph) => Task.Run(() =>
		{
			try
			{

				var (width, height, rovers) = Utils.ProcessInput(input);

				if (rovers == null || rovers.Count == 0)
				{
					output.Invoke((MethodInvoker)delegate { output.Text = "Input contained no valid commands"; });
					return;
				}

				var plateau = new Plateau(width, height, rovers);

				plateau.Simulate(enableCollision);

				output.Invoke((MethodInvoker)delegate { output.Text = plateau.ReportCurrentPositions(); });

				Draw(graph, plateau);

			}
			catch (ExceptionCollision e)
			{
				output.Invoke((MethodInvoker)delegate { output.Text = $"Collision: {e.Rover.StartingState.X} {e.Rover.StartingState.Y} {Utils.GetDescription(e.Rover.StartingState.Orientation)}"; });
			}
			catch (Exception e)
			{
				output.Invoke((MethodInvoker)delegate { output.Text = e.ToString(); });
			}

		});

		private static void Draw(PictureBox graph, Plateau plateau) => graph.Invoke((MethodInvoker)delegate
		{
			int widthRatio = (graph.Width - 1) / plateau.Width;
			int heightRatio = (graph.Height - 1) / plateau.Height;
			const int size = 10;

			Bitmap bitmap = new Bitmap(graph.Width, graph.Height);

			using (Graphics gfx = Graphics.FromImage(bitmap))
			{

				using (var brush = new SolidBrush(Color.White))
					gfx.FillRectangle(brush, 0, 0, graph.Width, graph.Height);

				graph.Image = bitmap;

				for (int i = 0; i < plateau.Width; i++)
					gfx.DrawLine(
						new Pen(Color.Gray),
						new Point(i * widthRatio, 0),
						new Point(i * widthRatio, graph.Height));

				for (int i = 0; i < plateau.Height; i++)
					gfx.DrawLine(
						new Pen(Color.Gray),
						new Point(0, i * heightRatio),
						new Point(graph.Width, i * heightRatio));

				foreach (var rover in plateau.Rovers)
				{
					gfx.DrawEllipse(
						new Pen(Color.Black),
						new Rectangle(
							new Point(
								widthRatio * rover.StartingState.X - widthRatio / 2 - size / 2,
								graph.Height - heightRatio * rover.StartingState.Y - heightRatio / 2 - size / 2
							),
							new Size(size, size)
						)
					);

					gfx.DrawEllipse(
						new Pen(Color.Black),
						new Rectangle(
							new Point(
								widthRatio * rover.CurrentState.X - widthRatio / 2 - size / 2,
								graph.Height - heightRatio * rover.CurrentState.Y - heightRatio / 2 - size / 2
							),
							new Size(size, size)
						)
					);

					gfx.DrawLines(
						new Pen(Color.Black),
						(
							from state in rover.StateHistory
							select new PointF(
								widthRatio * state.X - widthRatio / 2,
								graph.Height - heightRatio * state.Y - heightRatio / 2
							)
						).ToArray()
					);


				}

			}



		});



	}
}
