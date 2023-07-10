namespace Infrastructure.Dto.InputModels
{
    using System.Xml.Serialization;
    
    public class OddInputModel
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }

        [XmlAttribute("SpecialBetValue")]
        public string SpecialBetValue { get; set; }
    }
}