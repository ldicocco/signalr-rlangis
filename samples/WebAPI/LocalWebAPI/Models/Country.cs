using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalWebAPI.Models
{
//	[System.Runtime.Serialization.DataContract]
	public class Country
	{
//		[System.Runtime.Serialization.DataMember]
		public string Name { get; set; }
//		[System.Runtime.Serialization.DataMember]
		public int Population { get; set; }

		public Country(string name, int population)
		{
			Name = name;
			Population = population;
		}
	}
}
