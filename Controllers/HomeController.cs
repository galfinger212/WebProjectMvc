using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WebProject.Helpers;
using WebProject.Models;
using WebProject.Services;

namespace WebProject.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsServiceSql _postService;
        public HomeController(ILogger<HomeController> logger, IProductsServiceSql service, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            this._postService = service;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            checkAllProducts();
            List<ProductModel> list =  await _postService.GetAllProductsAsync();
            ViewData["CurrentPartialView"] = "Home";
            return View(list);
        }
        //check all hour (if the user refresh the page) all the products that in peoples cart more than a hour
        private void checkAllProducts()
        {
            if(HttpContext.Request.Cookies["ExpireDate"]==null)
            {
                List<ProductModel> products = _postService.RemoveAllForgottenProducts();
                if (HttpContext.Request.Cookies["UserName"] == null)
                {
                    if(HttpContext.Request.Cookies["Products"]!=null)
                    {
                        foreach (var product in products)
                        {
                            HttpContext.Response.Cookies.Append("Products", HttpContext.Request.Cookies["Products"].Replace(product.Id.ToString() + ",", ""));
                        }
                    }
                }
                var options = new CookieOptions { Expires = DateTime.Now.AddHours(1) };
                HttpContext.Response.Cookies.Append("ExpireDate","1",options);
            }
        }
        [HttpGet("SortByDate")]
        public async Task<IActionResult> SortByDate()
        {
            List<ProductModel> list = await _postService.GetAllProductsAsync();
            list.Sort((p, p2) => p.Date.CompareTo(p2.Date));
            ViewData["CurrentPartialView"] = "Home";
            return View("Views/Home/Index.cshtml", list);
        }
        [HttpGet("SortByTitle")]
        public async Task<IActionResult> SortByTitle()
        {
            List<ProductModel> list = await _postService.GetAllProductsAsync();
            list.Sort((p, p2) => p.Title.CompareTo(p2.Title));
            ViewData["CurrentPartialView"] = "Home";
            return View("Views/Home/Index.cshtml",list);
        }
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long id)//go to the details page of this product
        {
            var product = await _postService.GetProductByIdAsync(id);
            if (product == null)
                return RedirectToAction("Index", "Home");
            TempData["Product"] = product;
            ViewData["CurrentPartialView"] = "Details";
            return View("Views/Home/Index.cshtml");
        }
        [HttpGet("AboutUs")]
        public IActionResult AboutUs()//go to about us page
        {
            ViewData["CurrentPartialView"] = "AboutUs";
            return View("Views/Home/Index.cshtml");
        }
        [HttpGet("PostAdd")]
        public IActionResult PostAdd()//go to post add page
        {
            ViewData["CurrentPartialView"] = "PostAnAdd";
            return View("Views/Home/Index.cshtml");
        }
        [HttpPost("PostAdd")]
        public IActionResult PostAddResult(ProductModel product)//post an add
        {
            FileUpload fileUpload = new FileUpload();
            product.Image1 = fileUpload.UpLoadFile(product.Image1File);
            product.Image2 = fileUpload.UpLoadFile(product.Image2File);
            product.Image3 = fileUpload.UpLoadFile(product.Image3File);
            product.Date = DateTime.Now;
            product.State = State.Available;
            try
            {
                _postService.PostAdd(product);
            }
            catch (ArgumentException er)
            {
                ErrorViewModel error = new ErrorViewModel();
                error.RequestId = er.Message;
                return View("Error", error);
            }
            return RedirectToAction("PostAdd");
        }
        [HttpGet("ShoppingCart")]
        public async Task<IActionResult> ShoppingCart()//go to the shopping cart that belong to this user id 
        {
            List<ProductModel> list = new List<ProductModel>();
            List<string> listId = new List<string>();
            if (HttpContext.Request.Cookies["id"]!=null)
            {
                var id = int.Parse(HttpContext.Request.Cookies["id"]);
                list = await _postService.GetAllByUserIdAsync(id);
                foreach (ProductModel product in list) listId.Add(product.Id.ToString());
            }
            else
            {
                string ProductsCookies = HttpContext.Request.Cookies["Products"];
                if(ProductsCookies!=null)
                {
                    string[] listOfId = ProductsCookies.Split(",");
                    for (int i = 0; i < listOfId.Length - 1; i++)
                    {
                        ProductModel product = await _postService.GetProductByIdAsync(int.Parse(listOfId[i]));
                        if(product.State==State.InCart)
                        {
                            list.Add(product);
                            listId.Add(product.Id.ToString());
                        }
                    }
                }
            }
            ViewData["CurrentPartialView"] = "ShoppingCart";
            TempData["listItems"] = list;
            TempData["listItemsId"] = listId;
            TempData.Keep("listItemsId");
            return View("Views/Home/Index.cshtml");
        }
        [HttpPost("ShoppingCart/{id}")]
        public async Task<IActionResult> ShoppingCartSubmit(long id)//remove product with this id from the cart
        {
            var product = await _postService.GetProductByIdAsync(id);
            try
            {
                _postService.RemoveFromCart(product);
            }
            catch (ArgumentNullException er)
            {
                ErrorViewModel error = new ErrorViewModel();
                error.RequestId = er.Message;
                return View("Error", error);
            }  
            if(HttpContext.Request.Cookies["UserName"] == null)
            {
                HttpContext.Response.Cookies.Append("Products", HttpContext.Request.Cookies["Products"].Replace(id.ToString()+",",""));
            }
            return RedirectToAction("ShoppingCart");
        }
        [HttpGet("AddToCart/{id}")]
        public async Task<IActionResult> AddToCart(long id)//add the current product to the cart
        {
            var product = await _postService.GetProductByIdAsync(id);
            product.ExpireDate = DateTime.Now;
            if (HttpContext.Request.Cookies["Id"]!=null)
            {
                product.UserId = new UserModel();
                product.UserId.Id = int.Parse(HttpContext.Request.Cookies["Id"]);
            }
            else
            {
                if(HttpContext.Request.Cookies["Products"]==null)
                {
                    var options = new CookieOptions { Expires = DateTime.Now.AddDays(7) };
                    HttpContext.Response.Cookies.Append("Products", id+",", options);
                }
                else HttpContext.Response.Cookies.Append("Products", HttpContext.Request.Cookies["Products"] + id + ",");
            }
            _postService.AddToCart(product);
            return RedirectToAction("Index");
        }
        [HttpGet("BuyProduct")]
        public async Task<IActionResult> BuyProduct()//buy all the products in the cuurent cart
        {
            string[] arrProductsId = TempData["listItemsId"] as string[];
            if(arrProductsId!=null)
            {
                for (int i = 0; i < arrProductsId.Length; i++)
                {
                    ProductModel product = await _postService.GetProductByIdAsync(int.Parse(arrProductsId[i]));
                    _postService.BuyProduct(product);
                }
            }
            else
            {
                TempData["ZeroProducts"] = "Your Cart Is Empty!";
                return RedirectToAction("ShoppingCart");
            }
            TempData["SuccessBuy"] = "Thank you for your buying!";
            return RedirectToAction("ShoppingCart");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>  View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
