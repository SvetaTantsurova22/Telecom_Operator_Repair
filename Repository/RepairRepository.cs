using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Telecom_Operator_Repair.Repository
{
    public class RepairRepository
    {
        private string connectionString;

        public RepairRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool AddOrder(Order order)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"INSERT INTO Orders
                        (OrderNumber, OrderType, CustomerAccount, CustomerName, CustomerPhone, Address, CreatedDate, CreatedBy, StatusID)
                         VALUES
                        (@OrderNumber, @OrderType, @CustomerAccount, @CustomerName, @CustomerPhone, @Address, @CreatedDate, @CreatedBy, 1)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderNumber", Guid.NewGuid().ToString().Substring(0, 8).ToUpper());
                cmd.Parameters.AddWithValue("@OrderType", order.OrderType);
                cmd.Parameters.AddWithValue("@CustomerAccount", order.AccountNumber);
                cmd.Parameters.AddWithValue("@CustomerName", order.CustomerName);
                cmd.Parameters.AddWithValue("@CustomerPhone", order.CustomerPhone);
                cmd.Parameters.AddWithValue("@Address", order.Address);
                cmd.Parameters.AddWithValue("@CreatedDate", order.CreatedDate);
                cmd.Parameters.AddWithValue("@CreatedBy", order.CreatedBy);

                return cmd.ExecuteNonQuery() > 0;
            }
        }


    }
}
