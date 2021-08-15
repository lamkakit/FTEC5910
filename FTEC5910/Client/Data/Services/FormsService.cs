using FTEC5910.Shared.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.StaticFiles;

namespace FTEC5910.Client.Data.Services
{
    public class FormsService
    {
        private readonly HttpClient _http;
        public FormsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> SubmitAddressForm(AddressFormModel form, IBrowserFile attachmentFile)
        {
            try
            {
                //SubmitAddressFormRequest request = new ();
                //request.Data = form;

                //var submitFormResult = await _http.PostAsJsonAsync("/api/forms/SubmitAddressForm", request);
                //var submitFormContent = await submitFormResult.Content.ReadAsStringAsync();
                //return submitFormContent;
                var content = new MultipartFormDataContent();
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");

                if (attachmentFile != null)
                {
                    using (var ms = attachmentFile.OpenReadStream(attachmentFile.Size))
                    {
                        string contentType;
                        new FileExtensionContentTypeProvider().TryGetContentType(attachmentFile.Name, out contentType);
                        content.Add(new StreamContent(ms, Convert.ToInt32(attachmentFile.Size)) , "Attachment1", attachmentFile.Name);
                        content.ElementAt(0).Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    }
                }
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                };
                content.Add(new StringContent(JsonSerializer.Serialize(form, options), System.Text.Encoding.UTF8), "AddressForm");

                var submitFormResult = await _http.PostAsync("/api/forms/SubmitAddressForm", content);
                var submitFormContent = await submitFormResult.Content.ReadAsStringAsync();
                return submitFormContent;

            }
            catch (Exception ex)
            {
                return $"Failed - {ex.Message}";
            }
        }

        public async Task<List<AddressFormModel>> GetAddressForms()
        {
            try
            {
                var getAddressFormsResult = await _http.GetAsync($"/api/forms/getAddressForms");
                var getAddressFormsContent = await getAddressFormsResult.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                };

                var result = JsonSerializer.Deserialize<List<AddressFormModel>>(getAddressFormsContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result;
            }
            catch (Exception ex)
            {
                return new List<AddressFormModel>();
            }
        }
    }
}
