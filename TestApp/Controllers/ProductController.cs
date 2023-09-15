using EcommerceDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using TestApp.Models;

namespace TestApp.Controllers
{
    public class ProductController : ApiController
    {
        [HttpGet]
        public IEnumerable<ProductViewModel> GetProduct()
        {
            List<ProductViewModel> pvm = new List<ProductViewModel>();
            using (EcomEntities _context = new EcomEntities())
            {                
                var productDetl = ProductAll();
                           
                foreach (var products in productDetl)
                {
                    ProductViewModel PvModel = new ProductViewModel();
                    List<FeatureViewModel> lst = new List<FeatureViewModel>();
                    PvModel.Name = products.ProductName;
                    PvModel.Description = products.Description;
                    PvModel.CategoryId = products.CategoryId;
                    var featureProductWise = _context.Features.Where(x => x.ProductID == products.ProductId).Select(x =>x).ToList();
                                    
                    foreach (var f in featureProductWise)
                    {
                        FeatureViewModel tmpfeature = new FeatureViewModel()
                        {
                            //FeatureID = f.FeatureID,
                            FeatureName = f.AttributeName,
                            FeatureDescription = f.FeatureDetails,
                            //ProductID = products.ProductId
                        };
                       lst.Add(tmpfeature);
                     }
                    PvModel.AttributeDetails = lst;
                    pvm.Add(PvModel);
                }

                return pvm;
                //return _context.Products.ToList();
            }
        }

        [HttpGet]
        public HttpResponseMessage GetProduct(int id)
        {
            using (EcomEntities entities = new EcomEntities())
            {
                var entity = entities.Products.FirstOrDefault(e => e.ProductId == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with Id " + id.ToString() + " not found");
                }
            }
        }

        [HttpPost]      
        public HttpResponseMessage CreateProduct([FromBody]ProductViewModel productDetl)
        {
            try
            {
                using (EcomEntities entities = new EcomEntities())
                {
                    List<Feature> lstFeature = new List<Feature>();
                    Product p = new Product();
                    p.ProductName = productDetl.Name;
                    p.Description = productDetl.Description;
                    p.CategoryId = productDetl.CategoryId;
                    entities.Products.Add(p);
                    entities.SaveChanges();
                    foreach (var f in productDetl.AttributeDetails)
                    {
                        Feature tmpfeature = new Feature()
                        {
                            //FeatureID = f.FeatureID,
                            AttributeName = f.FeatureName,
                            FeatureDetails = f.FeatureDescription,
                            ProductID = p.ProductId
                        };
                        lstFeature.Add(tmpfeature);
                        //PvModel.AttributeDetails.Add(tmpfeature);
                    }
                    entities.Features.AddRange(lstFeature);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, p);
                    message.Headers.Location = new Uri(Request.RequestUri + p.ProductId.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteProduct(int id)
        {
            try
            {
                using (EcomEntities entities = new EcomEntities())
                {
                    var entity = entities.Products.FirstOrDefault(e => e.ProductId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Product with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Products.Remove(entity);
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
        public HttpResponseMessage UpdateProduct(int id, [FromBody]Product product)
        {
            try
            {
                using (EcomEntities entities = new EcomEntities())
                {
                    entities.Configuration.ProxyCreationEnabled = false;
                    var entity = entities.Products.FirstOrDefault(e => e.ProductId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Product with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.ProductName = product.ProductName;
                        entity.Description = product.Description;
                        entity.CategoryId = product.CategoryId;
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

        [HttpPost]
        [Route("[action]")]
        public IEnumerable<Product> ProductAll()
        {
            using (EcomEntities _context = new EcomEntities())
            {
                return _context.Products.ToList();
            }
        }

    }
}
