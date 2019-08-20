using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Taxes.Models;

namespace Taxes.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MunicipalitiesController : Controller
    {
        private TaxesContent db = new TaxesContent();

        [HttpPost]
        public ActionResult Index(MunicipalitiesView view)
        {
            var municipalities = db.Municipalities
                .Include(m => m.Department)
                .OrderBy(m => m.Department.Name)
                .ThenBy(m => m.Name).ToList();


            if (!string.IsNullOrEmpty(view.Department))
            {
                municipalities = municipalities
                    .Where(m => m.Department.Name.ToUpper()
                    .Contains(view.Department.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(view.Municipality))
            {
                municipalities = municipalities
                    .Where(m => m.Name.ToUpper()
                    .Contains(view.Municipality.ToUpper())).ToList();
            }

            view.Municipalities = municipalities;
            
            return View(view);
        }

        // GET: Municipalities
        public ActionResult Index()
        {
            var municipalities = db.Municipalities
                .Include(m => m.Department)
                .OrderBy(m => m.Department.Name)
                .ThenBy(m => m.Name);

            var view = new MunicipalitiesView
            {
                Municipalities = municipalities.ToList(),
            };


            return View(view);
        }

        // GET: Municipalities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Municipality municipality = db.Municipalities.Find(id);
            if (municipality == null)
            {
                return HttpNotFound();
            }
            return View(municipality);
        }

        // GET: Municipalities/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name");
            return View();
        }

        // POST: Municipalities/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MunicipalityId,DepartmentId,Name")] Municipality municipality)
        {
            if (ModelState.IsValid)
            {
                db.Municipalities.Add(municipality);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", municipality.DepartmentId);
            return View(municipality);
        }

        // GET: Municipalities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Municipality municipality = db.Municipalities.Find(id);
            if (municipality == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", municipality.DepartmentId);
            return View(municipality);
        }

        // POST: Municipalities/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MunicipalityId,DepartmentId,Name")] Municipality municipality)
        {
            if (ModelState.IsValid)
            {
                db.Entry(municipality).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Name", municipality.DepartmentId);
            return View(municipality);
        }

        // GET: Municipalities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Municipality municipality = db.Municipalities.Find(id);
            if (municipality == null)
            {
                return HttpNotFound();
            }
            return View(municipality);
        }

        // POST: Municipalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Municipality municipality = db.Municipalities.Find(id);
            db.Municipalities.Remove(municipality);
            db.SaveChanges();
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
