using FTEC5910.Shared.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;

namespace FTEC5910.Client.Data.Services
{
    public class FormsService
    {
        private readonly HttpClient _http;
        public FormsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> SubmitAddressForm(AddressFormModel form)
        {
            try
            {
                SubmitAddressFormRequest request = new ();
                request.Data = form;

                var submitFormResult = await _http.PostAsJsonAsync("/api/forms/SubmitAddressForm", request);
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
