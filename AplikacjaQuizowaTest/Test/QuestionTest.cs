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
    public class QuestionTest
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private QuestionsController controller = new QuestionsController();

        [TestMethod]
        public void index()
        {
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void Details()
        {
            var question = db.Questions.First();
            var result = controller.Details(question.QuestionId) as ViewResult;
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

            QuestionsViewModel questionModel = new QuestionsViewModel()
            {
                Contents = "test",
                Answer1 = "test",
                Answer2 = "test",
                Answer3 = "test",
                CorrectAnswer = "test"
            };

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in db.Categories)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.CategorieId, Name = item.Name, Checked = true });
            }

            questionModel.Categories = MyCheckBoxList;

            var result = controller.Create(questionModel) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);

        }

   
        [TestMethod]
        public void Edit()
        {
            var question = db.Questions.First();
            var result = controller.Edit(question.QuestionId) as ViewResult;
            Assert.AreEqual("Edit", result.ViewName);
        }


        [TestMethod]
        public void PostEdit()
        {
            var question = db.Questions.First();
            QuestionsViewModel questionModel = new QuestionsViewModel()
            {
                QuestionId = question.QuestionId,
                Contents = "test",
                Answer1 = "test",
                Answer2 = "test",
                Answer3 = "test",
                CorrectAnswer = "test",
            };

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in db.Categories)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.CategorieId, Name = item.Name, Checked = true });
            }

            questionModel.Categories = MyCheckBoxList;

            var result = controller.Edit(questionModel) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }


        [TestMethod]
        public void Delete()
        {
            var question = db.Questions.First();
            var result = controller.Delete(question.QuestionId) as ViewResult;
            Assert.AreEqual("Delete", result.ViewName);
        }

        [TestMethod]
        public void StartSolve()
        {
            var c = new AplikacjaQuizowa.Models.Categorie()
            {
                Name = "Test"
            };

            db.Categories.Add(c);
            db.SaveChanges();

            var categorie = db.Categories.First();
            var result = controller.StartSolve(categorie.CategorieId) as ViewResult;
            Assert.AreEqual("StartSolve", result.ViewName);
        }

    }
}
