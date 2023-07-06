namespace Infrastructure.InputModels
{
    using System.Xml.Serialization;
    
    public class MatchInputModel
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("StartDate")]
        public DateTime StartDate { get; set; }

        [XmlAttribute("MatchType")]
        public Models.Enums.MatchType MatchType { get; set; }

        [XmlElement("Bet")]
        public BetInputModel[] Bets { get; set; }
    }
}