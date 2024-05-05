using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Globalization;


namespace kursovoi.Model
{
    public class MyDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=D:\\kursovoi\\kursovoi\\BD\\salonBeauty.db");
        }
        // Method to create the database
        public void CreateDatabase()
        {
            try
            {
                this.Database.EnsureCreated();
                Console.WriteLine("База данных успешно создана.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при создании базы данных: " + ex.Message);
                // Дополнительно можно вывести стек вызовов для более детальной информации:
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static List<T> GetEntities<T>(string connectionString, string sqlQuery) where T : new()
        {
            var entities = new List<T>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = new SqliteCommand(sqlQuery, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var entity = new T();
                        var properties = typeof(T).GetProperties();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var property = properties.FirstOrDefault(p => p.Name.Equals(reader.GetName(i), StringComparison.OrdinalIgnoreCase));
                            if (property != null && !reader.IsDBNull(i))
                            {
                                if (property.PropertyType == typeof(TimeSpan))
                                {
                                    var timeSpanString = reader.GetString(i);
                                    TimeSpan timeSpanValue;
                                    if (TimeSpan.TryParse(timeSpanString, out timeSpanValue))
                                    {
                                        property.SetValue(entity, timeSpanValue);
                                    }
                                    else
                                    {
                                        throw new FormatException($"Не удалось преобразовать '{timeSpanString}' в TimeSpan для свойства '{property.Name}'.");
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        var value = reader.GetValue(i);
                                        var convertedValue = Convert.ChangeType(value, property.PropertyType);
                                        property.SetValue(entity, convertedValue);
                                    }
                                    catch (FormatException ex)
                                    {
                                        if (property.PropertyType == typeof(decimal))
                                        {
                                            decimal decimalValue;
                                            if (decimal.TryParse(reader.GetString(i), NumberStyles.Any, CultureInfo.InvariantCulture, out decimalValue))
                                            {
                                                property.SetValue(entity, decimalValue);
                                            }
                                            else
                                            {
                                                throw new FormatException($"Ошибка преобразования decimal: Не удалось преобразовать '{reader.GetString(i)}' в decimal для свойства '{property.Name}'.");
                                            }
                                        }
                                        else
                                        {
                                            throw new FormatException($"Не удалось преобразовать значение для свойства '{property.Name}'. Тип данных: {property.PropertyType}.", ex);
                                        }
                                    }
                                }
                            }
                        }
                        entities.Add(entity);
                    }
                }
            }
            return entities;
        }




        public void DeleteEntityFromDatabase<T>(T entity) where T : class
        {
            using (var context = new MyDbContext())
            {
                var entry = context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    context.Attach(entity);
                }
                context.Remove(entity);
                context.SaveChanges();
            }
        }

        public void SaveEntity<T>(T entity) where T : class
        {
            using (var context = new MyDbContext())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        public void SaveEntities<T>(ObservableCollection<T> entities) where T : class
        {
            using (MyDbContext dbContext = new MyDbContext())
            {
                try
                {
                    dbContext.AddRange(entities);
                    dbContext.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException;
                    while (innerException != null)
                    {
                        Console.WriteLine(innerException.Message);
                        innerException = innerException.InnerException;
                    }
                }
            }
        }
        public void UpdateEntity<T>(T entity) where T : class
        {
            using (var context = new MyDbContext())
            {
                context.Set<T>().Update(entity);
                context.SaveChanges();
            }
        }
    }
}

