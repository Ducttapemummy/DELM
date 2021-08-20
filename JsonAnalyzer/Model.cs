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
using System.Windows.Media;

namespace JsonAnalyzer
{
	class Model : UI_helper
	{
		public Model()
		{
			CommandAddChar = new RelayCommand(AddChar, "");

			Properties.Settings.Default.Upgrade();
			settings = Saves.FromSave(Properties.Settings.Default.SavedResource);
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

		private void AddChar(object obj)
		{
			Char CharToAdd = new Char();
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
						case "attr":
							AttributesToChar(child, CharToAdd);
							break;
						case "belongings":
							ItemsToChar(child, CharToAdd);
							break;
					}
				}
				foreach(Char c in AllChars)
				{	//if already exists it is overwritten
					if (CharToAdd.Name == c.Name) AllChars.Remove(c);
				}
				AllChars.Add(CharToAdd);
				SelectedChar = CharToAdd;
			}
		}

		internal void CharSelectionChanged()
		{
			SelectedCharName = SelectedChar.Name;
		}

		private void AttributesToChar(JProperty child, Char charToAdd)
		{
			foreach(JProperty prop in child.First.Children())
			{
				switch(prop.Name)
				{
					case "ae":
						charToAdd.Asp.Max = (int)prop.Value;
						break;
					case "kp":
						charToAdd.Kap.Max = (int)prop.Value;
						break;
					case "lp":
						charToAdd.Lep.Max = (int)prop.Value;
						break;

				}
			}
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

			settings.ToSave(settings);
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

		public RelayCommand CommandAddChar {get; set;}
	}
}
