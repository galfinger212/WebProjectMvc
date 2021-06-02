using Dal;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProject.Services
{
    public class PostsServiceSql : IProductsServiceSql
    {
        private MyContext context;
        public PostsServiceSql(MyContext context) => this.context = context;
        public void AddToCart(ProductModel product)
        {
            UserModel user = null;
            UserModel owner = null;
            List<UserModel> list = context.Users.AsNoTracking().ToList();
            owner = list.Where((us) => us.Id == product.OwnerId.Id).SingleOrDefault();
            if (product.UserId != null)  user = list.Where((us) => us.Id == product.UserId.Id).SingleOrDefault();
            product.OwnerId = null;
            product.UserId = null;
            product.State = State.InCart;
            context.Products.Update(product);
            product.UserId = user;
            product.OwnerId = owner;
            context.SaveChanges();
        }
        public void BuyProduct(ProductModel product)
        {
            if (product != null)
            {
                product.OwnerId = null;
                product.UserId = null;
                product.State = State.purchase;
                context.Products.Update(product);
                context.SaveChanges();
            }
        }
        public Task<List<ProductModel>> GetAllProductsAsync() => Task.Run(() => GetAllProducts());
        public List<ProductModel> GetAllProducts() => context.Products.AsNoTracking().Include("OwnerId").Where(p => p.State.Equals(State.Available)).ToList();
        public Task<ProductModel> GetProductByIdAsync(long id) => Task.Run(() => GetProductById(id));
        public ProductModel GetProductById(long id) => context.Products.AsNoTracking().Include("OwnerId").Include("UserId").Where((product) => product.Id == id).SingleOrDefault();
        public Task<List<ProductModel>> GetAllByUserIdAsync(long id) => Task.Run(() => GetAllByUserId(id));
        public List<ProductModel> GetAllByUserId(long id) => context.Products.AsNoTracking().Include("OwnerId").Where(p => p != null && p.UserId.Id == id && p.State == State.InCart).ToList();
        public void PostAdd(ProductModel product)
        {
            var owner = context.Users.Where((us) => us.Id == product.OwnerId.Id).SingleOrDefault();
            product.OwnerId = null;
            if (owner != null)
            {
                context.Products.Add(product);
                product.OwnerId = owner;
                context.SaveChanges();
            }
            else throw new ArgumentException("We dont find the current user");
        }
        //remove product from cart
        public void RemoveFromCart(ProductModel product)
        {
            if(product != null)
            {
                product.State = State.Available;
                context.Entry(product).Navigation("UserId").CurrentValue = null;
                context.Entry(product).Navigation("UserId").IsModified = true;
                context.Products.Update(product);
                context.SaveChanges();
            }
            else throw new ArgumentNullException("There was an error with this product" + product);
        }
        //remove all the products that in peoples cart more than a hour
        public List<ProductModel> RemoveAllForgottenProducts()
        {
            List<ProductModel> productsNeedsToremove = new List<ProductModel>();
            List<ProductModel> products = context.Products.Where((p) => p.State==State.InCart).ToList();
            foreach (ProductModel item in products)
            {
                if(item.ExpireDate!=null)
                {
                    if(item.ExpireDate.Value.AddHours(1).CompareTo(DateTime.Now)<0)
                    {
                        item.State = State.Available;
                        productsNeedsToremove.Add(item);
                    }
                }
            }
            context.UpdateRange(productsNeedsToremove);
            context.SaveChanges();
            return productsNeedsToremove;
        }
    }
}