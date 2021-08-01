using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastTowAssignment.Models;
using FastTowAssignment.Data;
using Microsoft.IdentityModel.Protocols;
using System.Data.SqlClient;
using FastTowAssignment.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace FastTowAssignment.Controllers
{
    [Authorize(Policy = "readpolicy")]
    public class OrderController : Controller
    {
        private readonly FastTowAssignmentContext _ft;
        private readonly UserManager<FastTowAssignmentUser> _user;
        public OrderController(FastTowAssignmentContext ft, UserManager<FastTowAssignmentUser> user)
        {
            _ft = ft;
            _user = user;
        }

        // GET: OrderController

        [Authorize(Roles = "Admin, Driver")]
        public IActionResult Index()
        {
            var orders = _ft.Orders.Where(a => a.CurrentStatusId != 3).Include(a => a.DepartureCity).Include(b => b.DestinationCity).Include(a => a.CurrentStatus)
                .Include(a => a.Client);
            ViewBag.CurrentDriver = _user.GetUserId(HttpContext.User).ToString();
            return View(orders);
        }

        public IActionResult DriverHistory()
        {
            var orders = _ft.Orders.Where(m => m.DriverId == _user.GetUserId(HttpContext.User).ToString())
                .Include(a => a.DepartureCity).Include(b => b.DestinationCity).Include(a => a.CurrentStatus);
            return View(orders);
        }

        [Authorize(Roles = "Client")]
        public IActionResult History()
        {
            var orders = _ft.Orders.Where(m => m.ClientId == _user.GetUserId(HttpContext.User).ToString())
                .Include(a => a.DepartureCity).Include(b => b.DestinationCity).Include(a => a.CurrentStatus);   
            return View(orders);
        }

        // GET: OrderController/Details/5

        [Authorize(Roles = "Admin, Driver")]
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize(Roles = "Client")]
        public ActionResult Create()
        {
            ViewBag.Cities = new SelectList(_ft.Cities.ToList(), "Id", "Name");
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [Authorize(Roles = "Client")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] Order orderData)
        {
            Order order = new Order();
            ViewBag.Cities = new SelectList(_ft.Cities.ToList(), "Id", "Name");
            if (ModelState.IsValid)
            {
                try
                {
                    order.ClientId = _user.GetUserId(HttpContext.User);
                    order.DepartureCityId = orderData.DepartureCityId;
                    order.DestinationCityId = orderData.DestinationCityId;
                    order.Car = orderData.Car;
                    order.Price = orderData.Price;
                    order.CurrentStatusId = 1;
                    order.CreationDate = DateTime.Now;
               
                    _ft.Add(order);
                    await _ft.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(History));
            }

            return View(orderData);
        }

        // GET: OrderController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) 
            {
                return NotFound();
            }
            
            Order order = await _ft.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Price, Car")] Order order)
        {
            Order oldOrder = await _ft.Orders
                .FirstOrDefaultAsync(m => m.Id == order.Id);
            _ft.Entry(oldOrder).State = EntityState.Detached;
            order.ClientId = _user.GetUserId(HttpContext.User);

            if (ModelState.IsValid)
            {
                //order.ClientId = "51697ae8-0175-4657-a86f-45f96dccec4e";
                try
                {
                    order.CurrentStatusId = 1;
                    order.DepartureCityId = oldOrder.DepartureCityId;
                    order.DestinationCityId = oldOrder.DestinationCityId;
                    _ft.Update(order);
                    await _ft.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(History));
            }

            return View(order);
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: OrderController/AssignToDriver/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignToDriver(int id)
        {
            // Get the model
            Order model = _ft.Orders.Where(m => m.Id == id).FirstOrDefault();
            // Update properties
            model.DriverId = _user.GetUserId(HttpContext.User);
            model.CurrentStatusId = 2;
            // Save and redirect
            _ft.Entry(model).State = EntityState.Modified;
            _ft.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: OrderController/FinishOrder/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinishOrder(int id)
        {
            // Get the model
            Order model = _ft.Orders.Where(m => m.Id == id).FirstOrDefault();
            // Update properties
            model.CurrentStatusId = 3;
            // Save and redirect
            _ft.Entry(model).State = EntityState.Modified;
            _ft.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
