using System.Xml.Serialization;

namespace Infrastructure.InputModels
{
    [XmlType("XmlSports")]
    public class XmlSportInputModel
    {
        [XmlAttribute("CreateDate")]
        public DateTime CreateDate { get; set; }

        // This could be potentially an array
        [XmlElement("Sport")]
        public SportInputModel Sport { get; set; }
    }
}
