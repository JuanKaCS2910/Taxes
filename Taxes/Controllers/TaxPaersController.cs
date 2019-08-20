namespace Taxes.Controllers
{
    using Clases;
    using Models;
    using PagedList;

    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;


    public class TaxPaersController : Controller
    {
        private TaxesContent db = new TaxesContent();

        #region MyTaxes

        [Authorize(Roles = "TaxPaer")]
        public ActionResult MyTaxes()
        {
            var taxpaer = db.TaxPaers
                .Where(tp => tp.UserName == User.Identity.Name)
                .FirstOrDefault();

            decimal total = 0;

            foreach (var property in taxpaer.Properties.ToList())
            {
                foreach (var taxProperty in property.TaxProperties.ToList())
                {
                    if (taxProperty.IsPay)
                    {
                        property.TaxProperties.Remove(taxProperty);
                    }
                    else
                    {
                        total += taxProperty.Value;
                    }
                }
            }

            var view = new TaxPaerWithTotal
            {
                TaxPaer = taxpaer,
                Total = total,
            };

            return View(view);
        }

        #endregion

        #region MyProperties

        [Authorize(Roles = "TaxPaer")]
        public ActionResult MyProperties()
        {
            var taxpaer = db.TaxPaers
                .Where(tp => tp.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (taxpaer != null)
            {
                
                return View(taxpaer.Properties);
            }

            return RedirectToAction("Index", "Home");
            
        }

        #endregion

        #region Roles

        [HttpPost]
        public ActionResult MySettings(TaxPaer view)
        {
            if (ModelState.IsValid)
            {
                view.Department = db.Departments.Find(view.DepartmentId);
                view.Municipality = db.Municipalities.Find(view.MunicipalityId);
                db.Entry(view).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null &&
                         ex.InnerException.InnerException != null &&
                         ex.InnerException.InnerException.Message.Contains("Index"))
                    {
                        ModelState.AddModelError(string.Empty, "El campo ya se encuentra registrado");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    return View(view);
                }
                return RedirectToAction("Index", "Home");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments,
               "DepartmentId", "Name", view.DepartmentId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                    .Where(m => m.DepartmentId == view.DepartmentId)
                    .OrderBy(m => m.Name),
                "MunicipalityId", "Name", view.MunicipalityId);

            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes,
                "DocumentTypeId", "Description", view.DocumentTypeId);

            return View(view);
        }

        [Authorize(Roles = "TaxPaer")]
        public ActionResult MySettings()
        {

            var taxpaer = db.TaxPaers
                .Where(tp => tp.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (taxpaer != null)
            {
                ViewBag.DepartmentId = new SelectList(db.Departments,
               "DepartmentId", "Name", taxpaer.DepartmentId);
                ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == taxpaer.DepartmentId)
                .OrderBy(m => m.Name),
                "MunicipalityId", "Name", taxpaer.MunicipalityId);

                ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes,
                    "DocumentTypeId", "Description", taxpaer.DocumentTypeId);

                return View(taxpaer);
            }

            return RedirectToAction("Index", "Home");

        }

        #endregion

        #region Properties

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteProperty(int? propertyId)
        {
            if (propertyId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var property = db.Properties.Find(propertyId);

            if (property == null)
            {
                return HttpNotFound();
            }

            db.Properties.Remove(property);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(string.Format("Details/{0}", property.TaxPaerId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditProperty(Property view)
        {
            if (ModelState.IsValid)
            {
                db.Entry(view).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.Contains("Index"))
                    {
                        ModelState.AddModelError(string.Empty, "El campo ya se encuentra registrado");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    return View(view);
                }

                return RedirectToAction(string.Format("Details/{0}", view.TaxPaerId));
            }

            return View(view);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditProperty(int? propertyId, int? taxpaerId)
        {
            if (propertyId == null || taxpaerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var property = db.Properties.Find(propertyId);

            if (property == null)
            {
                return HttpNotFound();
            }

            this.ReturnView(property, string.Empty);

            return View(property);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProperty(Property view)
        {
            if (ModelState.IsValid)
            {

                db.Properties.Add(view);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //TODO: Catch error to improve the messages
                    this.ReturnView(view, ex.Message);
                    return View(view);
                }

                return RedirectToAction(string.Format("Details/{0}", view.TaxPaerId));
            }

            this.ReturnView(view, string.Empty);
            return View(view);

        }

        [Authorize(Roles = "Admin")]
        private void ReturnView(Property view, string error)
        {
            if (!string.IsNullOrEmpty(error))
                ModelState.AddModelError(string.Empty, error);

            ViewBag.DepartmentId = new SelectList(db.Departments,
                "DepartmentId", "Name", view.DepartmentId);

            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == view.DepartmentId)
                .OrderBy(m => m.Name),
                "MunicipalityId", "Name", view.MunicipalityId);

            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes,
                "PropertyTypeId", "Description", view.PropertyTypeId);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddProperty(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var view = new Property
            {
                TaxPaerId = id.Value,
            };

            ViewBag.DepartmentId = new SelectList(db.Departments,
                "DepartmentId", "Name");

            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == db.Departments.FirstOrDefault().DepartmentId)
                .OrderBy(m => m.Name),
                "MunicipalityId", "Name");

            ViewBag.PropertyTypeId = new SelectList(db.PropertyTypes,
                "PropertyTypeId", "Description");

            return View(view);
        }

        #endregion

        [Authorize(Roles = "Admin")]
        // GET: TaxPaers
        public ActionResult Index(int? page)
        {
            page = (page ?? 1);

            var taxPaers = db.TaxPaers
                            .OrderBy(tp => tp.LastName)
                            .ThenBy(tp => tp.FirstName);

            /*var taxPaers = db.TaxPaers
                .Include(t => t.Department)
                .Include(t => t.DocumentType)
                .Include(t => t.Municipality);
            return View(taxPaers.ToList());*/

            return View(taxPaers.ToPagedList((int)page, 1));
        }

        [Authorize(Roles = "Admin")]
        // GET: TaxPaers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var taxPaers = db.TaxPaers.Find(id);

            if (taxPaers == null)
            {
                return HttpNotFound();
            }

            var view = new TaxPaerView
            {
                TaxPaerId = taxPaers.TaxPaerId,
                Department = taxPaers.Department,
                DepartmentId = taxPaers.DepartmentId,
                DocumentType = taxPaers.DocumentType,
                DocumentTypeId = taxPaers.DocumentTypeId,
                Municipality = taxPaers.Municipality,
                MunicipalityId = taxPaers.MunicipalityId,
                FirstName = taxPaers.FirstName,
                LastName = taxPaers.LastName,
                UserName = taxPaers.UserName,
                Phone = taxPaers.Phone,
                Address = taxPaers.Address,
                Document = taxPaers.Document,
                PropertyList = taxPaers.Properties.OrderBy(p => p.Department.Name).ToList()
            };


            return View(view);
        }

        [Authorize(Roles = "Admin")]
        // GET: TaxPaers/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments,
                "DepartmentId", "Name");
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes,
                "DocumentTypeId", "Description");
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == db.Departments
                .FirstOrDefault().DepartmentId),
                "MunicipalityId", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: TaxPaers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaxPaerId,FirstName,LastName,UserName,Phone,DepartmentId,MunicipalityId,Address,DocumentTypeId,Document")] TaxPaer taxPaer)
        {
            if (ModelState.IsValid)
            {
                db.TaxPaers.Add(taxPaer);
                try
                {
                    db.SaveChanges();
                    Utilities.CreateUserASP(taxPaer.UserName, "TaxPaer");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "");
                    ViewBag.DepartmentId = new SelectList(db.Departments,
                "DepartmentId", "Name", taxPaer.DepartmentId);
                    ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes,
                        "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
                    ViewBag.MunicipalityId = new SelectList(db.Municipalities
                        .Where(m => m.DepartmentId == taxPaer.DepartmentId).OrderBy(m => m.Name),
                        "MunicipalityId", "Name", taxPaer.MunicipalityId);
                    return View(taxPaer);
                }

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments,
                "DepartmentId", "Name", taxPaer.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes,
                "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == taxPaer.DepartmentId).OrderBy(m => m.Name),
                "MunicipalityId", "Name", taxPaer.MunicipalityId);
            return View(taxPaer);
        }

        [Authorize(Roles = "Admin")]
        // GET: TaxPaers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxPaer taxPaer = db.TaxPaers.Find(id);
            if (taxPaer == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments,
                "DepartmentId", "Name", taxPaer.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes,
                "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == taxPaer.DepartmentId).OrderBy(m => m.Name),
                "MunicipalityId", "Name", taxPaer.MunicipalityId);
            return View(taxPaer);
        }

        [Authorize(Roles = "Admin")]
        // POST: TaxPaers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaxPaerId,FirstName,LastName,UserName,Phone,DepartmentId,MunicipalityId,Address,DocumentTypeId,Document")] TaxPaer taxPaer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taxPaer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments,
                "DepartmentId", "Name", taxPaer.DepartmentId);
            ViewBag.DocumentTypeId = new SelectList(db.DocumentTypes,
                "DocumentTypeId", "Description", taxPaer.DocumentTypeId);
            ViewBag.MunicipalityId = new SelectList(db.Municipalities
                .Where(m => m.DepartmentId == taxPaer.DepartmentId).OrderBy(m => m.Name),
                "MunicipalityId", "Name", taxPaer.MunicipalityId);
            return View(taxPaer);
        }

        [Authorize(Roles = "Admin")]
        // GET: TaxPaers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxPaer taxPaer = db.TaxPaers.Find(id);
            if (taxPaer == null)
            {
                return HttpNotFound();
            }
            return View(taxPaer);
        }

        [Authorize(Roles = "Admin")]
        // POST: TaxPaers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaxPaer taxPaer = db.TaxPaers.Find(id);
            db.TaxPaers.Remove(taxPaer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Admin")]
        public JsonResult GetMunicipalities(int departmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var municipalities = db.Municipalities
                .Where(m => m.DepartmentId == departmentId)
                .OrderBy(m => m.Name);
            return Json(municipalities);
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
