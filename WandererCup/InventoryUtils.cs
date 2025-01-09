using System;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WandererCup
{
    public static class InventoryUtils
    {
        public static void DeductInventoryQuantity(int productId, int quantityOrdered)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        UPDATE inventory i
                        JOIN Ingredients ing ON i.ProductName = ing.IngredientName
                        SET i.Quantity = i.Quantity - (ing.Quantity * @QuantityOrdered)
                        WHERE ing.ProductID = @ProductID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@QuantityOrdered", quantityOrdered);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}
