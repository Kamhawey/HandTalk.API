using Google.Protobuf;
using HandTalkModel.Module.Services;
using Shared.Core.CQRS;
using Shared.DTOs.Common;
using Shared.DTOs.Common.Response;
using Shared.DTOs.HandTalk;

namespace HandTalkModel.Module.Features.GlossToPoseFeature.Handler;

public record GlossToPoseQuery(string Gloss) : IQuery<Result<GlossToPoseResponse>>;

public class GlossToPoseQueryHandler : IQueryHandler<GlossToPoseQuery, Result<GlossToPoseResponse>>
{
    private readonly IHandTalkService _handTalkService;

    public GlossToPoseQueryHandler(IHandTalkService handTalkService)
    {
        _handTalkService = handTalkService;
    }

    public async Task<Result<GlossToPoseResponse>> Handle(GlossToPoseQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return new GlossToPoseResponse();
            var poseResult = await _handTalkService.GlossToPoseAsync(request.Gloss);
        
            if (poseResult == null || !poseResult.Success || poseResult.RenderedImage is null)
            {
                return ErrorCode.HandTalkGlossNotFound;
            }
        
            var response = new GlossToPoseResponse
            {
                ImageBytes = poseResult.RenderedImage.ToByteArray(),
                Words = poseResult.Words,
                TimeTaken = poseResult.TimeTaken
            };
        
            return Result.Success(response);
        }
        catch (System.Exception ex)
        {
            return ErrorCode.HandTalkProcessingError;
        }
    }
}

public record HandTalkHealthQuery() : IQuery<bool>;

public class HandTalkHealthQueryHandler : IQueryHandler<HandTalkHealthQuery, bool>
{
    private readonly IHandTalkService _handTalkService;

    public HandTalkHealthQueryHandler(IHandTalkService handTalkService)
    {
        _handTalkService = handTalkService;
    }

    public async Task<bool> Handle(HandTalkHealthQuery request, CancellationToken cancellationToken)
    {
        return await _handTalkService.IsHealthyAsync();
    }
}

public record HandTalkVersionQuery() : IQuery<string>;

public class HandTalkVersionQueryHandler : IQueryHandler<HandTalkVersionQuery, string>
{
    private readonly IHandTalkService _handTalkService;

    public HandTalkVersionQueryHandler(IHandTalkService handTalkService)
    {
        _handTalkService = handTalkService;
    }

    public async Task<string> Handle(HandTalkVersionQuery request, CancellationToken cancellationToken)
    {
        return await _handTalkService.GetVersionAsync();
    }
}