namespace Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BaseModel<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public TKey Id { get; set; }

        // Max length?
        [Required]
        public string Name { get; set; }
    }
}
