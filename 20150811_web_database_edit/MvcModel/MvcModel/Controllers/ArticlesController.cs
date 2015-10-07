using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using MvcModel.Models;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;


namespace MvcModel.Controllers
{
    public class ArticlesController : Controller
    {
        private MvcModelContext db = new MvcModelContext();

        // ログを有効化
        //public ArticlesController()
        //{
        //  db.Database.Log = sql =>
        //  {
        //    Debug.Write(sql);
        //  };
        //}

        public ActionResult Navigation(int? id)
        {
            Article article = db.Articles.Find(id);
            return View(article);
        }

        // GET: Articles
        public ActionResult Index()
        {
            //return View(db.Articles.ToList());
            var articles = db.Database.SqlQuery<Article>("SELECT * FROM Articles;");
            return View(articles);
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Url,Title,Category,Description,Viewcount,Published,Released")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Url,Title,Category,Description,Viewcount,Published,Released")] Article article)
        // public ActionResult Edit([Bind(Include = "Id,Url,Title,Category,Description,Viewcount,Published,Released,Timestamp")] Article article)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(article).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                ModelState.AddModelError(string.Empty, "更新の競合が検出されました。");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Article article = db.Articles.Find(id);
            //db.Articles.Remove(article);
            //db.SaveChanges();
            int cnt = db.Database.ExecuteSqlCommand(
              "DELETE FROM Articles WHERE id={0}", id);
            return RedirectToAction("Index");
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