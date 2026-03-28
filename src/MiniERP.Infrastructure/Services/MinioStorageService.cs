using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using MiniERP.Domain.Interfaces;

namespace MiniERP.Infrastructure.Services;

public class MinioStorageService : IStorageService
{
    private readonly AmazonS3Client _s3Client;
    private readonly string _bucketName;

    public MinioStorageService(IConfiguration configuration)
    {
        var endpoint = configuration["MinioSettings:Endpoint"];
        var accessKey = configuration["MinioSettings:AccessKey"];
        var secretKey = configuration["MinioSettings:SecretKey"];
        _bucketName = configuration["MinioSettings:BucketName"]!;

        var config = new AmazonS3Config
        {
            ServiceURL = $"http://{endpoint}",
            ForcePathStyle = true
        };

        _s3Client = new AmazonS3Client(accessKey, secretKey, config);
    }

    public async Task<string> UploadArquivoAsync(string nomeArquivo, Stream arquivoStream, string contentType)
    {
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);
        if (!bucketExists)
        {
            await _s3Client.PutBucketAsync(_bucketName);
        }

        var nomeUnico = $"{Guid.NewGuid()}-{nomeArquivo}";

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = arquivoStream,
            Key = nomeUnico,
            BucketName = _bucketName,
            ContentType = contentType
        };

        using var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"{_bucketName}/{nomeUnico}";
    }
}