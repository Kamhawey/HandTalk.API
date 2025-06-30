namespace Shared.DTOs.Dictionary.Favorites;

public class UserFavoriteGlossDto
{
    public long Id { get; set; }
    public long DictionaryEntryId { get; set; }
    public string Gloss { get; set; }
    public string VideoUrl { get; set; }
    public DateTime FavoritedOn { get; set; }
}