using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JsonAnalyzer
{
    public class Attributes
    {
        public AttributeID ID { get; set; }
        public int Value { get; set; } = 8;

    }
    public class Lep
    {
        public int Max { get; set; } = 0;
        public int Current { get; set; } = 0;
    }

    public class Asp
    {
        public int Max { get; set; } = 0;
        public int Current { get; set; } = 0;
    }

    public class Kap
    {
        public int Max { get; set; } = 0;
        public int Current { get; set; } = 0;
    }

    public class Arm
    {
        public int Current { get; set; } = 0;
    }

    public class Enc
    {
        public int Current { get; set; } = 0;
    }

    public class Mon
    {
        public int D { get; set; } = 0;
        public int S { get; set; } = 0;
        public int H { get; set; } = 0;
        public int K { get; set; } = 0;
    }

    public class Pai
    {
        public string Current { get; set; } = "";
    }

    public class Char
    {
        public List<Attributes> Attributes { get; set; } = new List<Attributes>();
        public Race Race { get; set; } = new Race();
        public int Profession { get; set; }
        public Lep Lep { get; set; } = new Lep();
        public Asp Asp { get; set; } = new Asp();
        public Kap Kap { get; set; } = new Kap();
        public Arm Arm { get; set; } = new Arm();
        public Enc Enc { get; set; } = new Enc();
        public Mon Mon { get; set; } = new Mon();
        public Pai Pai { get; set; } = new Pai();
        public bool IsMagic { get; set; } = false;
        public bool IsHoly { get; set; } = false;
        public string Name { get; set; } = "";
        public override string ToString()
        {
            return this.Name;
        }

    }

    public class Saves
    {
        public List<Char> Char { get; set; }
        public string Selected { get; set; }



        public ObservableCollection<Char> GetCharCollection()
        {
            ObservableCollection<Char> CharCollection = new ObservableCollection<Char>();
            foreach (Char c in Char)
            {
                CharCollection.Add(c);
            }
            return CharCollection;
        }

        public void ToSave()
        {
            Properties.Settings.Default.SavedResource = JsonConvert.SerializeObject(this);
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
        }

        public static Saves FromSave()
        {

            return JsonConvert.DeserializeObject<Saves>(Properties.Settings.Default.SavedResource);
        }
    }
}