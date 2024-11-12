using BudgetBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BudgetBuddy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SpendSmartDbContext _spendSmart;

        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext spendSmart)
        {
            _logger = logger;
            _spendSmart = spendSmart;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var allExpenses =_spendSmart.Expenses.ToList();
           
            var totalExpenses = allExpenses
                .Sum(e => e.Value);
            ViewBag.Expenses = totalExpenses;

            return View(allExpenses);
        }
        public IActionResult CreateEditExpense(int? id)
        {
            if (id == null)
            {
                var expenseInDb = _spendSmart
                .Expenses
                .SingleOrDefault(expense =>
                expense.Id == id);
                return View(expenseInDb);
            }            
            return View();
        }
        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _spendSmart
                .Expenses
                .SingleOrDefault(expense =>
                expense.Id == id);
            _spendSmart.Expenses.Remove(expenseInDb);
            _spendSmart.SaveChanges();
            return RedirectToAction("Expenses");
        }
        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if (model.Id == 0)
            {
                //Create
                _spendSmart.Expenses.Add(model);
            }
            else
            {
                //Editing
                _spendSmart.Expenses.Update(model);
            }
            
            _spendSmart.SaveChanges();
            return RedirectToAction("Expenses");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
