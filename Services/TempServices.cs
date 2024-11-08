using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Pos.Class;
using Pos.Models;
using System.Data;

namespace Pos.Services
{
    public class TempServices
    {
        private readonly AppDb _constring;
        public IConfiguration Configuration;

        public TempServices(AppDb constring, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _constring = constring;
            Configuration = configuration;
        }
        public async Task<List<temp>> Temp()
        {
            List<temp> t = new List<temp>();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("ViewTemp", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                    while (await rdr.ReadAsync().ConfigureAwait(false))
                    {
                        t.Add(new temp
                        {
                            tempID = Convert.ToInt32(rdr["tempID"]),
                            date = Convert.ToDateTime( rdr["date"]),
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

        public async Task<int> AddTemp(temp t)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("AddTemp", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    com.Parameters.AddWithValue("_code", t.code);
                    com.Parameters.AddWithValue("_date", t.date);
                    com.Parameters.AddWithValue("_quantity", t.quantity);
                    return await com.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Handle the exception here
                }
                finally
                {
                    await con.CloseAsync().ConfigureAwait(false);
                }
            }
            return 0;
        }

        public async Task<int> UpdateTemp(temp t)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("UpdateTemp", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    com.Parameters.AddWithValue("_tempID", t.tempID);
                    com.Parameters.AddWithValue("_quantity", t.quantity);
                    return await com.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Handle the exception here
                }
                finally
                {
                    await con.CloseAsync().ConfigureAwait(false);
                }
            }
            return 0;
        }

    }
}
