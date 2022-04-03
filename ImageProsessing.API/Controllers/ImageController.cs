using Microsoft.AspNetCore.Mvc;
using ImageProsessing.API.Services.Interfaces;
namespace ImageProsessing.API.Controllers;


[ApiController]
[Route("[Controller]")]
public class ImageController: ControllerBase
{
    private readonly IBlobsManagement _blobsManagement;
    private readonly IQueuesManagement _queuesManagement;
    private readonly IConfiguration _configuration;
    public ImageController(IBlobsManagement blobsManagement, IQueuesManagement queuesManagement, IConfiguration configuration)
    {
        _blobsManagement = blobsManagement;
        _queuesManagement = queuesManagement;
        _configuration = configuration;

    }

    [HttpPost]
    [Route("ImageUpload")]
    public async Task<IActionResult> ImageUpload(IFormFile? image){
        if(image == null){
            return BadRequest();
        }
        await UploadFile(image, 300, 300);
        return Ok("File uploaded");
    }
    [NonAction]
    private async Task UploadFile(IFormFile image, int width, int height ){
        if(image is not { Length: > 0}) return;
        var connectionString = _configuration["AzureStorageConfig:BlobConnectionString"];

        // creating a file byte
        byte[]? fileBytes = null;
        MemoryStream? stream = null;
        await using( stream = new MemoryStream()){
            await image.CopyToAsync(stream);
            fileBytes = stream.ToArray();
        }
        if(fileBytes == null ) return;
        var fileName = Path.GetRandomFileName() + " " + DateTime.UtcNow.ToString("dd/MM/yyyy").Replace("/","_");
        var fileUrl = _blobsManagement.UploadFile("images", fileName, fileBytes, connectionString);
    }
}
