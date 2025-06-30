namespace Module.Dictionary.Extensions;

public static class DictionaryEndpointConstants
{
    private const string ApiBase = "/api";
    private const string DictionaryBase = $"{ApiBase}/Dictionary";
        
    public static class Routes
    {
        public const string Search = $"{DictionaryBase}/Search";
        public const string SearchHistory = $"{DictionaryBase}/SearchHistory";
        public const string Create = $"{DictionaryBase}";
        public const string Update = $"{DictionaryBase}";
        public const string Delete = $"{DictionaryBase}/{{id}}";
        public const string GetPopularGlosses = $"{DictionaryBase}/Popular";
        public const string GetAll = $"{DictionaryBase}/GetAll";
     

        public const string GetUserFavorites = $"{DictionaryBase}/Favorites";
        public const string ToggleFavorite = $"{DictionaryBase}/Favorites/Toggle";
    }
        
    public static class Tags
    {
        public const string Dictionary = "Dictionary";
    }
}