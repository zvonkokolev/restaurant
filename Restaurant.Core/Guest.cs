using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Core
{
	public class Guest
	{
		private string _name;
		public Guest(string name)
		{
			Name = name;
		}

		public string Name { get => _name; set => _name = value; }
	}
}
