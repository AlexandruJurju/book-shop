namespace BuildingBlocks.Application.FileStorage;

public record FileDownloadOutput(
    byte[] Data,
    string ContentType
);
