namespace Shared.DTOs.HandTalk;

public class GlossToPoseResponse
{

    public byte[] ImageBytes { get; set; } = [];
    
    public List<string> Words { get; set; } = [];
    
    public double TimeTaken { get; set; }
}