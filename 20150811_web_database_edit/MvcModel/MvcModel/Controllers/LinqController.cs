using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcModel.Models;
using System.Diagnostics;


namespace MvcModel.Controllers
{
    public class LinqController : Controller
    {
        private MvcModelContext db = new MvcModelContext();
        // ログを有効化
        //public LinqController()
        //{
        //  db.Database.Log = sql =>
        //  {
        //    Debug.Write(sql);
        //  };
        //}

        public ActionResult Search(string keyword, bool? released)
        {
            var articles = from a in db.Articles
                           select a;

            if (!String.IsNullOrEmpty(keyword))
            {
                articles = articles.Where(a => a.Title.Contains(keyword));
                //articles = articles.Where(a => a.Title.StartsWith(keyword));

            }
            if (released.HasValue && released.Value)
            {
                articles = articles.Where(a => a.Released);
            }
            return View(articles);
        }

        public ActionResult Where()
        {
            var article = db.Articles.Single(
              a => a.Url == "http://codezine.jp/article/corner/518");
            return Content(article.Title);
        }

        public ActionResult Orderby()
        {
            var articles = from a in db.Articles
                           orderby a.Published descending, a.Title
                           select a;
            return View(articles);
        }

        public ActionResult Orderby2()
        {
            var articles = db.Articles
                       .OrderByDescending(a => a.Published)
                       .ThenBy(a => a.Title);
            return View(articles);
        }

        public ActionResult Sort(string sort)
        {
            ViewBag.Title = String.IsNullOrEmpty(sort) ? "dTitle" : "";
            ViewBag.Category = (sort == "Category" ? "dCategory" : "Category");
            ViewBag.Published = (sort == "Published" ? "dPublished" : "Published");
            ViewBag.Viewcount = (sort == "Viewcount" ? "dViewcount" : "Viewcount");

            var articles = from a in db.Articles select a;
            switch (sort)
            {
                case "Category":
                    articles = articles.OrderBy(a => a.Category);
                    break;
                case "Published":
                    articles = articles.OrderBy(a => a.Published);
                    break;
                case "Viewcount":
                    articles = articles.OrderBy(a => a.Viewcount);
                    break;
                case "dTitle":
                    articles = articles.OrderByDescending(a => a.Title);
                    break;
                case "dCategory":
                    articles = articles.OrderByDescending(a => a.Category);
                    break;
                case "dPublished":
                    articles = articles.OrderByDescending(a => a.Published);
                    break;
                case "dViewcount":
                    articles = articles.OrderByDescending(a => a.Viewcount);
                    break;
                default:
                    articles = articles.OrderBy(a => a.Title);
                    break;
            }

            return View(articles);
        }

        public ActionResult Select()
        {
            var articles = from a in db.Articles
                           orderby a.Published descending
                           select new ArticleView
                           {
                               Title = a.Title.Substring(0, 10),
                               Viewcount = (int)Math.Ceiling(a.Viewcount / 1000.0),
                               Released = (a.Released ? "公開中" : "公開予定")
                           };

            //メソッド構文
            //var articles = db.Articles
            //             .OrderByDescending(a => a.Published)
            //             .Select(a => new ArticleView
            //             {
            //               Title = a.Title.Substring(0, 10),
            //               Viewcount = (int)Math.Ceiling(a.Viewcount / 1000.0),
            //               Released = (a.Released ? "公開中" : "公開予定")
            //             });

            return View(articles);
        }

        public ActionResult SelectMany()
        {
            var comments = db.Articles
                             .Where(a => a.Category == CategoryEnum.Reference)
                             .Select(a => a.Comments);

            return View(comments);
        }

        public ActionResult SelectMany2()
        {
            var comments = db.Articles
                             .Where(a => a.Category == CategoryEnum.Reference)
                             .SelectMany(a => a.Comments);

            return View(comments);
        }

        public ActionResult SelectMany3()
        {
            var comments = from a in db.Articles
                           where a.Category == CategoryEnum.Reference
                           from c in a.Comments
                           select c;

            return View(comments);
        }

