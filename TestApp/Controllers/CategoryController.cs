using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EcommerceDataAccess;
using System.Data.Entity;

namespace TestApp.Controllers
{
    public class CategoryController : ApiController
    {
        [HttpGet]
        public IEnumerable<Category> GetCatetgory()
        {
            using (EcomEntities _context = new EcomEntities())
            {
                _context.Configuration.ProxyCreationEnabled = false;
                return _context.Categories.ToList();
            }
        }

        public HttpResponseMessage GetCatetgory(int id)
        {
            using (EcomEntities entities = new EcomEntities())
            {
                var entity = entities.Categories.FirstOrDefault(e => e.CategoryId == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Category with Id " + id.ToString() + " not found");
                }
            }
        }

        public HttpResponseMessage CreateCategory([FromBody]Category category)
        {
            try
            {
                using (EcomEntities entities = new EcomEntities())
                {
                    entities.Categories.Add(category);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, category);
                    message.Headers.Location = new Uri(Request.RequestUri + category.CategoryId.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteCategory(int id)
        {
            try
            {
                using (EcomEntities entities = new EcomEntities())
                {
                    var entity = entities.Categories.FirstOrDefault(e => e.CategoryId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Catgory with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Categories.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateCategory(int id, [FromBody]Category category)
        {
            try
            {
                using (EcomEntities entities = new EcomEntities())
                {
                    entities.Configuration.ProxyCreationEnabled = false;
                    var entity = entities.Categories.FirstOrDefault(e => e.CategoryId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Category with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.CategoryName = category.CategoryName;
                        entity.ParentId = category.ParentId;
                       
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
