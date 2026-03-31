using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using MiniERP.Domain.Interfaces;

namespace MiniERP.Infrastructure.Services;

public class S3StorageService : IStorageService
{
    private readonly AmazonS3Client _s3Client;
    private readonly string _bucketName;

    public S3StorageService(IConfiguration configuration)
    {
        var endpoint = configuration["StorageSettings:Endpoint"];
        var accessKey = configuration["StorageSettings:AccessKey"];
        var secretKey = configuration["StorageSettings:SecretKey"];
        var region = configuration["StorageSettings:Region"] ?? "us-east-1";
        
        _bucketName = configuration["StorageSettings:BucketName"]!;

        var config = new AmazonS3Config();

        if (!string.IsNullOrWhiteSpace(endpoint))
        {
            if (!endpoint.StartsWith("http"))
            {
                endpoint = $"http://{endpoint}";
            }
            
            config.ServiceURL = endpoint;
            config.ForcePathStyle = true;
        }
        else
        {
            config.RegionEndpoint = RegionEndpoint.GetBySystemName(region);
        }

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