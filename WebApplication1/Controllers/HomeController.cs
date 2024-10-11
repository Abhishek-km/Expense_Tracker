using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        static void SetDateTime()
        {
            DateTime currentDate = DateTime.Now;

            // Convert to string in dd/MM/yy format
            expenseDate = currentDate.ToString("dd/MM/yy");

        }

        private readonly ExpenseDBContext expense;
        static string expenseDate;

        public HomeController(ExpenseDBContext expense) {
            this.expense = expense;
            SetDateTime();
        }

        /* 
         Created a index view , which will be called in the start, got the table data from the dbset present in the ExpenseDBContext class,
            and passed the data to view.
         */
        public IActionResult Index()
        {
            var expenses = expense.Expenses.ToList();
            List<Expense> sortedExpenses = expenses.OrderBy(expense => expense.ID).ToList();
            return View(sortedExpenses);
        }

        /*
         Create new i.e Add expense functionality
         */

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Expense exp)
        {
            if (exp == null)
            {
                return View("Error");
            }
            exp.Date = expenseDate;
            expense.Expenses.Add(exp);
            expense.SaveChanges();
            return RedirectToAction("Index");
        }

        /*
         Edit functionaity
         */
        public IActionResult Edit(int id) { 
            var exp = expense.Expenses.Find(id);
            return View(exp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Expense exp)
        {
            if (expense == null)
            {
                return View("Error");
            }
            expense.Expenses.Update(exp);
            expense.SaveChanges();
            return RedirectToAction("Index");

        }
        /*
         * Delete Functionality
         */
        public IActionResult Delete(int id)
        {
            var exp = expense.Expenses.Find(id);
            return View(exp);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var exp = expense.Expenses.Find(id);
            if (exp != null)
            {
                expense.Expenses.Remove(exp);
                expense.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        /*
         * Details Functionality
         */
        public IActionResult Details(int id)
        {
            var exp = expense.Expenses.Find(id);
            return View(exp);
        }

        /*
         * SHow Total Functionality
         */
        public IActionResult ShowTotal()
        {

            var expenses = expense.Expenses.ToList();
            decimal totalExpenses = 0;
            decimal totalMonthlyExpenses = 0;
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            foreach (var expense in expenses)
            {
                // Parse the date string
                if (DateTime.TryParseExact(expense.Date, "dd-MM-yy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    // Calculate the total expenses
                    totalExpenses += expense.Amount;

                    // Calculate monthly expenses
                    if (parsedDate.Month == currentMonth && parsedDate.Year == currentYear)
                    {
                        totalMonthlyExpenses += expense.Amount;
                    }
                }
            }
            var totalExpenseModel = new TotalExpense
            {
                Total_Expense = totalExpenses,
                Total_MonthyExpense = totalMonthlyExpenses
            };
            return View(totalExpenseModel);
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
