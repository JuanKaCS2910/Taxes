using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Taxes.Models;

namespace Taxes.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {
        private TaxesContent db = new TaxesContent();

        #region Municipality


        [HttpPost]
        public ActionResult EditMunicipality(Municipality view)
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
                

                return RedirectToAction(string.Format("Details/{0}",view.DepartmentId));
            }
            return View(view);
        }

        [HttpGet]
        public ActionResult EditMunicipality(int? municipalityId, int? departmentId)
        {
            if (municipalityId == null || departmentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var municipality = db.Municipalities.Find(municipalityId);

            if (municipality == null)
            {
                return HttpNotFound();
            }

            return View(municipality);

        }

        public ActionResult DeleteMunicipality(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var municipality = db.Municipalities.Find(id);

            if (municipality == null)
            {
                return HttpNotFound();
            }

            db.Municipalities.Remove(municipality);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction(string.Format("Details/{0}", municipality.DepartmentId));
        }

        [HttpPost]
        public ActionResult AddMunicipality(Municipality view)
        {

            if (ModelState.IsValid)
            {
                db.Municipalities.Add(view);
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

                return RedirectToAction(string.Format("Details/{0}", view.DepartmentId));
            }
            return View(view);

        }

        public ActionResult AddMunicipality(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }

            var view = new Municipality
            {
                DepartmentId = department.DepartmentId
            };

            return View(view);
        }



        #endregion

        // GET: Departments
        public ActionResult Index()
        {
            var departments = db.Departments.ToList();
            var views = new List<DepartmentView>();

            foreach (var department in departments)
            {
                var view = new DepartmentView
                {
                    DepartmentId = department.DepartmentId,
                    MunicipalityList = department.Municipalities.ToList(),
                    Name = department.Name,
                };

                views.Add(view);

            }

            return View(views);
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = db.Departments.Find(id);

            if (department == null)
            {
                return HttpNotFound();
            }

            var view = new DepartmentView
            {
                DepartmentId = department.DepartmentId,
                MunicipalityList = department.Municipalities.OrderBy(m => m.Name).ToList(),
                Name = department.Name
            };

            return View(view);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentId,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
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
                    return View(department);
                }

                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = db.Departments.Find(id);

            if (department == null)
            {
                return HttpNotFound();
            }

            return View(department);
        }

        // POST: Departments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentId,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
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
                    return View(department);

                }

                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var department = db.Departments.Find(id);
            db.Departments.Remove(department);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ModelState.AddModelError(string.Empty, "The record can't be delete because has related record");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return View(department);
            }

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
