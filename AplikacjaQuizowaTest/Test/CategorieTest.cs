using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AplikacjaQuizowa.Controllers;
using AplikacjaQuizowa.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AplikacjaQuizowaTest.Test
{
    [TestClass]
    public class CategorieTest
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CategoriesController controller = new CategoriesController();

        [TestMethod]
        public void index()
        {
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void Details()
        {
            var question = db.Categories.First();
            var result = controller.Details(question.CategorieId) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod]
        public void Create()
        {
            var result = controller.Create() as ViewResult;
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod]
        public void PostCreate()
        {

            Categorie categorie = new Categorie()
            {
                Name = "Sport"
            };

            var result = controller.Create(categorie) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);

        }


        [TestMethod]
        public void Edit()
        {
            var categorie = db.Categories.First();
            var result = controller.Edit(categorie.CategorieId) as ViewResult;
            Assert.AreEqual("Edit", result.ViewName);
        }


        [TestMethod]
        public void PostEdit()
        {
            var categorieFirst = db.Categories.First();
            Categorie categorie = new Categorie()
            {
                CategorieId = categorieFirst.CategorieId,
                Name = "Test"
            };
        
            var result = controller.Edit(categorie) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void SelectCategory()
        {
            var result = controller.SelectCategory() as ViewResult;
            Assert.AreEqual("SelectCategory", result.ViewName);
        }

        [TestMethod]
        public void Delete()
        {
            var categorieFirst = db.Categories.First();
            var result = controller.Delete(categorieFirst.CategorieId) as ViewResult;
            Assert.AreEqual("Delete", result.ViewName);
        }


    }
}
