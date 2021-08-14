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
                        content.Add(new StreamContent(ms, Convert.ToInt32(attachmentFile.Size)), "Attachment1", attachmentFile.Name);
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
    }
}
