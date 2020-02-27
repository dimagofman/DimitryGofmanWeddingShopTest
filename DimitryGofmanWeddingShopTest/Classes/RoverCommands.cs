using System;
using System.Collections.Generic;

namespace DimitryGofmanWeddingShopTest.Classes
{
	/// <summary>A list of commands for a Rover, <see cref="Utils.Command"/></summary>
	public class RoverCommands : List<Utils.Command>
	{
		public RoverCommands()
			: base()
		{
		}

		public RoverCommands(IEnumerable<Utils.Command> lst)
			: base(lst)
		{
		}

		/// <summary>Parse a string of L R and M into a list of commands</summary>
		/// <returns>I like the TryParse pattern, I think it fits here</returns>
		public static bool TryParse(string input, out RoverCommands nav)
		{

			nav = default;

			if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(input);

			var lst = new List<Utils.Command>();
			foreach (var chr in input.Trim().ToUpperInvariant().ToCharArray())
			{
				switch (chr)
				{
					case 'L':
						lst.Add(Utils.Command.Left);
						break;
					case 'R':
						lst.Add(Utils.Command.Right);
						break;
					case 'M':
						lst.Add(Utils.Command.Move);
						break;
					default:
						return false;
				}
			}

			if (lst.Count == 0) return false;

			nav = new RoverCommands(lst);

			return true;

		}

	}
}
