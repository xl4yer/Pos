using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Pos.Class;
using Pos.Models;
using System.Data;

namespace Pos.Services
{
    public class PurchaseServices
    {
        private readonly AppDb _constring;
        public IConfiguration Configuration;

        public PurchaseServices(AppDb constring, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _constring = constring;
            Configuration = configuration;
        }
        public async Task<List<purchase>> GetPurchase()
        {
            List<purchase> t = new List<purchase>();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("GetPurchase", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                    while (await rdr.ReadAsync().ConfigureAwait(false))
                    {
                        t.Add(new purchase
                        {
                            purchaseID = Convert.ToInt32(rdr["purchaseID"]),
                            date = Convert.ToDateTime(rdr["date"]),
                            code = rdr["code"].ToString(),
                            name = rdr["name"].ToString(),
                            price = Convert.ToDouble(rdr["price"]),
                            quantity = Convert.ToInt32(rdr["quantity"]),
                            total = Convert.ToDouble(rdr["total"])
                        });
                    }
                    await rdr.CloseAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Handle the exception (log or rethrow as needed)
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                finally
                {
                    await con.CloseAsync().ConfigureAwait(false);
                }
            }
            return t;
        }

        public async Task<double> GetTodaySales()
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                await con.OpenAsync().ConfigureAwait(false);
                var com = new MySqlCommand("GetTodaySales", con)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                return Convert.ToInt32(await com.ExecuteScalarAsync().ConfigureAwait(false));
            }
        }

        public async Task<double> GetMonthSales()
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                await con.OpenAsync().ConfigureAwait(false);
                var com = new MySqlCommand("GetMonthSales", con)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                return Convert.ToInt32(await com.ExecuteScalarAsync().ConfigureAwait(false));
            }
        }

        public async Task<List<purchase>> GetMonthlySales()
        {
            var monthlySales = new List<purchase>();

            using (var connection = new MySqlConnection(_constring.GetConnection()))
            {
                using (var command = new MySqlCommand("GetMonthlySales", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            monthlySales.Add(new purchase
                            {
                                month = reader.GetInt32("month"),
                                total_sales = reader.GetDouble("total_sales")
                            });
                        }
                    }
                }
            }
            return monthlySales;
        }
    }

}

