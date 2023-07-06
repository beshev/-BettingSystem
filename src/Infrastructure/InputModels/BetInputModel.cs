namespace Infrastructure.InputModels
{
    using System.Xml.Serialization;

    public class BetInputModel
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("IsLive")]
        public bool IsLive { get; set; }

        [XmlElement("Odd")]
        public OddInputModel[] Odds { get; set; }
    }
}