
namespace Taxes.Controllers
{
    using Clases;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;


    [Authorize(Roles = "Admin")]
    public class TaxPaersController : Controller
    {
        private TaxesContent db = new TaxesContent();

        // GET: TaxPaers
        public ActionResult Index()
        {
            var taxPaers = db.TaxPaers
                .Include(t => t.Department)
                .Include(t => t.DocumentType)
                .Include(t => t.Municipality);
            return View(taxPaers.ToList());
        }

        // GET: TaxPaers/Details/5
        public ActionResult Details(int? id)
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