        public ActionResult SelectMany4()
        {
            var comments = db.Articles
               .Where(a => a.Category == CategoryEnum.Reference)
               .SelectMany(a => a.Comments
               .Select(c => new ArticleCommentView { Title = a.Title, Body = c.Body })
            );

            // クエリー式構文
            //var comments = from a in db.Articles
            //  where a.Category == CategoryEnum.Reference
            //  from c in a.Comments
            //  select new ArticleCommentView { Title = a.Title, Body = c.Body };

            return View(comments);
        }

        public ActionResult Distinct()
        {
            var comments = (from c in db.Comments
                            orderby c.Name
                            select c.Name).Distinct();

            return View(comments);
        }

        public ActionResult Skip()
        {
            var articles = (from a in db.Articles
                            orderby a.Published descending
                            select a).Skip(4).Take(3);

            return View(articles);
        }

        public ActionResult Paging(int? id)
        {
            var pageSize = 3;
            var pageNum = (id ?? 1) - 1;
            var articles = (from a in db.Articles
                            orderby a.Published descending
                            select a).Skip(pageSize * pageNum).Take(pageSize);
            return View(articles);
        }

        public ActionResult First()
        {
            var articles = (from a in db.Articles
                            orderby a.Published descending
                            select a).First();

            return Content(articles.Title);
        }

        public ActionResult FirstOrDefault()
        {
            var articles = (from a in db.Articles
                            where a.Url == "http://nothing.org/"
                            select a).FirstOrDefault();
            if (articles == null)
            {
                return Content("該当する記事情報は存在しません。");
            }
            return Content(articles.Title);
        }

        public ActionResult Group()
        {
            var articles = from a in db.Articles
                           group a by a.Category;
            // メソッド構文
            // var articles = db.Articles.GroupBy(a => a.Category);

            return View(articles);
        }

        public ActionResult Group2()
        {
            var articles = from a in db.Articles
                           group new ArticleLinkView { Url = a.Url, Title = a.Title }
                           by a.Category;
            // メソッド構文
            // var articles = db.Articles.GroupBy(a => a.Category,
            //   a => new ArticleLinkView { Url = a.Url, Title = a.Title });

            return View(articles);
        }

        public ActionResult MultiGroup()
        {
            var articles = from a in db.Articles
                           group a by new ArticleGroup
                           {
                               Category = a.Category,
                               Published = a.Published
                           };
            // メソッド構文
            //var articles = db.Articles.
            //    GroupBy(a => new ArticleGroup
            //    {
            //      Category = a.Category, Published = a.Published
            //    });

            return View(articles);
        }

        public ActionResult Having()
        {
            var articles = from a in db.Articles
                           group a by a.Category into cgroup
                           where cgroup.Average(a => a.Viewcount) > 10000
                           select new ArticleHaving
                           {
                               Category = cgroup.Key,
                               ViewAverage = cgroup.Average(a => a.Viewcount)
                           };
            // メソッド構文
            //var articles = db.Articles.GroupBy(a => a.Category)
            //    .Where(group => group.Average(a => a.Viewcount) > 10000)
            //    .Select(group => new ArticleHaving
            //    {
            //      Category = group.Key,
            //      ViewAverage = group.Average(a => a.Viewcount)
            //    });
            return View(articles);
        }

        public ActionResult Having2()
        {
            var articles = from a in db.Articles
                           group a by a.Category into cgroup
                           orderby cgroup.Key.ToString()
                           select cgroup;
            // メソッド構文
            //var articles = db.Articles
            //  .GroupBy(a => a.Category)
            //  .OrderBy(group => group.Key.ToString());

            return View(articles);
        }

        public ActionResult Join()
        {
            var articles = from a in db.Articles
                           join c in db.Comments on a equals c.Article
                           select new ArticleCommentView
                           {
                               Title = a.Title,
                               Body = c.Body
                           };

            //var articles = db.Articles
            //                 .Join(db.Comments, a => a, c => c.Article,
            //                  (a, c) => new ArticleCommentView
            //                  {
            //                    Title = a.Title, Body = c.Body
            //                  });
            return View(articles);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}