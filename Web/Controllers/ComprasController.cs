﻿
using DataAccess.Dao;
using DataAccess.Tablas;
using Negocio.Modelo;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers {
    public class ComprasController : Controller {
        private Entidades db;

        public ComprasController() {
            db = new Entidades();
        }

        public ActionResult Pedidos() {
            if (Session["factura"] == null) {
                Session["factura"] = new Factura {
                    DETALLEFACTURAS = new List<DetalleFactura>(),
                    CLIENTES = (Cliente)Session["cliente"]
                };
            }
            var categoriaComidaIdStr = Request.Form.Get("categorias");
            var categoriaComidaId = categoriaComidaIdStr == null ? null : new decimal?(Convert.ToDecimal(categoriaComidaIdStr));

            var categoriaComidaDao = new CategoriaComidaDao(db);
            var categoriasComidas = categoriaComidaDao.GetCategoriasComida();

            var comidaDao = new ComidaDao(db);
            var comidas = categoriaComidaId != null ?
                comidaDao.GetComidas(categoriaComidaId.Value) : new List<Comida>();

            ViewBag.CategoriasComidas = categoriasComidas;
            ViewBag.CategoriaComidaId = categoriaComidaId;

            ViewBag.Comidas = comidas;
            return View();
        }

        public ActionResult SeleccionarPlato() {
            var comidaId = Request.Form.Get("comidaId");
            var cantidad = Request.Form.Get("cantidad");
            var precio = Request.Form.Get("precio");

            var detalleFactura = new DetalleFactura {
                ComidaId = Convert.ToDecimal(comidaId),
                Precio = Convert.ToDecimal(precio),
                Cantidad = Convert.ToDecimal(cantidad)
            };
            var empleadoLogeado = (DataAccess.Tablas.Empleado)Session["empleado"];
            var comidaDao = new ComidaDao(db);
            var comida = comidaDao.GetComida(detalleFactura.ComidaId);
            detalleFactura.COMIDAS = comida;
            detalleFactura.SEDES = empleadoLogeado.SEDES;

            var factura = (Factura)Session["factura"];
            factura.DETALLEFACTURAS.Add(detalleFactura);

            Response.Redirect("/Compras/Pedidos?comida=" + comidaId);
            return View();
        }

        public ActionResult RealizarCompra() {

            var factura = (Factura)Session["factura"];
            var facturaDao = new FacturaDao(db);
            facturaDao.crearFactura(factura);

            Response.Redirect("/Compras/Pedidos");
            TempData["Mensaje"] = "La compra se ha relizado exitosamente";
            Session["factura"] = null;
            return View();
        }


    }
}