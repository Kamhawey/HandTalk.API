namespace HandTalkModel.Module.Extensions;

public static class HandTalkEndpointConstants
{
    private const string ApiBase = "/api";
    private const string HandTalkBase = $"{ApiBase}/handtalk";
    
    public static class Routes
    {
        public const string GlossToPose = $"{HandTalkBase}/gloss-to-pose";
        public const string Health = $"{HandTalkBase}/health";
        public const string Version = $"{HandTalkBase}/version";
    }
    
    public static class Tags
    {
        public const string HandTalk = "HandTalk";
        public const string Health = "Health";
    }
}