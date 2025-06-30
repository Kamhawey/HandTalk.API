namespace HandTalkModel.Module.Extensions;

public class HandTalkOptions
{ 
    public const string SectionName = "TextToPoseModel";
    public string GrpcBaseUrl { get; set; } = "http://localhost:50051";
}