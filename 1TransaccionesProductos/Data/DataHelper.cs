using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Reflection.Metadata;

namespace _1TransaccionesProductos.Data
{
    internal class DataHelper
    {
        private static DataHelper? _instance;
        SqlConnection _conn;
        SqlTransaction? _transaction;

        private DataHelper()
        {
            _conn = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=PRACTICA_01_PII;Integrated Security=True;Encrypt=False");
        }

        public static DataHelper GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DataHelper();
            }
            return _instance;
        }

        public void BeginTransaction()
        {
            if (_conn.State != ConnectionState.Open) { _conn.Open(); }
            _transaction = _conn.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void RollBack()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        public void Cleanup()
        {
            _transaction = null;
            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }
        }

        public DataTable ExecuteSPRead(string sp, Parameter? p = null)
        {
            DataTable dt = new DataTable();

            try
            {
                if (_conn.State != ConnectionState.Open) { _conn.Open(); }

                using (SqlCommand cmd = new SqlCommand(sp, _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (p != null)
                    {
                        cmd.Parameters.AddWithValue(p.Name, p.Value);
                    }

                    dt.Load(cmd.ExecuteReader());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error With the Data Base: " + ex.Message);
                throw;
            }
            finally
            {
                if (_transaction == null) { _conn.Close(); }
            }

            return dt;
        }

        public (int affectedRows, int output) ExecuteSPModify(string sp, List<Parameter> parameters)
        {
            int affectedRows = 0;
            int output = 0;
            SqlParameter? outputParam = null;

            try
            {
                if (_conn.State != ConnectionState.Open) { _conn.Open(); }

                using (SqlCommand cmd = new SqlCommand(sp, _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (_transaction != null) { cmd.Transaction = _transaction; }

                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (Parameter p in parameters)
                        {
                            if (p.isOutput)
                            {
                                outputParam = new SqlParameter(p.Name, SqlDbType.Int);
                                outputParam.Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(outputParam);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(p.Name, p.Value);
                            }
                        }
                    }

                    affectedRows = cmd.ExecuteNonQuery();

                    if (outputParam != null && outputParam.Value != DBNull.Value)
                    {
                        output = Convert.ToInt32(outputParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error With the Data Base: " + ex.Message);
                throw;
            }
            finally
            {
                if (_transaction == null) { _conn.Close(); }
            }

            return (affectedRows, output);
        }

        public decimal ExecuteSPScalarDecimal(string sp, Parameter p)
        {
            decimal result = 0;
            try
            {
                if (_conn.State != ConnectionState.Open) _conn.Open();

                using (SqlCommand cmd = new SqlCommand(sp, _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(p.Name, p.Value);

                    object? scalar = cmd.ExecuteScalar();
                    if (scalar != null && scalar != DBNull.Value)
                    {
                        result = Convert.ToDecimal(scalar);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing scalar: " + ex.Message);
            }
            finally
            {
                if (_transaction == null) _conn.Close();
            }

            return result;
        }

    }
}
