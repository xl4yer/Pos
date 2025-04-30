using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Pos.Class;
using Pos.Models;
using System.Data;

namespace Pos.Services
{
    public class QtyServices
    {
        private readonly AppDb _constring;
        public IConfiguration Configuration;

        public QtyServices(AppDb constring, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _constring = constring;
            Configuration = configuration;
        }

        public async Task<List<Qty>> GetQty()
        {
            List<Qty> q = new List<Qty>();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("GetQty", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                    while (await rdr.ReadAsync().ConfigureAwait(false))
                    {
                        q.Add(new Qty
                        {
                            qtyId = Convert.ToInt32(rdr["qtyId"]),
                            date = Convert.ToDateTime(rdr["date"]),
                            code = rdr["code"].ToString(),
                            name = rdr["name"].ToString(),
                            qty = Convert.ToInt32(rdr["qty"]),
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
            return q;
        }

        public async Task<int> AddQty(Qty q)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                try
                {
                    await con.OpenAsync().ConfigureAwait(false);
                    var com = new MySqlCommand("AddQty", con)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };
                    com.Parameters.AddWithValue("_date", q.date);
                    com.Parameters.AddWithValue("_code", q.code);
                    com.Parameters.AddWithValue("_qty", q.qty);
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
