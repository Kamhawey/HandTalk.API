using Grpc.Net.Client;
using HandTalkModel.Module.Domain.Models;
using HandTalkModel.Module.Extensions;
using HandTalkModel.Module.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HandTalkModel.Module.Infrastructure.Services;

public class HandTalkService : IHandTalkService, IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly Handtalk.HandTalk.HandTalkClient _client;
    private readonly ILogger<HandTalkService> _logger;
    private readonly HandTalkOptions _options;

    public HandTalkService(
        IOptions<HandTalkOptions> options,
        ILogger<HandTalkService> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        _channel = GrpcChannel.ForAddress(_options.GrpcBaseUrl);
        _client = new Handtalk.HandTalk.HandTalkClient(_channel);
    }

    public async Task<PoseResult> GlossToPoseAsync(string gloss)
    {
        try
        {
            _logger.LogInformation("Converting gloss to pose via gRPC: {Gloss}", gloss);
            
            var request = new Handtalk.GlossRequest
            {
                Gloss = gloss,
                Format = Handtalk.GlossRequest.Types.OutputFormat.RenderedImage
            };
            
            var response = await _client.GlossToPoseAsync(request);
            
            if (!response.Success)
            {
                _logger.LogWarning("HandTalk API returned unsuccessful response: {ErrorMessage}", response.ErrorMessage);
            }
            
            return new PoseResult
            {
                Success = response.Success,
                ErrorMessage = response.ErrorMessage,
                RenderedImage = response.RenderedImage,
                Words = response.Words.ToList(),
                TimeTaken = response.ProcessingTime,
                ServerTime = response.ServerTime
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling HandTalk gRPC API for gloss: {Gloss}", gloss);
            throw;
        }
    }

    public async Task<bool> IsHealthyAsync()
    {
        try
        {
            var request = new Handtalk.HealthRequest();
            var response = await _client.HealthCheckAsync(request);
            return response.Status.Equals("healthy", StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking HandTalk gRPC API health");
            return false;
        }
    }

    public async Task<string> GetVersionAsync()
    {
        try
        {
            var request = new Handtalk.VersionRequest();
            var response = await _client.GetVersionAsync(request);
            return response.Version;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting HandTalk gRPC API version");
            return "Unknown";
        }
    }
    
    public void Dispose()
    {
        _channel?.Dispose();
    }
}