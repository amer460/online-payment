using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Text;
using XCoreAssignment.Helpers;
using XCoreAssignment.Models;
using XCoreAssignment.ViewModels.Utility;

namespace XCoreAssignment.Services
{
    public interface IUtilityService
    {
        Task<ViewResultDTO> PayAsync(UtilityTemplateVM vm);
        ViewResultDTO Template();
    }

    public class UtilityService : IUtilityService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;


        public UtilityService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public ViewResultDTO Template()
        {
            var vm = new UtilityTemplateVM();
            return new ViewResultDTO("Template", vm);
        }

        public async Task<ViewResultDTO> PayAsync(UtilityTemplateVM vm)
        {

            StringBuilder errorMessage = new();

            var minimumSendingAmountStr = _configuration.GetValue<string>("MinimumSendingAmount");
            if (!double.TryParse(minimumSendingAmountStr, NumberStyles.Currency, CultureInfo.InvariantCulture, out var minimumSendingAmount))
                errorMessage.AppendLine("Minimum sending amount not defined or not set correctly.");

            var maximumSendingAmountStr = _configuration.GetValue<string>("MaximumSendingAmount");
            if (!double.TryParse(maximumSendingAmountStr, NumberStyles.Currency, CultureInfo.InvariantCulture, out var maximumSendingAmount))
                errorMessage.AppendLine("Maximum sending amount not defined or not set correctly.");

            if (errorMessage.Length > 0)
            {
                vm.ErrorMessage = errorMessage.ToString();
                return new ViewResultDTO("Pay", vm);
            }

            double amount = double.Parse(vm.Amount, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"));
            if (amount < minimumSendingAmount)
                errorMessage.AppendLine($"Sending amount is lower than defined minimum amount. ({minimumSendingAmount})");

            if (amount > maximumSendingAmount)
                errorMessage.AppendLine($"Sending amount is higher than defined maximum amount. ({maximumSendingAmount})");

            if (errorMessage.Length > 0)
            {
                return new ViewResultDTO("Suprice");
            }

            var successAPIUrl = _configuration.GetValue<string>("SuccessAPIUrl");
            if (string.IsNullOrWhiteSpace(successAPIUrl))
                return new ViewResultDTO("Exception", "API url is null or empty");

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(successAPIUrl);

            if (!response.IsSuccessStatusCode)
                return new ViewResultDTO("Exception", $"Error calling API -> {successAPIUrl}");


            var responseJSON = await response.Content.ReadAsStringAsync();
            JokeDTO? joke;

            try
            {
                joke = JsonConvert.DeserializeObject<JokeDTO>(responseJSON);
                joke ??= new();
            }
            catch (Exception)
            {
                return new ViewResultDTO("Exception", "Error parsing joke response");

            }

            return new ViewResultDTO("Joke", joke);
        }
    }
}
