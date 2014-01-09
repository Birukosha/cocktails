using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Cocktails.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        List<CocktailDBO> GetCocktails();
        [OperationContract]
        CocktailInfoDBO GetCocktailByID(int id);

        [OperationContract]
        List<string> GetIngredients();
        [OperationContract]
        List<string> GetIngredientsByID(int id);

        [OperationContract]
        bool AddIngredient(string user, string password, string ingredient);
        // TODO: Add your service operations here
    }
    [DataContract]
    public class CocktailDBO
    {
        [DataMember]
        public int ID;
        [DataMember]
        public string Name;
        [DataMember]
        public string Description;
        [DataMember]
        public string Preparation;
        [DataMember]
        public string ImageURL;
    }

    [DataContract]
    public class IngredientDBO
    {
        [DataMember]
        public string Name;
        [DataMember]
        public string Number;
    }

    [DataContract]
    public class CocktailInfoDBO
    {
        [DataMember]
        public CocktailDBO Cocktail;
        [DataMember]
        public List<IngredientDBO> Ingredients;
    }
}
