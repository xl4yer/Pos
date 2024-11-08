using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Pos.Class;
using Pos.Models;
using System.Data;
using static MudBlazor.CategoryTypes;

namespace Pos.Services
{
    public class ProductServices
    {
        private readonly AppDb _constring;
        public IConfiguration Configuration;

        public ProductServices(AppDb constring, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _constring = constring;
            Configuration = configuration;
        }
        public async Task<List<products>> Products()
        {
            List<products> m = new List<products>();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("ViewProduct", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                    while (await rdr.ReadAsync().ConfigureAwait(false))
                    {
                        m.Add(new products
                        {
                            productID = Convert.ToInt32( rdr["productID"]),
                            photo = (byte[])rdr["photo"],
                            name = rdr["name"].ToString(),
                            code = rdr["code"].ToString(),
                            price = rdr["price"].ToString(),
                            status = rdr["status"].ToString()
                        });
                    }
                    await rdr.CloseAsync().ConfigureAwait(false);
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
            return m;
        }
        public async Task<int> AddProduct(products p)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("AddProduct", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    com.Parameters.AddWithValue("_photo", p.photo);
                    com.Parameters.AddWithValue("_name", p.name);
                    com.Parameters.AddWithValue("_code", p.code);
                    com.Parameters.AddWithValue("_price", p.price);
                    com.Parameters.AddWithValue("_status", p.status);
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
        public async Task<int> UpdateProduct(products p)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("UpdateProduct", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    com.Parameters.AddWithValue("_productID", p.productID);
                    com.Parameters.AddWithValue("_code", p.code);
                    com.Parameters.AddWithValue("_photo", p.photo);
                    com.Parameters.AddWithValue("_name", p.name);
                    com.Parameters.AddWithValue("_price", p.price);
                    com.Parameters.AddWithValue("_status", p.status);
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

        public async Task<List<products>> SProduct(string search)
        {
            List<products> p = new List<products>();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("SearchProduct", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    com.Parameters.Clear();
                    com.Parameters.AddWithValue("search", search);
                    com.Parameters.AddWithValue("@searchWildcard", $"{search}%");
                    var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                    while (await rdr.ReadAsync().ConfigureAwait(false))
                    {
                        p.Add(new products
                        {
                            productID = Convert.ToInt32(rdr["productID"]),
                            code = rdr["code"].ToString(),
                            name = rdr["name"].ToString(),
                            price = rdr["price"].ToString(),
                            status = rdr["status"].ToString(),
                          
                        });
                    }
                    await rdr.CloseAsync().ConfigureAwait(false);
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
            return p;
        }
    }
}
