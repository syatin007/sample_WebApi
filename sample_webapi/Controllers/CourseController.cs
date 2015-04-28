using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using sample_webapi.Models;


namespace sample_webapi.Controllers
{
     [RoutePrefix("api/Course")]
    public class CourseController : ApiController
    {
        private database3Entities db = new database3Entities();

        [HttpGet]
        public IEnumerable<Course> GetCourses()
        {
            return db.Courses.AsEnumerable();
        }
         //name is given to route(GetCourseById)
         [Route("{id:int:min(1)}", Name = "GetCourseById")]
       
        public Course GetCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return course;
        }

         [Route("{course}")]
         [HttpPut]
       public HttpResponseMessage PutCourse( Course course)
        {
            var id = course.CourseId;
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != course.CourseId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
         [Route("{course}")]
         [HttpPost]
        public HttpResponseMessage PostCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, course);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = course.CourseId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
         [Route("{id:int:min(1)}")]
      [HttpDelete]
        public HttpResponseMessage DeleteCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Courses.Remove(course);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, course);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}