using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Dictionary.Domain.Models;

public class UserSearchHistory
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public long UserId { get; set; }
    
    [Required]
    public long MatchedDictionaryEntryId { get; set; }
    
    [ForeignKey(nameof(MatchedDictionaryEntryId))]
    public virtual DictionaryEntry MatchedDictionaryEntry { get; set; }
    
    [Required]
    public string SearchText { get; set; } = string.Empty;
    
    public DateTime SearchDate { get; set; } = DateTime.UtcNow;
}