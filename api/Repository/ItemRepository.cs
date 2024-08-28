using api.Models;

namespace api.Repository;

public class ItemRepository
{
    
    public List<Item> GetAllItems(){
       return DataSource();
    }


    public Item GetItemById(int id){
       return DataSource().Where(x => x.Id == id).FirstOrDefault();
    }

    public List<Item> SearchItem(string title){
        return DataSource().Where(x=> x.Name.Contains(title)).ToList();
    }



private List<Item> DataSource(){
    return new List<Item>(){
        new Item(){Id=1, Name="Phone", Description="This is a description for a phone"},
        new Item(){Id=2, Name="Tablet", Description="This is a description for a Tablet"},
        new Item(){Id=3, Name="Speaker", Description="This is a description for a Speaker"},
        new Item(){Id=4, Name="Phone", Description="This is a description for a phone 2"},

    };
}
}
