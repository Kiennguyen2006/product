using Microsoft.AspNetCore.Mvc;
using Nkklession06.Models;

namespace Nkklession06.ViewComponents
{
    public class CategoryViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(int? n)
        {
            List<Category> categorys=new List<Category>() { 
                new Category() { CategoryId=1,CategoryName="bánh mì ",IsActive=true},
                new Category() { CategoryId=2,CategoryName="bánh kem",IsActive=false},
                new Category() { CategoryId=3,CategoryName="bánh cá",IsActive=true},
                new Category() { CategoryId=4,CategoryName="bánh gấu",IsActive=false},
            };
            n = n ?? 0;
            var search = categorys.Where(c => c.CategoryId > n).ToList();
            return View(search);
        }
    }
}
