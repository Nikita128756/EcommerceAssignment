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
    public class FeatureController : ApiController
    {
        [HttpGet]
        public IEnumerable<Feature> GetFeature()
        {
            using (EcomEntities _context = new EcomEntities())
            {
                return _context.Features.ToList();
            }
        }

        public HttpResponseMessage GetFeature(int id)
        {
            using (EcomEntities entities = new EcomEntities())
            {
                var entity = entities.Features.FirstOrDefault(e => e.FeatureID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Feature with Id " + id.ToString() + " not found");
                }
            }
        }

        public HttpResponseMessage CreateFeature([FromBody]Feature Feature)
        {
            try
            {
                using (EcomEntities entities = new EcomEntities())
                {
                    entities.Features.Add(Feature);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, Feature);
                    message.Headers.Location = new Uri(Request.RequestUri + Feature.FeatureID.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteFeature(int id)
        {
            try
            {
                using (EcomEntities entities = new EcomEntities())
                {
                    var entity = entities.Features.FirstOrDefault(e => e.FeatureID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Catgory with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Features.Remove(entity);
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
        public HttpResponseMessage UpdateFeature(int id, [FromBody]Feature Feature)
        {
            try
            {
                using (EcomEntities entities = new EcomEntities())
                {
                    var entity = entities.Features.FirstOrDefault(e => e.FeatureID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Feature with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.AttributeName = Feature.AttributeName;
                        entity.FeatureDetails = Feature.FeatureDetails;

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
