using HandTalkModel.Module.Domain.Models;

namespace HandTalkModel.Module.Services;

public interface IHandTalkService
{
    Task<PoseResult> GlossToPoseAsync(string gloss);
    Task<bool> IsHealthyAsync();
    Task<string> GetVersionAsync();
}