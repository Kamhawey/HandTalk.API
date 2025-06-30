using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Dictionary.Domain.Models;

public class DictionaryEntry
{
    [Key]
    public long Id { get; set; }
        
    [Required]
    public string Gloss { get; set; }

    public string Source { get; set; }
    [Required]
    public string VideoUrl { get; set; }
        
    public int SearchCount { get; set; } = 0;
        
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
    public DateTime? LastModifiedOn { get; set; }
    
    [InverseProperty(nameof(DictionaryEntry))]
    public virtual ICollection<UserFavoriteGloss> UserFavorites { get; set; } = new List<UserFavoriteGloss>();
}