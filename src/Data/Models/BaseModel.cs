namespace Models
{
    using System.ComponentModel.DataAnnotations;
    
    public class BaseModel<TKey>
    {
        [Key]
        public TKey Id { get; set; }

        // Max length?
        [Required]
        public string Name { get; set; }
    }
}
