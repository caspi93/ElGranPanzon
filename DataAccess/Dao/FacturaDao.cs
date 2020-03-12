﻿using DataAccess.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dao {
    public class FacturaDao {
        private Entidades db;
        public FacturaDao(Entidades db) {
            this.db = db;
        }
        /*
         * Autor: Luis Carlos Pedroza Pineda
         * Método que crea la factura
         */
        public Factura crearFactura(Factura factura) {
            var detallesFacturas = new List<DetalleFactura>();
            foreach (var df in factura.DETALLEFACTURAS) {
                var detalleFactura = new DetalleFactura {
                    ComidaId = df.COMIDAS.Id,
                    SedeId = df.SEDES.Id,
                    Cantidad = df.Cantidad,
                    Precio = df.Precio,
                    Subtotal = df.Subtotal
                };
                detallesFacturas.Add(detalleFactura);
            }
        
            var f = new Factura {
                FechaCreacion = DateTime.Now,
                ClienteId = factura.CLIENTES.Id,
                Total = factura.Total,
                DETALLEFACTURAS = detallesFacturas
            };

            db.Facturas.Add(f);
            db.SaveChanges();
            return f;
        }
    }
}