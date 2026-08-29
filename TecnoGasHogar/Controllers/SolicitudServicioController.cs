using Microsoft.AspNetCore.Mvc;
using TecnoGasHogar.Data;
using TecnoGasHogar.Models;

namespace TecnoGasHogar.Controllers;

public class SolicitudServicioController : Controller
{
    private readonly AppDbContext _context;

    public SolicitudServicioController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SolicitudServicio solicitud)
    {
        if (ModelState.IsValid)
        {
            _context.SolicitudesServicio.Add(solicitud);
            await _context.SaveChangesAsync();
            TempData["MensajeExito"] = "Solicitud de servicio registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        return View(solicitud);
    }
}