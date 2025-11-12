using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using PracticalWork.Library.Abstractions.Services.Infrastructure;

namespace PracticalWork.Library.Data.Minio;

public class MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioOptions _minioOptions;
    
    public MinioFileStorageService(IOptions<MinioOptions> options)
    {
        _minioOptions = options.Value;
        _minioClient = new MinioClient()
            .WithEndpoint(_minioOptions.Endpoint)
            .WithCredentials(_minioOptions.AccessKey, _minioOptions.SecretKey)
            .WithSSL(_minioOptions.UseSsl)
            .Build();
    }

    public async Task<string> UploadFileAsync(string path, Stream stream, string contentType,
        CancellationToken cancellationToken = default)
    {
        await EnsureBucketExistsAsync(_minioOptions.BucketName, cancellationToken);
        
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_minioOptions.BucketName)
            .WithObject(path)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType);
        
        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

        var filePath = $"{_minioOptions.Endpoint}/{_minioOptions.BucketName}/{path}";
        
        return filePath;

    }

    public async Task<string> GetFilePathAsync(string path, CancellationToken cancellationToken = default)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(_minioOptions.BucketName)
            .WithObject(path);
        
        var url = await _minioClient.PresignedGetObjectAsync(args);

        return url;
    }

    public async Task DeleteFileAsync(string path, CancellationToken cancellationToken = default)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(_minioOptions.BucketName)
            .WithObject(path);

        await _minioClient.RemoveObjectAsync(args, cancellationToken);
    }

    public async Task<bool> ExistsFileAsync(string path, CancellationToken cancellationToken = default)
    {
        try
        {
            await _minioClient.StatObjectAsync(
                new StatObjectArgs()
                    .WithBucket(_minioOptions.BucketName)
                    .WithObject(path), cancellationToken);

            return true;
        }
        catch (ObjectNotFoundException)
        {
            return false;
        }
    }
    
    private async Task EnsureBucketExistsAsync(string bucketName, CancellationToken cancellationToken = default)
    {
        var existsArgs = new BucketExistsArgs()
            .WithBucket(bucketName);
        
        bool exists = await _minioClient.BucketExistsAsync(existsArgs, cancellationToken);

        if (!exists)
        {
            var makeArgs = new MakeBucketArgs().WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(makeArgs, cancellationToken);
        }
    }
}