using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {
    [HttpGet("/categories")]
    public ActionResult Index()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }

    [HttpGet("/categories/new")]
    public ActionResult New()
    {
        return View();
    }

    [HttpPost("/categories/{categoryId}/items")]
    public ActionResult Create(int categoryId, string itemDescription)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category foundCategory = Category.Find(categoryId);
      Item newItem = new Item(itemDescription);
      newItem.Save();
      foundCategory.AddItem(newItem);
      List<Item> categoryItems = foundCategory.Items;
      model.Add("items", categoryItems);
      model.Add("category", foundCategory);
      return View("Show", model);
    }

    [HttpPost("/categories")]
    public ActionResult Create(string categoryName)
    {
        Category newCategory = new Category(categoryName);
        return RedirectToAction("Index");
    }

    [HttpGet("/categories/{id}")]
    public ActionResult Show(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Category selectedCategory = Category.Find(id);
        List<Item> categoryItems = selectedCategory.Items;
        model.Add("category", selectedCategory);
        model.Add("items", categoryItems);
        return View(model);
    }

    public ActionResult Details(int id)
    {
        var thisCategory = _db.Categories
            .Include(category => category.Items)
            .ThenInclude(join => join.Item)
            .FirstOrDefault(category => category.CategoryId == id);
        return View(thisCategory);
    }
  }
}