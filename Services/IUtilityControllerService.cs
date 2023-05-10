using Application.HelperModels;
using Application.Utility;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using XCoreAssignment.ViewModels.Utility;

namespace XCoreAssignment.Services;

public interface IUtilityControllerService
{
    Task<ViewResultHelper> TemplatePostAsync(UtilityTemplateVM vm);
    ViewResultHelper TemplateGet();
}

public class UtilityControllerService : IUtilityControllerService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;


    public UtilityControllerService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public ViewResultHelper TemplateGet()
    {
        var vm = new UtilityTemplateVM();
        return new ViewResultHelper("Template", vm);
    }

    public async Task<ViewResultHelper> TemplatePostAsync(UtilityTemplateVM vm)
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
            return new ViewResultHelper("Template", vm);
        }

        double amount = double.Parse(vm.Amount, NumberStyles.Number, CultureInfo.GetCultureInfo("en-US"));
        if (amount < minimumSendingAmount)
            errorMessage.AppendLine($"Sending amount is lower than defined minimum amount. ({minimumSendingAmount})");

        if (amount > maximumSendingAmount)
            errorMessage.AppendLine($"Sending amount is higher than defined maximum amount. ({maximumSendingAmount})");

        if (errorMessage.Length > 0)
        {
            return new ViewResultHelper("Suprice");
        }

        var successAPIUrl = _configuration.GetValue<string>("SuccessAPIUrl");
        if (string.IsNullOrWhiteSpace(successAPIUrl))
            return new ViewResultHelper("Exception", "API url is null or empty");

        var httpClient = _httpClientFactory.CreateClient();
        HttpResponseMessage response = await httpClient.GetAsync(successAPIUrl);

        //if it returns null on the setup, try multiple times
        JokeDTO? joke = null;
        for (int i = 0; i < 3; i++)
        {
            var responseJSON = await response.Content.ReadAsStringAsync();

            try
            {
                joke = JsonConvert.DeserializeObject<JokeDTO>(responseJSON);
                joke ??= new();
            }
            catch (Exception)
            {
                return new ViewResultHelper("Exception", "Error parsing joke response");

            }

            if (!string.IsNullOrWhiteSpace(joke.Setup))
                break;

            await Task.Delay(500);
        }

        if (!response.IsSuccessStatusCode)
            return new ViewResultHelper("Exception", $"Error calling API -> {successAPIUrl}");

        return new ViewResultHelper("Joke", joke);
    }
}