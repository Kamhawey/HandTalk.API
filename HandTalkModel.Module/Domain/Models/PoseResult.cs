using Google.Protobuf;

namespace HandTalkModel.Module.Domain.Models;

public class PoseResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public ByteString RenderedImage  { get; set; } 
    public List<string> Words { get; set; } = [];
    public double TimeTaken { get; set; }
    public string ServerTime { get; set; } = string.Empty;
}
