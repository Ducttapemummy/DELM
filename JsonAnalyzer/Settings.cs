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

    [XmlRoot(ElementName = "attributes")]
    public class Attributes
    {
        [XmlAttribute(AttributeName = "id")]
        public AttributeID ID { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public int Value { get; set; } = 8;

    }
    [XmlRoot(ElementName = "lep")]
    public class Lep
    {
        [XmlAttribute(AttributeName = "max")]
        public int Max { get; set; } = 0;
        [XmlAttribute(AttributeName = "current")]
        public int Current { get; set; } = 0;
    }

    [XmlRoot(ElementName = "asp")]
    public class Asp
    {
        [XmlAttribute(AttributeName = "max")]
        public int Max { get; set; } = 0;
        [XmlAttribute(AttributeName = "current")]
        public int Current { get; set; } = 0;
    }

    [XmlRoot(ElementName = "kap")]
    public class Kap
    {
        [XmlAttribute(AttributeName = "max")]
        public int Max { get; set; } = 0;
        [XmlAttribute(AttributeName = "current")]
        public int Current { get; set; } = 0;
    }

    [XmlRoot(ElementName = "arm")]
    public class Arm
    {
        [XmlAttribute(AttributeName = "current")]
        public int Current { get; set; } = 0;
    }

    [XmlRoot(ElementName = "enc")]
    public class Enc
    {
        [XmlAttribute(AttributeName = "current")]
        public int Current { get; set; } = 0;
    }

    [XmlRoot(ElementName = "mon")]
    public class Mon
    {
        [XmlAttribute(AttributeName = "d")]
        public int D { get; set; } = 0;
        [XmlAttribute(AttributeName = "s")]
        public int S { get; set; } = 0;
        [XmlAttribute(AttributeName = "h")]
        public int H { get; set; } = 0;
        [XmlAttribute(AttributeName = "k")]
        public int K { get; set; } = 0;
    }

    [XmlRoot(ElementName = "pai")]
    public class Pai
    {
        [XmlAttribute(AttributeName = "current")]
        public string Current { get; set; } = "";
    }

    [XmlRoot(ElementName = "char")]
    public class Char
    {
        [XmlElement(ElementName = "attributes")]
        public List<Attributes> Attributes { get; set; } = new List<Attributes>();
        [XmlElement(ElementName = "race")]
        public Race Race { get; set; } = new Race();
        [XmlElement(ElementName = "hp")]
        public Lep Lep { get; set; } = new Lep();
        [XmlElement(ElementName = "asp")]
        public Asp Asp { get; set; } = new Asp();
        [XmlElement(ElementName = "kap")]
        public Kap Kap { get; set; } = new Kap();
        [XmlElement(ElementName = "arm")]
        public Arm Arm { get; set; } = new Arm();
        [XmlElement(ElementName = "enc")]
        public Enc Enc { get; set; } = new Enc();
        [XmlElement(ElementName = "mon")]
        public Mon Mon { get; set; } = new Mon();
        [XmlElement(ElementName = "pai")]
        public Pai Pai { get; set; } = new Pai();
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; } = "";
        public override string ToString()
        {
            return this.Name;
        }

    }

    [XmlRoot(ElementName = "saves")]
    public class Saves
    {
        [XmlElement(ElementName = "char")]
        public List<Char> Char { get; set; }
        [XmlAttribute(AttributeName = "selected")]
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

        public void ToSave(Saves settings)
        {
            XmlSerializer serializer = new XmlSerializer(settings.GetType());
            StringBuilder sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, settings);
            }

            Properties.Settings.Default.SavedResource = sb.ToString();
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            string s = Properties.Settings.Default.SavedResource;
        }

        public static Saves FromSave(string objectData)
        {
            Saves result = new Saves();
            using (TextReader reader = new StringReader(objectData))
            {
                var serializer = new XmlSerializer(typeof(Saves));
                result = (Saves)serializer.Deserialize(reader);
            }

            return result;
        }
    }
}