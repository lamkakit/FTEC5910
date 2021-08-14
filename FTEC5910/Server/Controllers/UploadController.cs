using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FTEC5910.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string _azureConnectionString;
        public UploadController(IConfiguration configuration)
        {
            _azureConnectionString = configuration.GetConnectionString("AzureConnectionString");
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var x = formCollection.Where(a => a.Key.Equals("AddressForm")).FirstOrDefault().Value;
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                };
                var data = JsonSerializer.Deserialize<AddressFormModel>(x, options);

                //var file = formCollection.Files.First();
                //if (file.Length > 0)
                //{
                //    var container = new BlobContainerClient(_azureConnectionString, "ftec5910-attachments");
                //    var createResponse = await container.CreateIfNotExistsAsync();
                //    if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                //        await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                //    var blob = container.GetBlobClient(file.FileName);
                //    await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                //    using (var fileStream = file.OpenReadStream())
                //    {
                //        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
                //    }
                //    return Ok(blob.Uri.ToString());
                //}
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
