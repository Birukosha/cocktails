using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace Cocktails.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service1 : IService1
    {
        public List<CocktailDBO> GetCocktails()
        {
            List<CocktailDBO> cocktails = new List<CocktailDBO>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select * from Cocktails order by Name";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CocktailDBO cocktail = new CocktailDBO()
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Preparation = reader.GetString(3),
                                ImageURL = reader.GetString(4)
                            };
                            cocktails.Add(cocktail);
                        }
                        reader.Close();
                    }
                }
                conn.Close();
            }
            return cocktails;
        }


        public CocktailInfoDBO GetCocktailByID(int id)
        {
            CocktailInfoDBO cocktailInfo = new CocktailInfoDBO();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select * from Cocktails where ID = @IDc";
                    cmd.Parameters.AddWithValue("@IDc", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cocktailInfo.Cocktail = new CocktailDBO();
                        while (reader.Read())
                        {
                            cocktailInfo.Cocktail.ID = reader.GetInt32(0);
                            cocktailInfo.Cocktail.Name = reader.GetString(1);
                            cocktailInfo.Cocktail.Description = reader.GetString(2);
                            cocktailInfo.Cocktail.Preparation = reader.GetString(3);
                            cocktailInfo.Cocktail.ImageURL = reader.GetString(4);
                        }
                        reader.Close();
                    }
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select * from CocktailsIngredients where IDctail = @IDc";
                    cmd.Parameters.AddWithValue("@IDc", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cocktailInfo.Ingredients = new List<IngredientDBO>();
                        while (reader.Read())
                        {
                            IngredientDBO ingridient = new IngredientDBO()
                            {
                                Name = reader.GetString(2),
                                Number = reader.GetString(3)
                            };
                            cocktailInfo.Ingredients.Add(ingridient);
                        }
                        reader.Close();
                    }
                }
                conn.Close();

            }
            return cocktailInfo;

        }
        public List<string> GetIngredients()
        {
            List<string> ingredients = new List<string>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select * from Ingredients order by Name";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ingredient = reader.GetString(1);
                            ingredients.Add(ingredient);
                        }
                        reader.Close();
                    }
                }
                conn.Close();
            }
            return ingredients;
        }
        public List<string> GetIngredientsByID(int id)
        {
            List<string> ingredients = new List<string>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select * from CocktailsIngredients where IDctail = @IDc order by NameIngr";
                    cmd.Parameters.AddWithValue("@IDc", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ingredient = reader.GetString(2);
                            ingredients.Add(ingredient);
                        }
                        reader.Close();
                    }
                }
                conn.Close();
            }
            return ingredients;

        }

        public bool AddIngredient(string user, string password, string ingredient)
        {
            if (user != "birukosha" || password != "a3b7f6d5s4")
                return false;

            List<string> ingredients = new List<string>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select * from Ingredients where Name = @ingredient order by Name";
                    cmd.Parameters.AddWithValue("@ingredient", ingredient);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ingredientFromDB = reader.GetString(1);
                            ingredients.Add(ingredientFromDB);
                        }
                        reader.Close();
                    }
                    if (ingredients.Count() != 0)
                        return false;
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "Insert Into Ingredients values (@ingredient)";
                    cmd.Parameters.AddWithValue("@ingredient", ingredient);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            return true;
        }
    }
}
