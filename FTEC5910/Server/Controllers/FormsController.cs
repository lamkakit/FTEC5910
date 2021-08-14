using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FTEC5910.Server.Data;
using FTEC5910.Shared.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FTEC5910.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly DataContext _db;
        private readonly string _azureConnectionString;

        public FormsController(DataContext db, IConfiguration configuration)
        {
            _db = db;
            _azureConnectionString = configuration.GetConnectionString("AzureConnectionString");

        }

        [HttpPost("SubmitAddressForm")]
        public async Task<IActionResult> SubmitAddressForm() 
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();

                //Upload file
                var file = formCollection.Files.First();
                var attachmentUrl = "";
                if (file.Length > 0)
                {
                    var container = new BlobContainerClient(_azureConnectionString, "ftec5910-attachments");
                    var createResponse = await container.CreateIfNotExistsAsync();
                    if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                        await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                    var blob = container.GetBlobClient($"{Guid.NewGuid().ToString().Replace("-", "")}{Path.GetExtension(file.FileName)}"); //file.FileName
                    await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                    using (var fileStream = file.OpenReadStream())
                    {
                        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
                        attachmentUrl = blob.Uri.ToString();
                    }
                }

                //Save Form
                var addressFormString = formCollection.Where(a => a.Key.Equals("AddressForm")).FirstOrDefault().Value;
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                };
                var addressForm = JsonSerializer.Deserialize<AddressFormModel>(addressFormString, options);

                Form form = new();
                form.FormId = addressForm.FormID;
                form.UserId = addressForm.UserId;
                form.UserName = addressForm.UserName;
                form.AttachmentUrl = attachmentUrl;

                form.Data = JsonSerializer.Serialize(addressForm, options);
                _db.Forms.Add(form);
                _db.SaveChanges();
                return Ok("OK");
            }
            catch (Exception ex)
            {
                return Ok($"error - {ex.Message}");
            }
        }


        [HttpPost("SubmitAddressFormOld")]
        public async Task<IActionResult> SubmitAddressFormOld([FromBody] SubmitAddressFormRequest request)
        {
            try
            {               
                Form form = new();
                form.FormId = request.Data.FormID;
                form.UserId = request.Data.UserId;
                form.UserName = request.Data.UserName;
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                };
                form.Data = JsonSerializer.Serialize(request.Data, options);
                _db.Forms.Add(form);
                _db.SaveChanges();
                return Ok("OK");
            }
            catch (Exception ex) 
            {
                return Ok($"error - {ex.Message}");
            }
        }
    }
}
