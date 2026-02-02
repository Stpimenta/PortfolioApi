using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace PortfolioApi.Application.Services;

public class AmazonS3Service
{
    
    private readonly IAmazonS3 _client;
    private readonly string _bucketName;

    public AmazonS3Service(IAmazonS3 client, IConfiguration configuration)
    {
        _client = client;
        _bucketName = configuration["AWS:BucketName"] ?? throw new ArgumentNullException("bucket name is not configured");
    }

    public async Task<string> UploadFile(Stream fileStream, string keyName)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = keyName,
            BucketName = _bucketName,
            // CannedACL = S3CannedACL.PublicRead 
        };

        var transferUtility = new TransferUtility(_client);
        await transferUtility.UploadAsync(uploadRequest);
        
        return keyName;
    }

    public async Task DeleteFileAsync(string keyName)
    {
        var deleteRequest = new Amazon.S3.Model.DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName
        };

        await _client.DeleteObjectAsync(deleteRequest);
    }
    
    
    public async Task<string> GetFileAsStringAsync(string keyName)
    {
        var response = await _client.GetObjectAsync(_bucketName, keyName);
        using var reader = new StreamReader(response.ResponseStream);
        return await reader.ReadToEndAsync();
    }

}