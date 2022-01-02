using CollectionHierarchy.Core;
using CollectionHierarchy.Models;

namespace CollectionHierarchy
{
    

    public class StartUp
    {
        public static void Main()
        {
            var addCollection = new AddCollection();
            var addRemoveCollection = new AddRemoveCollection();
            var myList = new MyList();

            var engine = new Engine(
                addCollection,
                addRemoveCollection,
                myList);

            engine.Run();
        }
    }
}