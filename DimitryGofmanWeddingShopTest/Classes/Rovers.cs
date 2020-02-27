using System.Collections.Generic;

namespace DimitryGofmanWeddingShopTest.Classes
{
	/// <summary>Just a list of Rovers, this class could be used to reorganise priority of rovers, which one received commands first</summary>
	public class Rovers : List<Rover>
	{

		public Rovers(List<Rover> lst)
			: base(lst)
		{
		}


	}
}
