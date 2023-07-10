namespace Infrastructure.Dto.InputModels
{
    using System.Xml.Serialization;

    public class SportInputModel
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("Event")]
        public EventInputModel[] Events { get; set; }
    }
}