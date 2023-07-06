using System.Xml.Serialization;

namespace Infrastructure.InputModels
{
    public class EventInputModel
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("IsLive")]
        public bool IsLive { get; set; }

        [XmlAttribute("CategoryID")]
        public int CategoryId { get; set; }

        [XmlElement("Match")]
        public MatchInputModel[] Matches { get; set; }

    }
}