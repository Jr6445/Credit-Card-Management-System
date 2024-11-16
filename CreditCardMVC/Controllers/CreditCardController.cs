using CreditCardMVC.Models;
using CreditCardMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

namespace CreditCardMVC.Controllers
{
    public class CreditCardController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public CreditCardController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public IActionResult AddTransaction()
        {
            var model = new AddTransactionViewModel();
            return View(model);
        }

        public async Task<IActionResult> Statement(int id)
        {
            var client = _clientFactory.CreateClient("CreditCardApiClient");
            var response = await client.GetAsync($"api/CreditCard/{id}/statement");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var statement = await response.Content.ReadFromJsonAsync<CreditCardStatementViewModel>();
            return View(statement);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction(AddTransactionViewModel model)
        {
            var client = _clientFactory.CreateClient("CreditCardApiClient");
            var response = await client.PostAsJsonAsync("api/CreditCard/transactions", model);

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            return RedirectToAction("Statement", new { id = model.CardHolderID });
        }

        public IActionResult AddPayment()
        {
            var model = new AddTransactionViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(AddTransactionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Enviar la información del pago a la API
            var client = _clientFactory.CreateClient("CreditCardApiClient");
            model.TransactionType = "Pago"; // Asegurarse de que sea un pago
            await client.PostAsJsonAsync("api/CreditCard/transactions", model);

            return RedirectToAction("Statement", new { id = model.CardHolderID });
        }

        public async Task<IActionResult> TransactionHistory(int id)
        {
            var client = _clientFactory.CreateClient("CreditCardApiClient");
            var response = await client.GetAsync($"api/CreditCard/{id}/transactions");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var transactions = await response.Content.ReadFromJsonAsync<List<TransactionViewModel>>();
            return View(transactions);
        }

    }
}
