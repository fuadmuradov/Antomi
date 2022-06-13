using Antomi.DataAccsessLayer;
using Antomi.Extension;
using Antomi.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    public class AboutController : Controller
    {
        private readonly AntomiDbContext context;
        private readonly IWebHostEnvironment webHost;

        public AboutController(AntomiDbContext context, IWebHostEnvironment webHost)
        {
            this.context = context;
            this.webHost = webHost;
        }

        #region About
        public IActionResult Index()
        {
            About about = context.Abouts.First();
            return View(about);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(About about)
        {
            if (!ModelState.IsValid)
            {
                return View(about);
            }

            About existAbout = context.Abouts.FirstOrDefault(x => x.Id == about.Id);

            if (about.Photo != null)
            {
                try
                {
                    string folder = @"assets\img\about\";
                    string newImg = await about.Photo.SavaAsync(webHost.WebRootPath, folder);
                    FileExtension.Delete(webHost.WebRootPath, folder, existAbout.Image);
                    existAbout.Image = newImg;
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "unexpected error for Update");
                    return View();
                }
            }
            if (about.SignPhoto != null)
            {
                try
                {
                    string folder = @"assets\img\about\";
                    string newImg = await about.SignPhoto.SavaAsync(webHost.WebRootPath, folder);
                    FileExtension.Delete(webHost.WebRootPath, folder, existAbout.Signature);
                    existAbout.Signature = newImg;
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "unexpected error for Update");
                    return View();
                }
            }



            existAbout.Title = about.Title;
            existAbout.Description = about.Description;
         


            await context.SaveChangesAsync();


            return View(about);
        }

        #endregion

        #region Question
        public IActionResult QuestionPage()
        {
            List<Question> questions = context.Questions.ToList();
            return View(questions);
        }

        public IActionResult AddQuestion()
        {           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddQuestion(Question question)
        {
            if (!ModelState.IsValid) return View(question);
            context.Questions.Add(question);
            context.SaveChangesAsync();
            return LocalRedirect("~/admin/About/QuestionPage");
        }

        public IActionResult EditQuestion(int id)
        {
            Question question = context.Questions.FirstOrDefault(x => x.Id == id);
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditQuestion(Question question)
        {
            if(!ModelState.IsValid)  return View(question);
            Question existQuestion = context.Questions.FirstOrDefault(x => x.Id == question.Id);
            existQuestion.Issue = question.Issue;
            existQuestion.Answer = question.Answer;
            context.SaveChangesAsync();
            return LocalRedirect("~/admin/About/QuestionPage");
        }


        public IActionResult DeleteQuestion(int id)
        {
            Question question = context.Questions.FirstOrDefault(x => x.Id == id);
            if (question == null) return NotFound();
            context.Questions.Remove(question);
            return View(question);
        }

        #endregion

        #region Testimonial
        public IActionResult Testimonial() 
        {
            List<Testimonial> testimonials = context.Testimonials.ToList();
            return View(testimonials);
        }

        public IActionResult AddTestimonial()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTestimonial(Testimonial testimonial)
        {
            if (!ModelState.IsValid) return View(testimonial);
            string folder = @"assets\img\about\";
            testimonial.Image = testimonial.Photo.SavaAsync(webHost.WebRootPath, folder).Result;

            context.Testimonials.Add(testimonial);
            context.SaveChanges();

            return LocalRedirect("~/admin/About/testimonial"); 
        }


        public IActionResult UpdateTestimonial(int id)
        {
            Testimonial testimonial = context.Testimonials.FirstOrDefault(x => x.Id == id);
            if (testimonial == null)
                return NotFound();

            return View(testimonial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTestimonial(Testimonial testimonial)
        {
            if (!ModelState.IsValid) return View(testimonial);
            Testimonial existTestimonial = context.Testimonials.FirstOrDefault(x => x.Id == testimonial.Id);

            if (testimonial.Photo != null)
            {
                try
                {
                    string folder = @"assets\img\about\";
                    string newImg = await testimonial.Photo.SavaAsync(webHost.WebRootPath, folder);
                    FileExtension.Delete(webHost.WebRootPath, folder, existTestimonial.Image);
                    existTestimonial.Image = newImg;
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "unexpected error for Update");
                    return View();
                }
            }
            existTestimonial.Fullname = testimonial.Fullname;
            existTestimonial.Jobname = testimonial.Jobname;
            existTestimonial.Description = testimonial.Description;

            await context.SaveChangesAsync();


            return LocalRedirect("~/admin/About/testimonial");
        }

        public IActionResult DeleteTestimonial(int id)
        {
            Testimonial testimonial = context.Testimonials.FirstOrDefault(x=>x.Id==id);
            if (testimonial == null)
                return NotFound();
            string folder = @"assets\img\about\";
            FileExtension.Delete(webHost.WebRootPath, folder, testimonial.Image);
            context.Testimonials.Remove(testimonial);
            context.SaveChangesAsync();
            return LocalRedirect("~/admin/About/testimonial");
        }

        #endregion


    }
}
