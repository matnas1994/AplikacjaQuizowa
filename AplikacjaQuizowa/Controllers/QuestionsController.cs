using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AplikacjaQuizowa.Models;
using Rotativa;

namespace AplikacjaQuizowa.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        [Authorize(Roles = "Administrator,Moderator")]
        //GET: Questions
        public ActionResult Index()
        {

            List<QuestionsViewModel> MyViewModel = new List<QuestionsViewModel>();
            foreach (var question in db.Questions)
            {
                var MyCheckBoxList = new List<CheckBoxViewModel>();
                var Categories = from c in db.Categories
                                 select new
                                 {
                                     c.CategorieId,
                                     c.Name,
                                     Checked = ((from ab in db.QuestionToCategories
                                                 where
                                   (ab.QuestionId == question.QuestionId) & (ab.CategorieId == c.CategorieId)
                                                 select ab).Count() > 0)
                                 };
                foreach (var categorie in Categories)
                {
                    MyCheckBoxList.Add(new CheckBoxViewModel { Id = categorie.CategorieId, Name = categorie.Name + " ", Checked = categorie.Checked });
                }

                MyViewModel.Add(new QuestionsViewModel
                {
                    QuestionId = question.QuestionId,
                    Contents = question.Contents,
                    Answer1 = question.Answer1,
                    Answer2 = question.Answer2,
                    Answer3 = question.Answer3,
                    CorrectAnswer = question.CorrectAnswer,
                    Categories = MyCheckBoxList
                });

            }
            return View(MyViewModel);
        }

        public ActionResult StartSolve(int ?id)
        {

            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }

            var categorie = db.QuestionToCategories.Where(i => i.CategorieId == id);

            QuestionToAnsersModelView questionToAnsersModelView = new QuestionToAnsersModelView();
            questionToAnsersModelView.Categorie = db.Categories.Find(id);

            foreach (var item in categorie.ToList())
            {
                Question question = db.Questions.Find(item.QuestionId);
                QuestionToAnswer questionToAnswer = new QuestionToAnswer(question.Answer1, question.Answer2, question.Answer3, question.CorrectAnswer);
                questionToAnswer.QuestionId = question.QuestionId;
                questionToAnswer.Contents = question.Contents;
                questionToAnswer.mixingAnswer();
                questionToAnsersModelView.QuestionToAnswers.Add(questionToAnswer);
            }

            if (questionToAnsersModelView.QuestionToAnswers.Count == 0)
            {
                throw new HttpException(404, "Not found");
            }

            return View("StartSolve",questionToAnsersModelView);
        }

        [HttpPost]
        public ActionResult StartSolve( FormCollection collection)
        {

            if (collection == null)
            {
                throw new HttpException(400, "Bad Request");
            }

            int score = 0;
            int CategorieId;

            bool test = Int32.TryParse(collection["CategorieId"], out CategorieId);

            if (!test)
            {
                throw new HttpException(404, "Bad Request");
            }

            var questionId = collection["QuestionId"].Split(',').ToList();
            
            foreach (var id in questionId)
            {
                Question question = db.Questions.Find(Convert.ToInt16(id));
                if (Convert.ToString(collection["answer" + Convert.ToInt16(id)]) == question.CorrectAnswer)
                  {
                    score++;
                   }
            }
            ViewData["Score"] = score;
            ViewData["CountQuestion"] = questionId.Count;
            ViewData["Percent"] = Math.Round((((double)score / (double)questionId.Count)*100),2);

            var user = System.Web.HttpContext.Current.User.Identity.GetUserId();

          

            Score s = new Score();
            s.UserId = db.Users.FirstOrDefault(x => x.Id == user);
            s.CategoriId = db.Categories.Find(CategorieId);


            foreach (var item in db.Score)
            {
                if (item.UserId == s.UserId && item.CategoriId == s.CategoriId)
                {
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
            }

     

            s.Result = Math.Round((((double)score / (double)questionId.Count)), 2);

            db.Score.Add(s);
            db.SaveChanges();

            ViewData["CategorieId"] = CategorieId;

            return View("Results");
        }



        [Authorize(Roles = "Administrator,Moderator")]
        //GET: Questions/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                throw new HttpException(404, "Not found");
            }

            var Results = from c in db.Categories
                          select new
                          {
                              c.CategorieId,
                              c.Name,
                              Checked = ((from ab in db.QuestionToCategories
                                          where
                            (ab.QuestionId == id) & (ab.CategorieId == c.CategorieId)
                                          select ab).Count() > 0)
                          };

            var MyViewModel = new QuestionsViewModel();
            MyViewModel.QuestionId = id.Value;
            MyViewModel.Contents = question.Contents;
            MyViewModel.Answer1 = question.Answer1;
            MyViewModel.Answer2 = question.Answer2;
            MyViewModel.Answer3 = question.Answer3;
            MyViewModel.CorrectAnswer = question.CorrectAnswer;

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.CategorieId, Name = item.Name, Checked = item.Checked });
            }

            MyViewModel.Categories = MyCheckBoxList;
            return View("Details",MyViewModel);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Create()
        {
            var MyViewModel = new QuestionsViewModel();

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in db.Categories)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.CategorieId, Name = item.Name, Checked = false });
            }

            MyViewModel.Categories = MyCheckBoxList;

            return View("Create",MyViewModel);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionsViewModel questionModel)
        {
            if (ModelState.IsValid)
            {
                Question question = new Question
                {
                    Contents = questionModel.Contents,
                    Answer1 = questionModel.Answer1,
                    Answer2 = questionModel.Answer2,
                    Answer3 = questionModel.Answer3,
                    CorrectAnswer = questionModel.CorrectAnswer
                };
                db.Questions.Add(question);

                foreach (var item in questionModel.Categories)
                {
                    if (item.Checked)
                    {
                        db.QuestionToCategories.Add(new QuestionToCategories()
                        {
                            QuestionId = question.QuestionId,
                            CategorieId = item.Id
                        });
                    }
                }

                db.SaveChanges();
                TempData["alert"] = "Stworzyłeś nowa pytanie!";
                return RedirectToAction("Index");
            }

            return View(questionModel);
        }

        // GET: Questions/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                throw new HttpException(404, "Not found");
            }

            var Results = from c in db.Categories
                          select new
                          {
                              c.CategorieId,
                              c.Name,
                              Checked = ((from ab in db.QuestionToCategories
                                          where
                             (ab.QuestionId == id) & (ab.CategorieId == c.CategorieId)
                                          select ab).Count() > 0)
                          };

            var MyViewModel = new QuestionsViewModel();
            MyViewModel.QuestionId = id.Value;
            MyViewModel.Contents = question.Contents;
            MyViewModel.Answer1 = question.Answer1;
            MyViewModel.Answer2 = question.Answer2;
            MyViewModel.Answer3 = question.Answer3;
            MyViewModel.CorrectAnswer = question.CorrectAnswer;

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.CategorieId, Name = item.Name, Checked = item.Checked });
            }

            MyViewModel.Categories = MyCheckBoxList;
            return View("Edit",MyViewModel);
        }

        // POST: Questions/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionsViewModel question)
        {
            if (ModelState.IsValid)
            {
                var MyQuestion = db.Questions.Find(question.QuestionId);

                MyQuestion.Contents = question.Contents;
                MyQuestion.Answer1 = question.Answer1;
                MyQuestion.Answer2 = question.Answer2;
                MyQuestion.Answer3 = question.Answer3;
                MyQuestion.CorrectAnswer = question.CorrectAnswer;

                foreach (var item in db.QuestionToCategories)
                {
                    if (item.QuestionId == question.QuestionId)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                }

                foreach (var item in question.Categories)
                {
                    if (item.Checked)
                    {
                        db.QuestionToCategories.Add(new QuestionToCategories()
                        {
                            QuestionId = question.QuestionId,
                            CategorieId = item.Id
                        });
                    }
                }

                // db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                TempData["alert"] = "Zmieniłeś pytanie!";
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                throw new HttpException(404, "Not found");
            }

            var Results = from c in db.Categories
                          select new
                          {
                              c.CategorieId,
                              c.Name,
                              Checked = ((from ab in db.QuestionToCategories
                                          where
       (ab.QuestionId == id) & (ab.CategorieId == c.CategorieId)
                                          select ab).Count() > 0)
                          };

            var MyViewModel = new QuestionsViewModel();
            MyViewModel.QuestionId = id.Value;
            MyViewModel.Contents = question.Contents;
            MyViewModel.Answer1 = question.Answer1;
            MyViewModel.Answer2 = question.Answer2;
            MyViewModel.Answer3 = question.Answer3;
            MyViewModel.CorrectAnswer = question.CorrectAnswer;

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.CategorieId, Name = item.Name, Checked = item.Checked });
            }

            MyViewModel.Categories = MyCheckBoxList;
            return View("Delete",MyViewModel);
        }

        // POST: Questions/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            foreach (var item in db.QuestionToCategories)
            {
                if (item.QuestionId == id)
                {
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
            }
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            TempData["alert"] = "Usunełeś pytanie!";
            return RedirectToAction("Index");
        }

        public ActionResult DownloadViewPDF()
        {
            var model = db.Categories.ToList();
            //Code to get content
            return new Rotativa.ViewAsPdf("index", model) { FileName = "TestViewAsPdf.pdf" };
        }

        public ActionResult QuestionPDF(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }

            var categorie = db.QuestionToCategories.Where(i => i.CategorieId == id);

            QuestionToAnsersModelView questionToAnsersModelView = new QuestionToAnsersModelView();
            questionToAnsersModelView.Categorie = db.Categories.Find(id);

            foreach (var item in categorie.ToList())
            {
                Question question = db.Questions.Find(item.QuestionId);
                QuestionToAnswer questionToAnswer = new QuestionToAnswer(question.Answer1, question.Answer2, question.Answer3, question.CorrectAnswer); ;
                questionToAnswer.QuestionId = question.QuestionId;
                questionToAnswer.Contents = question.Contents;
                questionToAnswer.mixingAnswer();
                questionToAnsersModelView.QuestionToAnswers.Add(questionToAnswer);
            }

            if (questionToAnsersModelView.QuestionToAnswers.Count == 0)
            {
                throw new HttpException(404, "Not found");
            }


            return new ViewAsPdf(questionToAnsersModelView) { FileName = "Quiz.pdf" };
        }

        public ActionResult CorrectAnswerPDF(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var categorie = db.QuestionToCategories.Where(i => i.CategorieId == id);

            if (categorie == null)
            {
                throw new HttpException(404, "Not found");
            }


            CorrectAnswerViewModel correctAnswerViewModel = new CorrectAnswerViewModel();
            correctAnswerViewModel.Categorie = db.Categories.Find(id);

            foreach (var item in categorie.ToList())
            {
                Question question = db.Questions.Find(item.QuestionId);
                CorrectAnswer correctAnswer = new CorrectAnswer();


                correctAnswerViewModel.CorrectAnswer.Add(new CorrectAnswer()
                {
                    Question = question.Contents,
                    Answer = question.CorrectAnswer
                });
            }

            if (correctAnswerViewModel.CorrectAnswer.Count == 0)
            {
                throw new HttpException(404, "Not found");
            }

            return new ViewAsPdf(correctAnswerViewModel) { FileName = "Odpowiedzi.pdf" };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
