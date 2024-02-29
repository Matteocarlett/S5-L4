using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Scarpe.Models;
using System;
using System.Collections.Generic;

namespace ShoeStore.Controllers
{
    public class ShoeController : Controller
    {
        private readonly string _connectionString = "Server=DESKTOP-7PVKHJM\\SQLEXPRESS01; Initial Catalog=Scarpe; Integrated Security=true; TrustServerCertificate=True";

        [HttpGet]
        public IActionResult Index()
        {
            var shoes = new List<Shoe>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM ScarpeD", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var shoe = new Shoe()
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Prezzo = (int)(decimal)reader["Prezzo"],
                                Descrizione = reader["Descrizione"].ToString()
                            };
                            shoes.Add(shoe);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestione degli errori
                return View("Error", ex.Message);
            }

            return View(shoes);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand("Select * from ScarpeD where Id = @Id",connection);
            command.Parameters.AddWithValue("@Id", id);
            var reader = command.ExecuteReader();
            reader.Read();
            var shoe = new Shoe()

            {
                Id = (int)reader["Id"],
                Nome = reader["Nome"].ToString(),
                Prezzo = (decimal)reader["Prezzo"],
                Descrizione = reader["Descrizione"].ToString()
            };
          
             
            
            
            if (shoe == null)
            {
                return View("Error", "Prodotto non trovato");
            }

            return View(shoe);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Shoe shoe)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        var command = new SqlCommand(@"
                            INSERT INTO Shoe (Nome, Prezzo, Descrizione) 
                            VALUES (@name, @price, @description)", connection);

                        command.Parameters.AddWithValue("@name", shoe.Nome);
                        command.Parameters.AddWithValue("@price", shoe.Prezzo);
                        command.Parameters.AddWithValue("@description", shoe.Descrizione);

                        command.ExecuteNonQuery();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Gestione degli errori
                    return View("Error", ex.Message);
                }
            }

            return View(shoe);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var shoe = GetById(id.Value);

            if (shoe == null)
            {
                return View("Error", "Prodotto non trovato");
            }

            return View(shoe);
        }

        [HttpPost]
        public IActionResult Edit(Shoe shoe)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        var command = new SqlCommand(@"
                            UPDATE Shoe 
                            SET Nome = @name, Prezzo = @price, Descrizione = @description
                            WHERE Id = @shoeId", connection);

                        command.Parameters.AddWithValue("@name", shoe.Nome);
                        command.Parameters.AddWithValue("@price", shoe.Prezzo);
                        command.Parameters.AddWithValue("@description", shoe.Descrizione);
                        command.Parameters.AddWithValue("@shoeId", shoe.Id);

                        command.ExecuteNonQuery();
                    }

                    return RedirectToAction("Details", new { id = shoe.Id });
                }
                catch (Exception ex)
                {
                    // Gestione degli errori
                    return View("Error", ex.Message);
                }
            }

            return View(shoe);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var shoe = GetById(id.Value);

            if (shoe == null)
            {
                return View("Error", "Prodotto non trovato");
            }

            return View(shoe);
        }

        [HttpPost]
        public IActionResult Delete(Shoe shoe)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("DELETE FROM Shoe WHERE ShoeId = @shoeId", connection);
                    command.Parameters.AddWithValue("@shoeId", shoe.Id);
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Gestione degli errori
                return View("Error", ex.Message);
            }
        }

        private Shoe GetById(int id)
        {
            Shoe shoe = null;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand("SELECT * FROM Shoe WHERE ShoeId = @shoeId", connection);
                    command.Parameters.AddWithValue("@shoeId", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            shoe = new Shoe()
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                Prezzo =(decimal)reader["Prezzo"],
                                Descrizione = reader["Descrizione"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestione degli errori
            }

            return shoe;
        }
    }
}
