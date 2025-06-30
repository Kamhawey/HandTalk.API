using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Dictionary.Domain.Models;

public class UserFavoriteGloss
{
    [Key]
    public long Id { get; set; }
        
    [Required]
    public long UserId { get; set; }
        
    [Required]
    public long DictionaryEntryId { get; set; }
        
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
    [ForeignKey(nameof(DictionaryEntryId))]
    public virtual DictionaryEntry DictionaryEntry { get; set; }
}
