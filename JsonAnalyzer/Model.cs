using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace JsonAnalyzer
{
	class Model : UI_helper
	{
		public Model()
		{
			CommandAddChar = new RelayCommand(AddChar, "");
			CommandDeleteChar = new RelayCommand(DeleteChar, "", CanDeleteChar);

			Properties.Settings.Default.Upgrade();
			settings = Saves.FromSave();
			AllChars = settings.GetCharCollection();
			SelectedCharName = settings.Selected;
			foreach(Char c in AllChars)
			{
				if (c.Name == SelectedCharName)
					SelectedChar = c;
			}
			if (SelectedChar == null)
				SelectedChar = AllChars.FirstOrDefault();
		}

        private bool CanDeleteChar(object obj)
        {
			return SelectedChar != null;
        }

        private void DeleteChar(object obj)
        {
			if (MessageBox.Show("Are you sure?", "Delete Character", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				AllChars.Remove(SelectedChar);
				settings.ToSave();
				SelectedChar = AllChars.FirstOrDefault();
			}

        }

        private void AddChar(object obj)
		{
			Char CharToAdd = new Char();
			List<Char> ToDelete = new List<Char>();
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "json Files (*.json)|*.json|All files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == true)
			{
				JObject jobj = JObject.Parse(File.ReadAllText(openFileDialog.FileName));
				foreach (JProperty child in jobj.Children())
				{
					switch (child.Name)
					{
						case "name":
							CharToAdd.Name = (string)child.Value;
							break;
						case "r":
							CharToAdd.Race = (Race)Convert.ToInt32(child.Value.ToString().Trim('R','_'));
							GetRaceLep(CharToAdd);
							break;
						case "attr":
							AttributesToChar(child, CharToAdd);
							break;
						case "activatable":
							ActivatablesToChar(child, CharToAdd);
							break;
						case "belongings":
							ItemsToChar(child, CharToAdd);
							break;
					}
				}
				CharToAdd.Asp.Current = CharToAdd.Asp.Max;
				CharToAdd.Kap.Current = CharToAdd.Kap.Max;
				CharToAdd.Lep.Current = CharToAdd.Lep.Max;



				foreach (Char c in AllChars)
				{   //if already exists it is overwritten
					if (CharToAdd.Name == c.Name) ToDelete.Add(c);
				}
				foreach(Char Delete in ToDelete)
                {
					AllChars.Remove(Delete);
                }

				AllChars.Add(CharToAdd);
				SelectedChar = CharToAdd;
			}
		}

        internal void CharSelectionChanged()
		{
			SelectedCharName = SelectedChar == null ? "":SelectedChar.Name ;
		}

		private void AttributesToChar(JProperty child, Char charToAdd)
		{
			foreach(JProperty prop in child.First.Children())
			{
				switch(prop.Name)
				{
					case "values":
						GetAttributevalues(prop, charToAdd);
						break;
					case "ae":
						charToAdd.Asp.Max += (int)prop.Value;
						break;
					case "kp":
						charToAdd.Kap.Max += (int)prop.Value;
						break;
					case "lp":
						charToAdd.Lep.Max += (int)prop.Value;
						break;

				}
			}
		}

        private void GetAttributevalues(JProperty jprop, Char charToAdd)
        {
            foreach(JObject obj in jprop.First.Children())
            {
				switch(obj["id"].ToString())
                {
					case "ATTR_1":
						charToAdd.Attributes.Add(new Attributes { ID = AttributeID.MU, Value = (int)obj["value"] });
						break;
					case "ATTR_2":
						charToAdd.Attributes.Add(new Attributes { ID = AttributeID.KL, Value = (int)obj["value"] });
						break;
					case "ATTR_3":
						charToAdd.Attributes.Add(new Attributes { ID = AttributeID.IN, Value = (int)obj["value"] });
						break;
					case "ATTR_4":
						charToAdd.Attributes.Add(new Attributes { ID = AttributeID.CH, Value = (int)obj["value"] });
						break;
					case "ATTR_5":
						charToAdd.Attributes.Add(new Attributes { ID = AttributeID.FF, Value = (int)obj["value"] });
						break;
					case "ATTR_6":
						charToAdd.Attributes.Add(new Attributes { ID = AttributeID.GE, Value = (int)obj["value"] });
						break;
					case "ATTR_7":
						charToAdd.Attributes.Add(new Attributes { ID = AttributeID.KO, Value = (int)obj["value"] });
						charToAdd.Lep.Max += ((int)obj["value"]) * 2;   //Life
						charToAdd.Lep.Min -= ((int)obj["value"]);		//Min. life  = -KO
						break;
					case "ATTR_8":
						charToAdd.Attributes.Add(new Attributes { ID = AttributeID.KK, Value = (int)obj["value"] });
						break;
				}
            }
		}

		private void ActivatablesToChar(JProperty jprop, Char charToAdd)
		{
			foreach(JProperty activatable in jprop.First.Children())
            {
				try
				{
					switch (activatable.Name)
					{
						case "ADV_23":  //High ASP
							charToAdd.Asp.Max += Convert.ToInt32(((JValue)activatable.First.First.First.First).Value);
							break;
						case "ADV_24":  //High KAP
							charToAdd.Kap.Max += Convert.ToInt32(((JValue)activatable.First.First.First.First).Value);
							break;
						case "ADV_25":  //High LEP
							charToAdd.Lep.Max += Convert.ToInt32(((JValue)activatable.First.First.First.First).Value);
							break;
						case "DISADV_26":  //Low ASP
							charToAdd.Asp.Max -= Convert.ToInt32(((JValue)activatable.First.First.First.First).Value);
							break;
						case "DISADV_27":  //Low KAP
							charToAdd.Kap.Max -= Convert.ToInt32(((JValue)activatable.First.First.First.First).Value);
							break;
						case "DISADV_28":  //Low LEP
							charToAdd.Lep.Max -= Convert.ToInt32(((JValue)activatable.First.First.First.First).Value);
							break;
						case "ADV_12":  //Holy
							charToAdd.Kap.Max += 20;
							charToAdd.IsHoly = true;
							break;
						case "ADV_50":  //Magic
							charToAdd.Asp.Max += 20;
							charToAdd.IsMagic = true;
							break;
						default:
							TryForTradition(charToAdd, activatable);
							break;
					}
				}
				catch(Exception e) { }
            }
		}

		private void GetRaceLep(Char charToAdd)
		{
			switch(charToAdd.Race)
            {
				case Race.Elf:
					charToAdd.Lep.Max += 2;
					break;
				case Race.Half_Elf:
				case Race.Human:
					charToAdd.Lep.Max += 5;
					break;
				case Race.Dwarf:
					charToAdd.Lep.Max += 8;
					break;
            }
		}

		private void TryForTradition(Char charToAdd,JProperty activatable)
		{
			switch(activatable.Name)
			{
				//Magic, KL
				case "SA_70":
				case "SA_346":
				case "SA_681":
					charToAdd.Asp.Max += charToAdd.Attributes.Find((a) => a.ID == AttributeID.KL).Value;
					break;
				//Magic, KL/2, Round Up
				case "SA_750":
					charToAdd.Asp.Max += (int)Math.Ceiling(charToAdd.Attributes.Find((a) => a.ID == AttributeID.KL).Value/2.0);
					break;
				//Magic, IN
				case "SA_345":
					charToAdd.Asp.Max += charToAdd.Attributes.Find((a) => a.ID == AttributeID.IN).Value;
					break;
				//Magic, CH
				case "SA_255":
				case "SA_676":
					charToAdd.Asp.Max += charToAdd.Attributes.Find((a) => a.ID == AttributeID.CH).Value;
					break;
				//Magic, CH/2, Round Up
				case "SA_677":
					charToAdd.Asp.Max += (int)Math.Ceiling(charToAdd.Attributes.Find((a) => a.ID == AttributeID.CH).Value / 2.0);
					break;
				//Holy, MU
				case "SA_682":
				case "SA_683":
				case "SA_689":
				case "SA_693":
				case "SA_696":
				case "SA_698":
					charToAdd.Kap.Max += charToAdd.Attributes.Find((a) => a.ID == AttributeID.MU).Value;
					break;
				//Holy, KL
				case "SA_86":
				case "SA_684":
				case "SA_688":
				case "SA_697":
				case "SA_1049":
					charToAdd.Kap.Max += charToAdd.Attributes.Find((a) => a.ID == AttributeID.KL).Value;
					break;
				//Holy, IN
				case "SA_685":
				case "SA_686":
				case "SA_691":
				case "SA_694":
					charToAdd.Kap.Max += charToAdd.Attributes.Find((a) => a.ID == AttributeID.IN).Value;
					break;
				//Holy, CH
				case "SA_687":
				case "SA_692":
				case "SA_695":
				case "SA_690":
					charToAdd.Kap.Max += charToAdd.Attributes.Find((a) => a.ID == AttributeID.CH).Value;
					break;
			}
			/*Magic:
			SA_70 = Gildenmagier, KL
			SA_346 = Druiden, KL
			SA_681 = Qabalyamagier, KL
			SA_750 = Zauberalchemist, KL/2
			SA_345 = Elfen, IN
			SA_255 = Hexen, CH
			SA_676 = Scharlatan, CH
			SA_677 = Zauberbarde, CH/2
			SA_679 = Intuitiver Zauberer, -
			SA_680 = Meistertalent, -

			Holy:
			SA_682 = Rondra, MU
			SA_683 = Boron, MU
			SA_689 = Firun, MU
			SA_693 = Namenloser, MU
			SA_696 = Kor, MU
			SA_698 = Swafnir, MU
			SA_86 = Praios, KL
			SA_684 = Hesinde, KL
			SA_688 = Travia, KL
			SA_697 = Nandus, KL
			SA_1049 = Numinor, KL
			SA_685 = Phex, IN
			SA_686 = Peraine, IN
			SA_691 = Ingerimm, IN
			SA_694 = Aves, IN
			SA_687 = Efferd, CH
			SA_690 = Tsa, CH
			SA_692 = Rahja, CH
			SA_695 = Ifirn, CH
			*/
		}

		private void ItemsToChar(JProperty belongings, Char charToAdd)
		{
			JProperty Armor = null;
			foreach (JProperty bel in belongings.First.Children())
			{
				if (bel.Name == "items")
				{
					foreach (JProperty item in bel.First.Children())
					{
						foreach (JProperty prop in item.First.Children())
						{
							if (prop.Name == "armorType")
							{
								Armor = item;
								break;
							}
						}
						if (Armor != null)
							break;
					}
					if (Armor != null)
					{
						foreach (JProperty prop in Armor.First.Children())
						{
							if (prop.Name == "enc")
							{
								charToAdd.Enc.Current = (int)prop.Value;
							}
							else if (prop.Name == "pro")
							{
								charToAdd.Arm.Current = (int)prop.Value;
							}
						}
					}
				}
				else if(bel.Name =="purse")
				{
					foreach(JProperty coin in bel.First.Children())
					{
						if(coin.Value.ToString() != String.Empty)
						switch(coin.Name)
						{
							case "d":
								charToAdd.Mon.D = (int)coin.Value;
								break;
							case "s":
								charToAdd.Mon.S = (int)coin.Value;
								break;
							case "h":
								charToAdd.Mon.H = (int)coin.Value;
								break;
							case "k":
								charToAdd.Mon.K = (int)coin.Value;
								break;
						}
					}
				}
			}
		}

		public void DoClosingStuff(object sender, System.ComponentModel.CancelEventArgs e)
		{
			settings.Char = AllChars.ToList();
			settings.Selected = SelectedCharName;

			settings.ToSave();
		}


		public Saves settings = new Saves();

		public bool TopMost { get => topMost; set => SetAndNotify(ref topMost, value); }
		private bool topMost = false;

		public Char SelectedChar { get => selectedChar; set => SetAndNotify(ref selectedChar, value); }
		private Char selectedChar;

		public string SelectedCharName { get => selectedCharName; set => SetAndNotify(ref selectedCharName, value); }
		private string selectedCharName = null;

		public ObservableCollection<Char> AllChars { get => allChars; set => SetAndNotify(ref allChars, value); }
		private ObservableCollection<Char> allChars = new ObservableCollection<Char>();

		public RelayCommand CommandAddChar {get; set; }
		public RelayCommand CommandDeleteChar { get; set; }

		public Brush WindowBackColor { get => windowBackcolor; set => SetAndNotify(ref windowBackcolor, value); }
		private Brush windowBackcolor = Brushes.DarkGray;

	}
	public enum Race
	{
		Human = 1,		//Le = 5 //1
		Elf = 2,		//Le = 2 //2
		Half_Elf = 3,	//Le = 5 //3
		Dwarf = 4		//Le = 8 //4
	}

	public enum AttributeID
	{
		MU = 1,
		KL = 2,
		IN = 3,
		CH = 4,
		FF = 5,
		GE = 6,
		KO = 7,
		KK = 8
	}
}
