//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JsonAnalyzer
//{
//	[Serializable]
//	public class Lep
//	{
//		public int Max { get; set; } = 0;
//		public int Current { get; set; } = 0;
//	}

//	[Serializable]
//	public class Asp
//	{
//		public int Max { get; set; } = 0;
//		public int Current { get; set; } = 0;
//	}

//	[Serializable]
//	public class Kap
//	{
//		public int Max { get; set; } = 0;
//		public int Current { get; set; } = 0;
//	}

//	[Serializable]
//	public class Arm
//	{
//		public int Current { get; set; } = 0;
//	}

//	[Serializable]
//	public class Enc
//	{
//		public int Current { get; set; } = 0;
//	}

//	[Serializable]
//	public class Mon
//	{
//		public int D { get; set; } = 0;
//		public int S { get; set; } = 0;
//		public int H { get; set; } = 0;
//		public int K { get; set; } = 0;
//	}

//	[Serializable]
//	public class Pai
//	{
//		public string Current { get; set; } = "";
//	}

//	[Serializable]
//	public class Char
//	{
//		public Lep Lep { get; set; } = new Lep();
//		public Asp Asp { get; set; } = new Asp();
//		public Kap Kap { get; set; } = new Kap();
//		public Arm Arm { get; set; } = new Arm();
//		public Enc Enc { get; set; } = new Enc();
//		public Mon Mon { get; set; } = new Mon();
//		public Pai Pai { get; set; } = new Pai();
//		public string Name { get; set; } = "";
//		public override string ToString()
//		{
//			return this.Name;
//		}
//		public Char()
//		{

//		}
//	}

//	[Serializable]
//	public class User_Save
//	{
//		public List<Char> Char { get; set; }
//		public string Selected { get; set; }

//		public User_Save()
//		{
//			Char = new List<Char>();
//			Selected = "";
//		}
//	}
//}
