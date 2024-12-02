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
        public async Task<List<temp>> GetTemp()
        {
            List<temp> t = new List<temp>();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("GetTemp", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                    while (await rdr.ReadAsync().ConfigureAwait(false))
                    {
                        t.Add(new temp
                        {
                            tempID = Convert.ToInt32(rdr["tempID"]),
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

        public async Task<int> DeleteTemp(temp t)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("DeleteTemp", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    com.Parameters.AddWithValue("_tempID", t.tempID);
                    var result = await com.ExecuteScalarAsync().ConfigureAwait(false);
                    return Convert.ToInt32(result);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error executing DeleteTemp", ex);
                }
                finally
                {
                    await con.CloseAsync().ConfigureAwait(false);
                }
            }
        }


        public async Task<int> TempToPurchase()
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);

                    var com = new MySqlCommand("TempToPurchase", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };

                    // Execute the command and fetch the result from the SELECT 1 query in the stored procedure
                    var result = await com.ExecuteScalarAsync().ConfigureAwait(false);

                    // If the result is not null and is a number (1), return it as success
                    if (result != null && int.TryParse(result.ToString(), out int successCode))
                    {
                        return successCode;  // This will return 1 if the operation was successful
                    }

                    return 0; // If something goes wrong, return 0
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0; // In case of error, return 0
                }
                finally
                {
                    await con.CloseAsync().ConfigureAwait(false);
                }
            }
        }


        public async Task<double> Total()
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                await con.OpenAsync().ConfigureAwait(false);
                var com = new MySqlCommand("TotalPrice", con)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                return Convert.ToInt32(await com.ExecuteScalarAsync().ConfigureAwait(false));
            }
        }

    }
   
}

